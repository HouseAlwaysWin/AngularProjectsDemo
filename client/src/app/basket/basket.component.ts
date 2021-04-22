import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Observable, Subscription } from 'rxjs';
import { IBasket, IBasketItem, IBasketTotals } from '../models/basket';
import { DialogComfirm } from '../shared/components/dialog-comfirm/dialog-comfirm.component';
import { BasketService } from './basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent implements OnInit {
  basket$: Observable<IBasket>;
  basketTotals$: Observable<IBasketTotals>;

  displayedColumns: string[] = []
  constructor(
    private router: Router,
    public translate: TranslateService,
    private basketService: BasketService,
    public dialog: MatDialog) { }


  openDialog() {
    const dialogRef = this.dialog.open(DialogComfirm, {
      disableClose: false,
      width: '500px',
      role: 'alertdialog',
    });

    dialogRef.afterClosed().subscribe(result => {
      this.basketService.loadBasket();
    });

  }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
    console.log(this.basket$);
    this.basketTotals$ = this.basketService.basketTotals$;
  }



  incrementItemQuantity(item: IBasketItem) {
    this.basketService.incrementItemQuantity(item);
  }

  decrementItemQuantity(item: IBasketItem) {
    this.basketService.decrementItemQuantity(item);
  }

  removeBasketItem(item: IBasketItem) {
    const dialogRef = this.dialog.open(DialogComfirm, {
      disableClose: false,
      width: '400px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.basketService.removeBasketItem(item);
      }
    });
  }

}
