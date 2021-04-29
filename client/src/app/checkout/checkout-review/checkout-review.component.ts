import { CdkStepper } from '@angular/cdk/stepper';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatStepper } from '@angular/material/stepper';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket, IBasketItem } from 'src/app/models/basket';
import { DialogComfirm } from 'src/app/shared/components/dialog-comfirm/dialog-comfirm.component';
import * as appReducer from '../../store/app.reducer';
import * as BasketActions from '../../basket/store/basket.actions';
import { Store } from '@ngrx/store';
import { IApiResponse } from 'src/app/models/apiResponse';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-checkout-review',
  templateUrl: './checkout-review.component.html',
  styleUrls: ['./checkout-review.component.scss']
})
export class CheckoutReviewComponent implements OnInit {
  @Input() stepper: MatStepper;
  isComplete: boolean = false;
  isLoading: boolean = false;
  // basket$: Observable<IBasket>;

  basket: IBasket;

  constructor(
    public dialog: MatDialog,
    private store: Store<appReducer.AppState>,
    private basketService: BasketService
  ) { }

  ngOnInit(): void {
    // this.basket$ = this.basketService.basket$;
    this.store.select('basket').subscribe(res => {
      this.basket = res.basket;
      this.isLoading = res.loading;
      if (res.paymentIntentSuccess) {
        this.stepper.selected.completed = true;
        this.stepper.next();
      }
    })
  }



  createPaymentIntent() {
    this.store.dispatch(BasketActions.CreatePaymentIntent());
  }

  removeBasketItem(item: IBasketItem) {
    const dialogRef = this.dialog.open(DialogComfirm);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.store.dispatch(BasketActions.RemoveBasketItem(item));
      }
    });
  }

}
