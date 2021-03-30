import { IAddress } from "./address";

export interface IOrderToCreate {
  basketId: string;
  deliveryMethodId: number;
  shipToAddress: IAddress
}

export interface IOrder {
  id: number;
  buyerEmail: string;
  orderDate: string;
  shipAddress: IAddress;
  deliveryMethod: string;
  totalPrice: number;
  orderItems: IOrderItem[],
  orderStatus: string;


}

export interface IOrderItem {

}
