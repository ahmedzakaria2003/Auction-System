import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AuthService } from '../Services/auth.service';

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

canActivate(route: ActivatedRouteSnapshot): boolean {
  const expectedRole = route.data['role'];
  const userRole = this.authService.getRole();
  console.log('Expected role:', expectedRole);
  console.log('User role:', userRole);

  if (userRole === expectedRole) {
    return true;
  }
  this.router.navigate(['/not-found']);
  return false;
}

}