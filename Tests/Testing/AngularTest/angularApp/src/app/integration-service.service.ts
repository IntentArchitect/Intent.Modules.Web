import { CustomDTO } from './../models/net-application/services/integration/custom.dto';
import { ApiService } from './core/api.service';
import { JsonResponse } from './../json-response';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpParams, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';


@Injectable()
export class IntegrationService {
  constructor(private apiService: ApiService) {
  }

  public queryParamOp(param1: string, param2: number): Observable<CustomDTO> {
    let url = `/api/integration/query-param-op`;
    let httpParams = new HttpParams()
    .set("param1", param1)
    .set("param2", param2)
    ;
    return this.apiService.get(url, httpParams)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public headerParamOp(param1: string): Observable<void> {
    let url = `/api/integration/header-param-op`;
    let headers = new HttpHeaders()
    .append("MY-HEADER", param1)
    ;
    return this.apiService.post(url, {}, undefined, headers, 'text')
      .pipe(map((response: any) => {
        if (response && (response.startsWith("\"") || response.startsWith("'"))) { response = response.substring(1, response.length - 1); }
        return response;
      }));
  }

  public routeParamOp(param1: string): Observable<void> {
    let url = `/api/integration/route-param-op/${param1}`;
    return this.apiService.post(url, {}, undefined, undefined, 'text')
      .pipe(map((response: any) => {
        if (response && (response.startsWith("\"") || response.startsWith("'"))) { response = response.substring(1, response.length - 1); }
        return response;
      }));
  }

  public bodyParamOp(param1: CustomDTO): Observable<void> {
    let url = `/api/integration/body-param-op`;
    return this.apiService.post(url, param1, undefined, undefined, 'text')
      .pipe(map((response: any) => {
        if (response && (response.startsWith("\"") || response.startsWith("'"))) { response = response.substring(1, response.length - 1); }
        return response;
      }));
  }

  public throwsException(): Observable<void> {
    let url = `/api/integration/throws-exception`;
    return this.apiService.post(url, {}, undefined, undefined, 'text')
      .pipe(map((response: any) => {
        if (response && (response.startsWith("\"") || response.startsWith("'"))) { response = response.substring(1, response.length - 1); }
        return response;
      }));
  }

  public getWrappedPrimitiveGuid(): Observable<string> {
    let url = `/api/integration/wrapped-primitive-guid`;
    return this.apiService.get(url)
      .pipe(map((response: JsonResponse<string>) => {
        return response.value;
      }));
  }

  public getWrappedPrimitiveString(): Observable<string> {
    let url = `/api/integration/wrapped-primitive-string`;
    return this.apiService.get(url)
      .pipe(map((response: JsonResponse<string>) => {
        return response.value;
      }));
  }

  public getWrappedPrimitiveInt(): Observable<number> {
    let url = `/api/integration/wrapped-primitive-int`;
    return this.apiService.get(url)
      .pipe(map((response: JsonResponse<number>) => {
        return response.value;
      }));
  }

  public getPrimitiveGuid(): Observable<string> {
    let url = `/api/integration/primitive-guid`;
    return this.apiService.get(url, undefined, undefined, 'text')
      .pipe(map((response: any) => {
        if (response && (response.startsWith("\"") || response.startsWith("'"))) { response = response.substring(1, response.length - 1); }
        return response;
      }));
  }

  public getPrimitiveString(): Observable<string> {
    let url = `/api/integration/primitive-string`;
    return this.apiService.get(url, undefined, undefined, 'text')
      .pipe(map((response: any) => {
        if (response && (response.startsWith("\"") || response.startsWith("'"))) { response = response.substring(1, response.length - 1); }
        return response;
      }));
  }

  public getPrimitiveInt(): Observable<number> {
    let url = `/api/integration/primitive-int`;
    return this.apiService.get(url, undefined, undefined, 'text')
      .pipe(map((response: any) => {
        if (response && (response.startsWith("\"") || response.startsWith("'"))) { response = response.substring(1, response.length - 1); }
        return response;
      }));
  }

  public getPrimitiveStringList(): Observable<string[]> {
    let url = `/api/integration/primitive-string-list`;
    return this.apiService.get(url)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public getInvoiceOpWithReturnTypeWrapped(): Observable<CustomDTO> {
    let url = `/api/integration/invoice-op-with-return-type-wrapped`;
    return this.apiService.get(url)
      .pipe(map((response: JsonResponse<CustomDTO>) => {
        return response.value;
      }));
  }
}