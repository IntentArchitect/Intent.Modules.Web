import { IntentIgnoreBody, IntentMerge, IntentIgnore } from './../../../intent/intent.decorators';
import { AddressType } from './../../../service-proxies/models/address-type';
import { CreateCustomerCommand } from './../../../service-proxies/models/my-back-end/services/customers/create-customer-command';
import { CreateCustomerPreferenceDto } from './../../../service-proxies/models/my-back-end/services/customers/create-customer-preference-dto';
import { CreateCustomerCommandLoyaltyDto } from './../../../service-proxies/models/my-back-end/services/customers/create-customer-command-loyalty-dto';
import { CreateCustomerCommandAddressesDto } from './../../../service-proxies/models/my-back-end/services/customers/create-customer-command-addresses-dto';
import { CategoryDto } from './../../../service-proxies/models/my-back-end/services/categories/category-dto';
import { SubCategoryDto } from './../../../service-proxies/models/my-back-end/services/sub-categories/sub-category-dto';
import { CategoriesService } from './../../../service-proxies/Categories/categories-service';
import { CustomersService } from './../../../service-proxies/Customers/customers-service';
import { SubCategoriesService } from './../../../service-proxies/SubCategories/sub-categories-service';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { finalize } from 'rxjs';

interface CreateCustomerModel {
    categoryId?: string | null;
    subCategoryId?: string | null;
    name: string;
    surname: string;
    email: string;
    isActive: boolean;
    preference: CreateCustomerPreferenceModel;
    loyalty?: CreateCustomerCommandLoyaltyModel | null;
    addresses: CreateCustomerCommandAddressesModel[];
}

interface CreateCustomerPreferenceModel {
    newsLetter: boolean;
    specials: boolean;
}

interface CreateCustomerCommandLoyaltyModel {
    loyaltyNo: string;
    points?: number | null;
}

interface CreateCustomerCommandAddressesModel {
    line1: string;
    line2?: string | null;
    city: string;
    postal: string;
    addressType: AddressType;
}

@IntentMerge()
@Component({
    selector: 'app-customer-add-dialog',
    standalone: true,
    templateUrl: 'customer-add-dialog.component.html',
    styleUrls: ['customer-add-dialog.component.scss'],
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
export class CustomerAddDialogComponent implements OnInit {

    public readonly AddressType = AddressType;

    serviceErrors = {
        loadCategoriesError: null as string | null,
        createCustomerError: null as string | null,
        loadSubCategoriesError: null as string | null
    };

    isLoading = false;
    categoriesModels: CategoryDto[] | null = null;
    subCategoriesModels: SubCategoryDto[] | null = null;

    model: CreateCustomerModel = {
        categoryId: '',
        subCategoryId: '',
        name: '',
        surname: '',
        email: '',
        isActive: true,
        preference: {
            newsLetter: false,
            specials: false
        },
        loyalty: null,
        addresses: []
    };

    hasLoyalty = false;

    constructor(
        private dialogRef: MatDialogRef<CustomerAddDialogComponent>,
        private readonly categoriesService: CategoriesService,
        private readonly customersService: CustomersService,
        private readonly subCategoriesService: SubCategoriesService
    ) { }

    ngOnInit(): void {
        this.hasLoyalty = !!this.model.loyalty;
        this.loadCategories();

        // Ensure at least one address row
        if (this.model.addresses.length === 0) {
            this.addAddress();
        }
    }

    // ---- Form submit wrapper ----
    onSave(form: NgForm): void {
        this.serviceErrors.createCustomerError = null;

        if (form.invalid) {
            form.control.markAllAsTouched();
            return;
        }

        this.createCustomer();
    }

    // ---- Service call ----
    createCustomer(): void {
        this.serviceErrors.createCustomerError = null;
        this.isLoading = true;

        const payload = {
            categoryId: this.model.categoryId ?? '',
            subCategoryId: this.model.subCategoryId ?? '',
            name: this.model.name,
            surname: this.model.surname,
            email: this.model.email,
            isActive: this.model.isActive,
            preference: {
                newsLetter: this.model.preference.newsLetter,
                specials: this.model.preference.specials,
            },
            loyalty: this.hasLoyalty && this.model.loyalty
                ? {
                    loyaltyNo: this.model.loyalty.loyaltyNo,
                    points: this.model.loyalty.points ?? 0,
                }
                : null,
            addresses: this.model.addresses.map(a => ({
                line1: a.line1,
                line2: a.line2,
                city: a.city,
                postal: a.postal,
                addressType: a.addressType,
            }))
        };

        this.customersService.createCustomer(payload)
            .pipe(finalize(() => {
                this.isLoading = false;
            }))
            .subscribe({
                next: () => {
                    // Close dialog and notify caller of success
                    this.dialogRef.close(true);
                },
                error: (err) => {
                    const message = err?.error?.message || err.message || 'Unknown error';
                    this.serviceErrors.createCustomerError = `Failed to call service: ${message}`;
                    console.error('Failed to call service:', err);
                }
            });
    }

    // ---- Loaders ----
    loadCategories(): void {
        this.serviceErrors.loadCategoriesError = null;
        this.isLoading = true;

        this.categoriesService.getCategories()
            .pipe(finalize(() => {
                this.isLoading = false;
            }))
            .subscribe({
                next: (data) => {
                    this.categoriesModels = data;
                },
                error: (err) => {
                    const message = err?.error?.message || err.message || 'Unknown error';
                    this.serviceErrors.loadCategoriesError = `Failed to call service: ${message}`;
                    console.error('Failed to call service:', err);
                }
            });
    }

    loadSubCategories(categoryId?: string | null): void {
        this.serviceErrors.loadSubCategoriesError = null;
        this.isLoading = true;

        this.subCategoriesService.getSubCategories(categoryId)
            .pipe(finalize(() => {
                this.isLoading = false;
            }))
            .subscribe({
                next: (data) => {
                    this.subCategoriesModels = data;
                },
                error: (err) => {
                    const message = err?.error?.message || err.message || 'Unknown error';
                    this.serviceErrors.loadSubCategoriesError = `Failed to call service: ${message}`;
                    console.error('Failed to call service:', err);
                }
            });
    }

    // ---- UI helpers ----
    onCategoryChange(id: string | null): void {
        this.model.categoryId = id ?? '';
        this.model.subCategoryId = '';
        this.subCategoriesModels = null;
        if (id) {
            this.loadSubCategories(id);
        }
    }

    addAddress(): void {
        const hasDeliver = this.model.addresses.some(a => a.addressType === AddressType.Deliver);
        this.model.addresses.push({
            line1: '',
            line2: '',
            city: '',
            postal: '',
            addressType: hasDeliver ? AddressType.Billing : AddressType.Deliver
        });
    }

    removeAddress(index: number): void {
        this.model.addresses.splice(index, 1);
    }

    onHasLoyaltyChange(checked: boolean): void {
        this.hasLoyalty = checked;
        if (checked && !this.model.loyalty) {
            this.model.loyalty = {
                loyaltyNo: '',
                points: 0
            };
        }
        if (!checked) {
            this.model.loyalty = null;
        }
    }

    // ---- Cancel ----
    cancel(): void {
        this.dialogRef.close(null);
    }
}
