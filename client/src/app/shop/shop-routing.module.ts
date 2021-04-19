import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { ShopComponent } from './shop.component';


const routes: Routes = [
  { path: '', component: ShopComponent },
  { path: ':id', component: ProductDetailComponent },
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ShopRoutingModule { }
