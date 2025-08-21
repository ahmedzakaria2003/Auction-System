import { UserDto } from './../Models/user.dto';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, tap } from 'rxjs';
import { RegisterDto } from '../Models/register.dto'; 
import { environment } from '../../../environments/environment';
import { LoginDto } from '../Models/login.dto';
import { ResetPasswordDto } from '../Models/reset-password.dto';
import { JwtHelperService } from '@auth0/angular-jwt';
import { log } from 'console';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiUrl + '/api/Authentication';  
private jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient) {}

  register(registerDto: RegisterDto): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, registerDto);
  }

  login(loginDto: LoginDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, loginDto).pipe(
      tap((response) => {
        if (response?.accessToken) {
          localStorage.setItem('accessToken', response.accessToken);
        }
        if (response?.refreshToken) {
          localStorage.setItem('refreshToken', response.refreshToken);
        }
        if (response?.role) {
          localStorage.setItem('role', response.role);
        }
      })
    );
  }

  refreshToken(): Observable<any> {
    const token = this.getRefreshToken();
    return this.http.post<any>(`${this.apiUrl}/refresh-token`, token).pipe(
      tap(response => {
        if (response?.accessToken) {
          localStorage.setItem('accessToken', response.accessToken);
        }
        if (response?.refreshToken) {
          localStorage.setItem('refreshToken', response.refreshToken);
        }
      })
    );
  }

  sendOtp(userEmail: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/send-otp`, { email: userEmail }, { responseType: 'text' }).pipe(
      tap(response => {
        console.log('OTP sent response:', response);
      })
    );
  }

verifyOtp(userEmail: string, otp: string): Observable<string> {
  return this.http.post(`${this.apiUrl}/verify-otp`, { email: userEmail, otp: otp }, { responseType: 'text' }).pipe(
    tap(response => {
      console.log('OTP verification response:', response);
    }),
    map(response => {
      return response === 'OTP verified successfully' ? 'OTP verified successfully' : 'OTP verification failed';
    })
  );
}


resetPasswordWithOtp(dto: ResetPasswordDto): Observable<any> {
  return this.http.post(`${this.apiUrl}/reset-password`, dto, { responseType: 'text' }).pipe(
    tap(response => {
      console.log('Password reset response:', response);
    }),
    map(response => {
      if (response === 'Password reset successfully') {
        return true; 
      }
      return false; 
    })
  );
}

get isLoggedIn(): boolean {
  return !!this.getAccessToken(); 
}

  getAccessToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  getRefreshToken(): string | null {
    return localStorage.getItem('refreshToken');
  }

  getRole(): string | null {
    return localStorage.getItem('role');
  }

getUserName(): string | null {
  const token = this.getAccessToken();
  if (!token) return null;

  const decodedToken = this.jwtHelper.decodeToken(token);
  return decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] || null;
      
     
}


  logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('role');
  }
}
