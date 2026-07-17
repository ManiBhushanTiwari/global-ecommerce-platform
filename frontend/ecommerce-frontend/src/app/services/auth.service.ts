import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = `${environment.apiUrl}/auth`;
  private tokenKey = 'jwtToken';
  private userIdKey = 'userId';

  constructor(private http: HttpClient, @Inject(PLATFORM_ID) private platformId: Object) {}

  login(credentials: { email: string; password: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials);
  }

  saveToken(token: string) {
    if (isPlatformBrowser(this.platformId)) {
      sessionStorage.setItem(this.tokenKey, token);   // ✅ sessionStorage
    }
  }

  // saveUserId(userId: number) {
  //   if (isPlatformBrowser(this.platformId)) {
  //     sessionStorage.setItem(this.userIdKey, userId.toString());  // ✅ sessionStorage
  //   }
  // }
saveUserId(userId?: number) {
  if (userId !== undefined && userId !== null) {
    localStorage.setItem('userId', userId.toString());
  }
}

  getToken(): string | null {
    if (isPlatformBrowser(this.platformId)) {
      return sessionStorage.getItem(this.tokenKey);   // ✅ sessionStorage
    }
    return null;
  }

  getUserId(): number {
    if (isPlatformBrowser(this.platformId)) {
      const stored = sessionStorage.getItem(this.userIdKey);      // ✅ sessionStorage
      return stored ? parseInt(stored, 10) : 0;
    }
    return 0;
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) return false;
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const isExpired = Date.now() >= payload.exp * 1000;
      if (isExpired) {
        this.logout(); // clear expired token
        return false;
      }
      return true;
    } catch {
      this.logout(); // clear invalid token
      return false;
    }
  }

  logout() {
    if (isPlatformBrowser(this.platformId)) {
      sessionStorage.removeItem(this.tokenKey);   // ✅ sessionStorage
      sessionStorage.removeItem(this.userIdKey);  // ✅ sessionStorage
    }
  }

  isLoggedIn(): boolean {
    return !!sessionStorage.getItem(this.tokenKey);  // ✅ sessionStorage
  }
}
