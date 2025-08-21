import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../../../Shared/Services/category.service'; 
import { CategoryDto } from '../../../Shared/Models/category.dto';
import { UpdateCategoryDto } from '../../../Shared/Models/update-category.dto';
import { CreateCategoryDto } from '../../../Shared/Models/create-category.dto';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuctionQueryDto } from '../../../Shared/Models/auction-query.dto';
import { PaginatedResult } from '../../../Shared/Models/PaginatedResult';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {

  categories: CategoryDto[] = [];
  filteredCategories: CategoryDto[] = [];
  searchTerm = '';
  currentPage = 1;
  pageSize = 6;
  totalPages = 1;
  totalCategories = 0;
  showCreateModal = false;
  showEditModal = false;
  backendError: string | null = null;  
  categoryForm = { id: '', name: '' };

  constructor(private categoryService: CategoryService) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories() {
    const queryParams: AuctionQueryDto = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize,
      search: this.searchTerm
    };

    this.categoryService.getAllCategories(queryParams).subscribe(
      (res: PaginatedResult<CategoryDto>) => {
        this.categories = res.data;
        this.filteredCategories = this.searchTerm
          ? this.categories.filter(c =>
              c.name.toLowerCase().includes(this.searchTerm.toLowerCase())
            )
          : [...this.categories];

        this.totalCategories = res.count;
        this.totalPages = Math.ceil(res.count / this.pageSize);
        this.backendError = '';
      },
      (error) => {
        console.error('Error fetching categories:', error);
        this.filteredCategories = [];
      }
    );
  }

  filterCategories() {
    this.currentPage = 1;
    this.loadCategories();
  }

  resetSearch() {
    this.searchTerm = '';
    this.currentPage = 1;
    this.loadCategories();
  }

  editCategory(category: CategoryDto) {
    this.categoryForm = { ...category };
    this.showEditModal = true;
  }

  async deleteCategory(categoryId: string) {
    const result = await Swal.fire({
      title: 'هل أنت متأكد من حذف فئة؟',
      text: 'لا يمكن التراجع عن الحذف!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'نعم، احذف',
      cancelButtonText: 'إلغاء'
    });

    if (result.isConfirmed) {
      this.categoryService.deleteCategory(categoryId).subscribe(
        () => this.loadCategories(),
        (error) => {
          console.error('Error deleting category:', error);
          this.backendError = error?.error?.errorMessage || 'لا يمكن حذف هذه الفئة.';
        }
      );
    }
  }

  saveCategory() {
    if (!this.categoryForm.name.trim()) {
      Swal.fire('تنبيه', 'يرجى إدخال اسم الفئة', 'warning');
      return;
    }

    if (this.showEditModal) {
      const updatedCategory: UpdateCategoryDto = { name: this.categoryForm.name };
      this.categoryService.updateCategory(this.categoryForm.id, updatedCategory).subscribe(
        () => {
          this.loadCategories();
          this.closeModal();
        },
        (error) => console.error('Error updating category:', error)
      );
    } else {
      const newCategory: CreateCategoryDto = { name: this.categoryForm.name };
      this.categoryService.createCategory(newCategory).subscribe(
        () => {
          this.loadCategories();
          this.closeModal();
        },
        (error) => console.error('Error creating category:', error)
      );
    }
  }

  closeModal() {
    this.showCreateModal = false;
    this.showEditModal = false;
    this.categoryForm = { id: '', name: '' };
  }

  changePage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadCategories();
    }
  }

  getPageNumbers(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }
}
