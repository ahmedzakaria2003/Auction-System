import { DepositService } from './../../../Shared/Services/deposit.service';
import { SellerFeedbackService } from './../../../Shared/Services/sellerfeedback.Service';
import { Component } from '@angular/core';
import { AuctionDetailsDto, BidDto } from '../../../Shared/Models/auction-details.dto';
import { AuctionListDto } from '../../../Shared/Models/auction-list.dto';
import { filter, interval, Subscription } from 'rxjs';
import { AuctionService } from '../../../Shared/Services/auction.service';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { BidService } from '../../../Shared/Services/bid.service';
import { SellerFeedbackDto } from '../../../Shared/Models/seller-feedback.dto';
import { AddBidDto } from '../../../Shared/Models/add-bid.dto';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { loadStripe, Stripe, StripeCardElement } from '@stripe/stripe-js';
import { NavBarComponent } from "../../../Shared/nav-bar/nav-bar.component";
import Swal from 'sweetalert2';
import { AuctionSignalRService } from '../../../Shared/Services/auction-signalr.service';
import { FooterComponent } from '../../../Shared/footer/footer.component';

@Component({
  selector: 'app-auction-details',
  standalone: true,
  imports: [CommonModule, FormsModule, NavBarComponent, FooterComponent],
  templateUrl: './auction-details.component.html',
  styleUrl: './auction-details.component.css'
})
export class AuctionDetailsComponent {
  auctionDetails: AuctionDetailsDto | null = null;
  bids: BidDto[] = [];
  highestBid: BidDto | null = null;
  sellerFeedbacks: SellerFeedbackDto[] = [];
  sellerAverageRating: number = 0;
  recommendedAuctions: AuctionListDto[] = [];
  categoryId: string | null = null;

  selectedImage: string = '';
  selectedImageIndex: number = 0;
  bidAmount: number = 0;
  hasUserPaidDeposit: boolean = false;
  showAllBids: boolean = false;
  baseUrl = 'https://localhost:7108';

  timeRemaining = { days: 0, hours: 0, minutes: 0, seconds: 0 };
  private timerSubscription: Subscription | null = null;

  private stripe: Stripe | null = null;
  private cardElement: StripeCardElement | null = null;

  private currentAuctionId: string | null = null;

  constructor(
    private bidService: BidService,
    private auctionService: AuctionService,
    private sellerFeedbackService: SellerFeedbackService,
    private depositService: DepositService,
    private route: ActivatedRoute,
    private router: Router,
    private signalRService: AuctionSignalRService
  ) {}

  async ngOnInit() {
    this.stripe = await loadStripe('pk_test_51RNQofA8QDGrzu3czRu6oG7OsDjz9NubihXdM3lJmHZVFVNt7mtAEYIQcMMEZSBFnVwrJrZQOpFRUFdxkYM4IoPp009SMxxYlx');

    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => window.scrollTo(0, 0));

