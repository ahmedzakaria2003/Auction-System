import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { AuctionService } from '../../../Shared/Services/auction.service';
import { CategoryService } from '../../../Shared/Services/category.service';
import { AuctionListDto } from '../../../Shared/Models/auction-list.dto';
import { CategoryDto } from '../../../Shared/Models/category.dto';
import { WinnerDto } from '../../../Shared/Models/winner.dto';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import Swal from 'sweetalert2';
import { AuctionSignalRService } from '../../../Shared/Services/auction-signalr.service';


@Component({
  selector: 'app-myauctions',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './myauctions.component.html',
  styleUrls: ['./myauctions.component.css']
})
export class MyauctionsComponent implements OnInit , OnDestroy {
  auctions: AuctionListDto[] = [];
  categories: CategoryDto[] = [];
  filteredAuctions: AuctionListDto[] = [];
  noAuctionsMessage = '';

  selectedStatus = '';
  selectedCategory = '';
  searchTerm = '';

  currentPage = 1;
  pageSize = 5;
  totalCount = 0;
  totalPages = 0;

  showCreateModal = false;
  showWinnerModal = false;
  showEditModal = false;

  selectedWinner: WinnerDto | null = null;
  selectedAuction: AuctionListDto | null = null;

  baseUrl = 'https://localhost:7108';
  minDateTime: string;

  auctionForm: FormGroup;
  editForm: FormGroup;

  // create
  createNewImages: File[] = [];
  createNewPreviews: string[] = [];
  createAuctionErrorMessage = '';

  // edit
  editNewImages: File[] = [];
  editNewPreviews: string[] = [];
  editOldImages: string[] = [];
  removedOldImagePaths: string[] = [];

  constructor(
    private fb: FormBuilder,
    private auctionService: AuctionService,
    private categoryService: CategoryService,
    private cdRef: ChangeDetectorRef,
  private auctionSignalR: AuctionSignalRService,

  ) {
    const now = new Date();
    this.minDateTime = now.toISOString().slice(0, 16);

    this.auctionForm = this.fb.group({
      Title: ['', Validators.required],
      Description: ['', [Validators.required, Validators.minLength(10)]],
      StartingPrice: [0, [Validators.required, Validators.min(1)]],
      StartTime: ['', Validators.required],
      EndTime: ['', Validators.required],
      CategoryId: ['', Validators.required],
      Images: [[] as File[]]
    });

    this.editForm = this.fb.group({
      Title: ['', Validators.required],
      Description: ['', [Validators.required, Validators.minLength(10)]],
      CategoryId: ['', Validators.required],
      Images: [[] as File[]]
    });
  }

