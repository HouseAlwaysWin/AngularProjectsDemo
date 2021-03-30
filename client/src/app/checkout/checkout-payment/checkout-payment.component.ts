import { AfterViewInit, Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { loadStripe, Stripe, StripeCardCvcElement, StripeCardExpiryElement, StripeCardNumberElement } from '@stripe/stripe-js';
import { BasketService } from 'src/app/basket/basket.service';
import { Basket, IBasket } from 'src/app/models/basket';
import { IOrderToCreate } from 'src/app/models/order';
import { environment } from 'src/environments/environment';
import { CheckoutService } from '../checkout.service';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements OnInit, OnDestroy, AfterViewInit {
  stripeApi: string = environment.stripeKey;
  @Input() checkoutForm: FormGroup;
  @ViewChild('cardNumber', { static: true }) cardNumberElement: ElementRef;
  @ViewChild('cardExpiry', { static: true }) cardExpiryElement: ElementRef;
  @ViewChild('cardCvc', { static: true }) cardCvcElement: ElementRef;
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


  constructor(private basketService: BasketService,
    public translate: TranslateService,
    private checkoutService: CheckoutService,
    private router: Router) { }

  ngOnDestroy(): void {
    this.cardNumber.destroy();
    this.cardExpiry.destroy();
    this.cardCvc.destroy();
  }

  async ngAfterViewInit(): Promise<void> {
    this.stripe = await loadStripe(this.stripeApi);

    const elements = this.stripe.elements();

    this.cardNumber = elements.create('cardNumber');
    console.log(this.cardNumber);
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
    console.log(event);
    if (event.error) {
      this.translate.onLangChange.subscribe(trans => {
        var stripeErrorMsg = trans.translations['StripeErrorMsg'];
        this.cardErrors = stripeErrorMsg[event.error.code];
      });
      var message = this.translate.instant(`StripeErrorMsg.${event.error.code}`);
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
    const basket = this.basketService.getCurrentBasketValue();
    try {
      const createOrder = await this.createOrder(basket);
      const paymentResult = await this.confirmPaymentWithStripe(basket);
      if (paymentResult.paymentIntent) {
        this.basketService.deleteLocalBasket(basket.id);
        const navigationExtras: NavigationExtras = { state: createOrder };
        this.router.navigate(['checkout/success'], navigationExtras);
      }
      else {

      }
    } catch (error) {

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
    console.log(basket);
    console.log(this.checkoutForm.get('deliveryForm').get('deliveryMethodId').value);
    console.log(this.checkoutForm.get('addressForm').value);
    const orderToCreate: IOrderToCreate =
    {
      basketId: basket.id,
      deliveryMethodId: parseInt(this.checkoutForm.get('deliveryForm').get('deliveryMethodId').value.id),
      shipToAddress: this.checkoutForm.get('addressForm').value
    };
    return this.checkoutService.createOrder(orderToCreate).toPromise();
  }

  ngOnInit(): void {
  }

}
