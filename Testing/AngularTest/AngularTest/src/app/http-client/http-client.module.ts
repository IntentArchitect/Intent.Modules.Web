import { NgModule } from '@angular/core';
import { TestPeopleClient } from './test-people-client.service';
import { IntentIgnore, IntentMerge } from './../../intent.decorators';
import { HttpClientRoutingModule } from './http-client-routing.module';
import { CommonModule } from '@angular/common';

@IntentMerge()
@NgModule({
  declarations: [],
  providers: [
    TestPeopleClient
  ],
  imports: [
    CommonModule,
    HttpClientRoutingModule
  ]
})
export class HttpClientModule { }
