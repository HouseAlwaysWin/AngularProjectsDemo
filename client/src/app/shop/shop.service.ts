import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IApiPagingResponse, IApiResponse } from '../models/apiResponse';
import { IProduct, IProductCategory } from '../models/product';
import { ShopParams } from '../models/shopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = environment.apiUrl;

  productListSub: BehaviorSubject<IApiPagingResponse<IProduct[]>> = new BehaviorSubject(null);
  productList$: Observable<IApiPagingResponse<IProduct[]>> = this.productListSub.asObservable();
  productCategoriesSub: BehaviorSubject<IProductCategory[]> = new BehaviorSubject(null);
  productCategories$: Observable<IProductCategory[]> = this.productCategoriesSub.asObservable();

  constructor(private http: HttpClient) { }


  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();


    if (shopParams.categoryId) {
      params = params.append('categoryId', shopParams.categoryId.toString());
    }

    if (shopParams.search) {
      params = params.append('search', shopParams.sort);
    }

    params = params.append('sort', shopParams.sort);
    params = params.append('pageIndex', shopParams.pageIndex.toString());
    params = params.append('pageSize', shopParams.pageSize.toString());
    params = params.append('loadPictures', true.toString());

    var response = this.http.get<IApiPagingResponse<IProduct[]>>(
      `${this.baseUrl}product`, { params })
      .pipe(
        map(response => {
          if (response.isSuccessed) {
            this.productListSub.next(response);
          }
          return response;
        })
      );

    return response;
  }

  getProductById(id: string) {
    return this.http.get<IApiResponse<IProduct>>(
      `${this.baseUrl}product/${id}`
    ).pipe(
      map(response => {
        return response.data;
      })
    );
  }

  getAutocomplete(shopParams: ShopParams) {
    let params = new HttpParams();

    if (shopParams.search) {
      params = params.append('search', shopParams.search);
    }
    params = params.append('pageIndex', shopParams.pageIndex.toString());
    params = params.append('pageSize', shopParams.pageSize.toString());

    var response = this.http.get<IApiPagingResponse<IProduct[]>>(
      `${this.baseUrl}product/autocomplete`, { observe: 'response', params })
      .pipe(
        map(response => {
          return response.body;
        })
      );

    return response;
  }

  getSearch(shopParams: ShopParams) {
    let params = new HttpParams();

    if (shopParams.search) {
      params = params.append('search', shopParams.search);
    }
    params = params.append('pageIndex', shopParams.pageIndex.toString());
    params = params.append('pageSize', shopParams.pageSize.toString());

    var response = this.http.get<IApiPagingResponse<IProduct[]>>(
      `${this.baseUrl}product/search`, { observe: 'response', params })
      .pipe(
        map(response => {
          this.productListSub.next(response.body);
          return response.body;
        })
      );

    return response;
  }

  getCategories() {
    return this.http.get<IApiResponse<IProductCategory[]>>(
      `${this.baseUrl}product/categoriestree`
    ).pipe(
      map(response => {
        if (response.isSuccessed) {
          this.productCategoriesSub.next(response.data);
        }
        else {
          console.log(response.message);
        }
        return response;
      })
    )
  }

}
