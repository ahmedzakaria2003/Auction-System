import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { AuctionService } from '../../../Shared/Services/auction.service';
import { AuctionDetailsDto } from '../../../Shared/Models/auction-details.dto';
import { AuctionListDto } from '../../../Shared/Models/auction-list.dto';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CategoryDto } from '../../../Shared/Models/category.dto';
import { CategoryService } from '../../../Shared/Services/category.service';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
})
export class ReportsComponent implements OnInit {
  auctions: AuctionListDto[] = [];
  filteredAuctions: AuctionListDto[] = [];
  selectedAuction: AuctionDetailsDto | null = null;
  showReportModal: boolean = false;

  dateFrom = '';
  dateTo = '';
  selectedCategory = '';
  selectedStatus = '';
  categories: CategoryDto[] = [];
  noAuctionsMessage = '';
  currentPage = 1;
  pageSize = 5;
  totalCount = 0;
  totalPages = 0;

  constructor(
    private auctionService: AuctionService,
    private categoryService: CategoryService,
    private cdRef: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadAuctions();
    this.loadCategories();
  }

  loadCategories() {
    this.categoryService.GetAllCategoriesForDropdown().subscribe({
      next: (categories) => (this.categories = categories),
      error: (err) => console.error('Error loading categories:', err)
    });
  }

  loadAuctions() {
    this.noAuctionsMessage = ''; 

    const queryParams: any = {
      PageNumber: this.currentPage,
      PageSize: this.pageSize
    };

    if (this.selectedCategory) queryParams.CategoryId = this.selectedCategory;
    if (this.selectedStatus) queryParams.Status = this.selectedStatus;
    if (this.dateFrom) {
      queryParams.StartDate = new Date(this.dateFrom).toISOString().split('T')[0];
    }
    if (this.dateTo) {
      queryParams.EndDate = new Date(this.dateTo).toISOString().split('T')[0];
    }

    this.auctionService.getAllAuctions(queryParams).subscribe(
      (response: any) => {
        const auctionsList = response?.data?.data || [];

        if (auctionsList.length > 0) {
          this.auctions = auctionsList;
          this.filteredAuctions = [...this.auctions];
          this.totalCount = response.data?.count || 0;
          this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        } else {
          this.auctions = [];
          this.filteredAuctions = [];
          this.noAuctionsMessage = 'لا توجد مزادات مطابقة للبحث.';
        }
      },
      (error) => {
        console.error('Error loading auctions:', error);
        this.auctions = [];
        this.filteredAuctions = [];
          this.noAuctionsMessage = 'لا توجد مزادات مطابقة للبحث.';
      }
    );
  }
  

  viewAuctionReport(auctionId: string) {
    this.auctionService.getAuctionDetails(auctionId).subscribe(
      (response: any) => {
        this.selectedAuction = response.data;
        this.showReportModal = true;
        this.cdRef.detectChanges();
      },
      (error) => {
        console.error('Error fetching auction details:', error);
      }
    );
  }

  // ---------- Pagination Handlers ----------
  changePage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.loadAuctions();
  }

  getPageNumbers(): number[] {
    return Array(this.totalPages)
      .fill(0)
      .map((_, i) => i + 1);
  }

  // ---------- Filters handlers with resetting page ----------
  onStatusChange(newStatus: string) {
    this.selectedStatus = newStatus;
    this.currentPage = 1;
    this.loadAuctions();
  }

  onCategoryChange(newCategory: string) {
    this.selectedCategory = newCategory;
    this.currentPage = 1;
    this.loadAuctions();
  }

  resetFilters() {
    this.selectedStatus = '';
    this.selectedCategory = '';
    this.dateFrom = '';
    this.dateTo = '';
    this.currentPage = 1;
    this.loadAuctions();
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'open':
        return 'status-open';
      case 'closed':
        return 'status-closed';
      case 'canceled':
        return 'status-canceled';
      default:
        return '';
    }
  }

  getStatusText(status: string): string {
    switch (status) {
      case 'open':
        return 'مفتوح';
      case 'closed':
        return 'مُغلق';
      case 'canceled':
        return 'مُلغى';
      default:
        return status;
    }
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('ar-EG');
  }

  formatDateTime(date: string): string {
    return new Date(date).toLocaleString('ar-EG');
  }

  printReport() {
    window.print();
  }

  downloadReportPDF() {
    console.log('Download PDF report for:', this.selectedAuction?.id);
  }

  exportToExcel() {
    console.log('Export to Excel:', this.filteredAuctions);
  }

  generateFullReport() {
    console.log('Generate full report');
  }

  getCurrentDate(): string {
    const today = new Date();
    return today.toLocaleDateString('ar-EG');
  }
}






