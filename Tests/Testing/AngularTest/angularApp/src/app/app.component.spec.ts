import { TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { IntegrationService } from './integration-service.service';
import { ApiService } from './core/api.service';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { environment } from 'src/environments/environment';
import { lastValueFrom } from 'rxjs';

describe('AppComponent', () => {
  beforeEach(() => TestBed.configureTestingModule({
    declarations: [AppComponent],
    imports: [HttpClientModule, AppRoutingModule],
    providers: [IntegrationService, ApiService]
  }));

  it('test aspnet core services - success', async () => {
    environment.api_base_url = "https://localhost:44335";
    const fixture = TestBed.createComponent(AppComponent);
    let aggregateObservable = fixture.componentInstance.performSuccessfulServiceCalls();
    let fetchedResults = await lastValueFrom(aggregateObservable);
    expect(fetchedResults).toEqual([
      { referenceNumber: 'refnumber_1234' }, '', '', '', '', 'b7698947-5237-4686-9571-442335426771', 'string value', 55, 'b7698947-5237-4686-9571-442335426771', 'string value', '55', ['string value'], undefined
    ]);
  });

  it('test aspnet core services - failed', async () => {
    environment.api_base_url = "https://localhost:44335";
    const fixture = TestBed.createComponent(AppComponent);
    let aggregateObservable = fixture.componentInstance.performFailedServiceCall();
    await expectAsync(lastValueFrom(aggregateObservable)).toBeRejected();
  });
});
