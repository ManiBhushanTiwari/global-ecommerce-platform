import { Injectable } from '@angular/core';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderStatusService {
  private socket$: WebSocketSubject<any> | undefined;

  connect(orderId: number): Observable<any> {
    // Replace with your backend WebSocket URL
    this.socket$ = webSocket(`ws://localhost:5071/ws/orders/${orderId}`);
    return this.socket$.asObservable();
  }

  disconnect() {
    if (this.socket$) {
      this.socket$.complete();
      this.socket$ = undefined;
    }
  }
}
