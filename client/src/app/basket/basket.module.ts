import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BasketRoutingModule } from './basket-routing.module';
import { SharedModule } from '../shared/shared.module';
import { BasketSummaryComponent } from './basket-summary/basket-summary.component';
import { BasketComponent } from './basket.component';
import { BaskeTotalSummaryComponent } from './baske-total-summary/baske-total-summary.component';



@NgModule({
  declarations: [BasketComponent, BasketSummaryComponent, BaskeTotalSummaryComponent],
  imports: [
    CommonModule,
    BasketRoutingModule,
    SharedModule
  ],
  exports: [
    BaskeTotalSummaryComponent,
    BasketSummaryComponent
  ]
})
export class BasketModule { }
