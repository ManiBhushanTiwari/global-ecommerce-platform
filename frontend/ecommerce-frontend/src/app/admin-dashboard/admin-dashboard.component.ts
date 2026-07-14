import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { AnalyticsService } from '../services/analytics.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, HttpClientModule, NgxChartsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {
  ordersPerDay: any[] = [];
  conversionRates: any[] = [];
  failedPayments: number = 0;
  view: [number, number] = [700, 300]; // chart size

  constructor(private analyticsService: AnalyticsService) {}

ngOnInit() {
  this.analyticsService.getOrdersPerDay().subscribe({
    next: (data: any[]) => {
      console.log('API response:', data);

      this.ordersPerDay = [
        {
          name: 'Orders',
          series: (data || []).map(d => ({
            name: new Date(d.date).toLocaleDateString('en-GB'),
            value: d.count
          }))
        }
      ];

      console.log('Chart data:', this.ordersPerDay);
    },
    error: () => this.ordersPerDay = []
  });
   this.analyticsService.getConversionRates().subscribe({
      next: (data: any[]) => {
        this.conversionRates = (data || []).map(d => ({
          name: d.metric,
          value: d.value
        }));
      },
      error: () => this.conversionRates = []
    });

    // Failed payments
    this.analyticsService.getFailedPayments().subscribe({
      next: (count: number) => this.failedPayments = count,
      error: () => this.failedPayments = 0
    });
}


}
