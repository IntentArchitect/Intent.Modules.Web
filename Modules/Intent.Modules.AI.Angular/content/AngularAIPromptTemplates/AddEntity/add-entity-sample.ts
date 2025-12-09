import { IntentIgnoreBody, IntentMerge, IntentIgnore } from './../../../intent/intent.decorators';
import { CreateCustomerCommand } from './../../../service-proxies/models/my-back-end/services/customers/create-customer-command.dto';
import { CreateCustomerPreferenceDto } from './../../../service-proxies/models/my-back-end/services/customers/create-customer-preference.dto';
import { CreateCustomerCommandLoyaltyDto } from './../../../service-proxies/models/my-back-end/services/customers/create-customer-command-loyalty.dto';
import { CreateCustomerCommandAddressesDto } from './../../../service-proxies/models/my-back-end/services/customers/create-customer-command-addresses.dto';
import { CategoryDto } from './../../../service-proxies/models/my-back-end/services/categories/category.dto';
import { SubCategoryDto } from './../../../service-proxies/models/my-back-end/services/sub-categories/sub-category.dto';
import { CategoriesService } from './../../../service-proxies/Categories/categories-service';
import { CustomersService } from './../../../service-proxies/Customers/customers-service';
import { SubCategoriesService } from './../../../service-proxies/SubCategories/sub-categories-service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AddressType } from '../../../service-proxies/models/address-type';

interface CreateCustomerModel {
    name: string;
    surname: string;
}

interface CreateCustomerModel1 {
    categoryId: string | null;
    subCategoryId: string | null;
    name: string;
    surname: string;
    email: string;
    isActive: boolean;
    preference: CreateCustomerPreferenceModel;
    loyalty: CreateCustomerCommandLoyaltyModel | null;
    addresses: CreateCustomerCommandAddressesModel[];
}

interface CreateCustomerPreferenceModel {
    newsLetter: boolean;
    specials: boolean;
}

interface CreateCustomerCommandLoyaltyModel {
    loyaltyNo: string;
    points: number | null;
}

interface CreateCustomerCommandAddressesModel {
    line1: string;
    line2: string | null;
    city: string;
    postal: string;
    addressType: AddressType;
}

@IntentMerge()
@Component({
    selector: 'app-customer-add',
    standalone: true,
    templateUrl: 'customer-add.component.html',
    styleUrls: ['customer-add.component.scss'],
})
export class CustomerAddComponent implements OnInit {
    serviceErrors = {
        loadCategoriesError: null as string | null,
        createCustomerError: null as string | null,
        loadSubCategoriesError: null as string | null
    };

    isLoading = false;
    categoriesModels: CategoryDto[] | null = null;

    // main model used by the form
    model: CreateCustomerModel1 = {
        categoryId: '',
        subCategoryId: '',
        name: '',
        surname: '',
        email: '',
        isActive: false,
        preference: {
            newsLetter: false,
            specials: false
        },
        loyalty: {
            loyaltyNo: '',
            points: 0
        },
        addresses: []
    };

    subCategoriesModels: SubCategoryDto[] | null = null;

    // expose AddressType enum to the template
    AddressType = AddressType;

    // loyalty toggle flag (separate from model.loyalty)
    hasLoyalty = false;

    constructor(
        private router: Router,
        private readonly categoriesService: CategoriesService,
        private readonly customersService: CustomersService,
        private readonly subCategoriesService: SubCategoriesService
    ) { }

    ngOnInit(): void {
        // load categories when the page opens
        this.loadCategories();

        // ensure addresses collection exists
        if (!this.model.addresses) {
            this.model.addresses = [];
        }

        // default one Delivery address like the Blazor version
        if (this.model.addresses.length === 0) {
            this.model.addresses.push({
                line1: '',
                line2: null,
                city: '',
                postal: '',
                addressType: AddressType.Deliver
            });
        }

        // default: no loyalty; model.loyalty cleared
        this.hasLoyalty = false;
        this.model.loyalty = null;
    }

    // ---- UI helper methods (no direct service/router calls) -----------------

    /**
     * Called when category changes from the UI.
     * Resets subcategory and triggers loading of subcategories.
     */
    onCategoryChanged(categoryId: string | null): void {
        this.model.categoryId = categoryId ?? '';
        this.model.subCategoryId = '';
        this.subCategoriesModels = null;

        if (categoryId) {
            this.loadSubCategories(categoryId);
        }
    }

    /**
     * Add a new address, choosing Deliver for the first one,
     * then Billing for subsequent ones (similar to Blazor logic).
     */
    addAddress(): void {
        const hasDelivery = this.model.addresses.some(
            a => a.addressType === AddressType.Deliver
        );

        const nextType = hasDelivery ? AddressType.Billing : AddressType.Deliver;

        this.model.addresses.push({
            line1: '',
            line2: null,
            city: '',
            postal: '',
            addressType: nextType
        });
    }

    /**
     * Remove an address at the given index.
     */
    removeAddress(index: number): void {
        if (index >= 0 && index < this.model.addresses.length) {
            this.model.addresses.splice(index, 1);
        }
    }

    /**
     * Toggle loyalty info on/off. When enabled, ensure a loyalty
     * object exists; when disabled, clear it.
     */
    onHasLoyaltyChange(value: boolean): void {
        this.hasLoyalty = value;

        if (this.hasLoyalty) {
            if (!this.model.loyalty) {
                this.model.loyalty = {
                    loyaltyNo: '',
                    points: 0
                };
            }
        } else {
            this.model.loyalty = null;
        }
    }

    // ---- Service / navigation methods (left as generated) -------------------

    createCustomer(): void {
        this.serviceErrors.createCustomerError = null;
        this.isLoading = true;

        this.customersService
            .createCustomer({
                categoryId: this.model.categoryId!,
                subCategoryId: this.model.subCategoryId!,
                name: this.model.name,
                surname: this.model.surname,
                email: this.model.email,
                isActive: this.model.isActive,
                preference: {
                    newsLetter: this.model.preference.newsLetter,
                    specials: this.model.preference.specials
                },
                ...(this.model.loyalty && {
                    loyalty: {
                        loyaltyNo: this.model.loyalty.loyaltyNo,
                        points: this.model.loyalty.points ?? 0
                    }
                }),
                addresses: this.model.addresses.map(a => ({
                    line1: a.line1,
                    line2: a.line2!,
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
                    this.serviceErrors.createCustomerError = `Failed to call service: ${message}`;

                    console.error('Failed to call service:', err);
                }
            });
    }

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
}
