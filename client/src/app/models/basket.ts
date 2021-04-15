import { v4 as uuidv4 } from 'uuid';
import { IProduct } from './product';

export interface IBasket {
  id: string;
  basketItems: IBasketItem[];
  clientSecret?: string;
  paymentIntentId?: string;
  deliveryMethodId?: number;
  shippingPrice?: number;
}



export interface IBasketItem {
  id: number;
  name: string;
  description: string;
  price: number;
  imgUrl: string;
  productCategoryName: string;
  quantity: number
}


export class Basket implements IBasket {
  basketItems: IBasketItem[] = [];
  clientSecret?: string;
  paymentIntentId?: string;
  deliveryMethodId?: number;
  shippingPrice?: number;
  id: string = uuidv4();
}

export interface IBasketTotals {
  shipping: number;
  subtotal: number;
  total: number;
}
