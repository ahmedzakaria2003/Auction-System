import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../../Shared/Services/admin.service';
import { UserDto } from '../../../Shared/Models/user.dto';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserQueryDto } from '../../../Shared/Models/user-query.dto';
import { PaginatedResult } from '../../../Shared/Models/PaginatedResult';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  users: UserDto[] = [];
  selectedUserType = '';
  selectedStatus = '';
  searchTerm = '';
  currentPage = 1;
  pageSize = 10;
  totalPages = 1;
  totalUsers = 0;
  bannedUsers = 0;
  nonUsersMessage = '';

  showUserModal = false;
  showNotificationModal = false;
  selectedUser: UserDto | null = null;

  constructor(private adminService: AdminService) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    const queryParams: UserQueryDto = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize,
      search: this.searchTerm,
      status: this.selectedStatus,
      userType: this.selectedUserType
    };

    this.adminService.getAllUsers(queryParams).subscribe(
      (res: PaginatedResult<UserDto>) => {
        this.users = res.data;
        this.totalUsers = res.count;
        this.totalPages = Math.ceil(res.count / this.pageSize);
        this.bannedUsers = this.users.filter(u => u.isBanned).length;
        this.nonUsersMessage = this.users.length === 0
          ? 'لا يوجد مستخدمين مطابقين للفلتر الحالي.'
          : '';
      },
      err => {
        console.error('Error loading users:', err);
        this.nonUsersMessage = 'حدث خطأ أثناء تحميل المستخدمين.';
      }
    );
  }

  filterUsers() {
    this.currentPage = 1;
    this.loadUsers();
  }

  resetFilters() {
    this.selectedUserType = '';
    this.selectedStatus = '';
    this.searchTerm = '';
    this.currentPage = 1;
    this.loadUsers();
  }

  getUserTypeClass(userType?: string): string {
    switch (userType) {
      case 'Bidder': return 'type-bidder';
      case 'Seller': return 'type-seller';
      default: return '';
    }
  }

  getUserTypeText(userType?: string): string {
    switch (userType) {
      case 'Bidder': return 'مزايد';
      case 'Seller': return 'بائع';
      default: return 'غير محدد';
    }
  }

  viewUserDetails(userId: string) {
    this.selectedUser = this.users.find(u => u.userId === userId) || null;
    this.showUserModal = true;
  }

  async toggleUserBan(user: UserDto) {
    const action = user.isBanned ? 'إلغاء حظر' : 'حظر';
    const result = await Swal.fire({
      title: `هل أنت متأكد من ${action} المستخدم ${user.fullName}?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'نعم',
      cancelButtonText: 'إلغاء'
    });

    if (!result.isConfirmed) return;

    const serviceCall = user.isBanned
      ? this.adminService.unbanUser(user.userId)
      : this.adminService.banUser(user.userId);

    serviceCall.subscribe(
      () => {
        this.loadUsers();
        user.isBanned = !user.isBanned;
        this.bannedUsers = this.users.filter(u => u.isBanned).length;
        this.nonUsersMessage = this.users.length === 0
          ? 'لا يوجد مستخدمين مطابقين للفلتر الحالي.'
          : '';
      },
      err => console.error('Error toggling ban:', err)
    );
  }

  changePage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadUsers();
    }
  }

  getPageNumbers(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }
}
