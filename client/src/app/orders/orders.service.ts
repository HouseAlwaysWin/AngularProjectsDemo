import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IApiPagingResponse, IApiResponse } from '../models/apiResponse';
import { IBasketItem } from '../models/basket';
import { IOrder, IOrderItem, OrderListParam } from '../models/order';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  orderDetailState: BehaviorSubject<IOrderItem[]> = new BehaviorSubject<IOrderItem[]>(null);
  orderDetailStateSource$: Observable<IOrderItem[]> = this.orderDetailState.asObservable();
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }


  getOrderListPaging(orderParam: OrderListParam) {
    let params = new HttpParams();
    params = params.append('sort', orderParam.sort);
    params = params.append('pageIndex', orderParam.pageIndex.toString());
    params = params.append('pageSize', orderParam.pageSize.toString());

    return this.http.get(`${this.baseUrl}orders/list`, { params }).pipe(
      map((res: IApiPagingResponse<IOrder[]>) => {
        console.log(res);
        if (res.isSuccessed) {
          return res.data;
        }
      }));
  }

  getOrderById(id: string) {
    let params = new HttpParams();
    return this.http.get(`${this.baseUrl}orders/${id}`).pipe(
      map((res: IApiResponse<IOrder>) => {
        if (res.isSuccessed) {
          return res.data;
        }
      }))
  }
}
