import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CollapseModule } from 'ngx-bootstrap/collapse';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { IntentIgnore, IntentMerge } from './../intent.decorators';
import { CoreModule } from './core/core.module';
import { HttpClientModule } from './http-client/http-client.module';
import { AppRoutingModule } from './app-routing.module';
  

@IntentMerge()
@NgModule({
  declarations: [
    AppComponent
  ],
  providers: [
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    CollapseModule.forRoot(),
    CoreModule,
    HttpClientModule,
    AppRoutingModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
