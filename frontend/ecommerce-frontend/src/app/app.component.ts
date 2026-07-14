import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';   // ✅ Angular Router, not express

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(public authService: AuthService, private router: Router) {}

  title = 'ecommerce-frontend';
  ngOnInit() {
    // ✅ Run this check on first load
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
    }
  }
  logout() {
    this.authService.logout();              // clear token + userId
    this.router.navigate(['/login']);       // ✅ redirect to login page
  }
  
}
