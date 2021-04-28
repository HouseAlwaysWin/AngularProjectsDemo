import { createAction, props } from "@ngrx/store";
import { IProduct, IProductCategory } from "src/app/models/product";
import { ShopParams } from "src/app/models/shopParams";


export const PRODUCTLIST_SUCCESS = '[Shop] Product List Success';
export const AUTOCOMPLETE_SUCCESS = '[Shop] Autocomplete Success';
export const PRODUCT_SUCCESS = '[Shop] Product Success';
export const PRODUCTCATEGORIES_SUCCESS = '[Shop] ProductCategories Success';

export const PRODUCTLIST = '[Shop] Product List';
export const PRODUCTBYID = '[Shop] Product By Id';
export const AUTOCOMPLETE = '[Shop] Product Autocomplete';
export const SEARCH = '[Shop] Product Search';
export const CATEGORIES = '[Shop] Product Categories';


// output
export const GetProductListSuccess = createAction(PRODUCTLIST_SUCCESS, props<{ products: IProduct[], totalCount: number }>());
export const GetAutocompleteSuccess = createAction(AUTOCOMPLETE_SUCCESS, props<{ searchOptions: IProduct[] }>());
export const GetProductByIdSuccess = createAction(PRODUCT_SUCCESS, props<{ product: IProduct }>());
export const GetProductCategoriesSuccess = createAction(PRODUCTCATEGORIES_SUCCESS, props<{ productCategories: IProductCategory[] }>());

// input
export const GetProductList = createAction(PRODUCTLIST, props<ShopParams>());
export const GetProductById = createAction(PRODUCTBYID, props<{ id: string }>());
export const AutoComplete = createAction(AUTOCOMPLETE, props<ShopParams>());
export const Search = createAction(SEARCH, props<ShopParams>());
export const GetCategories = createAction(CATEGORIES);

