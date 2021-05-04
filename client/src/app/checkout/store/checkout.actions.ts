import { createAction, props } from "@ngrx/store";
import { IApiResponse } from "src/app/models/apiResponse";
import { IDeliveryMethod } from "src/app/models/deliveryMethod";

export const GETDELIVERYMETHOD_SUCCESS = '[Checkout] Get Delivery Method Success';
export const GETDELIVERYMETHOD = '[Checkout] Get Delivery Method';
export const FAILEDACTION = '[Checkout] Failed Action';

export const GetDeliveryMethod = createAction(GETDELIVERYMETHOD);
export const GetDeliveryMethodSuccess = createAction(GETDELIVERYMETHOD_SUCCESS, props<IApiResponse<IDeliveryMethod[]>>());
export const FailedAction = createAction(FAILEDACTION, props<{ error: any }>());
