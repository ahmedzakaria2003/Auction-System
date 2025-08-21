import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { loadStripe, Stripe, StripeElements, StripeCardElement } from '@stripe/stripe-js';

import { AuctionWonDto } from '../../../Shared/Models/AuctionWon.dto';
import { Subscription } from 'rxjs';
import { PaymentService } from '../../../Shared/Services/payment.service';
import { ProfileService } from '../../../Shared/Services/profile.service';
import { SellerFeedbackService } from '../../../Shared/Services/sellerfeedback.Service';
import { AddFeedbackRequest } from '../../../Shared/Models/seller-feedback.dto';
import { NavBarComponent } from "../../../Shared/nav-bar/nav-bar.component";
import { FooterComponent } from "../../../Shared/footer/footer.component";
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../Shared/Services/auth.service';

@Component({
  selector: 'app-won-auctions',
  standalone: true,
  imports: [CommonModule, FormsModule, NavBarComponent, FooterComponent , RouterLink],
  templateUrl: './won-auctions.component.html',
  styleUrl: './won-auctions.component.css'
})
export class WonAuctionsComponent {
  wonAuctions: AuctionWonDto[] = [];
  activeFilter: 'all' | 'paid' | 'unpaid' = 'all';

  selectedAuction: AuctionWonDto | null = null;

  processingPayment: string | null = null;
  showPaymentModal = false;

  private stripe: Stripe | null = null;
  private elements: StripeElements | null = null;
  private cardElement: StripeCardElement | null = null;
  private currentClientSecret: string | null = null;
  private currentPaymentIntentId: string | null = null;

  submittingFeedback = false;
  feedbackForm = { rating: 0, comment: '' };
  submittedFeedbacks: Set<string> = new Set();

  carouselStates: { [auctionId: string]: number } = {};
  currentSlide = 0;
  isTransitioning = false;
  slideWidth = 420;
  userName: string | null = null;

  baseUrl = 'https://localhost:7108';
  private subscriptions: Subscription[] = [];

  constructor(
    private paymentService: PaymentService,
    private profileService: ProfileService,
    private sellerFeedbackService: SellerFeedbackService, 
    private authService: AuthService
  ) {}

  async ngOnInit() {
    this.userName = this.authService.getUserName();
    this.loadWonAuctions();
    this.stripe = await loadStripe('pk_test_51RNQofA8QDGrzu3czRu6oG7OsDjz9NubihXdM3lJmHZVFVNt7mtAEYIQcMMEZSBFnVwrJrZQOpFRUFdxkYM4IoPp009SMxxYlx');
    if (!this.stripe) console.error('Stripe failed to initialize');
  }

  ngOnDestroy() {
    this.subscriptions.forEach(s => s.unsubscribe());
    this.unmountCard();
  }

  // ===================== Data =====================
  loadWonAuctions() {
    const sub = this.profileService.getWonAuctions().subscribe({
      next: (auctions) => {
        this.wonAuctions = auctions;
        auctions.forEach(a => this.carouselStates[a.id] = 0);
        this.wonAuctions.forEach(a => this.checkFeedback(a.id));
      },
      error: (err) => console.error('Error loading won auctions:', err)
    });
    this.subscriptions.push(sub);
  }

  // ===================== Filters / Stats =====================
  setFilter(filter: 'all' | 'paid' | 'unpaid') { this.activeFilter = filter; this.currentSlide = 0; }
  getFilteredAuctions(): AuctionWonDto[] {
    switch (this.activeFilter) {
      case 'paid': return this.wonAuctions.filter(a => a.isPaid);
      case 'unpaid': return this.wonAuctions.filter(a => !a.isPaid);
      default: return this.wonAuctions;
    }
  }
  getPaidAuctionsCount() { return this.wonAuctions.filter(a => a.isPaid).length; }
  getUnpaidAuctionsCount() { return this.wonAuctions.filter(a => !a.isPaid).length; }
  getTotalSpent() { return this.wonAuctions.filter(a => a.isPaid).reduce((t, a) => t + a.winningBidAmount, 0); }

  // ===================== Cards Slider =====================
  getVisibleSlides() { if (window.innerWidth < 768) return 1; if (window.innerWidth < 992) return 2; return 3; }
  getMaxSlide() { const n = this.getFilteredAuctions().length; return Math.max(0, n - this.getVisibleSlides()); }
  getSliderTransform() { return -this.currentSlide * this.slideWidth; }
  nextSlide() { if (this.currentSlide < this.getMaxSlide()) { this.isTransitioning = true; this.currentSlide++; setTimeout(()=>this.isTransitioning=false,500);} }
  previousSlide() { if (this.currentSlide > 0) { this.isTransitioning = true; this.currentSlide--; setTimeout(()=>this.isTransitioning=false,500);} }
  goToSlide(i: number) { this.isTransitioning = true; this.currentSlide = i; setTimeout(()=>this.isTransitioning=false,500); }
  getSliderIndicators() { return Array(this.getMaxSlide() + 1).fill(0).map((_, i) => i); }

  // ===================== Image Carousel =====================
  getCurrentImageIndex(id: string) { return this.carouselStates[id] || 0; }
  getCarouselTransform(id: string) { return -this.getCurrentImageIndex(id) * 100; }
  nextImage(id: string) { const a = this.wonAuctions.find(x => x.id === id); if (!a) return; const i = this.getCurrentImageIndex(id); this.carouselStates[id] = (i + 1) % a.images.length; }
  previousImage(id: string) { const a = this.wonAuctions.find(x => x.id === id); if (!a) return; const i = this.getCurrentImageIndex(id); this.carouselStates[id] = i === 0 ? a.images.length - 1 : i - 1; }
  goToImage(id: string, idx: number) { this.carouselStates[id] = idx; }

