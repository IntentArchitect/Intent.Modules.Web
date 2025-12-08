import { Component, OnInit } from '@angular/core';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IntentMerge } from './../../../intent/intent.decorators';
import { CustomerDto } from './../../../service-proxies/models/my-back-end/services/customers/customer.dto';
import { CustomersService } from './../../../service-proxies/Customers/customers-service';
import { PagedResult } from './../../../service-proxies/models/paged-result';
import { GetCustomersQuery } from './../../../service-proxies/models/my-back-end/services/customers/get-customers-query.dto';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';

@IntentMerge()
@Component({
  selector: 'app-customer-list',
  standalone: true,
  imports: [
    NgIf,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatTableModule
  ],
  templateUrl: 'customer-list.component.html',
  styleUrls: ['customer-list.component.scss']
})
export class CustomerListComponent implements OnInit {
  customersModels: PagedResult<CustomerDto> | null = null;
    // MatTable column order
  displayedColumns: string[] = ['id', 'name', 'surname'];

  customerSearchCriteria: GetCustomersQuery = {
    searchTerm: '',
    pageNo: 1,
    pageSize: 10,
    orderBy: 'name'
  }; 

  serviceError: string | null = null;
  serviceCallSuccess: boolean = false;
  isLoading = false;

  constructor(private customersService: CustomersService) {
  }

  @IntentMerge()
  ngOnInit(): void {
    this.loadCustomers();
  }

  loadCustomers(): void {
    this.isLoading = true;

    this.customersService
    .getCustomers(this.customerSearchCriteria).subscribe({
      next: (data) => {
        this.serviceError = null;
        this.serviceCallSuccess = true;
        this.customersModels = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.serviceCallSuccess = false;
        const message = err?.error?.message || err.message || 'Unknown error';
        this.serviceError = `Failed to call service: ${message}`;

        console.error('Failed to call service:', err);
        this.customersModels = null;
        this.isLoading = false;
      }
    });
  }
onSearch(): void {
    // Always reset to first page when searching
    this.customerSearchCriteria.pageNo = 1;
    this.loadCustomers();
  }

  clearSearch(): void {
    this.customerSearchCriteria.searchTerm = '';
    this.customerSearchCriteria.pageNo = 1;
    this.loadCustomers();
  }

  changePageSize(size: number): void {
    this.customerSearchCriteria.pageSize = size;
    this.customerSearchCriteria.pageNo = 1;
    this.loadCustomers();
  }

  goToPage(page: number): void {
    if (!this.customersModels) return;
    if (page < 1 || page > this.customersModels.pageCount) return;

    this.customerSearchCriteria.pageNo = page;
    this.loadCustomers();
  }

  prevPage(): void {
    if (!this.customersModels) return;
    this.goToPage(this.customersModels.pageNumber - 1);
  }

  nextPage(): void {
    if (!this.customersModels) return;
    this.goToPage(this.customersModels.pageNumber + 1);
  }  
}
