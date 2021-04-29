import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Inject } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { exhaustMap, map, mergeMap, switchMap, tap, withLatestFrom } from "rxjs/operators";
import { IApiPagingResponse, IApiResponse } from "src/app/models/apiResponse";
import { IProduct } from "src/app/models/product";
import { ShopParams } from "src/app/models/shopParams";
import { environment } from "src/environments/environment";
import { ShopService } from "../shop.service";
import * as ShopActions from "./shop.actions";


@Injectable()
export class ShopEffects {
  baseUrl = environment.apiUrl;

  constructor(
    private action$: Actions,
    private http: HttpClient,
    private shopService: ShopService) {
  }


  productList = createEffect(() => this.action$.pipe(
    ofType(ShopActions.GetProductList),
    switchMap((action) => {
      console.log(action);
      let params = new HttpParams();

      if (action.categoryId) {
        params = params.append('categoryId', action.categoryId.toString());
      }

      if (action.search) {
        params = params.append('search', action.sort);
      }

      params = params.append('sort', action.sort);
      params = params.append('pageIndex', action.pageIndex.toString());
      params = params.append('pageSize', action.pageSize.toString());
      params = params.append('loadPictures', true.toString());

      return this.http.get<IApiPagingResponse<IProduct[]>>(
        `${this.baseUrl}product`, { params })
        .pipe(
          map(res => {
            return ShopActions.GetProductListSuccess({
              products: res.data, totalCount: res.totalCount
            });
          }
          )
        );
    })
  ));


  productById = createEffect(() => this.action$.pipe(
    ofType(ShopActions.GetProductById),
    switchMap(action => {
      return this.http.get<IApiResponse<IProduct>>(
        `${this.baseUrl}product/${action.id}`
      ).pipe(
        map(res => {
          return ShopActions.GetProductByIdSuccess({ product: res.data });
        })
      );
    })
  ));


  autocomplete = createEffect(() => this.action$.pipe(
    ofType(ShopActions.AutoComplete),
    switchMap(action => {
      return this.shopService.getAutocomplete(action).pipe(
        map(res => ShopActions.GetAutocompleteSuccess({
          searchOptions: res.data
        }))
      )
    })
  ));

  search = createEffect(() => this.action$.pipe(
    ofType(ShopActions.Search),
    switchMap(action => {
      return this.shopService.getSearch(action).pipe(
        map(res =>
          ShopActions.GetProductListSuccess({
            products: res.data, totalCount: res.totalCount
          }))
      )
    })
  ));


  categories = createEffect(() => this.action$.pipe(
    ofType(ShopActions.GetCategories),
    switchMap(() => {
      return this.shopService.getCategories().pipe(
        map(res => ShopActions.GetProductCategoriesSuccess({
          productCategories: res.data
        })))
    })
  ))



}
