import { CdkStepper, StepState } from '@angular/cdk/stepper';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { BasketService } from '../basket/basket.service';
import { IBasketTotals } from '../models/basket';
import * as appReducer from '../store/app.reducer';
import * as BasketActions from '../basket/store/basket.actions';
import { Store } from '@ngrx/store';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit, OnDestroy {
  checkoutForm: FormGroup;
  basketTotals$: Observable<IBasketTotals>;
  basketTotals: IBasketTotals;
  stepState: StepState;
  private _onDestroy = new Subject();
  constructor(
    private formBuilder: FormBuilder,
    public translate: TranslateService,
    private store: Store<appReducer.AppState>
  ) { }

  ngOnDestroy(): void {
    this._onDestroy.next();
  }

  ngOnInit(): void {
    this.validateCheckoutForm();
    this.store.select('basket')
      .pipe(takeUntil(this._onDestroy))
      .subscribe(res => {
        this.basketTotals = res.basketTotal;
      })
  }


  validateCheckoutForm() {
    this.checkoutForm = this.formBuilder.group({
      addressForm: this.formBuilder.group({
        firstName: [null, [Validators.required]],
        lastName: [null, [Validators.required]],
        street: [null, [Validators.required]],
        city: [null, [Validators.required]],
        state: [null, [Validators.required]],
        zipCode: [null, [Validators.required]]
      }),
      deliveryForm: this.formBuilder.group({
        deliveryMethodId: [0, [Validators.required]]
      }),
      paymentForm: this.formBuilder.group({
        nameOnCard: [null, [Validators.required]]
      })
    })
  }

}
