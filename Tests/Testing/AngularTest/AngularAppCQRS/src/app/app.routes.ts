import { Routes } from '@angular/router';
import { ExamplePageComponent } from './pages/example-page/example-page.component';
import { HomeComponent } from './pages/home/home.component';
import { ProductAddPageComponent } from './pages/product-add-page/product-add-page.component';
import { ProductEditPageComponent } from './pages/product-edit-page/product-edit-page.component';
import { ProductListComponent } from './pages/product-list/product-list.component';

export const routes: Routes = [
  { path: 'example-page/:title', component: ExamplePageComponent },
  { path: '', component: HomeComponent },
  { path: 'product-add', component: ProductAddPageComponent },
  { path: 'product-edit/:productId', component: ProductEditPageComponent },
  { path: 'product-list', component: ProductListComponent },
];