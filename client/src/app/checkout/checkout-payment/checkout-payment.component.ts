import { AfterViewInit, Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { loadStripe, Stripe, StripeCardCvcElement, StripeCardExpiryElement, StripeCardNumberElement } from '@stripe/stripe-js';
import { IBasket } from 'src/app/models/basket';
import { IOrderToCreate } from 'src/app/models/order';
import { environment } from 'src/environments/environment';
import { CheckoutService } from '../checkout.service';
import * as appReducer from '../../store/app.reducer';
import * as BasketActions from '../../basket/store/basket.actions';
import { Store } from '@ngrx/store';
import { MatDialog } from '@angular/material/dialog';
import { DialogMessage } from 'src/app/shared/components/dialog-message/dialog-message.component';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements OnInit, OnDestroy, AfterViewInit {
  @Input() checkoutForm: FormGroup;
  @ViewChild('cardNumber', { static: true }) cardNumberElement: ElementRef;
  @ViewChild('cardExpiry', { static: true }) cardExpiryElement: ElementRef;
  @ViewChild('cardCvc', { static: true }) cardCvcElement: ElementRef;
  basket: IBasket;
  stripe: Stripe;
  cardNumber: StripeCardNumberElement;
  cardExpiry: StripeCardExpiryElement;
  cardCvc: StripeCardCvcElement;
  cardErrors: any;
  cardHandler = this.onChange.bind(this);
  loading = false;
  cardNumberValid: boolean = false;
  cardExpiryValid: boolean = false;
  cardCvcValid: boolean = false;
  isLoading: boolean = false;


  constructor(
    // private basketService: BasketService,
    public dialog: MatDialog,
    private store: Store<appReducer.AppState>,
    public translate: TranslateService,
    private checkoutService: CheckoutService,
    private router: Router) { }
  private _onDestroy = new Subject;

  ngOnDestroy(): void {
    this.cardNumber.destroy();
    this.cardExpiry.destroy();
    this.cardCvc.destroy();
    this._onDestroy.next();
  }

  async ngAfterViewInit(): Promise<void> {
    this.stripe = await loadStripe(environment.stripeKey);

    const elements = this.stripe.elements();

    this.cardNumber = elements.create('cardNumber');
    this.cardNumber.mount(this.cardNumberElement.nativeElement);
    this.cardNumber.on('change', this.cardHandler);

    this.cardExpiry = elements.create('cardExpiry');
    this.cardExpiry.mount(this.cardExpiryElement.nativeElement);
    this.cardExpiry.on('change', this.cardHandler);

    this.cardCvc = elements.create('cardCvc');
    this.cardCvc.mount(this.cardCvcElement.nativeElement);
    this.cardCvc.on('change', this.cardHandler);
  }

  onChange(event) {
    if (event.error) {
      this.translate.onLangChange
        .pipe(takeUntil(this._onDestroy))
        .subscribe(trans => {
          var stripeErrorMsg =
            trans.translations['StripeErrorMsg'].hasOwnProperty(event.error.code) ?
              trans.translations['StripeErrorMsg'][event.error.code] :
              event.error.message;
          this.cardErrors = stripeErrorMsg;
        });
      var message = this.translate.instant('StripeErrorMsg').hasOwnProperty(event.error.code) ?
        this.translate.instant(`StripeErrorMsg.${event.error.code}`) :
        event.error.message;

      this.cardErrors = message;
    }
    else {
      this.cardErrors = null;
    }
    switch (event.elementType) {
      case 'cardNumber':
        this.cardNumberValid = event.complete;
        break;
      case 'cardExpiry':
        this.cardExpiryValid = event.complete;
        break;
      case 'cardCvc':
        this.cardCvcValid = event.complete;
        break;
    }
  }

  async submitOrder() {
    this.isLoading = true;

    const basket = this.basket;
    try {
      const createOrder = await this.createOrder(basket);
      const paymentResult = await this.confirmPaymentWithStripe(basket);
      console.log(paymentResult);
      if (paymentResult.paymentIntent) {
        localStorage.removeItem('basket_id');
        this.store.dispatch(BasketActions.DeleteBasket());
        this.store.dispatch(BasketActions.GetBasket());

        const navigationExtras: NavigationExtras = { state: createOrder };
        this.isLoading = false;
        this.router.navigate(['checkout/success'], navigationExtras);
      }
      else {
        if (paymentResult.error.code === 'payment_intent_unexpected_state') {
          localStorage.removeItem('basket_id');
          this.store.dispatch(BasketActions.DeleteBasket());
          this.store.dispatch(BasketActions.GetBasket());
          this.dialog.open(DialogMessage, {
            data: {
              message: paymentResult.error.message
            }
          });
          this.router.navigate(['/shop']);
        }
        else {
          this.dialog.open(DialogMessage, {
            data: {
              message: paymentResult.error.message
            }
          });
        }
      }
      this.isLoading = false;
    } catch (error) {
      this.isLoading = false;
      this.dialog.open(DialogMessage, {
        data: {
          message: this.translate.instant('StripeErrorMsg.PayFailed')
        }
      });
      console.log(error);
      console.log(error.error);
    }

  }

  private async confirmPaymentWithStripe(basket: IBasket) {
    return this.stripe.confirmCardPayment(
      basket.clientSecret, {
      payment_method: {
        card: this.cardNumber,
        billing_details: {
          name: this.checkoutForm.get('paymentForm').get('nameOnCard').value
        }
      }
    }
    );
  }

  private async createOrder(basket: IBasket) {
    const orderToCreate: IOrderToCreate =
    {
      basketId: basket.id,
      deliveryMethodId: parseInt(this.checkoutForm.get('deliveryForm').get('deliveryMethodId').value.id),
      shipToAddress: this.checkoutForm.get('addressForm').value
    };
    return this.checkoutService.createOrder(orderToCreate).toPromise();
  }

  ngOnInit(): void {
    this.store.select('basket')
      .pipe(takeUntil(this._onDestroy))
      .subscribe(async res => {
        this.basket = res.basket;
      })

  }

}
