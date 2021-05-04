import { ActionReducerMap } from "@ngrx/store";
import * as ShopReducer from "../shop/store/shop.reducer";
import * as BasketReducer from "../basket/store/basket.reducer";
import * as CheckoutReducer from "../checkout/store/checkout.reducer";

export interface AppState {
  shop: ShopReducer.State,
  basket: BasketReducer.State
  category: ShopReducer.CategoryState,
  checkout: CheckoutReducer.State
}


export const appReducer: ActionReducerMap<AppState> = {
  shop: ShopReducer.shopReducer,
  category: ShopReducer.categoryReduct,
  basket: BasketReducer.basektReducer,
  checkout: CheckoutReducer.checkoutReducer
}