    this.route.paramMap.subscribe(params => {
      const auctionId = params.get('id');

      if (this.currentAuctionId) {
        this.signalRService.leaveAuctionGroup(this.currentAuctionId);
      }

      if (auctionId) {
        this.currentAuctionId = auctionId;

        this.loadAuctionDetails(auctionId);
        this.loadBids(auctionId);
        this.loadHighestBid(auctionId);
        this.checkDepositStatus(auctionId);
        this.loadRecommendedAuctions();
        this.startTimer();

        this.signalRService.startConnection(auctionId);
        this.signalRService.onNewBid((amount, bidder) => {
          this.loadAuctionDetails(auctionId);
          this.loadBids(auctionId);
          this.loadHighestBid(auctionId);
          this.bidAmount = this.getMinimumBid();
        });
      }
    });
  }

  ngOnDestroy() {
    if (this.timerSubscription) this.timerSubscription.unsubscribe();
    if (this.currentAuctionId) {
      this.signalRService.leaveAuctionGroup(this.currentAuctionId);
    }
  }

  // ---------------------- Modal & Stripe ----------------------
  openDepositModal() {
    const modal = document.getElementById('depositModal');
    if (!modal) return;

    modal.classList.add('show');
    modal.style.display = 'block';
    modal.setAttribute('aria-modal', 'true');
    modal.removeAttribute('aria-hidden');

    setTimeout(() => {
      if (this.stripe && !this.cardElement) {
        const elements = this.stripe.elements();
        const cardContainer = document.getElementById('card-element');
        if (cardContainer) {
          this.cardElement = elements.create('card', { hidePostalCode: true });
          this.cardElement.mount(cardContainer);
        } else {
          console.error('Stripe card element container not found!');
        }
      }
    }, 0);
  }

  public closeModal(modalId: string) {
    const modal = document.getElementById(modalId);
    if (!modal) return;

    modal.classList.remove('show');
    modal.style.display = 'none';
    modal.setAttribute('aria-hidden', 'true');
    modal.removeAttribute('aria-modal');

    if (this.cardElement) {
      this.cardElement.unmount();
      this.cardElement = null;
    }
  }

  async processDeposit() {
    if (!this.auctionDetails || !this.stripe || !this.cardElement) return;

    this.depositService.createDepositIntent(this.auctionDetails.id).subscribe({
      next: async (paymentIntent: any) => {
        const { error, paymentIntent: confirmedIntent } = await this.stripe!.confirmCardPayment(
          paymentIntent.clientSecret,
          { payment_method: { card: this.cardElement! } }
        );

        if (error) {
          console.error('Payment failed:', error);
          Swal.fire({ icon: 'error', title: 'فشل الدفع', text: error.message });
          return;
        }

        if (confirmedIntent?.status === 'succeeded') {
          this.depositService.confirmDepositIntent(confirmedIntent.id).subscribe({
            next: () => {
              Swal.fire({ icon: 'success', title: 'نجاح', text: 'تم تأكيد الدفع بنجاح!', timer: 2000, showConfirmButton: false });
              this.checkDepositStatus(this.auctionDetails!.id);
              this.closeModal('depositModal');
            },
            error: (err) => {
              console.error('Error confirming deposit:', err);
              Swal.fire({ icon: 'error', title: 'خطأ', text: 'حدث خطأ أثناء تأكيد الدفع: ' + err.message });
            }
          });
        }
      },
      error: (err) => {
        console.error('Error creating deposit intent:', err);
        Swal.fire({ icon: 'error', title: 'خطأ', text: 'حدث خطأ أثناء إنشاء الدفع.' });
      }
    });
  }

  // ---------------------- Auction & Bids ----------------------
  loadAuctionDetails(auctionId: string) {
    this.auctionService.getAuctionDetails(auctionId).subscribe({
      next: (response: any) => {
        if (response?.data) {
          this.auctionDetails = response.data;

          if (this.auctionDetails?.itemImageUrls?.length) {
            this.selectedImage = this.auctionDetails.itemImageUrls[0];
          }

          this.bidAmount = this.getMinimumBid();

          if (this.auctionDetails?.sellerId) {
            this.loadSellerFeedbacks(this.auctionDetails.sellerId);
          }
        } else {
          console.error('No auction details found.');
        }
      },
      error: (error) => console.error('Error loading auction details:', error)
    });
  }

  loadBids(auctionId: string) {
    this.bidService.getAuctionBids(auctionId).subscribe({
      next: (bids) => this.bids = bids.sort(
        (a, b) => new Date(b.bidTime).getTime() - new Date(a.bidTime).getTime()
      ),
      error: (error) => console.error('Error loading bids:', error)
    });
  }

  loadHighestBid(auctionId: string) {
    this.bidService.getHighestBid(auctionId).subscribe({
      next: (bid) => this.highestBid = bid,
      error: (error) => console.error('Error loading highest bid:', error)
    });
  }

  loadSellerFeedbacks(sellerId: string) {
    this.sellerFeedbackService.getFeedbacksForSeller(sellerId).subscribe({
      next: (feedbacks) => this.sellerFeedbacks = feedbacks,
      error: (error) => console.error('Error loading seller feedbacks:', error)
    });
    this.sellerFeedbackService.getAverageRatingForSeller(sellerId).subscribe({
      next: (rating) => this.sellerAverageRating = rating,
      error: (error) => console.error('Error loading seller rating:', error)
    });
  }

  loadRecommendedAuctions() {
    this.auctionService.getRecommendedAuctionsForBidder().subscribe({
      next: (auctions) => this.recommendedAuctions = auctions.slice(0, 6),
      error: (error) => console.error('Error loading recommended auctions:', error)
    });
  }

  checkDepositStatus(auctionId: string) {
    this.depositService.hasPaidDeposit(auctionId).subscribe({
      next: (res) => this.hasUserPaidDeposit = res.hasPaid,
      error: (error) => console.error('Error checking deposit status:', error)
    });
  }

  placeBid() {
    if (!this.canPlaceBid() || !this.auctionDetails) return;

    const bidDto: AddBidDto = { auctionId: this.auctionDetails.id, amount: this.bidAmount };
    this.bidService.placeBid(bidDto).subscribe({
      next: (success) => {
        if (success) {
          Swal.fire({ icon: 'success', title: 'نجاح', text: 'تم وضع المزايدة بنجاح!', timer: 2000, showConfirmButton: false });
        }
      },
      error: (err) => {
        let message = 'لا يمكنك وضع المزايده لانك محظور من اداره المزاد';
        if (err?.status === 401 && err?.errorMessage) message = err.errorMessage;
        Swal.fire({ icon: 'error', title: 'خطأ', text: message });
      }
    });
  }

  canPlaceBid(): boolean {
    return this.hasUserPaidDeposit && this.isAuctionActive() && this.bidAmount >= this.getMinimumBid();
  }

  getMinimumBid(): number {
    if (!this.auctionDetails) return 0;
    const currentHighest = this.highestBid?.amount || this.auctionDetails.startingPrice;
    return currentHighest + 1;
  }

  isAuctionActive(): boolean {
    if (!this.auctionDetails) return false;
    const now = new Date().getTime();
    const startTime = new Date(this.auctionDetails.startTime + 'Z').getTime();
    const endTime = new Date(this.auctionDetails.endTime + 'Z').getTime();
    return now >= startTime && now <= endTime;
  }

  selectImage(index: number) {
    this.selectedImageIndex = index;
    this.selectedImage = this.auctionDetails!.itemImageUrls[index];
  }

  openImageModal() {
    const modal = document.getElementById('imageModal');
    if (modal) modal.classList.add('show');
  }

  normalizeImageUrl(path: string) {
    if (!path) return '';
    if (path.startsWith('http') || path.startsWith('data:')) return path;
    return this.baseUrl.endsWith('/') || path.startsWith('/') ? this.baseUrl + path : `${this.baseUrl}/${path}`;
  }

  startTimer() {
    this.timerSubscription = interval(1000).subscribe(() => this.updateTimeRemaining());
  }

  updateTimeRemaining() {
    if (!this.auctionDetails) return;
    const now = new Date().getTime();
    const end = new Date(this.auctionDetails.endTime + 'Z').getTime();
    const diff = end - now;
    this.timeRemaining = this.getTimeRemaining(diff);
  }

  getTimeRemaining(diff: number) {
    if (diff <= 0) return { days: 0, hours: 0, minutes: 0, seconds: 0 };
    const days = Math.floor(diff / (1000 * 60 * 60 * 24));
    const hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
    const seconds = Math.floor((diff % (1000 * 60)) / 1000);
    return { days, hours, minutes, seconds };
  }

  formatDateUTC(dateString: string): string {
    const date = new Date(dateString);
    if (isNaN(date.getTime())) {
      throw new Error("Invalid date string");
    }
    date.setHours(date.getHours() + 3);
    return date.toLocaleDateString('ar-EG', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
      timeZone: 'Africa/Cairo'
    });
  }

  getStarsArray(rating: number) { 
    return Array(Math.floor(rating)).fill(0); 
  }

  getEmptyStarsArray(rating: number) { 
    return Array(5 - Math.floor(rating)).fill(0); 
  }

  getAuctionStatusClass(): string {
    if (!this.auctionDetails) return '';
    const now = new Date().getTime();
    const startTime = new Date(this.auctionDetails.startTime + 'Z').getTime();
    const endTime = new Date(this.auctionDetails.endTime + 'Z').getTime();
    if (now < startTime) return 'status-pending';
    if (now > endTime) return 'status-ended';
    return 'status-active';
  }

  getAuctionStatusText(): string {
    if (!this.auctionDetails) return '';
    const now = new Date().getTime();
    const startTime = new Date(this.auctionDetails.startTime + 'Z').getTime();
    const endTime = new Date(this.auctionDetails.endTime + 'Z').getTime();
    if (now < startTime) return 'لم يبدأ بعد';
    if (now > endTime) return 'انتهى';
    return 'جاري الآن';
  }

 viewAuctionDetails(auctionId: string) {
    this.router.navigate(['/bidder/auction', auctionId]);
  }

}

