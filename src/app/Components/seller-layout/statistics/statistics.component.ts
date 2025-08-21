import { Component } from '@angular/core';
import { AuctionStatisticsDto } from '../../../Shared/Models/auction-statistics.dto';
import { SellerService } from '../../../Shared/Services/seller.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-statistics',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './statistics.component.html',
  styleUrl: './statistics.component.css'
})
export class SellerStatisticsComponent {

    statistics: AuctionStatisticsDto | undefined; 
  
    constructor(private sellerservice: SellerService) {}
  
    ngOnInit() {
      this.sellerservice.getSellerStatistics().subscribe(
        (data: AuctionStatisticsDto) => {
          this.statistics = data;
        },
        (error) => {
          console.error('Error fetching statistics:', error);
        }
      );
    }
}