  // ===================== Payment (Stripe) =====================
  async initiatePayment(auction: AuctionWonDto) {
    if (!this.stripe) { this.showErrorMessage('Stripe غير مُهيّأ'); return; }
    this.selectedAuction = auction; this.processingPayment = auction.id;

    const sub = this.paymentService.createPaymentIntentForAuction(auction.id).subscribe({
      next: async (pi) => {
        this.currentClientSecret = pi.clientSecret; this.currentPaymentIntentId = pi.paymentIntentId;
        this.openPaymentModalAndMountCard();
      },
      error: (err) => console.error('create intent error', err)
    });
    this.subscriptions.push(sub);
  }

  private async openPaymentModalAndMountCard() {
    this.showPaymentModal = true;
    setTimeout(() => { this.mountCard(); this.processingPayment = null; }, 0);
    const modal = document.getElementById('paymentModal');
    if (modal) { modal.classList.add('show'); modal.style.display = 'block'; }
  }

  private mountCard() {
    if (!this.stripe) return;
    this.unmountCard();
    this.elements = this.stripe.elements();
    this.cardElement = this.elements.create('card', { hidePostalCode: true });
    this.cardElement.mount('#card-element');
  }

  private unmountCard() { if (this.cardElement) { this.cardElement.unmount(); this.cardElement = null; } this.elements = null; }

  async confirmPayment() {
    if (!this.stripe || !this.cardElement || !this.currentClientSecret || !this.selectedAuction) return;
    this.processingPayment = this.selectedAuction.id;

    const result = await this.stripe.confirmCardPayment(this.currentClientSecret, { payment_method: { card: this.cardElement } });

    if (result.error) { console.error(result.error.message); this.showErrorMessage('فشل الدفع: ' + result.error.message); this.processingPayment = null; return; }

    if (result.paymentIntent && result.paymentIntent.status === 'succeeded') {
      if (this.currentPaymentIntentId) {
        const sub = this.paymentService.confirmPaymentIntent(this.currentPaymentIntentId).subscribe({
          next: () => { this.selectedAuction!.isPaid = true; this.showSuccessMessage('تم الدفع بنجاح!'); this.closePaymentModal(); },
          error: (err) => { console.error('confirm-intent error', err); this.showErrorMessage('تم الخصم، لكن حدثت مشكلة في تحديث الحالة. أعد فتح الصفحة لاحقًا.'); this.closePaymentModal(); }
        });
        this.subscriptions.push(sub);
      } else { this.selectedAuction.isPaid = true; this.showSuccessMessage('تم الدفع بنجاح!'); this.closePaymentModal(); }
    } else { this.showErrorMessage('تعذّر إتمام الدفع. حاول مرة أخرى.'); this.processingPayment = null; }
  }

  closePaymentModal() {
    this.showPaymentModal = false; this.processingPayment = null; this.currentClientSecret = null; this.currentPaymentIntentId = null; this.unmountCard();
    const modal = document.getElementById('paymentModal');
    if (modal) { modal.classList.remove('show'); modal.style.display = 'none'; }
  }

  // ===================== Feedback =====================
  openFeedbackModal(auction: AuctionWonDto) {
    this.selectedAuction = auction; this.feedbackForm = { rating: 0, comment: '' };
    this.checkFeedback(auction.id);
    const modal = document.getElementById('feedbackModal');
    if (modal) { modal.classList.add('show'); modal.style.display = 'block'; }
  }

  closeFeedbackModal() {
    this.selectedAuction = null;
    const modal = document.getElementById('feedbackModal');
    if (modal) { modal.classList.remove('show'); modal.style.display = 'none'; }
  }

  setRating(r: number) { this.feedbackForm.rating = r; }

  submitFeedback() {
    if (!this.selectedAuction || this.feedbackForm.rating === 0) return;
    this.submittingFeedback = true;

    const feedback: AddFeedbackRequest = { auctionId: this.selectedAuction.id, rating: this.feedbackForm.rating, comment: this.feedbackForm.comment };

    this.sellerFeedbackService.addFeedback(feedback).subscribe({
      next: () => { this.submittedFeedbacks.add(this.selectedAuction!.id); this.showSuccessMessage('تم إرسال التقييم بنجاح!'); this.closeFeedbackModal(); this.submittingFeedback = false; },
      error: (err) => { console.error('Error submitting feedback:', err); this.showErrorMessage('حدث خطأ أثناء إرسال التقييم'); this.submittingFeedback = false; }
    });
  }

  checkFeedback(auctionId: string) {
    this.sellerFeedbackService.hasUserRatedAuction(auctionId).subscribe({
      next: (res) => { if (res.hasRated) this.submittedFeedbacks.add(auctionId); },
      error: (err) => console.error(err)
    });
  }

  hasFeedback(auctionId: string): boolean { return this.submittedFeedbacks.has(auctionId); }
  canSubmitFeedback(auctionId: string): boolean { return !this.hasFeedback(auctionId); }

  // ===================== Utils =====================
  normalizeImageUrl(path: string) { if (!path) return ''; return path.startsWith('http') || path.startsWith('data:') ? path : this.baseUrl + (path.startsWith('/') ? path : '/' + path); }
  formatDate(date: string) { const d = new Date(date); return d.toLocaleDateString('ar-EG', { year:'numeric', month:'long', day:'numeric', hour:'2-digit', minute:'2-digit' }); }
  private showSuccessMessage(msg: string) { alert(msg); }
  private showErrorMessage(msg: string) { alert(msg); }
}
