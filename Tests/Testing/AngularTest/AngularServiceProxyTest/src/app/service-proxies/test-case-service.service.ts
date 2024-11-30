import { EnumTestDto } from './models/models/net-application/services/integration/enum-test.dto';
import { FileDownloadDto } from './models/models/net-application/services/common/file-download.dto';
import { ApiService } from './api.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpHeaders } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class TestCaseService {
  constructor(private apiService: ApiService) {
  }

  public enumTest(): Observable<EnumTestDto> {
    let url = `/api/test-case/enum-test`;
    return this.apiService.get(url)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public download(): Observable<FileDownloadDto> {
    let url = `/api/test-case/download`;
    return this.apiService.get(url)
      .pipe(map((response: any) => {
        return response;
      }));
  }

  public upload(content: Stream, filename: string, contentType: string, contentLength: number): Observable<void> {
    let url = `/api/test-case/upload`;
    let headers = new HttpHeaders()
    .append("Content-Type", contentType)
    .append("Content-Length", contentLength)
    ;
    return this.apiService.post(url, {}, undefined, headers, 'text')
      .pipe(map((response: any) => {
        if (response && (response.startsWith("\"") || response.startsWith("'"))) { response = response.substring(1, response.length - 1); }
        return response;
      }));
  }
}