 ngOnInit() {
  this.auctionSignalR.startConnection().then(() => {
    this.registerSignalREvents();
    this.loadAuctions();
    this.auctions.forEach(a => this.auctionSignalR.joinAuctionGroup(a.id));

    this.loadCategories();
  });
}
   ngOnDestroy() {
    this.auctionSignalR.stopConnection();
  }
  // ---------- Loading ----------
  loadAuctions() {
    this.noAuctionsMessage = '';
    const queryParams: any = { PageNumber: this.currentPage, PageSize: this.pageSize };
    if (this.selectedCategory) queryParams.CategoryId = this.selectedCategory;
    if (this.selectedStatus) queryParams.Status = this.selectedStatus;
    if (this.searchTerm?.trim()) queryParams.Search = this.searchTerm.trim();

    this.auctionService.getAuctionsByCreator(queryParams).subscribe({
      next: (response: any) => {
        if (response?.data?.data?.length) {
          this.auctions = response.data.data;
          this.filteredAuctions = [...this.auctions];
          this.totalCount = response.data?.count || 0;
          this.totalPages = Math.ceil(this.totalCount / this.pageSize);
          this.cdRef.detectChanges();
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
        this.cdRef.detectChanges();
      }
    });
  }

  loadCategories() {
    this.categoryService.GetAllCategoriesForDropdown().subscribe({
      next: (categories) => this.categories = categories,
      error: (err) => console.error('Error loading categories:', err)
    });
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

  // ---------- Helpers ----------
  public normalizeImageUrl(path: string) {
    if (!path) return '';
    if (path.startsWith('http') || path.startsWith('data:')) return path;
    return this.baseUrl.endsWith('/') || path.startsWith('/') ? this.baseUrl + path : `${this.baseUrl}/${path}`;
  }

  private appendNonImageFieldsToFormData(form: FormGroup, formData: FormData) {
    Object.keys(form.value).forEach(key => {
      if (key === 'Images') return;
      const val = form.value[key];
      if (val !== null && val !== undefined && val !== '') {
        formData.append(key, String(val));
      }
    });
  }


  // ---------- SignalR ----------
private startSignalRForAuctions() {
  if (!this.auctions || !this.auctions.length) return;
  this.auctions.forEach(a => this.auctionSignalR.joinAuctionGroup(a.id));
}

private registerSignalREvents() {
  this.auctionSignalR.onAuctionCreated((id, title) => {
    Swal.fire({ icon: 'info', title: 'مزاد جديد', text: `تم إنشاء مزاد: ${title}` });
    this.loadAuctions();
    this.auctions.forEach(a => this.auctionSignalR.joinAuctionGroup(a.id));

  });

  this.auctionSignalR.onAuctionUpdated((id, title) => {
    Swal.fire({ icon: 'info', title: 'تم تعديل المزاد', text: `تم تعديل مزاد: ${title}` });
    this.loadAuctions();
    this.auctions.forEach(a => this.auctionSignalR.joinAuctionGroup(a.id));

  });

  this.auctionSignalR.onAuctionDeleted((id, title) => {
    Swal.fire({ icon: 'warning', title: 'تم حذف المزاد', text: `تم حذف مزاد: ${title}` });
    this.auctions = this.auctions.filter(a => a.id !== id);
    this.filteredAuctions = [...this.auctions];
    this.auctions.forEach(a => this.auctionSignalR.joinAuctionGroup(a.id));

  });

  this.auctionSignalR.onWinnerDeclared((winner, amount) => {
    Swal.fire({ icon: 'success', title: 'تم إعلان الفائز', text: `الفائز: ${winner} بمبلغ ${amount}` });
    this.loadAuctions();
  });
}


  // ---------- Create ----------
  onCreateFileSelect(event: any) {
    const files = Array.from(event.target.files || []) as File[];
    if (!files.length) return;

    this.createNewImages = [...this.createNewImages, ...files];

    files.forEach(file => {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.createNewPreviews.push(e.target.result);
        this.cdRef.detectChanges();
      };
      reader.readAsDataURL(file);
    });

    this.auctionForm.get('Images')?.setValue(this.createNewImages);
  }

  removeCreatePreview(index: number) {
    this.createNewPreviews.splice(index, 1);
    this.createNewImages.splice(index, 1);
    this.auctionForm.get('Images')?.setValue(this.createNewImages);
  }

  createAuction() {
    if (this.auctionForm.invalid) {
      this.auctionForm.markAllAsTouched();
      this.cdRef.detectChanges();
      return;
    }

    const startTime = new Date(this.auctionForm.value.StartTime);
    const endTime = new Date(this.auctionForm.value.EndTime);
    const now = new Date();

    if (startTime < now) {
      Swal.fire({ icon: 'error', title: 'خطأ', text: 'تاريخ البداية يجب أن يكون في المستقبل!' });
      return;
    }

    if (endTime <= startTime) {
      Swal.fire({ icon: 'error', title: 'خطأ', text: 'تاريخ النهاية يجب أن يكون بعد البداية!' });
      return;
    }

    const formData = new FormData();
    this.appendNonImageFieldsToFormData(this.auctionForm, formData);
    formData.set('StartTime', startTime.toISOString());
    formData.set('EndTime', endTime.toISOString());

    if (this.createNewImages.length) {
      this.createNewImages.forEach(file => formData.append('Images', file, file.name));
    }

    this.createAuctionErrorMessage = '';

    this.auctionService.createAuction(formData).subscribe({
      next: () => {
        this.showCreateModal = false;
        this.auctionForm.reset();
        this.createNewImages = [];
        this.createNewPreviews = [];
        const fileInput = document.getElementById('createFileInput') as HTMLInputElement;
if (fileInput) {
  fileInput.value = '';
}
        Swal.fire({ icon: 'success', title: 'تم!', text: 'تم إنشاء المزاد بنجاح 🎉', timer: 2000, showConfirmButton: false });
        this.loadAuctions();
      },
      error: (err) => {
        console.error('Error creating auction:', err);
        this.cdRef.detectChanges();
      }
    });
  }

  // ---------- Edit ----------
  editAuction(auctionId: string) {
    const auction = this.auctions.find(a => a.id === auctionId);
    if (!auction) return;

    this.selectedAuction = { ...auction };
    this.editNewImages = [];
    this.editNewPreviews = [];
    this.editOldImages = [];
    this.removedOldImagePaths = [];

    const category = this.categories.find(c => c.name === auction.categoryName);

    this.editForm.patchValue({
      Title: auction.title,
      Description: auction.description || '',
      CategoryId: category?.id || ''
    });

    if (auction.thumbnailImage && auction.thumbnailImage.length) {
      this.editOldImages = auction.thumbnailImage.map((p: string) => this.normalizeImageUrl(p));
    }

    this.showEditModal = true;
    this.cdRef.detectChanges();
  }

  onEditFileSelect(event: any) {
    const files = Array.from(event.target.files || []) as File[];
    if (!files.length) return;

    this.editNewImages = [...this.editNewImages, ...files];

    files.forEach(file => {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.editNewPreviews.push(e.target.result);
        this.cdRef.detectChanges();
      };
      reader.readAsDataURL(file);
    });

    this.editForm.get('Images')?.setValue(this.editNewImages);
  }

