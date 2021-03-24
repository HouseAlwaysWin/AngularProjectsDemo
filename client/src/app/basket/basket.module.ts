import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BasketRoutingModule } from './basket-routing.module';
import { SharedModule } from '../shared/shared.module';
import { BasketSummaryComponent } from './basket-summary/basket-summary.component';
import { BasketComponent } from './basket.component';



@NgModule({
  declarations: [BasketComponent, BasketSummaryComponent],
  imports: [
    CommonModule,
    BasketRoutingModule,
    SharedModule
  ]
})
export class BasketModule { }
