import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PersonCreateDTO } from './models/person-create.dto';
import { PersonDTO } from './models/person.dto';
import { PersonUpdateDTO } from './models/person-update.dto';
import { DateDTO } from './models/date.dto';
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
    return this.apiService.post(url, dto, null, null, 'text')
      .pipe(map((response: any) => {
        if (response && (response.startsWith("\"") || response.startsWith("'"))) { response = response.substring(1, response.length - 1); }
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
}
