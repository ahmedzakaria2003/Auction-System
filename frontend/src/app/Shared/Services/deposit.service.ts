import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PaymentIntentDTO } from '../Models/paymentintent.dto';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class DepositService {

  private baseUrl = environment.apiUrl + '/api/Deposit'; 

  constructor(private http: HttpClient, private authService: AuthService) {}

  private createHeaders(): HttpHeaders {
    const token = this.authService.getAccessToken(); 
    return token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
  }

  createDepositIntent(auctionId: string): Observable<PaymentIntentDTO> {
    const headers = this.createHeaders();
    return this.http.post<PaymentIntentDTO>(
      `${this.baseUrl}/create-intent/${auctionId}`,
      {}, 
      { headers }
    );
  }

  hasPaidDeposit(auctionId: string): Observable<{ hasPaid: boolean }> {
    const headers = this.createHeaders();
    return this.http.get<{ hasPaid: boolean }>(
      `${this.baseUrl}/has-paid?auctionId=${auctionId}`,
      { headers }
    );
  }

  confirmDepositIntent(paymentIntentId: string): Observable<any> {
    const headers = this.createHeaders();
    return this.http.post(
      `${this.baseUrl}/confirm-intent/${paymentIntentId}`,
      {}, 
      { headers }
    );
  }
}
