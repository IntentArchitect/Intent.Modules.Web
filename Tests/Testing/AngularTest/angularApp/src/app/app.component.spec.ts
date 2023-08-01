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

  it('test aspnetcore - expect success', async () => {
    await performSuccessfulServiceCalls("https://localhost:44335");
  });

  it('test aspnetcore - expect exception', async () => {
    await performFailedServiceCall("https://localhost:44335");
  });

  it('test springboot - expect success', async () => {
    await performSuccessfulServiceCalls("http://localhost:8080");
  });

  it('test springboot - expect exception', async () => {
    await performFailedServiceCall("http://localhost:8080");
  });

  it('test nestjs - expect success', async () => {
    await performSuccessfulServiceCalls("http://localhost:3000");
  });

  it('test nestjs - expect exception', async () => {
    await performFailedServiceCall("http://localhost:3000");
  });
});

async function performSuccessfulServiceCalls(url: string) {
  environment.api_base_url = url;
  const fixture = TestBed.createComponent(AppComponent);
  let aggregateObservable = fixture.componentInstance.performSuccessfulServiceCalls();
  let fetchedResults = await lastValueFrom(aggregateObservable);
  expect(fetchedResults).toEqual([
    { ReferenceNumber: 'refnumber_1234' }, '', '', '', 'b7698947-5237-4686-9571-442335426771', 'string value', 55, 'b7698947-5237-4686-9571-442335426771', 'string value', '55', ['string value'], undefined
  ]);
}

async function performFailedServiceCall(url: string) {
  environment.api_base_url = url;
  const fixture = TestBed.createComponent(AppComponent);
  let aggregateObservable = fixture.componentInstance.performFailedServiceCall();
  await expectAsync(lastValueFrom(aggregateObservable)).toBeRejected();
}