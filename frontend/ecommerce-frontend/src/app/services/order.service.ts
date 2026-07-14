import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private apiUrl = `${environment.apiUrl}/orders`;

  constructor(private http: HttpClient) {}

  // ✅ Handles both single and multiple orders
  placeOrder(order: any | any[]): Observable<any> {
    if (Array.isArray(order)) {
      return this.http.post(`${this.apiUrl}/create-multiple`, order);
    } else {
      return this.http.post(this.apiUrl, order);
    }
  }

  getOrders(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  getOrdersByUser(userId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/user/${userId}`);
  }
}

