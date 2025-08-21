export interface AuctionWonDto {
  id: string; 
  title: string;
  categoryName: string;
  winningBidAmount: number; 
  endTime: string; 
  isPaid: boolean;
  images: string[];
}
