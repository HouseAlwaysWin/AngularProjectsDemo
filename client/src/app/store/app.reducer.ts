import { ActionReducerMap } from "@ngrx/store";
import * as ShopReducer from "../shop/store/shop.reducer";
import * as BasketReducer from "../basket/store/basket.reducer";

export interface AppState {
  shop: ShopReducer.State,
  basket: BasketReducer.State
  // shopSearchReducer: ShopReducer.ShopSearchState
}


export const appReducer: ActionReducerMap<AppState> = {
  shop: ShopReducer.shopReducer,
  basket: BasketReducer.basektReducer
  // shopSearchReducer: ShopReducer.shopSearchReducer
}
