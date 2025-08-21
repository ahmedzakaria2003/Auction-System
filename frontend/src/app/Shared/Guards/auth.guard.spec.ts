import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { AuthGuard } from './auth.guard'; 

describe('AuthGuard', () => {   // صحيت الاسم هنا كمان
  let guard: AuthGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(AuthGuard);  // هنا بنعمل inject للـ AuthGuard
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
