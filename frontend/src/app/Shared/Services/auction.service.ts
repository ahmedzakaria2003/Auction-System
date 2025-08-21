import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuctionListDto } from '../Models/auction-list.dto';
import { AuctionDetailsDto } from '../Models/auction-details.dto';
import { AuthService } from './auth.service'; 
import { UpdateAuctionDto } from '../Models/update-auction.dto';
import { CreateAuctionDto } from '../Models/create-auction.dto';
import { WinnerDto } from '../Models/winner.dto';
import { AuctionQueryDto } from '../Models/auction-query.dto';
import { PaginatedResult } from '../Models/PaginatedResult';

@Injectable({
  providedIn: 'root'
})
export class AuctionService {
    private apiUrl = environment.apiUrl + '/api/Auction';  
  constructor(private http: HttpClient, private authService: AuthService) {}
  private createHeaders(): HttpHeaders {
    const token = this.authService.getAccessToken(); 
    return token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
  }
createAuction(formData: FormData): Observable<AuctionListDto> {
    const headers = this.createHeaders();
    return this.http.post<AuctionListDto>(`${this.apiUrl}`, formData , {headers});
  }

updateAuction(auctionId: string, dto: FormData): Observable<boolean> {
  const headers = this.createHeaders();
  return this.http.put<boolean>(`${this.apiUrl}/${auctionId}`, dto, { headers });
}


  deleteAuction(auctionId: string): Observable<boolean> {
    const headers = this.createHeaders(); 
    return this.http.delete<boolean>(`${this.apiUrl}/${auctionId}`, { headers });
  }

    cancelAuction(auctionId: string): Observable<any> {
      const headers = this.createHeaders(); 
      return this.http.put(`${this.apiUrl}/canceled/${auctionId}`, {}, { headers });
    }
  getAuctionDetails(auctionId: string): Observable<AuctionDetailsDto> {
    const headers = this.createHeaders(); 
    return this.http.get<AuctionDetailsDto>(`${this.apiUrl}/details/${auctionId}`, { headers });
  }

  getAllAuctions(queryParams: AuctionQueryDto): Observable<PaginatedResult<AuctionListDto>> {
    const headers = this.createHeaders(); 
      let params = new HttpParams();

  Object.entries(queryParams).forEach(([key, value]) => {
    if (value !== null && value !== undefined && value !== '') {
      params = params.set(key, value.toString());
    }
  });
    return this.http.get<PaginatedResult<AuctionListDto>>(`${this.apiUrl}/all-auctions`, { headers, params });
  }

getAuctionsByCreator(queryParams: AuctionQueryDto): Observable<PaginatedResult<AuctionListDto>> {
  const headers = this.createHeaders();

  let params = new HttpParams();

  Object.entries(queryParams).forEach(([key, value]) => {
    if (value !== null && value !== undefined && value !== '') {
      params = params.set(key, value.toString());
    }
  });

  return this.http.get<PaginatedResult<AuctionListDto>>(`${this.apiUrl}/my-auctions/`, { headers, params });
}


  getHotAuctions(take: number = 10): Observable<AuctionListDto[]> {
    const headers = this.createHeaders(); 
    return this.http.get<AuctionListDto[]>(`${this.apiUrl}/hot-auctions`, { headers, params: { take: take.toString() } });
  }

  getRecommendedAuctionsForBidder(): Observable<AuctionListDto[]> {
    const headers = this.createHeaders(); 
    return this.http.get<AuctionListDto[]>(`${this.apiUrl}/recommended-auctions`, { headers });
  }


declareWinner(auctionId: string): Observable<WinnerDto> {
    const headers = this.createHeaders();
    return this.http.post<WinnerDto>(`${this.apiUrl}/declare-winner/${auctionId}`, {}, { headers });
  }




}
