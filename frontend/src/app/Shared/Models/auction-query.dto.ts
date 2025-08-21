export interface AuctionQueryDto {
  search?: string;
  status?: string;
  categoryId?: string;  
  pageNumber?: number;
  pageSize?: number;
  sort?: number; 
  maxPrice?: number| null;
  minPrice?: number| null;
  isEndingSoon?: boolean;
  startDate?:Date;
  endDate?:Date;
}
