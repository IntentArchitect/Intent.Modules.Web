import { Component, OnInit } from '@angular/core';
import { IntentIgnore } from './intent/intent.decorators';
import { IntegrationService } from './integration-service.service';
import { Observable, forkJoin } from 'rxjs';
import { CustomDTO } from './models/custom.dto';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  isCollapsed: boolean = false;

  //@IntentCanAdd()
  constructor(private integrationService: IntegrationService) { }

  @IntentIgnore()
  ngOnInit() {
  }

  @IntentIgnore()
  public performSuccessfulServiceCalls() : Observable<(string | number | void | string[] | CustomDTO)[]> {

    let calls = [
      this.integrationService.queryParamOp("param 1", 42),
      this.integrationService.headerParamOp("param 1"),
      this.integrationService.formParamOp("param 1", 42),
      this.integrationService.routeParamOp("param 1"),
      this.integrationService.bodyParamOp({referenceNumber: "refnumber_1234"}),
      this.integrationService.getWrappedPrimitiveGuid(),
      this.integrationService.getWrappedPrimitiveString(),
      this.integrationService.getWrappedPrimitiveInt(),
      this.integrationService.getPrimitiveGuid(),
      this.integrationService.getPrimitiveString(),
      this.integrationService.getPrimitiveInt(),
      this.integrationService.getPrimitiveStringList(),
      this.integrationService.getInvoiceOpWithReturnTypeWrapped()
    ];
    return forkJoin(calls);
  }

  @IntentIgnore()
  public performFailedServiceCall() : Observable<void> {
    return this.integrationService.throwsException();
  }
}
