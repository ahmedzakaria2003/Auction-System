import { Component, OnInit } from '@angular/core';
import { AuctionListDto } from '../../../Shared/Models/auction-list.dto';
import { WinnerDto } from '../../../Shared/Models/winner.dto';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AdminService } from '../../../Shared/Services/admin.service';
import { AuctionService } from '../../../Shared/Services/auction.service';
import { CategoryDto } from '../../../Shared/Models/category.dto';
import { CategoryService } from '../../../Shared/Services/category.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-auction-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './auction-management.component.html',
  styleUrl: './auction-management.component.css'
})
export class AuctionManagementComponent implements OnInit {
  auctions: AuctionListDto[] = [];
  filteredAuctions: AuctionListDto[] = [];
  selectedStatus = '';
  selectedCategory = '';
  noAuctionsMessage = '';
  searchTerm = '';
  currentPage = 1;
  pageSize = 5; 
  totalCount = 0;
  totalPages = 0;
  showCreateModal = false;
  showWinnerModal = false;
  selectedWinner: WinnerDto | null = null;
  categories: CategoryDto[] = [];
  baseUrl = 'https://localhost:7108';

  constructor(
    private adminService: AdminService,
    private auctionService: AuctionService,
    private categoryService: CategoryService
  ) {}

  ngOnInit() {
    this.loadAuctions();
    this.loadCategories();
  }

  loadAuctions() {
    this.noAuctionsMessage = '';
    const queryParams: any = { PageNumber: this.currentPage, PageSize: this.pageSize };
    if (this.selectedCategory) queryParams.CategoryId = this.selectedCategory;
    if (this.selectedStatus) queryParams.Status = this.selectedStatus;
    if (this.searchTerm?.trim()) queryParams.Search = this.searchTerm.trim();

    this.adminService.getSellersAuctions(queryParams).subscribe({
      next: (response: any) => {
        if (response?.data?.data?.length) {
          this.auctions = response.data.data;
          this.filteredAuctions = [...this.auctions];
          this.totalCount = response.data?.count || 0;
          this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        } else {
          this.auctions = [];
          this.filteredAuctions = [];
          this.noAuctionsMessage = 'لا توجد مزادات مطابقة للبحث.';
        }
      },
      error: (err) => {
        if (err?.status === 404) {
          this.auctions = [];
          this.filteredAuctions = [];
          this.noAuctionsMessage = 'لا توجد مزادات مطابقة للبحث.';
        } else {
          console.error('Error loading auctions:', err);
          this.noAuctionsMessage = 'حدث خطأ أثناء تحميل المزادات.';
        }
      }
    });
  }

  loadCategories() {
    this.categoryService.GetAllCategoriesForDropdown().subscribe({
      next: (categories) => this.categories = categories,
      error: (err) => console.error('Error loading categories:', err)
    });
  }

  public normalizeImageUrl(path: string) {
    if (!path) return '';
    if (path.startsWith('http') || path.startsWith('data:')) return path;
    return this.baseUrl.endsWith('/') || path.startsWith('/') ? this.baseUrl + path : `${this.baseUrl}/${path}`;
  }

  // ---------- Pagination ----------
  changePage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.loadAuctions();
  }

  getPageNumbers(): number[] {
    return Array(this.totalPages).fill(0).map((_, i) => i + 1);
  }

  // ---------- Filters ----------
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

  onSearchTermChange(newTerm: string) {
    this.searchTerm = newTerm;
    this.currentPage = 1;
    this.loadAuctions();
  }

  resetFilters() {
    this.selectedStatus = '';
    this.selectedCategory = '';
    this.searchTerm = '';
    this.currentPage = 1;
    this.loadAuctions();
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'open': return 'status-open';
      case 'closed': return 'status-closed';
      case 'canceled': return 'status-canceled';
      default: return '';
    }
  }

  getStatusText(status: string): string {
    switch (status) {
      case 'open': return 'مفتوح';
      case 'closed': return 'مُغلق';
      case 'canceled': return 'مُلغى';
      default: return status;
    }
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('ar-EG');
  }

  // ---------- Cancel Auction ----------
  async cancelAuction(auctionId: string) {
    const result = await Swal.fire({
      title: 'هل أنت متأكد من إلغاء هذا المزاد؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'نعم، إلغاء',
      cancelButtonText: 'إلغاء'
    });

    if (result.isConfirmed) {
      this.auctionService.cancelAuction(auctionId).subscribe({
        next: () => {
          Swal.fire({ icon: 'success', title: 'تم!', text: 'تم إلغاء المزاد بنجاح 🎉', timer: 2000, showConfirmButton: false });
          this.auctions = this.auctions.map(a =>
            a.id === auctionId ? { ...a, auctionStatus: 'canceled' } : a
          );
          this.filteredAuctions = [...this.auctions];
        },
        error: (err) => console.error('Error canceling auction', err)
      });
    }
  }

  // ---------- Delete Auction ----------
  async deleteAuction(id: string) {
    const result = await Swal.fire({
      title: 'هل أنت متأكد من الحذف؟',
      text: 'لا يمكن التراجع عن الحذف!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'نعم، احذف',
      cancelButtonText: 'إلغاء'
    });

    if (result.isConfirmed) {
      this.auctionService.deleteAuction(id).subscribe({
        next: () => {
          Swal.fire({ icon: 'success', title: 'تم!', text: 'تم حذف المزاد بنجاح 🎉', timer: 2000, showConfirmButton: false });
          this.auctions = this.auctions.filter(a => a.id !== id);
          this.filteredAuctions = [...this.auctions];
          this.totalCount = this.totalCount - 1;
          this.totalPages = Math.ceil(this.totalCount / this.pageSize);
          if (this.auctions.length === 0 && this.currentPage > 1) {
            this.currentPage--;
            this.loadAuctions();
          }
        },
        error: (err) => console.error('Error deleting auction:', err)
      });
    }
  }
}





