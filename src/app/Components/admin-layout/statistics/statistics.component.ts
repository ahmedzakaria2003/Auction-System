import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuctionStatisticsDto } from '../../../Shared/Models/auction-statistics.dto';
import { AdminService } from '../../../Shared/Services/admin.service';

@Component({
  selector: 'app-statistics',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.css']
})
export class StatisticsComponent implements OnInit {
  statistics: AuctionStatisticsDto | undefined; 

  constructor(private adminService: AdminService) {}

  ngOnInit() {
    this.adminService.getAdminStatistics().subscribe(
      (data: AuctionStatisticsDto) => {
        this.statistics = data; 
      },
      (error) => {
        console.error('Error fetching statistics:', error);
      }
    );
  }
}
