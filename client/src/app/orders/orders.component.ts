import { animate, state, style, transition, trigger } from '@angular/animations';
import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource, _MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { AccountService } from '../account/account.service';
import { IBasketItem } from '../models/basket';
import { IOrder, IOrderItem, OrderListParam } from '../models/order';
import { OrdersService } from './orders.service';

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
export class OrdersComponent implements OnInit {
  displayedColumns = [
    'no', 'totalPrice', 'deliveryMethod', 'orderStatus', 'orderDate', 'detail'
  ];
  pageItems: MatTableDataSource<IOrder>;
  orderListParam: OrderListParam = new OrderListParam();
  orderList: IOrder[] = [];
  expandedElement: IBasketItem[] | null;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    private router: Router,
    private ordersService: OrdersService,
    private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.getOrderList();
  }

  getOrderList() {
    this.ordersService.getOrderListPaging(this.orderListParam)
      .subscribe((orderList: IOrder[]) => {
        console.log(orderList);
        this.orderList = orderList;
        this.pageItems = new MatTableDataSource<IOrder>(orderList);
        this.cdr.detectChanges();
        this.pageItems.paginator = this.paginator;
      })
  }
  goDetail(item: IOrder) {
    console.log(item);
    // this.ordersService.orderDetailState.next(item);
    // this.router.navigate(['/orders/detail']);
    this.router.navigate([`/orders/${item.id}`]);
  }

}

