import { ClientCreateDto } from './../models/net-application/services/net-client/client-create.dto';import { ClientDto } from './../models/net-application/services/net-client/client.dto';
import { ApiService } from './service-proxies/api.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResult } from './../paged-result';
import { HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';


@Injectable()
export class NetClientService {
  constructor(private apiService: ApiService) {
  }

  public createClient(dto: ClientCreateDto): Observable<string> {
    let url = `/api/net-client`;
    return this.apiService.post(url, dto, undefined, undefined, 'text')
      .pipe(map((response: any) => {
        if (response && (response.startsWith("\"") || response.startsWith("'"))) { response = response.substring(1, response.length - 1); }
        return response;
      }));
  }

  public findClients(pageNo: number, pageSize: number): Observable<PagedResult<ClientDto>> {
    let url = `/api/net-client/paginate/${pageNo}`;
    let httpParams = new HttpParams()
    .set("pageSize", pageSize)
    ;
    return this.apiService.get(url, httpParams)
      .pipe(map((response: any) => {
        return response;
      }));
  }
}