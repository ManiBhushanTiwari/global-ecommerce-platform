import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PaymentService } from '../services/payment.service';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [],
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent implements OnInit {

  orderId!: number;
  totalAmount!: number;

  constructor(
    private route: ActivatedRoute,
    private paymentService: PaymentService
  ) {}

  ngOnInit() {
    // Get orderId and totalAmount from route or state
    this.orderId = Number(this.route.snapshot.paramMap.get('orderId'));
    this.totalAmount = Number(this.route.snapshot.queryParamMap.get('amount'));

    // Call backend to start payment
    this.paymentService.startPayment(this.orderId, this.totalAmount)
      .subscribe(res => {
        // Redirect user to Stripe/PayPal checkout page
        window.location.href = res.paymentUrl;
      });
  }
}
