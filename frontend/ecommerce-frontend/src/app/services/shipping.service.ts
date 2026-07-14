import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ShippingService {
  private apiUrl = `${environment.apiUrl}/shipping`;

  constructor(private http: HttpClient) {}

  // Get tracking info for a specific order
  getTracking(orderId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/tracking/${orderId}`);
  }
}
