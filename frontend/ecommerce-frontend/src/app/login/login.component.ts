import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  loading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  login() {
    this.loading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.authService.login({ email: this.email, password: this.password }).subscribe({
      next: res => {
        this.authService.saveToken(res.token);
        this.authService.saveUserId(res.userId);

        this.loading = false;
        this.successMessage = 'Login successful 🎉 Redirecting...';

        // Redirect after short delay so user sees the message
        setTimeout(() => {
          this.router.navigate(['/products']); // or /cart, /dashboard
        }, 1500);
      },
      error: () => {
        this.loading = false;
        this.errorMessage = 'Invalid email or password';
      }
    });
  }
  //  logout() {
  //   this.authService.logout();
  //   this.router.navigate(['/login']); // ✅ redirect to login page
  // }
}
