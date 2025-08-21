import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuctionListDto } from '../../../Shared/Models/auction-list.dto';
import { CategoryDto } from '../../../Shared/Models/category.dto';
import { CommonModule } from '@angular/common';
import { CategoryService } from '../../../Shared/Services/category.service';
import { AuctionService } from '../../../Shared/Services/auction.service';
import { NavigationEnd, Router } from '@angular/router';
import { FooterComponent } from "../../../Shared/footer/footer.component";
import { NavBarComponent } from "../../../Shared/nav-bar/nav-bar.component";
import { filter } from 'rxjs';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FooterComponent, NavBarComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit, OnDestroy {

  hotAuctions: AuctionListDto[] = [];
  categories: CategoryDto[] = [];
  baseUrl = 'https://localhost:7108';
  remainingTimeMap: { [auctionId: string]: string } = {};
  private timerInterval: any;

  constructor(
    private categoryService: CategoryService,
    private auctionService: AuctionService,
    private router: Router
  ) {}

  ngOnInit() {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        window.scrollTo(0, 0);
      });

    this.loadHotAuctions();
    this.loadCategories();

    this.timerInterval = setInterval(() => {
      this.updateRemainingTimes();
    }, 1000);
  }

  ngOnDestroy() {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
    }
  }

  loadHotAuctions() {
    this.auctionService.getHotAuctions(10).subscribe({
      next: (data) => {
        this.hotAuctions = data;
        this.updateRemainingTimes(); 
      },
      error: (err) => {
        console.error('Error loading hot auctions:', err);
      }
    });
  }

  normalizeImageUrl(path: string) {
    if (!path) return '';
    if (path.startsWith('http') || path.startsWith('data:')) return path;
    return this.baseUrl.endsWith('/') || path.startsWith('/')
      ? this.baseUrl + path
      : `${this.baseUrl}/${path}`;
  }

  loadCategories() {
    this.categoryService.GetAllCategoriesForDropdown().subscribe({
      next: (data) => {
        this.categories = data;
      },
      error: (err) => {
        console.error('Error loading categories:', err);
      }
    });
  }

  getCategoryIcon(categoryName: string): string {
    const icons: { [key: string]: string } = {};
    return icons[categoryName] || 'fas fa-tag';
  }

private updateRemainingTimes() {
  const now = new Date().getTime();
  this.hotAuctions.forEach(auction => {
    const end = new Date(auction.endTime).getTime() + (3 * 60 * 60 * 1000);
    const diff = end - now;

    if (diff <= 0) {
      this.remainingTimeMap[auction.id] = 'انتهى';
    } else {
      const days = Math.floor(diff / (1000 * 60 * 60 * 24));
      const hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
      const minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
      const seconds = Math.floor((diff % (1000 * 60)) / 1000);

      if (days > 0) {
        this.remainingTimeMap[auction.id] = `${days} يوم ${hours} ساعة ${minutes} دقيقة  `;
      } else if (hours > 0) {
        this.remainingTimeMap[auction.id] = `${hours} ساعة ${minutes} دقيقة ${seconds} ثانية`;
      } else if (minutes > 0) {
        this.remainingTimeMap[auction.id] = `${minutes} دقيقة ${seconds} ثانية`;
      } else {
        this.remainingTimeMap[auction.id] = `${seconds} ثانية`;
      }
    }
  });
}


  scrollToHotAuctions() {
    document.getElementById('hot-auctions')?.scrollIntoView({
      behavior: 'smooth'
    });
  }

  viewAuctionDetails(auctionId: string) {
    this.router.navigate(['/bidder/auction', auctionId]);
  }

  viewCategoryAuctions(categoryId: string) {
    this.router.navigate(['/bidder/categories', categoryId]);
  }
}
