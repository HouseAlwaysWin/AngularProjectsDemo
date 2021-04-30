import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { IBasketItem } from 'src/app/models/basket';
import { IOrder, IOrderItem } from 'src/app/models/order';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.scss']
})
export class OrderDetailComponent implements OnInit, OnDestroy {
  content: IOrderItem[];
  order: IOrder;
  displayCols: string[] = [
    'no', 'imgUrl', 'name', 'description', 'price', 'quantity'
  ];
  private _onDestroy = new Subject();

  constructor(
    private router: Router,
    private activeRoute: ActivatedRoute,
    private ordersService: OrdersService) {

    console.log(this.router.getCurrentNavigation().extras.state);
  }

  ngOnDestroy(): void {
    this._onDestroy.next();
  }

  ngOnInit(): void {
    // this.getContent();
    this.getOrder();
  }

  getOrder() {
    var id = this.activeRoute.snapshot.paramMap.get('id');
    this.ordersService.getOrderById(id)
      .pipe(takeUntil(this._onDestroy))
      .subscribe(order => {
        console.log(order);
        this.order = order;
        this.content = order.orderItems;
      });

    console.log(this.content);
  }

  getContent() {

    this.ordersService.orderDetailStateSource$
      .pipe(takeUntil(this._onDestroy))
      .subscribe(
        (item: IOrderItem[]) => {
          if (item) {
            this.content = item;
          } else {
            this.router.navigate(['/orders'])
          }
        });
  }

}
