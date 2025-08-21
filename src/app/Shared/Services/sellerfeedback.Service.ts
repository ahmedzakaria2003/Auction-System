import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { SellerFeedbackDto } from '../Models/seller-feedback.dto';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class SellerFeedbackService {
  private baseUrl = environment.apiUrl + '/api/SellerFeedback'; 

  constructor(private http: HttpClient , private authService: AuthService) {}

   private createHeaders(): HttpHeaders {
     const token = this.authService.getAccessToken(); 
     return token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
   }
  addFeedback(feedback: SellerFeedbackDto): Observable<void> {
    const headers = this.createHeaders();
    return this.http.post<void>(`${this.baseUrl}`, feedback , {headers});
  }

 
  getFeedbacksForSeller(sellerId: string): Observable<SellerFeedbackDto[]> {
        const headers = this.createHeaders();

    return this.http.get<SellerFeedbackDto[]>(`${this.baseUrl}/seller/${sellerId}`,{headers});
  }

getAverageRatingForSeller(sellerId: string): Observable<number> {
    const headers = this.createHeaders();
    return this.http
        .get<{ averageRating: number }>(`${this.baseUrl}/seller/${sellerId}/average-rating`, { headers })
        .pipe(
            map(response => response.averageRating)
        );
}

hasUserRatedAuction(auctionId: string): Observable<{ hasRated: boolean }> {
  const headers = this.createHeaders();
  return this.http.get<{ hasRated: boolean }>(
    `${this.baseUrl}/auction/${auctionId}/has-rated`, 
    { headers }
  );
}

}
