import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private apiUrl = 'http://localhost:5071/api/Payments'; // adjust to your backend route

  constructor(private http: HttpClient) {}

  // Call backend to start payment (Stripe/PayPal)
  startPayment(orderId: number, amount: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/paypal`, { orderId, amount });
  }

  // Optional: capture payment after approval
  capturePayment(orderId: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/paypal//capture`, { orderId });
  }
  // PaymentService.ts
getPaymentStatus(orderId: string): Observable<any> {
  return this.http.get<any>(`${this.apiUrl}/status/${orderId}`);
}

}
