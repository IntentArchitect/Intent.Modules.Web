import { EnumTestDto } from './models/models/net-application/services/integration/enum-test.dto';
import { ApiService } from './api.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';


@Injectable({ providedIn: 'root' })
export class IntegrationService {
  constructor(private apiService: ApiService) {
  }

  public enumTest(): Observable<EnumTestDto> {
    let url = `/api/integration/enum-test`;
    return this.apiService.get(url)
      .pipe(map((response: any) => {
        return response;
      }));
  }
}