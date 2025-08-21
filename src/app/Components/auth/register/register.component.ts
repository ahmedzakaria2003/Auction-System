import { AuthService } from './../../../Shared/Services/auth.service';
import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { RegisterDto } from '../../../Shared/Models/register.dto';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrls: ['../login/login.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class RegisterComponent implements OnInit {

  registerData: RegisterDto = {
    userName: '',
    email: '',
    password: '',
    confirmPassword: '',
    userType: '',
    phoneNumber: '',
    address: '',
    fullName: '',
    terms: false
  };

  backendErrors: { [key: string]: string } = {};

  patterns = {
    userName: /^[A-Za-z][A-Za-z0-9_]{2,}$/,
    email: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
    phone: /^\d{10,15}$/,
    password: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/
  };

  constructor(private authService: AuthService) {}

  ngOnInit() {
    console.log('Register component loaded');
  }

  onSubmit(form: NgForm) {
    this.backendErrors = {};

    if (form.invalid) {
      form.control.markAllAsTouched();
      return;
    }

    if (this.registerData.password !== this.registerData.confirmPassword) {
      this.backendErrors['confirmPassword'] = 'كلمة المرور وتأكيد كلمة المرور غير متطابقتين';
      return;
    }

    this.authService.register(this.registerData).subscribe({
      next: (response) => {
        console.log('تم التسجيل بنجاح', response);
        alert('تم إنشاء حسابك بنجاح!');
        form.resetForm();
      },
      error: (error) => {
        console.error('حدث خطأ أثناء التسجيل', error);

        const errMsg = error?.error?.errorMessage;
        if (errMsg) {
          const msgLower = errMsg.toLowerCase();

          if (msgLower.includes('username')) {
            this.backendErrors['userName'] = errMsg;
          } else if (msgLower.includes('email')) {
            this.backendErrors['email'] = errMsg;
          } else if (msgLower.includes('password')) {
            this.backendErrors['password'] = errMsg;
          } else {
            this.backendErrors['general'] = errMsg;
          }
        } else {
          this.backendErrors['general'] = 'فشل في التسجيل، حاول مرة أخرى.';
        }
      }
    });
  }

  isValidPattern(value: string, pattern: RegExp): boolean {
    return pattern.test(value);
  }
}
