import { Component } from '@angular/core';
import { ProductService } from '../services/product.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product-create',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './product-create.component.html',
  styleUrls: ['./product-create.component.css']
})
export class ProductCreateComponent {
  product = { id: 0, name: '', price: 0, stock: 0, category: '', description: '' };

  constructor(private productService: ProductService, private router: Router) {}

  addProduct() {
    this.productService.createProduct(this.product).subscribe({
      next: () => this.router.navigate(['/products']),
      error: () => alert('Failed to add product')
    });
  }
}
