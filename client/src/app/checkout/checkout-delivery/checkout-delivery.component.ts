import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatStepper } from '@angular/material/stepper';
import { BasketService } from 'src/app/basket/basket.service';
import { IDeliveryMethod } from 'src/app/models/deliveryMethod';
import { CheckoutService } from '../checkout.service';

@Component({
  selector: 'app-checkout-delivery',
  templateUrl: './checkout-delivery.component.html',
  styleUrls: ['./checkout-delivery.component.scss']
})
export class CheckoutDeliveryComponent implements OnInit {
  @Input() checkoutForm: FormGroup;
  deliveryMethods: IDeliveryMethod[];
  selectDeliveryMethods: IDeliveryMethod;
  constructor(private checkoutService: CheckoutService,
    private basketService: BasketService) { }

  ngOnInit(): void {
    this.getDeliveryMethod();
  }

  getDeliveryMethod() {
    this.checkoutService.getDeliveryMethods().subscribe(
      (dm: IDeliveryMethod[]) => {
        this.deliveryMethods = dm;
        if (!this.selectDeliveryMethods) {
          this.selectDeliveryMethods = dm[0];
          console.log(this.selectDeliveryMethods);
          this.setShippingPrice(this.selectDeliveryMethods);
        }
      });
  }

  setShippingPrice(dm: IDeliveryMethod) {
    this.basketService.setShipping(dm);
  }



}
