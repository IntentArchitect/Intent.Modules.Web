//@IntentMerge()
import { IntentIgnoreBody, IntentMerge, IntentIgnore } from './../../intent/intent.decorators';
import { ProductDto } from './../../service-proxies/models/dot-net-back-end-service/services/products/product-dto';
import { ProductsService } from './../../service-proxies/products/products-service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';

@IntentMerge()
@Component({
  selector: 'app-product-list',
  standalone: true,
  templateUrl: 'product-list.component.html',
  styleUrls: ['product-list.component.scss'],
  imports: [
    CommonModule,
    MatCardModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatTooltipModule
  ]
})
export class ProductListComponent implements OnInit {
  serviceErrors = {
    loadProductsError: null as string | null
  };
  isLoading = false;
  productsModels: ProductDto[] | null = null;

  get data(): ProductDto[] {
    return this.productsModels ?? [];
  }

  //@IntentMerge()
  constructor(private router: Router, private readonly productsService: ProductsService) {
  }

  @IntentMerge()
  ngOnInit(): void {
    this.loadProducts();
  }

  @IntentMerge()
  loadProducts(): void {
    this.serviceErrors.loadProductsError = null;
    this.isLoading = true;
    
    this.productsService.getProducts()
    .pipe(
        finalize(() => {
          this.isLoading = false; 
        })
     )
    .subscribe({
      next: (data) => {
        this.productsModels = data;
      },
      error: (err) => {
        const message = err?.error?.message || err.message || 'Unknown error';
        this.serviceErrors.loadProductsError = `Failed to call service: ${message}`;

        console.error('Failed to call service:', err);
      }
    });
  }

  navigateToProductAddPage(): void {
    this.router.navigate(['/product-add']);
  }

  navigateToProductEditPage(productId: string): void {
    this.router.navigate(['/product-edit', productId]);
  }
}
