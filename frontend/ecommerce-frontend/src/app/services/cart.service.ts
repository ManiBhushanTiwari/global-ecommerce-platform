import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Product } from './product.service';

@Injectable({ providedIn: 'root' })
export class CartService {
  private items: Product[] = [];
  private quantityMap: { [productId: number]: number } = {};
  private storageKey = 'cartItems';
  private isBrowser: boolean;

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    this.isBrowser = isPlatformBrowser(this.platformId);

    if (this.isBrowser) {
      const saved = localStorage.getItem(this.storageKey);
      if (saved) {
        const parsed = JSON.parse(saved);
        this.items = parsed.items || [];
        this.quantityMap = parsed.quantityMap || {};
      }
    }
  }

  private save(): void {
    if (this.isBrowser) {
      localStorage.setItem(this.storageKey, JSON.stringify({
        items: this.items,
        quantityMap: this.quantityMap
      }));
    }
  }

  addToCart(product: Product, quantity: number = 1): void {
    const existing = this.items.find(p => p.id === product.id);
    if (!existing) {
      this.items.push(product);
    }
    this.quantityMap[product.id] = (this.quantityMap[product.id] || 0) + quantity;
    this.save();
  }

  getItems(): Product[] { return this.items; }
  getQuantity(productId: number): number { return this.quantityMap[productId] || 0; }

  removeItem(productId: number): void {
    this.items = this.items.filter(p => p.id !== productId);
    delete this.quantityMap[productId];
    this.save();
  }

  clearCart(): void {
    this.items = [];
    this.quantityMap = {};
    if (this.isBrowser) {
      localStorage.removeItem(this.storageKey);
    }
  }

  getTotal(): number {
    return this.items.reduce((sum, p) => sum + p.price * this.getQuantity(p.id), 0);
  }

  updateQuantity(productId: number, quantity: number): void {
    if (this.quantityMap[productId] !== undefined) {
      this.quantityMap[productId] = quantity;
      this.save();
    }
  }
}

