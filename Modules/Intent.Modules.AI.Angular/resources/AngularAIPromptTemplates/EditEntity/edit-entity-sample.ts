//@IntentMerge()
import { IntentIgnoreBody, IntentMerge, IntentIgnore } from './../../../intent/intent.decorators';
import { AddressType } from './../../../service-proxies/models/address-type';
import { UpdateCustomerCommand } from './../../../service-proxies/models/my-back-end/services/customers/update-customer-command';
import { CustomerDto } from './../../../service-proxies/models/my-back-end/services/customers/customer-dto';
import { UpdateCustomerPreferenceDto } from './../../../service-proxies/models/my-back-end/services/customers/update-customer-preference-dto';
import { CustomerPreferenceDto } from './../../../service-proxies/models/my-back-end/services/customers/customer-preference-dto';
import { UpdateCustomerCommandLoyaltyDto } from './../../../service-proxies/models/my-back-end/services/customers/update-customer-command-loyalty-dto';
import { CustomerLoyaltyDto } from './../../../service-proxies/models/my-back-end/services/customers/customer-loyalty-dto';
import { UpdateCustomerCommandAddressesDto } from './../../../service-proxies/models/my-back-end/services/customers/update-customer-command-addresses-dto';
import { CustomerAddressDto } from './../../../service-proxies/models/my-back-end/services/customers/customer-address-dto';
import { CategoryDto } from './../../../service-proxies/models/my-back-end/services/categories/category-dto';
import { SubCategoryDto } from './../../../service-proxies/models/my-back-end/services/sub-categories/sub-category-dto';
import { CustomersService } from './../../../service-proxies/Customers/customers-service';
import { CategoriesService } from './../../../service-proxies/Categories/categories-service';
import { SubCategoriesService } from './../../../service-proxies/SubCategories/sub-categories-service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

interface UpdateCustomerModel {
    id: string | null;
    categoryId: string | null;
    subCategoryId: string | null;
    name: string;
    surname: string;
    email: string;
    isActive: boolean;
    preference: UpdateCustomerPreferenceModel;
    loyalty: UpdateCustomerCommandLoyaltyModel | null;
    addresses: UpdateCustomerCommandAddressesModel[];
}

interface UpdateCustomerPreferenceModel {
    newsLetter: boolean;
    specials: boolean;
}

interface UpdateCustomerCommandLoyaltyModel {
    id: string | null;
    loyaltyNo: string;
    points: number | null;
}

interface UpdateCustomerCommandAddressesModel {
    id: string | null;
    line1: string;
    line2: string | null;
    city: string;
    postal: string;
    addressType: AddressType;
}

@IntentMerge()
@Component({
    selector: 'app-customer-edit',
    standalone: true,
    templateUrl: 'customer-edit.component.html',
    styleUrls: ['customer-edit.component.scss'],
    imports: [
        CommonModule,
        FormsModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatIconModule,
        MatSelectModule,
        MatSlideToggleModule,
        MatButtonModule,
        MatDividerModule,
        MatProgressSpinnerModule
    ]
})
export class CustomerEditComponent implements OnInit {
    serviceErrors = {
        loadCustomerByIdError: null as string | null,
        updateCustomerError: null as string | null,
        loadCategoriesError: null as string | null,
        loadSubCategoriesError: null as string | null
    };
    isLoading = false;
    customerId: string = '';
    customerByIdModels: CustomerDto | null = null;
    categoriesModels: CategoryDto[] | null = null;
    subCategoriesModels: SubCategoryDto[] | null = null;

    AddressType = AddressType;
    hasLoyalty = false;

    //@IntentMerge()
    constructor(private route: ActivatedRoute, private router: Router, private readonly customersService: CustomersService, private readonly categoriesService: CategoriesService, private readonly subCategoriesService: SubCategoriesService) {
    }

    @IntentMerge()
    ngOnInit(): void {
        const customerId = this.route.snapshot.paramMap.get('customerId');
        if (!customerId) {
            throw new Error("Expected 'customerId' not supplied");
        }
        this.customerId = customerId;
        this.loadCategories();
        this.loadCustomerById(this.customerId);
    }

