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
      localStorage.setItem(this.tokenKey, token);
    }
  }

  saveUserId(userId: number) {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(this.userIdKey, userId.toString());
    }
  }

  getToken(): string | null {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem(this.tokenKey);
    }
    return null;
  }

  getUserId(): number {
    if (isPlatformBrowser(this.platformId)) {
      const stored = localStorage.getItem(this.userIdKey);
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
      localStorage.removeItem(this.tokenKey);
      localStorage.removeItem(this.userIdKey);
    }
  }
 isLoggedIn(): boolean {
  return !!localStorage.getItem(this.tokenKey); // use jwtToken consistently
}



}
