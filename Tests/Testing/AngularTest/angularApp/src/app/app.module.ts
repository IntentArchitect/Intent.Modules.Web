import { NgModule } from '@angular/core';
import { ApiAuthorizationModule } from './api-authorization/api-authorization.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthorizeInterceptor } from './api-authorization/authorize.interceptor';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { IntegrationService } from './integration-service.service';
import { IntentIgnore, IntentMerge } from './intent/intent.decorators';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from './core/core.module';
import { AppRoutingModule } from './app-routing.module';
  

@IntentMerge()
@NgModule({
  declarations: [
    AppComponent
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    IntegrationService
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    ApiAuthorizationModule,
    HttpClientModule,
    CollapseModule.forRoot(),
    CoreModule,
    AppRoutingModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
