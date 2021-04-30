import { ActionReducerMap } from "@ngrx/store";
import * as ShopReducer from "../shop/store/shop.reducer";
import * as BasketReducer from "../basket/store/basket.reducer";

export interface AppState {
  shop: ShopReducer.State,
  basket: BasketReducer.State
  category: ShopReducer.CategoryState
}


export const appReducer: ActionReducerMap<AppState> = {
  shop: ShopReducer.shopReducer,
  category: ShopReducer.categoryReduct,
  basket: BasketReducer.basektReducer
}
