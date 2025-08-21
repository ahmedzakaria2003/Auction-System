import { ChangeDetectorRef, Component, OnInit, NgZone, OnDestroy } from '@angular/core';
import { CategoryService } from '../../../Shared/Services/category.service';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { AuctionQueryDto } from '../../../Shared/Models/auction-query.dto';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CategoryWithAuctionsDto, CategoryWithAuctionsResponse } from '../../../Shared/Models/category-with-auctions.dto';
import { AuctionListDto } from '../../../Shared/Models/auction-list.dto';
import { NavBarComponent } from "../../../Shared/nav-bar/nav-bar.component";
import { FooterComponent } from "../../../Shared/footer/footer.component";

@Component({
  selector: 'app-category-with-auctions',
  standalone: true,
  imports: [CommonModule, FormsModule, NavBarComponent, FooterComponent],
  templateUrl: './category-with-auctions.component.html',
  styleUrls: ['./category-with-auctions.component.css']
})
export class CategoryWithAuctionsComponent implements OnInit, OnDestroy {
  category: CategoryWithAuctionsDto | null = null;
  auctions: AuctionListDto[] = [];
  remainingTimeMap: { [auctionId: string]: string } = {};

  selectedStatus = '';
  searchTerm = '';
  currentPage = 1;
  pageSize = 6;
  totalPages = 1;
  minPrice: number | null = null;
  maxPrice: number | null = null;
  sortDirection = '';
  isEndingSoon: boolean | undefined = undefined;
  baseUrl = 'https://localhost:7108';

  private intervalId: any;

  constructor(
    private categoryService: CategoryService,
    private route: ActivatedRoute,
    private zone: NgZone,
    private router: Router
  ) {}

  ngOnInit() {
    const categoryId = this.route.snapshot.paramMap.get('id');
    if (categoryId) {
      this.loadCategoryAuctions(categoryId);
    } else {
      console.error('Category ID not found in route');
    }

    this.zone.runOutsideAngular(() => {
      this.intervalId = setInterval(() => {
        this.zone.run(() => {
          this.updateRemainingTimes();
        });
      }, 1000);
    });
  }

  ngOnDestroy() {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }

  loadCategoryAuctions(categoryId: string) {
    const queryParams: AuctionQueryDto = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize,
      search: this.searchTerm || undefined,
      status: this.selectedStatus || undefined,
      minPrice: this.minPrice ?? undefined,
      maxPrice: this.maxPrice ?? undefined,
      sort: this.sortDirection !== '' ? Number(this.sortDirection) : undefined,
      isEndingSoon: this.isEndingSoon === true ? true : undefined
    };

    console.log('Query Params:', queryParams);

    this.categoryService.getCategoryWithAuctions(categoryId, queryParams).subscribe(
      (response: CategoryWithAuctionsResponse) => {
        console.log('API Response:', response);

        if (response?.data?.length > 0) {
          this.category = response.data[0];
          const pagedAuctions = this.category?.pagedAuctions?.data;
          this.auctions = Array.isArray(pagedAuctions) ? pagedAuctions : [];
          this.totalPages = this.category?.pagedAuctions?.count
            ? Math.ceil(this.category.pagedAuctions.count / this.pageSize)
            : 1;

          this.updateRemainingTimes();
        } else {
          this.category = null;
          this.auctions = [];
          this.totalPages = 1;
        }
      },
      err => {
        console.error('Error loading auctions:', err);
        this.auctions = [];
        this.totalPages = 1;
      }
    );
  }

  updateRemainingTimes() {
    const now = new Date().getTime();
    this.auctions.forEach(auction => {
      const end = new Date(auction.endTime + 'Z').getTime();
      const diff = end - now;

      this.remainingTimeMap[auction.id] = this.getTimeRemaining(diff);
    });
  }

  getTimeRemainingForEndingSoon(endTime: string): number {
    const end = new Date(endTime).getTime();
    const now = new Date().getTime();
    const diff = end - now;
    return Math.max(0, Math.floor(diff / (1000 * 60 * 60))); 
  }

  getTimeRemaining(diff: number): string {
    if (diff <= 0) return 'انتهى المزاد';

    const days = Math.floor(diff / (1000 * 60 * 60 * 24));
    const hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
    const seconds = Math.floor((diff % (1000 * 60)) / 1000);

    let result = '';
    if (days > 0) result += `${days} يوم `;
    if (hours > 0) result += `${hours} ساعة `;
    if (minutes > 0) result += `${minutes} دقيقة `;
    if (seconds >= 0) result += `${seconds} ثانية`;

    return result.trim();
  }

  public normalizeImageUrl(path: string) {
    if (!path) return '';
    if (path.startsWith('http') || path.startsWith('data:')) return path;
    return this.baseUrl.endsWith('/') || path.startsWith('/') 
      ? this.baseUrl + path 
      : `${this.baseUrl}/${path}`;
  }

  changePage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      const categoryId = this.route.snapshot.paramMap.get('id');
      if (categoryId) {
        this.loadCategoryAuctions(categoryId);
      }
    }
  }

  getPageNumbers(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  applyFilters() {
    this.currentPage = 1;
    const categoryId = this.route.snapshot.paramMap.get('id');
    if (categoryId) {
      this.loadCategoryAuctions(categoryId);
    }
  }

  resetFilters() {
    this.selectedStatus = '';
    this.searchTerm = '';
    this.minPrice = null;
    this.maxPrice = null;
    this.sortDirection = '';
    this.isEndingSoon = undefined;
    this.currentPage = 1;
    const categoryId = this.route.snapshot.paramMap.get('id');
    if (categoryId) {
      this.loadCategoryAuctions(categoryId);
    }
  }

  viewAuctionDetails(auctionId: string) {
  this.router.navigate(['/bidder/auction', auctionId]);
  }
}
