import { Component, OnInit } from '@angular/core';
import { CartService } from '../services/cart.service';
import { Product } from '../services/product.service';
import { OrderService } from '../services/order.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { PaymentService } from '../services/payment.service';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule,FormsModule,HttpClientModule],
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  items: Product[] = [];
  total: number = 0;
  customer = { name: '', email: '', address: '' };

  constructor(
    private cartService: CartService,
    private orderService: OrderService,
    private paymentService: PaymentService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.items = this.cartService.getItems();
    this.total = this.cartService.getTotal();
  }

 placeOrder() {
  const currentUserId = this.authService.getUserId();

  const orderPayload = {
    userId: currentUserId,
    items: this.items.map(i => ({
      productId: i.id,
      quantity: this.cartService.getQuantity(i.id)
    })),
    totalAmount: this.total,
    status: "Pending",
    shippingAddress: this.customer.address,
  customerName: this.customer.name,
  customerEmail: this.customer.email
  };

  console.log('Payload being sent:', orderPayload);

  // ✅ Single order
  this.orderService.placeOrder(orderPayload).subscribe({
    next: (response) => {
      alert(`Order placed successfully! Order ID: ${response.id}`);
      this.cartService.clearCart();
      this.paymentService.startPayment(response.id, response.totalAmount)
        .subscribe(session => {
          window.location.href = session.url;
        });
    },
    error: (err) => {
      console.error('Order failed', err);
      alert('Failed to place order.');
    }
  });
}
}
