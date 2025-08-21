export interface CategoryAuctionStatsDto {
  category: string;
  auctionCount: number; 
  totalRevenue: number; 
}

// BidderStatsDto
export interface BidderStatsDto {
  bidderName: string; 
  bidderId: string; 
  totalBids: number; 
  totalAmountSpent: number; 
}

// MostBidAuctionDto
export interface MostBidAuctionDto {
  title: string; 
  count: number; 
}

// AuctionStatisticsDto
export interface AuctionStatisticsDto {
  totalAuctions: number; 
  totalAuctionsPaid: number; 
  totalAuctionsUnPaid: number; 
  totalBids: number; 
  totalRevenue: number; 
  totalCanceled: number; 
  openAuctions: number; 
  closedAuctions: number; 
  auctionsByCategory: CategoryAuctionStatsDto[]; 
  biddersStats: BidderStatsDto; 
  mostBidAuction: MostBidAuctionDto; 
}
