import { createAction, props } from "@ngrx/store";
import { IApiResponse } from "src/app/models/apiResponse";
import { IBasket, IBasketItem, IBasketTotals } from "src/app/models/basket";
import { IDeliveryMethod } from "src/app/models/deliveryMethod";
import { IProduct } from "src/app/models/product";


export const GETBASKETBYID_SUCCESS = '[Basket] Get Basket By ID Success';
export const UPDATEBASKET_SUCCESS = '[Basket] Update Basket Success';
export const DELETEBASKET_SUCCESS = '[Basket] Delete Basket Success';
export const CREATEPAYMENTINTENT_SUCCESS = '[Basket] Create PaymentIntent Success';

export const GetBasetSuccess = createAction(GETBASKETBYID_SUCCESS, props<IApiResponse<IBasket>>());
export const UpdateBasketSuccess = createAction(UPDATEBASKET_SUCCESS, props<{ basket: IBasket }>());
export const DeleteBasketSuccess = createAction(DELETEBASKET_SUCCESS);
export const CreatePaymentIntentSuccess = createAction(CREATEPAYMENTINTENT_SUCCESS, props<IBasket>());



export const GETBASKET = '[Basket] Get Basket By ID';
export const GETBASKETITEM = '[Basket] Get Basket Item';
export const UPDATEBASEKT = '[Basket] Update Basket';
export const UPDATESHIPPING = '[Basket] Update Shipping';
export const UPDATEORADDBASKETITEM = '[Basket] Update or Add Basket Item';
export const ADDPRODUCTTOBASKET = '[Basket]  Add Product to basket';
export const REMOVEBASKETITEM = '[Basket] Remove Basket Item';
export const INCREMENTITEMQUANTITY = '[Basket] Increment item quantity';
export const DECREMENTITEMQUANTITY = '[Basket] Decrement item quantity';
export const DELETEBASKET = '[Basket] Delete Local Basket';
export const CREATEPAYMENTINTENT = '[Basket] Create paymentintent';
export const FAILEDACTION = '[Basket] Failed Action';

export const GetBasket = createAction(GETBASKET);
export const GetBasketItemById = createAction(GETBASKETITEM, props<{ id: string }>());
export const UpdateBasket = createAction(UPDATEBASEKT, props<IBasket>());
export const UpdateShipping = createAction(UPDATESHIPPING, props<IDeliveryMethod>());
export const UpdateOrAddBasketItem = createAction(UPDATEORADDBASKETITEM, props<{ basketItem: IBasketItem, id: string }>());
export const AddProductToBasket = createAction(ADDPRODUCTTOBASKET, props<{ productToAdd: IProduct, key: string, quantity: number }>());
export const RemoveBasketItem = createAction(REMOVEBASKETITEM, props<IBasketItem>());
export const IncrementItemQuantity = createAction(INCREMENTITEMQUANTITY, props<IBasketItem>());
export const DecrementItemQuantity = createAction(DECREMENTITEMQUANTITY, props<IBasketItem>());
export const DeleteBasket = createAction(DELETEBASKET);
export const CreatePaymentIntent = createAction(CREATEPAYMENTINTENT);
export const FailedAction = createAction(FAILEDACTION, props<{ error: any }>());

