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
import { AuthGuard } from './services/auth.guard';
import { OrderHistoryComponent } from './order-history/order-history.component';
import { ProductCreateComponent } from './product-create/product-create.component';

export const routes: Routes = [
   { path: '', redirectTo: 'login', pathMatch: 'full' },  // ✅ default to login
  { path: 'login', component: LoginComponent },
  { path: 'products', component: ProductListComponent }, 
  { path: 'products/create', component: ProductCreateComponent },
  { path: 'product/:id', component: ProductDetailComponent },
  { path: 'cart', component: CartComponent },
  { path: 'checkout', component: CheckoutComponent,canActivate: [AuthGuard] },
  { path: 'payment', component: PaymentComponent },
  { path: 'payment-status', component: PaymentStatusComponent },
  { path: 'order-tracking/:id', component: OrderTrackingComponent },
  { path: 'order-history', component: OrderHistoryComponent, canActivate: [AuthGuard] },
  { path: 'shipping/:id', component: ShippingStatusComponent },
  { path: 'admin', component: AdminDashboardComponent, canActivate: [AuthGuard] },
];

