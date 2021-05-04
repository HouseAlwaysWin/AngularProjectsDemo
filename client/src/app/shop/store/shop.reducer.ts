import { state } from "@angular/animations";
import { Action, createReducer, createReducerFactory, on } from "@ngrx/store";
import { IApiPagingResponse } from "src/app/models/apiResponse";
import { IProduct, IProductCategory } from "src/app/models/product";
import { ShopParams } from "src/app/models/shopParams";
import * as ShopActions from "./shop.actions";



export interface State {
  products: IProduct[],
  searchOptions: IProduct[],
  product: IProduct,
  totalCount: number,
  loading: boolean;
}

export interface CategoryState {
  productCategories: IProductCategory[],
}


const initialState: State = {
  products: [],
  searchOptions: [],
  product: null,
  totalCount: 0,
  loading: false
}

const categoryState: CategoryState = {
  productCategories: []
}

export const categoryReduct = createReducer(
  categoryState,
  on(ShopActions.GetCategories, (state, action) => ({
    ...state,
    loading: true
  })),
  on(ShopActions.GetProductCategoriesSuccess, (state, action) => ({
    ...state,
    productCategories: action.productCategories,
    loading: false
  })),

)


export const shopReducer = createReducer(
  initialState,

  on(ShopActions.GetProductListSuccess, (state, action) => ({
    ...state,
    products: action.products,
    totalCount: action.totalCount,
    loading: false
  })),

  on(ShopActions.GetAutocompleteSuccess, (state, action) => ({
    ...state,
    searchOptions: action.searchOptions,
    loading: false
  })),

  on(ShopActions.GetProductByIdSuccess, (state, action) => {
    return ({
      ...state,
      product: action.product,
    })
  }),

  on(ShopActions.GetProductList, (state, action) => ({
    ...state,
    loading: true
  })),

  on(ShopActions.Search, (state, action) => ({
    ...state,
    loading: true
  })),


);

