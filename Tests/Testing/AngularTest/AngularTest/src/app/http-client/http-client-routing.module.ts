import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { IntentIgnore } from './../../intent.decorators';

const routes: Routes = [
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HttpClientRoutingModule { }
