import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

export interface DeleteDialogData {
    title: string;
    message: string;
}

@Component({
    selector: 'app-delete-customer-confirm-dialog',
    standalone: true,
    templateUrl: './delete-customer-confirm-dialog.component.html',
    imports: [CommonModule, MatDialogModule, MatButtonModule]
})
export class DeleteCustomerConfirmDialogComponent {
    constructor(
        public dialogRef: MatDialogRef<DeleteCustomerConfirmDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DeleteDialogData
    ) { }

    onConfirm(): void {
        this.dialogRef.close(true);
    }

    onCancel(): void {
        this.dialogRef.close(false);
    }
}
