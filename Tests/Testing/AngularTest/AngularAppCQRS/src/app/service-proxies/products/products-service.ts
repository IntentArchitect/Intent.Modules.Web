import { CreateProductCommand } from './../models/dot-net-back-end-service/services/products/create-product-command';import { ProductDto } from './../models/dot-net-back-end-service/services/products/product-dto';
import { UpdateProductCommand } from './../models/dot-net-back-end-service/services/products/update-product-command';
import { JsonResponse } from './../models/json-response';
import { Injectable } from '@angular/core';
import { environment } from './../../../environments/environment';
import { timeout, retry, map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  private readonly baseUrl = environment.dotNetBackEndServiceServiceConfig.services?.productsService?.baseUrl ?? environment.dotNetBackEndServiceServiceConfig.baseUrl;
  private readonly timeoutMs = environment.dotNetBackEndServiceServiceConfig.services?.productsService?.timeoutMs ?? environment.dotNetBackEndServiceServiceConfig.timeoutMs ?? 10_000;
  private readonly retries = environment.dotNetBackEndServiceServiceConfig.services?.productsService?.retries ?? environment.dotNetBackEndServiceServiceConfig.retries ?? 0;

  constructor(private httpClient: HttpClient) {
  }

  public createProduct(command: CreateProductCommand): Observable<string> {
    const url = `${this.baseUrl}api/products`;
    return this.httpClient.post<JsonResponse<string>>(url, command)
      .pipe(
        timeout(this.timeoutMs),
        retry(this.retries),
        map((response: JsonResponse<string>) => {
          return response.value;
      }));
  }

  public getProductById(id: string): Observable<ProductDto> {
    const url = `${this.baseUrl}api/products/${encodeURIComponent(id)}`;
    return this.httpClient.get<ProductDto>(url)
      .pipe(
        timeout(this.timeoutMs),
        retry(this.retries),
        map((response: ProductDto) => {
          return response;
      }));
  }

  public getProducts(): Observable<ProductDto[]> {
    const url = `${this.baseUrl}api/products`;
    return this.httpClient.get<ProductDto[]>(url)
      .pipe(
        timeout(this.timeoutMs),
        retry(this.retries),
        map((response: ProductDto[]) => {
          return response;
      }));
  }

  public updateProduct(command: UpdateProductCommand): Observable<void> {
    const url = `${this.baseUrl}api/products/${encodeURIComponent(command.id)}`;
    return this.httpClient.put<void>(url, command)
      .pipe(
        timeout(this.timeoutMs), 
        retry(this.retries)
      );
  }
}