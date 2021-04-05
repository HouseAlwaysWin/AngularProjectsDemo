import { IAddress } from "./address";
import { IDeliveryMethod } from "./deliveryMethod";

export interface IOrderToCreate {
  basketId: string;
  deliveryMethodId: number;
  shipToAddress: IAddress
}

export class OrderListParam {
  sort: string = '';
  pageIndex: number = 1;
  pageSize: number = 10;
}

export interface IOrder {
  id: number
  buyerName: string,
  buyerEmail: string
  totalPrice: number
  orderDate: string
  orderAddress: IAddress
  deliveryMethod: IDeliveryMethod
  orderItems: IOrderItem[]
  orderStatus: string
  paymentIntentId: string
}
export interface IOrderItem {
  id: number
  name: string
  description: string
  price: number
  imgUrl: string
  langCode: string
  quantity: number
  createdDate: string
}

