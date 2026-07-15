import { Routes } from '@angular/router';

// Import your components
import { ProductListComponent } from './product-list/product-list.component';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { CartComponent } from './cart/cart.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { PaymentComponent } from './payment/payment.component';
import { OrderTrackingComponent } from './order-tracking/order-tracking.component';
import { ShippingStatusComponent } from './shipping-status/shipping-status.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { PaymentStatusComponent } from './payment-status/payment-status.component';
import { LoginComponent } from './login/login.component';
import { OrderHistoryComponent } from './order-history/order-history.component';
import { ProductCreateComponent } from './product-create/product-create.component';
//import { OrderStatusComponent } from './order-status/order-status.component'; // ✅ new import

// Guards
import { AuthGuard } from './services/auth.guard';
import { LoginGuard } from './services/login-guard.service';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  // Public routes
  { path: 'login', component: LoginComponent, canActivate: [LoginGuard] },
  { path: 'products', component: ProductListComponent},
  { path: 'product/:id', component: ProductDetailComponent },

  // Protected routes
  { path: 'cart', component: CartComponent, canActivate: [AuthGuard] },
  { path: 'checkout', component: CheckoutComponent, canActivate: [AuthGuard] },
  { path: 'order-history', component: OrderHistoryComponent, canActivate: [AuthGuard] },
  { path: 'admin', component: AdminDashboardComponent, canActivate: [AuthGuard] },
  { path: 'products/create', component: ProductCreateComponent, canActivate: [AuthGuard] },

  // Status & tracking routes
  { path: 'payment-status', component: PaymentStatusComponent },
  { path: 'order-tracking/:id', component: OrderTrackingComponent },
  { path: 'shipping/:id', component: ShippingStatusComponent },
  // { path: 'order-status', component: OrderStatusComponent }, // ✅ added route
];
