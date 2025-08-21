 export interface ActiveBiddingDto {
  id: string;
  title: string;
  categoryName: string;
  currentHighestBid: number;
  yourBid: number;
  endTime: string;
  images: string[];
}