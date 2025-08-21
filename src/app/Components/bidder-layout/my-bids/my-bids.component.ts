import { Component, OnInit, HostListener, ChangeDetectorRef } from '@angular/core';
import { NavBarComponent } from "../../../Shared/nav-bar/nav-bar.component";
import { FooterComponent } from "../../../Shared/footer/footer.component";
import { ActiveBiddingDto } from '../../../Shared/Models/active-bidding.dto';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ProfileService } from '../../../Shared/Services/profile.service';

@Component({
  selector: 'app-my-bids',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, NavBarComponent, FooterComponent, HttpClientModule],
  templateUrl: './my-bids.component.html',
  styleUrls: ['./my-bids.component.css']
})
export class MyBidsComponent implements OnInit {
  activeBids: ActiveBiddingDto[] = [];
  currentSlide = 0;
  slidesPerView: number = this.getSlidesPerView();
  loading = false;
  errorMessage: string | null = null;
  baseUrl = 'https://localhost:7108';

  constructor(private activeBidsService: ProfileService, private router: Router, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.loadActiveBids();
    this.currentSlide = 0;
    this.slidesPerView = this.getSlidesPerView();
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.slidesPerView = this.getSlidesPerView();
    this.currentSlide = 0;
    this.cdr.detectChanges();
  }

  private getSlidesPerView(): number {
    if (window.innerWidth <= 768) {
      return 1;
    } else {
      return 3;
    }
  }

  loadActiveBids() {
    this.loading = true;
    this.activeBidsService.getActiveBids().subscribe({
      next: (data: ActiveBiddingDto[]) => {
        this.activeBids = data || [];
        console.log('Active Bids loaded:', this.activeBids);
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error loading active bids:', err);
        this.activeBids = [];
        this.loading = false;
      }
    });
  }

  getTotalSlides(): number {
    return Math.ceil(this.activeBids.length / this.slidesPerView);
  }

  getSlideArray(): number[] {
    return Array.from({ length: this.getTotalSlides() }, (_, i) => i);
  }

  nextSlide() {
    if (this.currentSlide < this.getTotalSlides() - 1) {
      this.currentSlide++;
      console.log('Next Slide:', this.currentSlide, 'Total Slides:', this.getTotalSlides(), 'Translate:', (this.currentSlide * -100) + '%');
      this.cdr.detectChanges();
    }
  }

  previousSlide() {
    if (this.currentSlide > 0) {
      this.currentSlide--;
      console.log('Previous Slide:', this.currentSlide, 'Total Slides:', this.getTotalSlides(), 'Translate:', (this.currentSlide * -100) + '%');
      this.cdr.detectChanges();
    }
  }

  goToSlide(index: number) {
    if (index >= 0 && index < this.getTotalSlides()) {
      this.currentSlide = index;
      console.log('Go to Slide:', this.currentSlide, 'Total Slides:', this.getTotalSlides(), 'Translate:', (this.currentSlide * -100) + '%');
      this.cdr.detectChanges();
    }
  }

  isWinningBid(bid: ActiveBiddingDto): boolean {
    return bid.yourBid >= bid.currentHighestBid;
  }

  getBidStatusClass(bid: ActiveBiddingDto): string {
    return this.isWinningBid(bid) ? 'winning' : 'losing';
  }

  getBidStatusText(bid: ActiveBiddingDto): string {
    return this.isWinningBid(bid) ? 'رابح' : 'مسبوق';
  }

  getTimeRemaining(endTime: string): string {
    const now = new Date();
    const end = new Date(endTime);
    const diff = end.getTime() - now.getTime() + 3 * 60 * 60 * 1000;
    if (diff <= 0) return 'انتهى';
    const days = Math.floor(diff / (1000 * 60 * 60 * 24));
    const hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    if (days > 0) return `${days} يوم`;
    return `${hours} ساعة`;
  }

  getEndingSoonCount(): number {
    return this.activeBids.filter(bid => {
      const now = new Date();
      const end = new Date(bid.endTime);
      const diff = end.getTime() - now.getTime();
      return diff > 0 && diff <= (24 * 60 * 60 * 1000);
    }).length;
  }

  getTotalBidAmount(): number {
    return this.activeBids.reduce((total, bid) => total + bid.yourBid, 0);
  }

  viewAuctionDetails(auctionId: string) {
    this.router.navigate(['/bidder/auction', auctionId]);
  }

  normalizeImageUrl(path: string) {
    if (!path) return '';
    if (path.startsWith('http') || path.startsWith('data:')) return path;
    return this.baseUrl.endsWith('/') || path.startsWith('/')
      ? this.baseUrl + path
      : `${this.baseUrl}/${path}`;
  }
}