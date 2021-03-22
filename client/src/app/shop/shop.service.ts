import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IApiPagingResponse, IApiResponse } from '../models/apiResponse';
import { IProduct } from '../models/product';
import { IProductBrand } from '../models/productBrand';
import { IProductCategory } from '../models/productCategory';
import { ShopParams } from '../models/shopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }


  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();

    if (shopParams.brandId !== 0) {
      params = params.append('brandId', shopParams.brandId.toString());
    }

    if (shopParams.categoryId !== 0) {
      params = params.append('categoryId', shopParams.categoryId.toString());
    }

    if (shopParams.search) {
      params = params.append('search', shopParams.sort);
    }

    params = params.append('sort', shopParams.sort);
    params = params.append('pageIndex', shopParams.pageIndex.toString());
    params = params.append('pageSize', shopParams.pageSize.toString());

    var response = this.http.get<IApiPagingResponse<IProduct[]>>(
      `${this.baseUrl}product/`, { observe: 'response', params })
      .pipe(
        map(response => {
          return response.body;
        })
      );

    return response;
  }

  getBrands() {
    return this.http.get<IApiResponse<IProductBrand[]>>(
      `${this.baseUrl}product/brands`
    )
  }

  getCategories() {
    return this.http.get<IApiResponse<IProductCategory[]>>(
      `${this.baseUrl}product/categories`
    )
  }

}
