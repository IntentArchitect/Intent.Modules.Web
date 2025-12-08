import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { CustomersService } from '../services/customers.service';
import { DeleteCustomerConfirmDialogComponent } from './delete-customer-confirm-dialog.component';

export interface CustomerSummaryDto {
    id: string;
    name: string;
    surname: string;
    email: string;
    isActive: boolean;
}

export interface PagedResult<T> {
    data: T[];
    totalCount: number;
}

@Component({
    selector: 'app-customers-management',
    standalone: true,
    templateUrl: './customers-management.component.html',
    styleUrls: ['./customers-management.component.scss'],
    imports: [
        CommonModule,
        FormsModule,
        // Material
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatIconModule,
        MatTableModule,
        MatPaginatorModule,
        MatSortModule,
        MatSelectModule,
        MatSnackBarModule,
        MatDialogModule,
        MatChipsModule,
        MatTooltipModule,
        MatProgressSpinnerModule
    ]
})
export class CustomersManagementComponent implements OnInit {
    // Filters
    searchText: string | null = '';
    isActive: boolean | null = null;

    // Table / paging / sorting
    displayedColumns: string[] = ['name', 'surname', 'email', 'status', 'actions'];
    data: CustomerSummaryDto[] = [];
    totalItems = 0;
    pageSize = 10;
    pageIndex = 0;

    sortField?: string;
    sortDirection: 'asc' | 'desc' | '' = '';

    // UI state
    isLoading = false;

    constructor(
        private customersService: CustomersService,
        private snackBar: MatSnackBar,
        private router: Router,
        private dialog: MatDialog
    ) { }

    ngOnInit(): void {
        this.loadCustomers();
    }

    private loadCustomers(): void {
        this.isLoading = true;

        const orderBy = this.sortField
            ? `${this.sortField} ${this.sortDirection || 'asc'}`
            : undefined;

        this.customersService
            .getCustomers({
                pageNo: this.pageIndex + 1,
                pageSize: this.pageSize,
                orderBy,
                searchTerm: this.searchText || undefined,
                isActive: this.isActive
            })
            .subscribe({
                next: (result) => {
                    this.data = result.data;
                    this.totalItems = result.totalCount;
                    this.isLoading = false;
                },
                error: (error) => {
                    const message =
                        error?.error?.message || 'Failed to load customers.';
                    this.snackBar.open(message, 'Close', { duration: 5000 });
                    this.isLoading = false;
                }
            });
    }

    onSearch(): void {
        this.pageIndex = 0;
        this.loadCustomers();
    }

    onPageChange(event: PageEvent): void {
        this.pageIndex = event.pageIndex;
        this.pageSize = event.pageSize;
        this.loadCustomers();
    }

    onSortChange(sort: Sort): void {
        if (!sort.direction) {
            this.sortField = undefined;
            this.sortDirection = '';
        } else {
            const map: Record<string, string> = {
                name: 'Name',
                surname: 'Surname',
                email: 'Email'
            };
            this.sortField = map[sort.active] ?? sort.active;
            this.sortDirection = sort.direction;
        }

        this.loadCustomers();
    }

    addCustomer(): void {
        this.router.navigate(['/templates/pages/customers/add']);
    }

    editCustomer(customerId: string): void {
        this.router.navigate(['/templates/pages/customers/edit', customerId]);
    }

    viewCustomer(customerId: string): void {
        this.router.navigate(['/templates/pages/customers/view', customerId]);
    }

    onDeleteCustomer(customerId: string): void {
        const dialogRef = this.dialog.open(DeleteCustomerConfirmDialogComponent, {
            width: '360px',
            disableClose: true,
            data: {
                title: 'Delete customer',
                message: 'Are you sure you want to delete this customer?'
            }
        });

        dialogRef.afterClosed().subscribe((confirmed: boolean) => {
            if (confirmed) {
                this.deleteCustomer(customerId);
            }
        });
    }

    private deleteCustomer(customerId: string): void {
        this.customersService.deleteCustomer(customerId).subscribe({
            next: () => {
                this.snackBar.open('Customer deleted successfully.', 'Close', {
                    duration: 3000
                });
                this.loadCustomers();
            },
            error: (error) => {
                const message = error?.error?.message || 'Failed to delete customer.';
                this.snackBar.open(message, 'Close', { duration: 5000 });
            }
        });
    }
}
