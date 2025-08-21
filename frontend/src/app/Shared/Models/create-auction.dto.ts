export interface CreateAuctionDto {
  Title: string;                  
  Description: string;            
  StartingPrice: number;         
  StartTime: Date;               
  EndTime: Date;                 
  CategoryId: string;             
  Images: File[];               
}