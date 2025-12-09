import { IntentIgnoreBody, IntentMerge, IntentIgnore } from './../../../intent/intent.decorators';
import { CategoryDto } from './../../../service-proxies/models/my-back-end/services/categories/category.dto';
import { SubCategoryDto } from './../../../service-proxies/models/my-back-end/services/sub-categories/sub-category.dto';
import { CustomerDto } from './../../../service-proxies/models/my-back-end/services/customers/customer.dto';
import { CustomersService } from './../../../service-proxies/Customers/customers-service';
import { CategoriesService } from './../../../service-proxies/Categories/categories-service';
import { SubCategoriesService } from './../../../service-proxies/SubCategories/sub-categories-service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AddressType } from '../../../service-proxies/models/address-type';

interface ViewCustomerPreferenceModel {
    newsLetter: boolean;
    specials: boolean;
}

interface ViewCustomerLoyaltyModel {
    id: string | null;
    loyaltyNo: string;
    points: number | null;
}

interface ViewCustomerAddressModel {
    id: string | null;
    line1: string;
    line2: string | null;
    city: string;
    postal: string;
    addressType: AddressType;
}

interface ViewCustomerModel {
    id: string | null;
    categoryId: string | null;
    subCategoryId: string | null;
    name: string;
    surname: string;
    email: string;
    isActive: boolean | undefined;
    preference: ViewCustomerPreferenceModel;
    loyalty: ViewCustomerLoyaltyModel | null;
    addresses: ViewCustomerAddressModel[];
}

@IntentMerge()
@Component({
    selector: 'app-customer-view',
    standalone: true,
    templateUrl: 'customer-view.component.html',
    styleUrls: ['customer-view.component.scss'],
})
export class CustomerViewComponent implements OnInit {
    serviceErrors = {
        loadCustomerByIdError: null as string | null,
        loadCategoriesError: null as string | null,
        loadSubCategoriesError: null as string | null
    };

    isLoading = false;
    customerId: string = '';

    model: ViewCustomerModel | null = null;
    categoriesModels: CategoryDto[] | null = null;
    subCategoriesModels: SubCategoryDto[] | null = null;

    // for template enums & loyalty flag
    AddressType = AddressType;
    hasLoyalty = false;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private readonly customersService: CustomersService,
        private readonly categoriesService: CategoriesService,
        private readonly subCategoriesService: SubCategoriesService
    ) { }

    ngOnInit(): void {
        this.customerId = this.route.snapshot.paramMap.get('customerId') ?? '';

        this.loadCategories();

        if (this.customerId) {
            this.loadCustomerById(this.customerId);
        }
    }

    private loadCustomerById(id: string): void {
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
                next: (data) => {
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

                    this.hasLoyalty = !!data.loyalty;

                    if (data.categoryId) {
                        this.loadSubCategories(data.categoryId);
                    }
                },
                error: (err) => {
                    const message = err?.error?.message || err.message || 'Unknown error';
                    this.serviceErrors.loadCustomerByIdError = `Failed to call service: ${message}`;
                    console.error('Failed to call service:', err);
                }
            });
    }

    private loadCategories(): void {
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

    private loadSubCategories(categoryId: string): void {
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
        this.router.navigate(['/test', 'customer-search']);
    }
}
