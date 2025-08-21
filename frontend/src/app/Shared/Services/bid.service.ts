import { AuthService } from './auth.service';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { BidDto } from '../Models/auction-details.dto';
import { AddBidDto } from '../Models/add-bid.dto';

@Injectable({
  providedIn: 'root'
})
export class BidService {
  private baseUrl = environment.apiUrl + `/api/Bid`;

  constructor(private http: HttpClient , private authService: AuthService) {}

  private createHeaders(): HttpHeaders {
    const token = this.authService.getAccessToken();
    return token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
  }

  placeBid(bidDto: AddBidDto): Observable<{ message: string; bidId: string }> {
    const headers = this.createHeaders();
    return this.http.post<{ message: string; bidId: string }>(`${this.baseUrl}/place-bid`, bidDto,{headers});
  }

  getAuctionBids(auctionId: string): Observable<BidDto[]> {
    const headers = this.createHeaders();
    return this.http.get<BidDto[]>(`${this.baseUrl}/history/${auctionId}`,{headers});
  }

  getHighestBid(auctionId: string): Observable<BidDto> {
    const headers = this.createHeaders();
    return this.http.get<BidDto>(`${this.baseUrl}/highest/${auctionId}`,{headers});
  }
}
