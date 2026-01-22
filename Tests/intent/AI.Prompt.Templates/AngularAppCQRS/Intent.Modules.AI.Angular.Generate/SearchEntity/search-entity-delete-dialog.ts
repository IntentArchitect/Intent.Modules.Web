//@IntentMerge()
import { IntentIgnoreBody, IntentMerge, IntentIgnore } from './../../../intent/intent.decorators';
import { Inject, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CustomersService } from './../../../service-proxies/Customers/customers-service';
import { finalize } from 'rxjs';

interface DeleteCustomerModel {
  id: string | null;
}

@IntentMerge()
@Component({
  selector: 'app-customer-delete-dialog',
  standalone: true,
  templateUrl: 'customer-delete-dialog.component.html',
  styleUrls: ['customer-delete-dialog.component.scss'],
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatIconModule, MatProgressSpinnerModule]
})
export class CustomerDeleteDialogComponent implements OnInit {
  serviceErrors = {
    deleteCustomerError: null as string | null
  };
  isLoading = false;
  customerId: string = '';
  model: DeleteCustomerModel | null = null;

  //@IntentMerge()
  constructor(private readonly customersService: CustomersService,
      @Inject(MAT_DIALOG_DATA) public data: { customerId: string },
      private dialogRef: MatDialogRef<CustomerDeleteDialogComponent>) {
  }

  @IntentMerge()
  ngOnInit(): void {
    if(!this.data?.customerId) {
      throw new Error("Expected 'customerId' not supplied");
    }
    this.customerId = this.data.customerId
    this.model = { id: this.customerId };
  }

  @IntentMerge()
  deleteCustomer(): void {
    this.serviceErrors.deleteCustomerError = null;
    this.isLoading = true;
    
    if(!this.model) {
      this.serviceErrors.deleteCustomerError = "Property 'model' cannot be null";
      this.isLoading = false;
      return;
    }
    this.customersService.deleteCustomer(this.model.id!)
    .pipe(
        finalize(() => {
          this.isLoading = false; 
        })
    )
    .subscribe({
      error: (err) => {
        const message = err?.error?.message || err.message || 'Unknown error';
        this.serviceErrors.deleteCustomerError = `Failed to call service: ${message}`;

        console.error('Failed to call service:', err);
      }
    });
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }

  onConfirm(): void {
    this.deleteCustomer();
    this.dialogRef.close(true);
  }
}
