import { CdkStepper, StepState } from '@angular/cdk/stepper';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { BasketService } from '../basket/basket.service';
import { IBasketTotals } from '../models/basket';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  checkoutForm: FormGroup;
  basketTotals$: Observable<IBasketTotals>;
  stepState: StepState;
  constructor(
    private formBuilder: FormBuilder,
    public translate: TranslateService,
    private basketService: BasketService) { }

  ngOnInit(): void {
    this.validateCheckoutForm();
    this.basketTotals$ = this.basketService.basketTotals$;
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

  onChangeSelect(e) {
    console.log(e);
  }

}
