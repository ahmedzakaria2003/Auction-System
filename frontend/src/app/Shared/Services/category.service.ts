import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CategoryDto } from '../Models/category.dto';
import { CreateCategoryDto } from '../Models/create-category.dto';
import { UpdateCategoryDto } from '../Models/update-category.dto';
import { AuthService } from './auth.service'; 
import { AuctionQueryDto } from '../Models/auction-query.dto';
import { PaginatedResult } from '../Models/PaginatedResult';
import { CategoryWithAuctionsResponse } from '../Models/category-with-auctions.dto';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  private apiUrl = environment.apiUrl + `/api/Category`;

  constructor(private http: HttpClient, private authService: AuthService) {} 

  private createHeaders(): HttpHeaders {
    const token = this.authService.getAccessToken(); 
    return token ? new HttpHeaders().set('Authorization', `Bearer ${token}`) : new HttpHeaders();
  }

  getAllCategories(queryParams : AuctionQueryDto): Observable<PaginatedResult<CategoryDto>> {

          let params = new HttpParams();
    
      Object.entries(queryParams).forEach(([key, value]) => {
        if (value !== null && value !== undefined && value !== '') {
          params = params.set(key, value.toString());
        }
      });
    return this.http.get<PaginatedResult<CategoryDto>>(`${this.apiUrl}` , { params });
  }
  GetAllCategoriesForDropdown(): Observable<CategoryDto[]> {
    return this.http.get<CategoryDto[]>(`${this.apiUrl}/dropdown`);
  }

getCategoryWithAuctions(categoryId: string, queryParams: AuctionQueryDto): Observable<CategoryWithAuctionsResponse> {
  let params = new HttpParams();
  Object.entries(queryParams).forEach(([key, value]) => {
    if (value !== null && value !== undefined && value !== '') {
      params = params.set(key, value.toString());
    }
  });
  return this.http.get<CategoryWithAuctionsResponse>(`${this.apiUrl}/with-active-auctions/${categoryId}`, { params });
}
  createCategory(dto: CreateCategoryDto): Observable<any> {
    const headers = this.createHeaders(); 
    return this.http.post<any>(`${this.apiUrl}`, dto, { headers });
  }

  updateCategory(categoryId: string, dto: UpdateCategoryDto): Observable<any> {
    const headers = this.createHeaders(); 
    return this.http.put<any>(`${this.apiUrl}/${categoryId}`, dto, { headers });
  }

  deleteCategory(categoryId: string): Observable<any> {
    const headers = this.createHeaders(); 
    return this.http.delete<any>(`${this.apiUrl}/${categoryId}`, { headers });
  }
}
