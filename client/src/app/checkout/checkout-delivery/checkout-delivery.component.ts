import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatStepper } from '@angular/material/stepper';
import { BasketService } from 'src/app/basket/basket.service';
import { IDeliveryMethod } from 'src/app/models/deliveryMethod';
import { CheckoutService } from '../checkout.service';
import * as appReducer from '../../store/app.reducer';
import * as BasketActions from '../../basket/store/basket.actions';
import { Store } from '@ngrx/store';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-checkout-delivery',
  templateUrl: './checkout-delivery.component.html',
  styleUrls: ['./checkout-delivery.component.scss']
})
export class CheckoutDeliveryComponent implements OnInit, OnDestroy {
  private _onDestroy = new Subject();
  @Input() checkoutForm: FormGroup;
  deliveryMethods: IDeliveryMethod[];
  selectDeliveryMethods: IDeliveryMethod;
  constructor(private checkoutService: CheckoutService,
    private store: Store<appReducer.AppState>) { }
  ngOnDestroy(): void {
    this._onDestroy.next();
  }

  ngOnInit(): void {
    this.getDeliveryMethod();
  }

  getDeliveryMethod() {

    this.checkoutService.getDeliveryMethods()
      .pipe(takeUntil(this._onDestroy))
      .subscribe(
        (dm: IDeliveryMethod[]) => {
          this.deliveryMethods = dm;
          if (!this.selectDeliveryMethods) {
            this.selectDeliveryMethods = dm[0];
            this.setShippingPrice(this.selectDeliveryMethods);
          }
        });
  }

  setShippingPrice(dm: IDeliveryMethod) {
    this.store.dispatch(BasketActions.UpdateShipping(dm));
  }



}
