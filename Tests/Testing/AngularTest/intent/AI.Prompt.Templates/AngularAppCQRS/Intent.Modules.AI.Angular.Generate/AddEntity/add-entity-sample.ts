//@IntentMerge()
import { IntentIgnoreBody, IntentMerge, IntentIgnore } from './../../../intent/intent.decorators';
import { AddressType } from './../../../service-proxies/models/address-type';
import { CategoryDto } from './../../../service-proxies/models/my-back-end/services/categories/category-dto';
import { SubCategoryDto } from './../../../service-proxies/models/my-back-end/services/sub-categories/sub-category-dto';
import { CustomersService } from './../../../service-proxies/Customers/customers-service';
import { CategoriesService } from './../../../service-proxies/Categories/categories-service';
import { SubCategoriesService } from './../../../service-proxies/SubCategories/sub-categories-service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

interface CreateCustomerModel {
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
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatSlideToggleModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatProgressSpinnerModule
  ],
  templateUrl: 'customer-add.component.html',
  styleUrls: ['customer-add.component.scss'],
})
export class CustomerAddComponent implements OnInit {
  serviceErrors = {
    createCustomerError: null as string | null,
    loadCategoriesError: null as string | null,
    loadSubCategoriesError: null as string | null
  };
  isLoading = false;
  model: CreateCustomerModel = {
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
  categoriesModels: CategoryDto[] | null = null;
  subCategoriesModels: SubCategoryDto[] | null = null;

  AddressType = AddressType;
  hasLoyalty = false;

  //@IntentMerge()
  constructor(private router: Router, private readonly customersService: CustomersService, private readonly categoriesService: CategoriesService, private readonly subCategoriesService: SubCategoriesService) {
  }

  @IntentMerge()
  ngOnInit(): void {
    if (!this.model.addresses) {
      this.model.addresses = [];
    }
    if (this.model.addresses.length === 0) {
      this.model.addresses.push({
        line1: '',
        line2: null,
        city: '',
        postal: '',
        addressType: AddressType.Deliver
      });
    }
    this.hasLoyalty = false;
    this.model.loyalty = null;
    this.loadCategories();
  }

  addAddress(): void {
    const hasDelivery = this.model.addresses.some(a => a.addressType === AddressType.Deliver);
    const nextType = hasDelivery ? AddressType.Billing : AddressType.Deliver;
    this.model.addresses.push({
      line1: '',
      line2: null,
      city: '',
      postal: '',
      addressType: nextType
    });
  }

  removeAddress(index: number): void {
    if (index >= 0 && index < this.model.addresses.length) {
      this.model.addresses.splice(index, 1);
    }
  }

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

  onCategoryChanged(categoryId: string | null): void {
    this.model.categoryId = categoryId ?? '';
    this.model.subCategoryId = '';
    this.subCategoriesModels = null;
    if (categoryId) {
      this.loadSubCategories(categoryId);
    }
  }

  save(form: NgForm): void {
    if (this.isLoading || form.invalid) {
      return;
    }
    this.createCustomer();
    this.navigateToCustomerSearch();
  }

  @IntentMerge()
  createCustomer(): void {
    this.serviceErrors.createCustomerError = null;
    this.isLoading = true;
    
    this.customersService.createCustomer({
      categoryId: this.model.categoryId!,
      subCategoryId: this.model.subCategoryId!,
      name: this.model.name,
      surname: this.model.surname,
      email: this.model.email,
      isActive: this.model.isActive,
      preference: {
        newsLetter: this.model.preference.newsLetter,
        specials: this.model.preference.specials,
      },
      loyalty: this.model.loyalty
        ? {
            loyaltyNo: this.model.loyalty.loyaltyNo,
            points: this.model.loyalty.points!,
          }
        : null,
      addresses: this.model.addresses.map(a => ({
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
        this.serviceErrors.createCustomerError = `Failed to call service: ${message}`;

        console.error('Failed to call service:', err);
      }
    });
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

  navigateToCustomerEdit(customerId: string): void {
    this.router.navigate(['/customer', 'edit', customerId]);
  }

  navigateToCustomerSearch(): void {
    this.router.navigate(['/customer', 'search']);
  }
}
