import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IBasketItem } from 'src/app/models/basket';
import { IOrderItem } from 'src/app/models/order';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.scss']
})
export class OrderDetailComponent implements OnInit {
  content: IOrderItem[];
  displayCols: string[] = [
    'no', 'imgUrl', 'name', 'description', 'price', 'quantity'
  ];
  constructor(
    private router: Router,
    private ordersService: OrdersService) {

    console.log(this.router.getCurrentNavigation().extras.state);
  }

  ngOnInit(): void {
    this.getContent();
  }

  getContent() {

    this.ordersService.orderDetailStateSource$.subscribe(
      (item: IOrderItem[]) => {
        if (item) {
          this.content = item;
        } else {
          this.router.navigate(['/orders'])
        }
      });
  }

}
