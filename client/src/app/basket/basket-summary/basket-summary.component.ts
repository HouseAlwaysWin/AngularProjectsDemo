import { AfterViewInit, ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Observable } from 'rxjs';
import { IBasket, IBasketItem } from 'src/app/models/basket';
import { BasketService } from '../basket.service';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss']
})
export class BasketSummaryComponent implements OnInit, AfterViewInit {
  @Input() items: Observable<IBasket>;
  pageItems: MatTableDataSource<IBasketItem>;

  @Output() increment: EventEmitter<IBasketItem> = new EventEmitter();
  @Output() decrement: EventEmitter<IBasketItem> = new EventEmitter();
  @Output() remove: EventEmitter<IBasketItem> = new EventEmitter();

  @ViewChild(MatPaginator) paginator: MatPaginator;

  displayedColumns = [
    'no', 'imgUrl', 'name', 'price', 'quantity', 'remove'
  ];

  constructor() {
  }

  ngOnInit(): void {
    this.items.subscribe((basket: IBasket) => {
      this.pageItems = new MatTableDataSource<IBasketItem>(basket.basketItems);
    })
  }

  ngAfterViewInit() {
    this.pageItems.paginator = this.paginator;
  }

  incrementItem(item: IBasketItem) {
    this.increment.emit(item);
  }

  decrementItem(item: IBasketItem) {
    this.decrement.emit(item);
  }

  removeBasketItem(item: IBasketItem) {
    this.remove.emit(item);
  }



}
