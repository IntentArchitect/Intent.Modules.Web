import { IntentMerge } from './../../../intent/intent.decorators';

import { CategoryDto } from './../../../service-proxies/models/my-back-end/services/categories/category-dto';
import { SubCategoryDto } from './../../../service-proxies/models/my-back-end/services/sub-categories/sub-category-dto';
import { CustomersService } from './../../../service-proxies/Customers/customers-service';
import { CategoriesService } from './../../../service-proxies/Categories/categories-service';
import { SubCategoriesService } from './../../../service-proxies/SubCategories/sub-categories-service';

import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { finalize } from 'rxjs';
import { AddressType } from '../../../service-proxies/models/address-type';

interface EditCustomerPreferenceModel {
    newsLetter: boolean;
    specials: boolean;
}

interface EditCustomerLoyaltyModel {
    id: string | null;
    loyaltyNo: string;
    points: number | null;
}

interface EditCustomerAddressModel {
    id: string | null;
    line1: string;
    line2: string | null;
    city: string;
    postal: string;
    addressType: AddressType;
}

interface EditCustomerModel {
    id: string | null;
    categoryId: string | null;
    subCategoryId: string | null;
    name: string;
    surname: string;
    email: string;
    isActive: boolean | undefined;
    preference: EditCustomerPreferenceModel;
    loyalty: EditCustomerLoyaltyModel | null;
    addresses: EditCustomerAddressModel[];
}

export interface CustomerEditDialogData {
    customerId: string;
}

@IntentMerge()
@Component({
    selector: 'app-customer-edit-dialog',
    standalone: true,
    templateUrl: 'customer-edit-dialog.component.html',
    styleUrls: ['customer-edit-dialog.component.scss'],
    imports: [
        CommonModule,
        FormsModule,
        MatDialogModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatSlideToggleModule,
        MatButtonModule,
        MatProgressSpinnerModule
    ]
})
export class CustomerEditDialogComponent implements OnInit {
    serviceErrors = {
        loadCustomerError: null as string | null,
        loadCategoriesError: null as string | null,
        loadSubCategoriesError: null as string | null,
        updateCustomerError: null as string | null
    };

    isLoading = false;

    model: EditCustomerModel | null = null;
    categoriesModels: CategoryDto[] | null = null;
    subCategoriesModels: SubCategoryDto[] | null = null;

    AddressType = AddressType;
    hasLoyalty = false;

    constructor(
        private dialogRef: MatDialogRef<CustomerEditDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: CustomerEditDialogData,
        private readonly customersService: CustomersService,
        private readonly categoriesService: CategoriesService,
        private readonly subCategoriesService: SubCategoriesService
    ) { }

    ngOnInit(): void {
        this.loadCategories();

        if (this.data.customerId) {
            this.loadCustomerById(this.data.customerId);
        }
    }

    // ---- Form submit ----
    onSave(form: NgForm): void {
        this.serviceErrors.updateCustomerError = null;

        if (form.invalid) {
            form.control.markAllAsTouched();
            return;
        }

        this.saveCustomer();
    }

    // ---- Service call ----
    private saveCustomer(): void {
        if (!this.model) {
            this.serviceErrors.updateCustomerError = 'No customer loaded.';
            return;
        }

        if (!this.hasLoyalty) {
            this.model.loyalty = null;
        }

        this.isLoading = true;

        this.customersService
            .updateCustomer({
                id: this.model.id!,
                categoryId: this.model.categoryId!,
                subCategoryId: this.model.subCategoryId!,
                name: this.model.name,
                surname: this.model.surname,
                email: this.model.email,
                isActive: this.model.isActive ?? true,
                preference: {
                    newsLetter: this.model.preference.newsLetter,
                    specials: this.model.preference.specials
                },
                loyalty: this.model.loyalty
                    ? {
                        id: this.model.loyalty.id!,
                        loyaltyNo: this.model.loyalty.loyaltyNo!,
                        points: this.model.loyalty.points!
                    }
                    : null,
                addresses: this.model.addresses.map(a => ({
                    id: a.id!,
                    line1: a.line1,
                    line2: a.line2 ?? undefined,
                    city: a.city,
                    postal: a.postal,
                    addressType: a.addressType
                }))
            })
            .pipe(finalize(() => (this.isLoading = false)))
            .subscribe({
                next: () => {
                    this.dialogRef.close(true);
                },
                error: err => {
                    const message = err?.error?.message || err.message || 'Unknown error';
                    this.serviceErrors.updateCustomerError = `Failed to update customer: ${message}`;
                    console.error('Failed to update customer', err);
                }
            });
    }

