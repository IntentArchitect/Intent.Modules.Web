//@IntentMerge()
import { IntentIgnoreBody, IntentMerge, IntentIgnore } from './../../../intent/intent.decorators';
import { CustomerDto } from './../../../service-proxies/models/my-back-end/services/customers/customer-dto';
import { CustomersService } from './../../../service-proxies/Customers/customers-service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AddressType } from './../../../service-proxies/models/address-type';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@IntentMerge()
@Component({
  selector: 'app-customer-view-page',
  standalone: true,
  templateUrl: 'customer-view-page.component.html',
  styleUrls: ['customer-view-page.component.scss'],
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSlideToggleModule,
    MatDividerModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
})
export class CustomerViewPageComponent implements OnInit {
  serviceErrors = {
    loadCustomerByIdError: null as string | null
  };
  isLoading = false;
  customerId: string = '';
  customerByIdModels: CustomerDto | null = null;

  AddressType = AddressType;

  get hasLoyalty(): boolean {
    return !!this.customerByIdModels?.loyalty;
  }

  //@IntentMerge()
  constructor(private route: ActivatedRoute, private router: Router, private readonly customersService: CustomersService) {
  }

  @IntentMerge()
  ngOnInit(): void {
    const customerId = this.route.snapshot.paramMap.get('customerId');
    if (!customerId) {
      throw new Error("Expected 'customerId' not supplied");
    }
    this.customerId = customerId;
    this.loadCustomerById(this.customerId);
  }

  @IntentMerge()
  loadCustomerById(id: string): void {
    this.serviceErrors.loadCustomerByIdError = null;
    this.isLoading = true;
    
    this.customersService.getCustomerById(id)
    .pipe(
        finalize(() => {
          this.isLoading = false; 
        })
     )
    .subscribe({
      next: (data) => {
        this.customerByIdModels = data;
      },
      error: (err) => {
        const message = err?.error?.message || err.message || 'Unknown error';
        this.serviceErrors.loadCustomerByIdError = `Failed to call service: ${message}`;

        console.error('Failed to call service:', err);
      }
    });
  }

  navigateToCustomerSearch(): void {
    this.router.navigate(['/customer', 'search']);
  }
}
