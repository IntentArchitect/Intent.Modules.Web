//@IntentMerge()
import { IntentIgnoreBody, IntentMerge, IntentIgnore } from '../../intent/intent.decorators';
import { CreateProductCommand } from '../../service-proxies/models/dot-net-back-end-service/services/products/create-product-command';
import { ProductsService } from '../../service-proxies/products/products-service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

interface CreateProductModel {
  name: string;
  description: string;
  sku: string;
  price: number | null;
}

@IntentMerge()
@Component({
  selector: 'app-product-add-page',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: 'product-add-page.component.html',
  styleUrls: ['product-add-page.component.scss'],
})
export class ProductAddPageComponent implements OnInit {
  serviceErrors = {
    createProductError: null as string | null
  };
  isLoading = false;
  model: CreateProductModel = {
    name: '',
    description: '',
    sku: '',
    price: 0
  };

  //@IntentMerge()
  constructor(private router: Router, private readonly productsService: ProductsService) {
  }

  @IntentMerge()
  ngOnInit(): void {
  }

  save(form: NgForm): void {
    if (this.isLoading || form.invalid) {
      return;
    }
    this.createProduct();
    this.navigateToProductList();
  }

  @IntentMerge()
  createProduct(): void {
    this.serviceErrors.createProductError = null;
    this.isLoading = true;
    
    this.productsService.createProduct({
      name: this.model.name,
      description: this.model.description,
      sku: this.model.sku,
      price: this.model.price!,
    })
    .pipe(
        finalize(() => {
          this.isLoading = false; 
        })
    )
    .subscribe({
      error: (err) => {
        const message = err?.error?.message || err.message || 'Unknown error';
        this.serviceErrors.createProductError = `Failed to call service: ${message}`;

        console.error('Failed to call service:', err);
      }
    });
  }

  navigateToProductList(): void {
    this.router.navigate(['/product-list']);
  }
}
