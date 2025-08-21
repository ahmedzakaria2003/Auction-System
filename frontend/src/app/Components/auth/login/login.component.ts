import { CommonModule } from '@angular/common';
import { Component, ViewEncapsulation } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';  
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../Shared/Services/auth.service';
import { LoginDto } from '../../../Shared/Models/login.dto';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterLink],  
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class LoginComponent {
  
  loginData: LoginDto = {
    email: '',
    password: '',
    terms: false
  };

  backendError: string | null = null;
  showEmailMessage: boolean = false;
  showPasswordMessage: boolean = false;

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(form: NgForm) {
    this.backendError = null;

    if (form.invalid) {
      this.showEmailMessage = !this.loginData.email;
      this.showPasswordMessage = !this.loginData.password;
      form.control.markAllAsTouched();
      return;
    }

    this.authService.login(this.loginData).subscribe({
      next: (response) => {
        const accessToken = this.authService.getAccessToken();
        const refreshToken = this.authService.getRefreshToken();
        const role = this.authService.getRole();

        console.log('Access Token:', accessToken);
        console.log('Refresh Token:', refreshToken);

        if (role === 'Admin') {
          this.router.navigate(['/layout']);
        } else if (role === 'Seller') {
          this.router.navigate(['/seller-layout']);
        } else if (role === 'Bidder') {
          this.router.navigate(['/bidder/home']);
        } else {
          this.router.navigate(['/']);
        }
      },
      error: (error) => {
        console.error('حدث خطأ أثناء تسجيل الدخول', error);
        this.backendError = error?.error?.errorMessage || 'فشل في تسجيل الدخول، حاول مرة أخرى.';
      }
    });
  }

  onFocusField(field: string) {
    if (field === 'email') {
      this.showEmailMessage = false;
    } else if (field === 'password') {
      this.showPasswordMessage = false;
    }
  }
}

