import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService, Product } from '../services/product.service';
import { HttpClientModule } from '@angular/common/http';
import { CartService } from '../services/cart.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule,HttpClientModule, FormsModule,RouterModule],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})

export class ProductListComponent implements OnInit {
  products: Product[] = [];
  filteredProducts: Product[] = [];
  categories: string[] = [];
  searchTerm: string = '';
  selectedCategory: string = '';

  constructor(private productService: ProductService,private cartService: CartService) {}

  ngOnInit() {
    this.productService.getProducts().subscribe({
      next: data => {
        this.products = data;
        this.filteredProducts = data;
        this.categories = [...new Set(data.map(p => p.category))];
      },
      error: () => {
        this.products = [];
        this.filteredProducts = [];
        this.categories = [];
      }
    });
  }

  filterProducts(): void {
  const search = this.searchTerm.toLowerCase();   // normalize search term
  const category = this.selectedCategory.toLowerCase();

  this.filteredProducts = this.products.filter(product => {
    const matchesSearch = search
      ? product.name.toLowerCase().includes(search) || product.category.toLowerCase().includes(search)
      : true;

    const matchesCategory = category
      ? product.category.toLowerCase() === category
      : true;

    return matchesSearch && matchesCategory;
  });
}

   addToCart(product: Product) {
    this.cartService.addToCart(product);
    alert(`${product.name} added to cart!`);
  }
}
