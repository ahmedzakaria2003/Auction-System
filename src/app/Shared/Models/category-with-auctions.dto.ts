import { AuctionListDto } from "./auction-list.dto";

export interface CategoryWithAuctionsResponse {
  pageNumber: number;
  pageSize: number;
  count: number;
  data: CategoryWithAuctionsDto[];
}

export interface CategoryWithAuctionsDto {
  id: string;
  name: string;
  pagedAuctions: PaginatedResult;
}

export interface PaginatedResult {
  pageNumber: number;
  pageSize: number;
  count: number;
  data: AuctionListDto[];
}