  removeEditNewPreview(index: number) {
    this.editNewPreviews.splice(index, 1);
    this.editNewImages.splice(index, 1);
    this.editForm.get('Images')?.setValue(this.editNewImages);
  }

  removeEditOldImage(index: number) {
    const removed = this.editOldImages.splice(index, 1)[0];
    if (removed) this.removedOldImagePaths.push(removed);
  }

  saveEditAuction() {
    if (this.editForm.invalid || !this.selectedAuction) {
      this.editForm.markAllAsTouched();
      this.cdRef.detectChanges();
      return;
    }

    const formData = new FormData();
    this.appendNonImageFieldsToFormData(this.editForm, formData);

    if (this.editNewImages.length) this.editNewImages.forEach(file => formData.append('Images', file, file.name));
    if (this.removedOldImagePaths.length) this.removedOldImagePaths.forEach(path => formData.append('RemovedOldImages', path));

    this.auctionService.updateAuction(this.selectedAuction.id, formData).subscribe({
      next: (success) => {
        if (success) {
          this.showEditModal = false;
          this.selectedAuction = null;
          this.editNewImages = [];
          this.editNewPreviews = [];
          this.editOldImages = [];
          this.removedOldImagePaths = [];
          Swal.fire({ icon: 'success', title: 'تم!', text: 'تم تعديل المزاد بنجاح 🎉', timer: 2000, showConfirmButton: false });
          this.loadAuctions();
        } else {
          Swal.fire({ icon: 'error', title: 'خطأ', text: 'فشل في تعديل المزاد.' });
        }
      },
      error: (err) => {
        console.error('Error updating auction:', err);
        Swal.fire({ icon: 'error', title: 'خطأ', text: 'حدث خطأ أثناء تعديل المزاد.' });
      }
    });
  }

  // ---------- Delete / Winner ----------
  async deleteAuction(id: string) {
    const result = await Swal.fire({
      title: 'هل أنت متأكد؟',
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

  declareWinner(id: string) {
    this.auctionService.declareWinner(id).subscribe({
      next: (winner) => {
        this.selectedWinner = { ...winner };
        this.showWinnerModal = true;
        this.cdRef.detectChanges();
        this.loadAuctions();
      },
      error: (err) => {
        console.error('Error declaring winner:', err);
        this.selectedWinner = null;
        this.showWinnerModal = true;
      }
    });
  }

  onFileSelect(event: any) {
    this.onCreateFileSelect(event);
  }

  getStatusClass(status: string): string {
    const classes: Record<string, string> = { open: 'status-open', closed: 'status-closed', canceled: 'status-canceled' };
    return classes[status] || '';
  }

  getStatusText(status: string): string {
    const texts: Record<string, string> = { open: 'مفتوح', closed: 'مغلق', canceled: 'ملغى' };
    return texts[status] || status;
  }

  formatDate(date: string) {
    return new Date(date).toLocaleDateString('ar-EG');
  }
}
