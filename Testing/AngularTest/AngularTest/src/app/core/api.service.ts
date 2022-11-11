import { Injectable } from '@angular/core';
import { environment } from "../../environments/environment";
import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IntentIgnore } from 'src/intent.decorators';

@IntentIgnore()
@Injectable()
export class ApiService {
  constructor(
    private http: HttpClient
  ) { }

  private formatErrors(error: any) {
    return throwError(error.error || error);
  }

  @IntentIgnore()
  get(path: string, params: HttpParams = new HttpParams(), headers: HttpHeaders = new HttpHeaders(), responseType:any = 'json'): Observable<any> {
    return this.http.get(`${environment.api_base_url}${path}`, { params, headers, responseType })
      .pipe(catchError(this.formatErrors));
  }

  @IntentIgnore()
  put(path: string, body: Object = {}, params: HttpParams = new HttpParams(), headers: HttpHeaders = new HttpHeaders(), responseType:any = 'json'): Observable<any> {
    headers = headers.append('Content-Type', 'application/json');

    return this.http.put(
      `${environment.api_base_url}${path}`,
      JSON.stringify(body),
      {
        params, headers, responseType
      }
    ).pipe(catchError(this.formatErrors));
  }

  @IntentIgnore()
  putWithFormData(path: string, formData: FormData, params: HttpParams = new HttpParams(), headers: HttpHeaders = new HttpHeaders(), responseType:any = 'json'): Observable<any> {
    return this.http.put(
      `${environment.api_base_url}${path}`,
      formData,
      { params, headers, responseType }
    ).pipe(catchError(this.formatErrors));
  }

  @IntentIgnore()
  post(path: string, body: Object = {}, params: HttpParams = new HttpParams(), headers: HttpHeaders = new HttpHeaders(), responseType:any = 'json'): Observable<any> {
    headers = headers.append('Content-Type', 'application/json');
    return this.http.post(
      `${environment.api_base_url}${path}`,
      JSON.stringify(body),
      { params, headers, responseType }
    ).pipe(catchError(this.formatErrors));
  }

  @IntentIgnore()
  postWithFormData(path: string, formData: FormData, params: HttpParams = new HttpParams(), headers: HttpHeaders = new HttpHeaders(), responseType:any = 'json'): Observable<any> {
    return this.http.post(
      `${environment.api_base_url}${path}`,
      formData,
      { params, headers, responseType }
    ).pipe(catchError(this.formatErrors));
  }

  @IntentIgnore()
  delete(path, params: HttpParams = new HttpParams(), headers: HttpHeaders = new HttpHeaders(), responseType:any = 'json'): Observable<any> {
    return this.http.delete(
      `${environment.api_base_url}${path}`,
      { params, headers, responseType }
    ).pipe(catchError(this.formatErrors));
  }
}