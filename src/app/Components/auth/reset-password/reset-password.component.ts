import { Component } from '@angular/core';
import { AuthService } from '../../../Shared/Services/auth.service';
import { ResetPasswordDto } from '../../../Shared/Models/reset-password.dto';
import { RouterLink, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, RouterLink],
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css'],
})
export class ResetPasswordComponent {
  email: string = '';
  otp: string = ''; 
  newPassword: string = '';
  currentStep: number = 1;
  errorMessage: string = '';
  emailRegex: RegExp = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
  passwordFieldType: string = 'password';

  get isOtpComplete(): boolean {
    return this.otp.length === 6; 
  }

  constructor(private authService: AuthService) {}

  validateEmail() {
    if (this.email && !this.emailRegex.test(this.email)) {
      this.errorMessage = 'البريد الإلكتروني غير صالح';
    } else {
      this.errorMessage = '';
    }
  }

  togglePasswordVisibility() {
    this.passwordFieldType = this.passwordFieldType === 'password' ? 'text' : 'password';
  }

  nextStep() {
    switch (this.currentStep) {
      case 1:
        if (!this.email) {
          this.errorMessage = 'يرجى إدخال البريد الإلكتروني';
          return;
        }

        this.authService.sendOtp(this.email).subscribe(
          (response) => {
            this.errorMessage = '';
            this.currentStep = 2;
          },
          (error) => {
            this.errorMessage = error.error?.errorMessage || 'البريد الإلكتروني غير مسجل';
            console.error('Error sending OTP', error);
          }
        );
        break;

      case 2:
        if (!this.otp || this.otp.length !== 6) {
          this.errorMessage = 'يرجى إدخال رمز التحقق المكون من 6 خانات';
          return;
        }

        this.authService.verifyOtp(this.email, this.otp).subscribe(
          (response) => {
            if (response === 'OTP verified successfully') {
              this.errorMessage = '';
              this.currentStep = 3;
            } else {
              this.errorMessage = 'رمز التحقق غير صحيح';
              this.currentStep = 2;
            }
          },
          (error) => {
            this.errorMessage = error.error?.errorMessage || 'رمز التحقق غير صحيح';
            console.error('Error verifying OTP', error);
          }
        );
        break;

      case 3:
        if (!this.newPassword) {
          this.errorMessage = 'يرجى إدخال كلمة المرور';
          return;
        }

        const resetDto: ResetPasswordDto = {
          email: this.email,
          otp: this.otp,
          newPassword: this.newPassword,
        };

        this.authService.resetPasswordWithOtp(resetDto).subscribe(
          (response) => {
            if (response) {
              this.errorMessage = '';
              this.currentStep = 4;
            } else {
              this.errorMessage = 'كلمه المرور  يجب ان تحتوي على حرف كبير وحرف صغير ورقم ورمز خاص';
            }
          },
          (error) => {
this.errorMessage = error.error?.errorMessage || 'يجب أن تحتوي كلمة المرور على حرف كبير، حرف صغير، رقم، ورمز خاص.';
            console.error('Error resetting password', error);
          }
        );
        break;

      default:
        break;
    }
  }
}
