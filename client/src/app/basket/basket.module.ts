import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BasketRoutingModule } from './basket-routing.module';
import { SharedModule } from '../shared/shared.module';
import { BasketComponent } from './basket.component';
import { BasketSummaryComponent } from './basket-summary/basket-summary.component';



@NgModule({
  declarations: [BasketComponent, BasketSummaryComponent],
  imports: [
    CommonModule,
    BasketRoutingModule,
    SharedModule
  ]
})
export class BasketModule { }
