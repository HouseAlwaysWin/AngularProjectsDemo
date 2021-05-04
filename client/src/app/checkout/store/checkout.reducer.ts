import { createReducer, on } from "@ngrx/store";
import { IDeliveryMethod } from "src/app/models/deliveryMethod";
import * as CheckoutActions from './checkout.actions';



export interface State {
  deliveryMethods: IDeliveryMethod[],
}


const checkoutState: State = {
  deliveryMethods: []
}

export const checkoutReducer = createReducer(
  checkoutState,
  on(CheckoutActions.GetDeliveryMethodSuccess, (state, action) => ({
    ...state,
    deliveryMethods: action.data
  })),

)
