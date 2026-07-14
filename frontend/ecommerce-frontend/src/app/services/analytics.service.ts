import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AnalyticsService {
  private apiUrl = 'http://localhost:5071/api/analytics';

  constructor(private http: HttpClient) {}

  getOrdersPerDay(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/orders-per-day`);
  }

  getConversionRates(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/conversion-rates`);
  }

  getFailedPayments(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/failed-payments`);
  }
}
