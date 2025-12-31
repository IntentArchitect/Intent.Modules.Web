//@IntentMerge()
import { IntentIgnoreBody, IntentMerge, IntentIgnore } from './../../intent/intent.decorators';
import { UpdateProductCommand } from './../../service-proxies/models/dot-net-back-end-service/services/products/update-product-command';
import { ProductDto } from './../../service-proxies/models/dot-net-back-end-service/services/products/product-dto';
import { ProductsService } from './../../service-proxies/products/products-service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

interface UpdateProductModel {
  id: string | null;
  name: string;
  description: string;
  sku: string;
  price: number | null;
}

@IntentMerge()
@Component({
  selector: 'app-product-edit-page',
  standalone: true,
  templateUrl: 'product-edit-page.component.html',
  styleUrls: ['product-edit-page.component.scss'],
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
})
export class ProductEditPageComponent implements OnInit {
  serviceErrors = {
    loadProductByIdError: null as string | null,
    updateProductError: null as string | null
  };
  isLoading = false;
  productId: string = '';
  productByIdModels: ProductDto | null = null;

  //@IntentMerge()
  constructor(private route: ActivatedRoute, private router: Router, private readonly productsService: ProductsService) {
  }

  @IntentMerge()
  ngOnInit(): void {
    const productId = this.route.snapshot.paramMap.get('productId');
    if (!productId) {
      throw new Error("Expected 'productId' not supplied");
    }
    this.productId = productId;
    this.loadProductById(this.productId);
  }

  @IntentMerge()
  loadProductById(id: string): void {
    this.serviceErrors.loadProductByIdError = null;
    this.isLoading = true;
    
    this.productsService.getProductById(id)
    .pipe(
        finalize(() => {
          this.isLoading = false; 
        })
     )
    .subscribe({
      next: (data) => {
        this.productByIdModels = data;
      },
      error: (err) => {
        const message = err?.error?.message || err.message || 'Unknown error';
        this.serviceErrors.loadProductByIdError = `Failed to call service: ${message}`;

        console.error('Failed to call service:', err);
      }
    });
  }

  navigateToProductList(): void {
    this.router.navigate(['/product-list']);
  }

  @IntentMerge()
  updateProduct(): void {
    this.serviceErrors.updateProductError = null;
    this.isLoading = true;
    
    if(!this.productByIdModels) {
      this.serviceErrors.updateProductError = "Property 'productByIdModels' cannot be null";
      this.isLoading = false;
      return;
    }
    this.productsService.updateProduct({
      id: this.productByIdModels.id,
      name: this.productByIdModels.name,
      description: this.productByIdModels.description,
      sku: this.productByIdModels.sku,
      price: this.productByIdModels.price,
    })
    .pipe(
        finalize(() => {
          this.isLoading = false; 
        })
    )
    .subscribe({
      error: (err) => {
        const message = err?.error?.message || err.message || 'Unknown error';
        this.serviceErrors.updateProductError = `Failed to call service: ${message}`;

        console.error('Failed to call service:', err);
      }
    });
  }

  save(form: NgForm): void {
    if (form.invalid || !this.productByIdModels) {
      form.control.markAllAsTouched();
      return;
    }
    this.updateProduct();
  }
}
