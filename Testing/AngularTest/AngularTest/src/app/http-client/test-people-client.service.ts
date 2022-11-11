import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PersonCreateDTO } from './models/person-create.dto';
import { PersonDTO } from './models/person.dto';
import { PersonUpdateDTO } from './models/person-update.dto';
import { ApiService } from './../core/api.service';
import { HttpHeaders, HttpParams } from '@angular/common/http';
import { JsonResponse } from './json-response';

@Injectable()
export class TestPeopleClient {
  constructor(
    private apiService: ApiService
  ) {
  }

  public create(dto: PersonCreateDTO): Observable<string> {
    let url = `/api/people`;
    return this.apiService.post(url, dto)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public findById(id: string): Observable<PersonDTO> {
    let url = `/api/people/${id}`;
    return this.apiService.get(url)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public findAll(): Observable<PersonDTO[]> {
    let url = `/api/people`;
    return this.apiService.get(url)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public update(id: string, dto: PersonUpdateDTO): Observable<boolean> {
    let url = `/api/people/${id}`;
    return this.apiService.put(url, dto)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public delete(id: string): Observable<boolean> {
    let url = `/api/people/${id}`;
    return this.apiService.delete(url)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public getWithQueryParam(idParam: string): Observable<PersonDTO> {
    let url = `/api/people/getwithqueryparam`;
        //url = `${url}?idParam=${idParam}`;
    let httpParams = new HttpParams()
      .set("idParam", idParam);
    return this.apiService.get(url, httpParams)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public getWithRouteParam(routeId: string): Observable<PersonDTO> {
    let url = `/api/people/getwithrouteparam/${routeId}`;
    return this.apiService.get(url)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public postWithFormParam(param1: string, param2: string): Observable<boolean> {
    let url = `/api/people/postwithformparam`;
        //url = `${url}?param1=${param1}&param2=${param2}`;
    let formData: FormData = new FormData();
    formData.append("param1", param1);
    formData.append("param2", param2);
    return this.apiService.postWithFormData(url, formData)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public postWithHeaderParam(param: string): Observable<boolean> {
    let url = `/api/people/postwithheaderparam`;
        //url = `${url}?param=${param}`;
    let headers = new HttpHeaders()
      .append("MY_HEADER", param);
    return this.apiService.post(url, null, null, headers)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public postWithBodyParam(param: PersonUpdateDTO): Observable<boolean> {
    let url = `/api/people/postwithbodyparam`;
    return this.apiService.post(url, param)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public getWithPrimitiveResultInt(): Observable<number> {
    let url = `/api/people/getwithprimitiveresultint`;
    return this.apiService.get(url, null, null, "text")
      .pipe(map((response: string) => {
        if (response.startsWith("\"") || response.startsWith("'")) { response = response.substring(1, response.length - 2); }
        return Number(response);
      }));
  }

  public getWithPrimitiveResultBool(): Observable<boolean> {
    let url = `/api/people/getwithprimitiveresultbool`;
    return this.apiService.get(url, null, null, "text")
      .pipe(map((response: any) => {
        return Boolean(response);
      }));
  }

  public getWithPrimitiveResultStr(): Observable<string> {
    let url = `/api/people/getwithprimitiveresultstr`;
    return this.apiService.get(url, null, null, "text")
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public getWithPrimitiveResultWrapInt(): Observable<number> {
    let url = `/api/people/getwithprimitiveresultwrapint`;
    return this.apiService.get(url)
      .pipe(map((response: JsonResponse<number>) => {
        return response.value;
      }));
  }

  public getWithPrimitiveResultWrapBool(): Observable<boolean> {
    let url = `/api/people/getwithprimitiveresultwrapbool`;
    return this.apiService.get(url)
      .pipe(map((response: JsonResponse<boolean>) => {
        return response.value;
      }));
  }

  public getWithPrimitiveResultWrapStr(): Observable<string> {
    let url = `/api/people/getwithprimitiveresultwrapstr`;
    return this.apiService.get(url)
      .pipe(map((response: JsonResponse<string>) => {
        return response.value;
      }));
  }
}
