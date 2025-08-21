export interface AuctionDetailsDto {
  id: string;
  title: string;
  description: string;
  startingPrice: number;
  finalPrice: number;
  startTime: string;
  endTime: string;
  categoryName: string;
  sellerName: string;
  totalBids: number;
  winnerName: string;
  bids: BidDto[];
  highestBidAmount: number;
  highestBidderName?: string;
  itemImageUrls: string[];
  auctionStatus: string;
  sellerId: string;
}

export  interface BidDto {
  id?: string;
  auctionId?: string;
  bidderId: string;
  amount: number;
  bidTime: string;
  bidderName: string;
}