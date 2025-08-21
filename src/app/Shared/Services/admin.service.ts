import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { UserDto } from '../Models/user.dto';
import { AuctionStatisticsDto } from '../Models/auction-statistics.dto';
import { AuctionListDto } from '../Models/auction-list.dto';
import { AuthService } from './auth.service';
import { PaginatedResult } from '../Models/PaginatedResult';
import { AuctionQueryDto } from '../Models/auction-query.dto';
import { UserQueryDto } from '../Models/user-query.dto';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private apiUrl = environment.apiUrl + '/api/Admin';  

  constructor(private http: HttpClient, private authService: AuthService) {}

  private createHeaders(): HttpHeaders {
    const token = this.authService.getAccessToken();  
    return token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
  }

  getAdminStatistics(): Observable<AuctionStatisticsDto> {
    const headers = this.createHeaders(); 
    return this.http.get<AuctionStatisticsDto>(`${this.apiUrl}/auctions-summary`, { headers });
  }

  getSellersAuctions(queryParams: AuctionQueryDto): Observable<PaginatedResult<AuctionListDto>>{
    const headers = this.createHeaders(); 
      let params = new HttpParams();
    
      Object.entries(queryParams).forEach(([key, value]) => {
        if (value !== null && value !== undefined && value !== '') {
          params = params.set(key, value.toString());
        }
      });

        return this.http.get<PaginatedResult<AuctionListDto>>(`${this.apiUrl}/seller-auctions-management`
          , { headers, params });

  }



  getAllUsers(queryParams : UserQueryDto): Observable<PaginatedResult<UserDto>> {
    const headers = this.createHeaders();
         let params = new HttpParams();
    
      Object.entries(queryParams).forEach(([key, value]) => {
        if (value !== null && value !== undefined && value !== '') {
          params = params.set(key, value.toString());
        }
      });
    return this.http.get<PaginatedResult<UserDto>>(`${this.apiUrl}/all-users`, { headers , params});
  }

  banUser(userId: string): Observable<any> {
    const headers = this.createHeaders();
    return this.http.post(`${this.apiUrl}/ban-user/${userId}`, {}, { headers });
  }

  unbanUser(userId: string): Observable<any> {
    const headers = this.createHeaders(); 
    return this.http.post(`${this.apiUrl}/unban-user/${userId}`, {}, { headers });
  }

}
