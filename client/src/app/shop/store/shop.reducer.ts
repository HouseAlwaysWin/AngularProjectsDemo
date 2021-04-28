import { state } from "@angular/animations";
import { Action, createReducer, on } from "@ngrx/store";
import { IApiPagingResponse } from "src/app/models/apiResponse";
import { IProduct, IProductCategory } from "src/app/models/product";
import { ShopParams } from "src/app/models/shopParams";
import * as ShopActions from "./shop.actions";



export interface State {
  products: IProduct[],
  searchOptions: IProduct[],
  product: IProduct,
  productCategories: IProductCategory[],
  totalCount: number,
  loading: boolean;
}


const initialState: State = {
  products: [],
  searchOptions: [],
  product: null,
  productCategories: [],
  totalCount: 0,
  loading: false
}


export const shopReducer = createReducer(
  initialState,

  // productlist output
  on(ShopActions.GetProductListSuccess, (state, action) => ({
    ...state,
    products: action.products,
    totalCount: action.totalCount,
    loading: false
  })),

  // productlist output
  on(ShopActions.GetAutocompleteSuccess, (state, action) => ({
    ...state,
    searchOptions: action.searchOptions,
    loading: false
  })),

  // product ouutput
  on(ShopActions.GetProductByIdSuccess, (state, action) => ({
    ...state,
    product: action.product,
  })),

  // categories output
  on(ShopActions.GetProductCategoriesSuccess, (state, action) => ({
    ...state,
    productCategories: action.productCategories,
    loading: false
  })),

  // product input
  on(ShopActions.GetProductList, (state, action) => ({
    ...state,
    loading: true
  })),

  // // autocomplete input
  on(ShopActions.AutoComplete, (state, action) => ({
    ...state,
  })),

  // search products input
  on(ShopActions.Search, (state, action) => ({
    ...state,
    loading: true
  })),

  on(ShopActions.GetCategories, (state, action) => ({
    ...state,
    loading: true
  }))

);

