import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Observable, Subscription } from 'rxjs';
import { IBasket, IBasketItem, IBasketTotals } from '../models/basket';
import { DialogComfirm } from '../shared/components/dialog-comfirm/dialog-comfirm.component';
import { BasketService } from './basket.service';
import * as appReducer from '../store/app.reducer'
import * as BasketActions from '../basket/store/basket.actions';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent implements OnInit {
  basket: IBasket;
  basketTotals: IBasketTotals;

  displayedColumns: string[] = []
  constructor(
    public translate: TranslateService,
    private store: Store<appReducer.AppState>,
    public dialog: MatDialog) { }


  openDialog() {
    const dialogRef = this.dialog.open(DialogComfirm, {
      disableClose: false,
      width: '500px',
      role: 'alertdialog',
    });

    dialogRef.afterClosed().subscribe(result => {
      this._SetBasketState();
    });

  }

  ngOnInit(): void {
    this._SetBasketState();
  }

  private _SetBasketState() {
    this.store.select('basket').subscribe(res => {
      this.basket = res.basket;
      this.basketTotals = res.basketTotal
    });
  }

  incrementItemQuantity(item: IBasketItem) {
    this.store.dispatch(BasketActions.IncrementItemQuantity(item));
  }

  decrementItemQuantity(item: IBasketItem) {
    this.store.dispatch(BasketActions.DecrementItemQuantity(item));
  }

  removeBasketItem(item: IBasketItem) {
    const dialogRef = this.dialog.open(DialogComfirm, {
      disableClose: false,
      width: '400px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.store.dispatch(BasketActions.RemoveBasketItem(item));
        this.store.dispatch(BasketActions.GetBasket());
      }
    });
  }

}
