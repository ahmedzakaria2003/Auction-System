import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AuthService } from "./auth.service";
import { Observable } from "rxjs";
import { AuctionStatisticsDto } from "../Models/auction-statistics.dto";



@Injectable({
  providedIn: 'root'
})
export class SellerService {




 private apiUrl = environment.apiUrl + '/api/Seller';  

  constructor(private http: HttpClient, private authService: AuthService) {}

  private createHeaders(): HttpHeaders {
    const token = this.authService.getAccessToken();  
    return token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
  }

  getSellerStatistics(): Observable<AuctionStatisticsDto> {
    const headers = this.createHeaders(); 
    return this.http.get<AuctionStatisticsDto>(`${this.apiUrl}/auctions-summary`, { headers });
  }


}