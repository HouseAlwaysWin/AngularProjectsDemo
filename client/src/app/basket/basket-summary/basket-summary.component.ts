import { AfterViewInit, ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Observable } from 'rxjs';
import { IBasket, IBasketItem } from 'src/app/models/basket';
import { BasketService } from '../basket.service';
import * as appReducer from '../../store/app.reducer';
import * as BasketActions from '../store/basket.actions';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss']
})
export class BasketSummaryComponent implements OnInit {
  @Input() showQuantityAdj: boolean = true;
  @Input() hiddenRemove: boolean = false;
  pageItems: MatTableDataSource<IBasketItem>;

  @Output() increment: EventEmitter<IBasketItem> = new EventEmitter();
  @Output() decrement: EventEmitter<IBasketItem> = new EventEmitter();
  @Output() remove: EventEmitter<IBasketItem> = new EventEmitter();

  @ViewChild(MatPaginator) paginator: MatPaginator;

  displayedColumns = [
    'no', 'imgUrl', 'name', 'price', 'quantity', 'remove'
  ];

  constructor(private cdr: ChangeDetectorRef,
    private store: Store<appReducer.AppState>
  ) {
  }

  ngOnInit(): void {
    this.store.select('basket').subscribe(res => {
      if (res.basket) {
        this.pageItems = new MatTableDataSource<IBasketItem>(res.basket.basketItems);
        this.cdr.detectChanges();
        this.pageItems.paginator = this.paginator;
      }
    })

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
