import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ActiveBiddingDto } from '../Models/active-bidding.dto';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';
import { AuctionWonDto } from '../Models/AuctionWon.dto';

@Injectable({
  providedIn: 'root'
})

export class ProfileService {

  private apiUrl = environment.apiUrl + '/api/Profile';

 constructor(private http: HttpClient, private authService: AuthService) { }

private createHeaders(): HttpHeaders {
  const token = this.authService.getAccessToken(); 
  return new HttpHeaders({
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  });
}


  getActiveBids(): Observable<ActiveBiddingDto[]> {
    const headers = this.createHeaders();
    return this.http.get<ActiveBiddingDto[]>(`${this.apiUrl}/active-bids`, { headers });
  }

    getWonAuctions(): Observable<AuctionWonDto[]> {
            const headers = this.createHeaders();
    return this.http.get<AuctionWonDto[]>(`${this.apiUrl}/won-auctions` , { headers });
  }
}
