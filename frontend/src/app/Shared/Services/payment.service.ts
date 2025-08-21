import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';

export interface PaymentIntentDTO {
  clientSecret: string;
  paymentIntentId: string;
}

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  private baseUrl = environment.apiUrl + '/api/Payment';

 constructor(private http: HttpClient, private authService: AuthService) { }

private createHeaders(): HttpHeaders {
  const token = this.authService.getAccessToken(); 
  return new HttpHeaders({
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  });
}


  createPaymentIntentForAuction(auctionId: string): Observable<PaymentIntentDTO> {
        const headers = this.createHeaders();

    return this.http.post<PaymentIntentDTO>(`${this.baseUrl}/create-intent/${auctionId}`, {},{headers});
  }

  confirmPaymentIntent(paymentIntentId: string): Observable<{ success: boolean }> {
            const headers = this.createHeaders();

    return this.http.post<{ success: boolean }>(`${this.baseUrl}/confirm-intent/${paymentIntentId}`, {},{headers});
  }


}
