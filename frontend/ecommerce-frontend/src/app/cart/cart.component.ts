import { Component, OnInit } from '@angular/core';
import { Product } from '../services/product.service';
import { CartService } from '../services/cart.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
  preserveWhitespaces: true
})
export class CartComponent implements OnInit {
  items: Product[] = [];
  total: number = 0;

  constructor(private cartService: CartService) {}

  ngOnInit() {
    this.items = this.cartService.getItems();
    this.total = this.cartService.getTotal();
  }

  clearCart() {
    this.cartService.clearCart();
    this.items = [];
    this.total = 0;
  }

  removeItem(productId: number) {
    this.cartService.removeItem(productId);
    this.items = this.cartService.getItems();
    this.total = this.cartService.getTotal();
  }

  getQuantity(productId: number): number {
    return this.cartService.getQuantity(productId);
  }
  
}
