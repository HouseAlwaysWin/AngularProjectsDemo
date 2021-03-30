import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IApiResponse } from '../models/apiResponse';
import { IDeliveryMethod } from '../models/deliveryMethod';
import { IOrderToCreate } from '../models/order';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  createOrder(order: IOrderToCreate) {
    return this.http.post(this.baseUrl + 'order/create', order);
  }

  getDeliveryMethods() {
    return this.http.get(this.baseUrl + 'order/deliveryMethods').pipe(
      map((dm: IApiResponse<IDeliveryMethod[]>) => {
        return dm.data.sort((a, b) => b.price - a.price);
      }));
  }


}
