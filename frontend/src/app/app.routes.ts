import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { AuthGuard } from './Shared/Guards/auth.guard';
import { RoleGuard } from './Shared/Guards/role.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'bidder/home', pathMatch: 'full' },
  { path: 'login', loadComponent: () => import('./Components/auth/login/login.component').then(m => m.LoginComponent) },
  { path: 'register', loadComponent: () => import('./Components/auth/register/register.component').then(m => m.RegisterComponent) },
  { path: 'login/reset-password', loadComponent: () => import('./Components/auth/reset-password/reset-password.component').then(m => m.ResetPasswordComponent) },
  { path: 'privacy', loadComponent: () => import('./Components/auth/privacy-policy/privacy-policy.component').then(m => m.PrivacyPolicyComponent) },
  { path: 'not-found', loadComponent: () => import('./Shared/not-found/not-found.component').then(m => m.NotFoundComponent) },

  // Bidder routes
  {
    path: 'bidder',
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'Bidder' },
    children: [
      { path: 'home', loadComponent: () => import('./Components/bidder-layout/home/home.component').then(m => m.HomeComponent) },
      { path: 'categories/:id', loadComponent: () => import('./Components/bidder-layout/category-with-auctions/category-with-auctions.component').then(m => m.CategoryWithAuctionsComponent) },
      { path: 'auction/:id', loadComponent: () => import('./Components/bidder-layout/auction-details/auction-details.component').then(m => m.AuctionDetailsComponent) },
      { path: 'my-bids', loadComponent: () => import('./Components/bidder-layout/my-bids/my-bids.component').then(m => m.MyBidsComponent) },
      { path: 'faq', loadComponent: () => import('./Components/bidder-layout/faq/faq.component').then(m => m.FAQComponent) },
      { path: 'won-auctions', loadComponent: () => import('./Components/bidder-layout/won-auctions/won-auctions.component').then(m => m.WonAuctionsComponent) }
    ]
  },

  // Admin routes
  {
    path: 'layout',
    loadComponent: () => import('./Components/admin-layout/layout/layout.component').then(m => m.LayoutComponent),
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'Admin' },
    children: [
      { path: '', redirectTo: 'statistics', pathMatch: 'full' },
      { path: 'auction-management', loadComponent: () => import('./Components/admin-layout/auction-management/auction-management.component').then(m => m.AuctionManagementComponent) },
      { path: 'reports', loadComponent: () => import('./Components/admin-layout/reports/reports.component').then(m => m.ReportsComponent) },
      { path: 'statistics', loadComponent: () => import('./Components/admin-layout/statistics/statistics.component').then(m => m.StatisticsComponent) },
      { path: 'users', loadComponent: () => import('./Components/admin-layout/user-management/user-management.component').then(m => m.UserManagementComponent) },
      { path: 'my-auctions', loadComponent: () => import('./Components/admin-layout/myauctions/myauctions.component').then(m => m.MyauctionsComponent) },
      { path: 'categories', loadComponent: () => import('./Components/admin-layout/categories/categories.component').then(m => m.CategoriesComponent) }
    ]
  },

  // Seller routes
  {
    path: 'seller-layout',
    loadComponent: () => import('./Components/seller-layout/seller-layout/layout.component').then(m => m.SellerLayoutComponent),
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'Seller' },
    children: [
      { path: '', redirectTo: 'seller-statistics', pathMatch: 'full' },
      { path: 'seller-reports', loadComponent: () => import('./Components/seller-layout/reports/reports.component').then(m => m.SellerReportsComponent) },
      { path: 'seller-statistics', loadComponent: () => import('./Components/seller-layout/statistics/statistics.component').then(m => m.SellerStatisticsComponent) },
      { path: 'seller-auctions', loadComponent: () => import('./Components/seller-layout/my-auctions/my-auctions.component').then(m => m.SellerAuctionsComponent) }
    ]
  },

  { path: '**', redirectTo: '/not-found' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'enabled', anchorScrolling: 'enabled' })],
  exports: [RouterModule]
})
export class AppRoutingModule {}
