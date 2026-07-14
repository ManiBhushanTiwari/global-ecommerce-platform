import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ShippingService } from '../services/shipping.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-order-tracking',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-tracking.component.html',
  styleUrls: ['./order-tracking.component.css']
})
export class OrderTrackingComponent implements OnInit {
  trackingInfo: any;

  constructor(private route: ActivatedRoute, private shippingService: ShippingService) {}

  ngOnInit() {
    const orderId = this.route.snapshot.paramMap.get('id');
    if (orderId) {
      this.shippingService.getTracking(orderId).subscribe({
        next: res => this.trackingInfo = res,
        error: () => this.trackingInfo = null
      });
    }
  }
}
