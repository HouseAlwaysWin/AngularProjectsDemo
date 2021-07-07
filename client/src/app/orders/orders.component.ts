import { animate, state, style, transition, trigger } from '@angular/animations';
import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource, _MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { AccountService } from '../account/account.service';
import { IBasketItem } from '../models/basket';
import { IOrder, IOrderItem, OrderListParam } from '../models/order';
import { OrdersService } from './orders.service';
import moment from 'moment';
import { map, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ])
  ]
})
export class OrdersComponent implements OnInit, OnDestroy {
  displayedColumns = [
    'no', 'totalPrice', 'deliveryMethod', 'orderStatus', 'orderDate', 'detail'
  ];
  pageItems: MatTableDataSource<IOrder>;
  orderListParam: OrderListParam = new OrderListParam();
  orderList: IOrder[] = [];
  expandedElement: IBasketItem[] | null;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  private _onDestroy = new Subject();

  constructor(
    private router: Router,
    private ordersService: OrdersService,
    private cdr: ChangeDetectorRef) { }
  ngOnDestroy(): void {
    this._onDestroy.next();
  }

  ngOnInit(): void {
    this.getOrderList();
  }

  getOrderList() {
    this.ordersService.getOrderListPaging(this.orderListParam)
      .pipe(
        map((orders: IOrder[]) => {
          orders.forEach(o => {
            o.orderDate = moment.utc(o.orderDate + "-08:00").format('YYYY-MM-DD hh:mm:ss a');
          });
          return orders;
        }),
      )
      .pipe(takeUntil(this._onDestroy))
      .subscribe((orderList: IOrder[]) => {
        this.orderList = orderList;
        this.pageItems = new MatTableDataSource<IOrder>(orderList);
        this.cdr.detectChanges();
        this.pageItems.paginator = this.paginator;
      })
  }
  goDetail(item: IOrder) {
    this.router.navigate([`/orders/${item.id}`]);
  }


}

