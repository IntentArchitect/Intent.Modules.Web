import { IntentIgnoreBody, IntentMerge, IntentIgnore } from './../../../intent/intent.decorators';
import { UpdateCustomerCommand } from './../../../service-proxies/models/my-back-end/services/customers/update-customer-command.dto';
import { UpdateCustomerPreferenceDto } from './../../../service-proxies/models/my-back-end/services/customers/update-customer-preference.dto';
import { UpdateCustomerCommandLoyaltyDto } from './../../../service-proxies/models/my-back-end/services/customers/update-customer-command-loyalty.dto';
import { UpdateCustomerCommandAddressesDto } from './../../../service-proxies/models/my-back-end/services/customers/update-customer-command-addresses.dto';
import { CustomerDto } from './../../../service-proxies/models/my-back-end/services/customers/customer.dto';
import { CustomerPreferenceDto } from './../../../service-proxies/models/my-back-end/services/customers/customer-preference.dto';
import { CustomerLoyaltyDto } from './../../../service-proxies/models/my-back-end/services/customers/customer-loyalty.dto';
import { CustomerAddressDto } from './../../../service-proxies/models/my-back-end/services/customers/customer-address.dto';
import { CategoryDto } from './../../../service-proxies/models/my-back-end/services/categories/category.dto';
import { SubCategoryDto } from './../../../service-proxies/models/my-back-end/services/sub-categories/sub-category.dto';
import { CategoriesService } from './../../../service-proxies/Categories/categories-service';
import { CustomersService } from './../../../service-proxies/Customers/customers-service';
import { SubCategoriesService } from './../../../service-proxies/SubCategories/sub-categories-service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AddressType } from '../../../service-proxies/models/address-type';

interface UpdateCustomerModel {
    id: string | null;
    categoryId: string | null;
    subCategoryId: string | null;
    name: string;
    surname: string;
    email: string;
    isActive: boolean | undefined;
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
})
export class CustomerEditComponent implements OnInit {
    serviceErrors = {
        loadCategoriesError: null as string | null,
        updateCustomerError: null as string | null,
        loadCustomerByIdError: null as string | null,
        loadSubCategoriesError: null as string | null
    };

    isLoading = false;
    customerId: string = '';
    categoriesModels: CategoryDto[] | null = null;
    model: UpdateCustomerModel | null = null;
    subCategoriesModels: SubCategoryDto[] | null = null;

    // expose enum & loyalty toggle for the template
    AddressType = AddressType;
    hasLoyalty = false;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private readonly categoriesService: CategoriesService,
        private readonly customersService: CustomersService,
        private readonly subCategoriesService: SubCategoriesService
    ) { }

    ngOnInit(): void {
        this.customerId = this.route.snapshot.paramMap.get('customerId') ?? '';

        // load supporting data
        this.loadCategories();

        if (this.customerId) {
            this.loadCustomerById(this.customerId);
        }
    }

    // ---------------------- UI helpers (state only) ---------------------- //

    onCategoryChanged(categoryId: string | null): void {
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

    /**
     * Save handler for the Save button.
     * - Validates that a model exists
     * - Calls the update service
     * - Navigates back to the search page on success using navigateToCustomerSearch().
     *
     * NOTE: This method does NOT modify updateCustomer(); it has its own service call.
     */
    save(): void {
        this.serviceErrors.updateCustomerError = null;

        if (!this.model) {
            this.serviceErrors.updateCustomerError = 'No customer model loaded.';
            return;
        }

        // if loyalty toggle is off, clear loyalty before sending
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
                addresses: this.model.addresses.map<UpdateCustomerCommandAddressesDto>(a => ({
                    id: a.id!,
                    line1: a.line1,
                    line2: a.line2 ?? undefined,
                    city: a.city,
                    postal: a.postal,
                    addressType: a.addressType
                }))
            })
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe({
                next: () => {
                    // success -> go back to search
                    this.navigateToCustomerSearch();
                },
                error: err => {
                    const message = err?.error?.message || err.message || 'Unknown error';
                    this.serviceErrors.updateCustomerError = `Failed to call service: ${message}`;

                    console.error('Failed to call service:', err);
                }
            });
    }

    // ----------------- Service & navigation methods (original) ----------------- //

    loadCategories(): void {
        this.serviceErrors.loadCategoriesError = null;
        this.isLoading = true;

        this.categoriesService
            .getCategories()
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe({
                next: data => {
                    this.categoriesModels = data;
                },
                error: err => {
                    const message = err?.error?.message || err.message || 'Unknown error';
                    this.serviceErrors.loadCategoriesError = `Failed to call service: ${message}`;

                    console.error('Failed to call service:', err);
                }
            });
    }

    loadCustomerById(id: string): void {
        this.serviceErrors.loadCustomerByIdError = null;
        this.isLoading = true;

        this.customersService
            .getCustomerById(id)
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe({
                next: data => {
                    this.model = {
                        id: data.id,
                        categoryId: data.categoryId,
                        subCategoryId: data.subCategoryId,
                        name: data.name,
                        surname: data.surname,
                        email: data.email,
                        isActive: data.isActive ?? undefined,
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

                    // ensure addresses array exists
                    if (!this.model.addresses) {
                        this.model.addresses = [];
                    }

                    // set loyalty toggle & load subcategories like Blazor version
                    this.hasLoyalty = !!data.loyalty;

                    if (data.categoryId) {
                        this.loadSubCategories(data.categoryId);
                    }
                },
                error: err => {
                    const message = err?.error?.message || err.message || 'Unknown error';
                    this.serviceErrors.loadCustomerByIdError = `Failed to call service: ${message}`;

                    console.error('Failed to call service:', err);
                }
            });
    }

    loadSubCategories(categoryId: string): void {
        this.serviceErrors.loadSubCategoriesError = null;
        this.isLoading = true;

        this.subCategoriesService
            .getSubCategories(categoryId)
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe({
                next: data => {
                    this.subCategoriesModels = data;
                },
                error: err => {
                    const message = err?.error?.message || err.message || 'Unknown error';
                    this.serviceErrors.loadSubCategoriesError = `Failed to call service: ${message}`;

                    console.error('Failed to call service:', err);
                }
            });
    }

    navigateToCustomerSearch(): void {
        this.router.navigate(['/test', 'customer-search']);
    }

    // NOTE: left intact as requested – not used by the Save button
    updateCustomer(): void {
        this.serviceErrors.updateCustomerError = null;
        this.isLoading = true;

        // 👇 Type guard so TS knows model is not null below
        if (!this.model) {
            this.serviceErrors.updateCustomerError = 'No customer model loaded.';
            return;
        }

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
                addresses: this.model.addresses.map<UpdateCustomerCommandAddressesDto>(a => ({
                    id: a.id!,
                    line1: a.line1,
                    line2: a.line2 ?? undefined,
                    city: a.city,
                    postal: a.postal,
                    addressType: a.addressType
                }))
            })
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe({
                error: err => {
                    const message = err?.error?.message || err.message || 'Unknown error';
                    this.serviceErrors.updateCustomerError = `Failed to call service: ${message}`;

                    console.error('Failed to call service:', err);
                }
            });
    }
}
