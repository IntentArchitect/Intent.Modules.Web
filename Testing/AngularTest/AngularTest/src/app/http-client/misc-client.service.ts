import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PersonDTO } from './models/person.dto';
import { PersonUpdateDTO } from './models/person-update.dto';
import { DateDTO } from './models/date.dto';
import { ApiService } from './../core/api.service';
import { JsonResponse } from './../../json-response';
import { HttpParams, HttpHeaders } from '@angular/common/http';

@Injectable()
export class MiscClient {
  constructor(
    private apiService: ApiService
  ) {
  }

  public getWithQueryParam(idParam: string): Observable<PersonDTO> {
    let url = `/api/misc/getwithqueryparam`;
    let httpParams = new HttpParams()
      .set("idParam", idParam)
    ;
    return this.apiService.get(url, httpParams)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public getWithRouteParam(routeId: string): Observable<PersonDTO> {
    let url = `/api/misc/getwithrouteparam/${routeId}`;
    return this.apiService.get(url)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public postWithHeaderParam(param: string): Observable<boolean> {
    let url = `/api/misc/postwithheaderparam`;
    let headers = new HttpHeaders()
      .append("MY_HEADER", param)
    ;
    return this.apiService.post(url, {}, null, headers)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public postWithBodyParam(param: PersonUpdateDTO): Observable<boolean> {
    let url = `/api/misc/postwithbodyparam`;
    return this.apiService.post(url, param)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public getWithPrimitiveResultInt(): Observable<number> {
    let url = `/api/misc/getwithprimitiveresultint`;
    return this.apiService.get(url, null, null, 'text')
      .pipe(map((response: any) => {
        if (response && (response.startsWith("\"") || response.startsWith("'"))) { response = response.substring(1, response.length - 2); }
        return response;
      }));
  }

  public getWithPrimitiveResultWrapInt(): Observable<number> {
    let url = `/api/misc/getwithprimitiveresultwrapint`;
    return this.apiService.get(url)
      .pipe(map((response: JsonResponse<number>) => {
        return response.value;
      }));
  }

  public getWithPrimitiveResultBool(): Observable<boolean> {
    let url = `/api/misc/getwithprimitiveresultbool`;
    return this.apiService.get(url, null, null, 'text')
      .pipe(map((response: any) => {
        if (response && (response.startsWith("\"") || response.startsWith("'"))) { response = response.substring(1, response.length - 2); }
        return response;
      }));
  }

  public getWithPrimitiveResultWrapBool(): Observable<boolean> {
    let url = `/api/misc/getwithprimitiveresultwrapbool`;
    return this.apiService.get(url)
      .pipe(map((response: JsonResponse<boolean>) => {
        return response.value;
      }));
  }

  public getWithPrimitiveResultStr(): Observable<string> {
    let url = `/api/misc/getwithprimitiveresultstr`;
    return this.apiService.get(url, null, null, 'text')
      .pipe(map((response: any) => {
        if (response && (response.startsWith("\"") || response.startsWith("'"))) { response = response.substring(1, response.length - 2); }
        return response;
      }));
  }

  public getWithPrimitiveResultWrapStr(): Observable<string> {
    let url = `/api/misc/getwithprimitiveresultwrapstr`;
    return this.apiService.get(url)
      .pipe(map((response: JsonResponse<string>) => {
        return response.value;
      }));
  }

  public getWithPrimitiveResultDate(): Observable<Date> {
    let url = `/api/misc/getwithprimitiveresultdate`;
    return this.apiService.get(url, null, null, 'text')
      .pipe(map((response: any) => {
        if (response && (response.startsWith("\"") || response.startsWith("'"))) { response = response.substring(1, response.length - 2); }
        return response;
      }));
  }

  public getWithPrimitiveResultWrapDate(): Observable<Date> {
    let url = `/api/misc/getwithprimitiveresultwrapdate`;
    return this.apiService.get(url)
      .pipe(map((response: JsonResponse<Date>) => {
        return response.value;
      }));
  }

  public postDateParam(date: Date, datetime: Date): Observable<boolean> {
    let url = `/api/misc/postdateparam`;
    let httpParams = new HttpParams()
      .set("date", date.toISOString())
      .set("datetime", datetime.toISOString())
    ;
    return this.apiService.post(url, {}, httpParams)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public postDateParamDto(dto: DateDTO): Observable<boolean> {
    let url = `/api/misc/postdateparamdto`;
    return this.apiService.post(url, dto)
      .pipe(map((response: any) => {
        return response;
      }));
  }
}
