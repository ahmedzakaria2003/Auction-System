 export interface AuctionListDto {
  id: string;
  title: string;
  startingPrice: number;
  endTime: string;
  categoryName: string;
  isCanceled: boolean;
  sellerName: string;
  bidsCount: number;
  description?: string;
  startTime?: string;
  thumbnailImage: string[];
  auctionStatus: string;

}