import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { Basket, IBasket, IBasketItem } from 'src/app/models/basket';
import { DialogComfirm } from 'src/app/shared/components/dialog-comfirm/dialog-comfirm.component';

@Component({
  selector: 'app-checkout-review',
  templateUrl: './checkout-review.component.html',
  styleUrls: ['./checkout-review.component.scss']
})
export class CheckoutReviewComponent implements OnInit {
  @Input() appStepper: CdkStepper;
  basket$: Observable<IBasket>;
  constructor(
    public dialog: MatDialog,
    private basketService: BasketService) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }

  createPaymentIntent() {
    return this.basketService.createPaymentIntent().subscribe(result => {
      console.log(result);
      this.appStepper.next();
    }, error => {
      console.log(error);
    });
  }


  removeBasketItem(item: IBasketItem) {
    const dialogRef = this.dialog.open(DialogComfirm);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.basketService.removeBasketItem(item);
      }
    });
  }

}