    @IntentMerge()
    loadCategories(): void {
        this.serviceErrors.loadCategoriesError = null;
        this.isLoading = true;

        this.categoriesService.getCategories()
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
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

    @IntentMerge()
    loadSubCategories(categoryId: string | null): void {
        this.serviceErrors.loadSubCategoriesError = null;
        this.isLoading = true;

        this.subCategoriesService.getSubCategories(categoryId)
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
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

    navigateToCustomerSearch(): void {
        this.router.navigate(['/customer', 'search']);
    }

    navigateToCustomerAdd(): void {
        this.router.navigate(['/customer', 'add']);
    }

    @IntentIgnore()
    updateCustomer(): void {
        this.serviceErrors.updateCustomerError = null;
        this.isLoading = true;

        if (!this.customerByIdModels) {
            this.serviceErrors.updateCustomerError = "Property 'customerByIdModels' cannot be null";
            this.isLoading = false;
            return;
        }

        if (!this.hasLoyalty) {
            this.customerByIdModels.loyalty = null;
        } else if (this.customerByIdModels.loyalty && !this.customerByIdModels.loyalty.id) {
            this.customerByIdModels.loyalty.id = crypto.randomUUID();
        }

        this.customersService.updateCustomer({
            id: this.customerByIdModels.id,
            categoryId: this.customerByIdModels.categoryId,
            subCategoryId: this.customerByIdModels.subCategoryId,
            name: this.customerByIdModels.name,
            surname: this.customerByIdModels.surname,
            email: this.customerByIdModels.email,
            isActive: this.customerByIdModels.isActive,
            preference: {
                newsLetter: this.customerByIdModels.preference.newsLetter,
                specials: this.customerByIdModels.preference.specials,
            },
            loyalty: this.customerByIdModels.loyalty
                ? {
                    id: this.customerByIdModels.loyalty.id,
                    loyaltyNo: this.customerByIdModels.loyalty.loyaltyNo,
                    points: this.customerByIdModels.loyalty.points,
                }
                : null,
            addresses: this.customerByIdModels.addresses.map(a => ({
                id: a.id,
                line1: a.line1,
                line2: a.line2,
                city: a.city,
                postal: a.postal,
                addressType: a.addressType,
            })),
        })
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe({
                error: (err) => {
                    const message = err?.error?.message || err.message || 'Unknown error';
                    this.serviceErrors.updateCustomerError = `Failed to call service: ${message}`;

                    console.error('Failed to call service:', err);
                }
            });
    }

    save(): void {
        this.updateCustomer();
    }

    onCategoryChanged(categoryId: string | null): void {
        if (!this.customerByIdModels) {
            return;
        }
        this.customerByIdModels.categoryId = categoryId as any;
        this.customerByIdModels.subCategoryId = null as any;
        this.subCategoriesModels = null;
        if (categoryId) {
            this.loadSubCategories(categoryId);
        }
    }

    addAddress(): void {
        if (!this.customerByIdModels) {
            return;
        }
        if (!this.customerByIdModels.addresses) {
            this.customerByIdModels.addresses = [];
        }
        const hasDelivery = this.customerByIdModels.addresses.some(a => a.addressType === AddressType.Deliver);
        const nextType = hasDelivery ? AddressType.Billing : AddressType.Deliver;
        this.customerByIdModels.addresses.push({
            id: '',
            line1: '',
            line2: null,
            city: '',
            postal: '',
            addressType: nextType
        });
    }

    removeAddress(index: number): void {
        if (!this.customerByIdModels || !this.customerByIdModels.addresses) {
            return;
        }
        if (index >= 0 && index < this.customerByIdModels.addresses.length) {
            this.customerByIdModels.addresses.splice(index, 1);
        }
    }

    onHasLoyaltyChange(value: boolean): void {
        this.hasLoyalty = value;
        if (!this.customerByIdModels) {
            return;
        }
        if (value) {
            if (!this.customerByIdModels.loyalty) {
                this.customerByIdModels.loyalty = {
                    id: '',
                    loyaltyNo: '',
                    points: 0
                };
            }
        } else {
            this.customerByIdModels.loyalty = null;
        }
    }
}