    // ---- Loaders ----
    private loadCategories(): void {
        this.serviceErrors.loadCategoriesError = null;
        this.isLoading = true;

        this.categoriesService
            .getCategories()
            .pipe(finalize(() => (this.isLoading = false)))
            .subscribe({
                next: data => {
                    this.categoriesModels = data;
                },
                error: err => {
                    const message = err?.error?.message || err.message || 'Unknown error';
                    this.serviceErrors.loadCategoriesError = `Failed to load categories: ${message}`;
                    console.error('Failed to load categories', err);
                }
            });
    }

    private loadSubCategories(categoryId: string): void {
        this.serviceErrors.loadSubCategoriesError = null;
        this.isLoading = true;

        this.subCategoriesService
            .getSubCategories(categoryId)
            .pipe(finalize(() => (this.isLoading = false)))
            .subscribe({
                next: data => {
                    this.subCategoriesModels = data;
                },
                error: err => {
                    const message = err?.error?.message || err.message || 'Unknown error';
                    this.serviceErrors.loadSubCategoriesError = `Failed to load subcategories: ${message}`;
                    console.error('Failed to load subcategories', err);
                }
            });
    }

    private loadCustomerById(id: string): void {
        this.serviceErrors.loadCustomerError = null;
        this.isLoading = true;

        this.customersService
            .getCustomerById(id)
            .pipe(finalize(() => (this.isLoading = false)))
            .subscribe({
                next: data => {
                    this.model = {
                        id: data.id,
                        categoryId: data.categoryId,
                        subCategoryId: data.subCategoryId,
                        name: data.name,
                        surname: data.surname,
                        email: data.email,
                        isActive: data.isActive ?? true,
                        preference: {
                            newsLetter: data.preference.newsLetter,
                            specials: data.preference.specials
                        },
                        loyalty: data.loyalty
                            ? {
                                id: data.loyalty.id,
                                loyaltyNo: data.loyalty.loyaltyNo,
                                points: data.loyalty.points
                            }
                            : null,
                        addresses: (data.addresses ?? []).map(a => ({
                            id: a.id,
                            line1: a.line1,
                            line2: a.line2 ?? null,
                            city: a.city,
                            postal: a.postal,
                            addressType: a.addressType
                        }))
                    };

                    this.hasLoyalty = !!data.loyalty;

                    if (data.categoryId) {
                        this.loadSubCategories(data.categoryId);
                    }

                    if (!this.model.addresses.length) {
                        this.model.addresses.push({
                            id: null,
                            line1: '',
                            line2: null,
                            city: '',
                            postal: '',
                            addressType: AddressType.Deliver
                        });
                    }
                },
                error: err => {
                    const message = err?.error?.message || err.message || 'Unknown error';
                    this.serviceErrors.loadCustomerError = `Failed to load customer: ${message}`;
                    console.error('Failed to load customer', err);
                }
            });
    }

    // ---- UI helpers ----
    onCategoryChange(categoryId: string | null): void {
        if (!this.model) return;

        this.model.categoryId = categoryId;
        this.model.subCategoryId = null;
        this.subCategoriesModels = null;

        if (categoryId) {
            this.loadSubCategories(categoryId);
        }
    }

    addAddress(): void {
        if (!this.model) return;

        const hasDelivery = this.model.addresses.some(
            a => a.addressType === AddressType.Deliver
        );

        const nextType = hasDelivery ? AddressType.Billing : AddressType.Deliver;

        this.model.addresses.push({
            id: null,
            line1: '',
            line2: null,
            city: '',
            postal: '',
            addressType: nextType
        });
    }

    removeAddress(index: number): void {
        if (!this.model) return;
        if (index >= 0 && index < this.model.addresses.length) {
            this.model.addresses.splice(index, 1);
        }
    }

    onHasLoyaltyChange(value: boolean): void {
        this.hasLoyalty = value;
        if (!this.model) return;

        if (this.hasLoyalty) {
            if (!this.model.loyalty) {
                this.model.loyalty = {
                    id: null,
                    loyaltyNo: '',
                    points: 0
                };
            }
        } else {
            this.model.loyalty = null;
        }
    }

    // ---- Cancel ----
    cancel(): void {
        this.dialogRef.close(null);
    }
}
