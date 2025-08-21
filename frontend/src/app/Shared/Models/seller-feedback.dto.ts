export interface SellerFeedbackDto {
  sellerId?: string;      
  auctionId: string;    
  rating: number;        
  comment: string;      
  createdAt?: string;  
   hasRated?: boolean;   
}


export interface AddFeedbackRequest {
  auctionId: string;
  rating: number;
  comment: string;
}
