//@IntentMerge()
import { IntentIgnoreBody, IntentMerge, IntentIgnore } from './../../../intent/intent.decorators';
import { GetCustomersQuery } from './../../../service-proxies/models/my-back-end/services/customers/get-customers-query';
import { CustomerSummaryQueryDto } from './../../../service-proxies/models/my-back-end/services/customers/customer-summary-query-dto';
import { CustomersService } from './../../../service-proxies/Customers/customers-service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { PagedResult } from './../../../service-proxies/models/paged-result';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { CustomerAddDialogComponent } from './../customer-add-dialog/customer-add-dialog.component';
import { CustomerEditDialogComponent } from './../customer-edit-dialog/customer-edit-dialog.component';
import { CustomerDeleteDialogComponent } from './../customer-delete-dialog/customer-delete-dialog.component';

interface DeleteCustomerModel {
  id: string | null;
}

@IntentMerge()
@Component({
  selector: 'app-customer-search',
  standalone: true,
  templateUrl: 'customer-search.component.html',
  styleUrls: ['customer-search.component.scss'],
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatSelectModule,
    MatChipsModule,
    MatTooltipModule,
    MatProgressSpinnerModule,
    MatDialogModule
  ]
})
export class CustomerSearchComponent implements OnInit {
  serviceErrors = {
    loadCustomersError: null as string | null
  };
  isLoading = false;
  customersModels: PagedResult<CustomerSummaryQueryDto> | null = null;
  model: DeleteCustomerModel | null = null;

  searchTerm: string | null = '';
  isActiveFilter: boolean | null = null;
  displayedColumns: string[] = ['name', 'surname', 'email', 'status', 'actions'];
  pageSize = 10;
  pageIndex = 0;
  sortField: string | null = null;
  sortDirection: 'asc' | 'desc' | '' = '';

  get data(): CustomerSummaryQueryDto[] {
    return this.customersModels?.data ?? [];
  }

  get totalItems(): number {
    return this.customersModels?.totalCount ?? 0;
  }

  //@IntentMerge()
  constructor(private router: Router, private readonly customersService: CustomersService, private dialog: MatDialog) {
  }

  @IntentMerge()
  ngOnInit(): void {
    this.refreshCustomers();
  }

  @IntentMerge()
  deleteCustomer(customerId: string): void {
    const dialogRef = this.dialog.open(CustomerDeleteDialogComponent, {
      width: '400px',
      disableClose: true,
      data: {
        customerId: customerId
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.refreshCustomers();
      }
    });
  }

  @IntentMerge()
  addCustomer(): void {
    const dialogRef = this.dialog.open(CustomerAddDialogComponent, {
      width: '800px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.refreshCustomers();
      }
    });
  }


  @IntentMerge()
  loadCustomers(pageNo: number, pageSize: number, orderBy: string | null, searchTerm: string | null, isActive: boolean | null): void {
    this.serviceErrors.loadCustomersError = null;
    this.isLoading = true;
    
    this.customersService.getCustomers({
      pageNo: pageNo,
      pageSize: pageSize,
      orderBy: orderBy,
      searchTerm: searchTerm,
      isActive: isActive,
    })
    .pipe(
        finalize(() => {
          this.isLoading = false; 
        })
     )
    .subscribe({
      next: (data) => {
        this.customersModels = data;
      },
      error: (err) => {
        const message = err?.error?.message || err.message || 'Unknown error';
        this.serviceErrors.loadCustomersError = `Failed to call service: ${message}`;

        console.error('Failed to call service:', err);
      }
    });
  }

  navigateToCustomerAdd(): void {
    this.router.navigate(['/customer', 'add']);
  }

  navigateToCustomerEdit(customerId: string): void {
    this.router.navigate(['/customer', 'edit', customerId]);
  }

  onSearch(): void {
    this.pageIndex = 0;
    this.refreshCustomers();
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.refreshCustomers();
  }

  onSortChange(sort: Sort): void {
    if (!sort.direction) {
      this.sortField = null;
      this.sortDirection = '';
    } else {
      const fieldMap: Record<string, string> = {
        name: 'Name',
        surname: 'Surname',
        email: 'Email'
      };
      this.sortField = fieldMap[sort.active] ?? sort.active;
      this.sortDirection = sort.direction;
    }
    this.refreshCustomers();
  }

  private getOrderBy(): string | null {
    if (!this.sortField || !this.sortDirection) {
      return null;
    }
    return `${this.sortField} ${this.sortDirection}`;
  }

  private refreshCustomers(): void {
    const trimmedSearch = this.searchTerm && this.searchTerm.trim().length > 0 ? this.searchTerm.trim() : null;
    this.loadCustomers(
      this.pageIndex + 1,
      this.pageSize,
      this.getOrderBy(),
      trimmedSearch,
      this.isActiveFilter
    );
  }

  @IntentMerge()
  editCustomer(customerId: string): void {
    const dialogRef = this.dialog.open(CustomerEditDialogComponent, {
      width: '800px',
      disableClose: true,
      data: {
        customerId: customerId
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.refreshCustomers();
      }
    });
  }

  navigateToCustomerViewPage(customerId: string): void {
    this.router.navigate(['/customer', 'view', customerId]);
  }
}
