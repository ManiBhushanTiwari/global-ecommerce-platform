import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PaymentService } from '../services/payment.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-payment-status',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './payment-status.component.html',
  styleUrls: ['./payment-status.component.css']
})
export class PaymentStatusComponent implements OnInit {
  status: string = 'Processing...';
  trackingInfo: any; 

  constructor(private route: ActivatedRoute, private paymentService: PaymentService) {}

  ngOnInit() {
    const orderId = this.route.snapshot.queryParamMap.get('orderId');
    if (orderId) {
      this.paymentService.getPaymentStatus(orderId).subscribe({
        next: res => {
          this.status = res.status;
          this.trackingInfo = res.trackingInfo; // only if backend returns shipping info
        },
        error: () => this.status = 'Failed'
      });
    }
  }
}
