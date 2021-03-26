import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input, OnInit } from '@angular/core';


@Component({
  selector: 'app-checkout-stepper',
  templateUrl: './checkout-stepper.component.html',
  styleUrls: ['./checkout-stepper.component.scss'],
  providers: [{ provide: CdkStepper, useExisting: CheckoutStepper }]
})
export class CheckoutStepper extends CdkStepper implements OnInit {
  @Input() prevLink?: string;
  @Input() nextLink?: string;
  ngOnInit(): void {
  }
  selectStepByIndex(index: number): void {
    this.selectedIndex = index;
  }

}
