import { NotFoundComponent } from './core/not-found/not-found.component';
import { HomeComponent } from './home/home.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { AuthGuard } from './account/auth.guard';
import { ShopComponent } from './shop/shop.component';
import { CheckoutGuard } from './checkout/checkout.guard';


const routes: Routes = [
  { path: '', component: ShopComponent },
  { path: 'not-found', component: NotFoundComponent },
  {
    path: 'products',
    loadChildren: () => import('./shop/shop.module').then(m => m.ShopModule),
  },
  {
    path: 'account',
    loadChildren: () => import('./account/account.module').then(m => m.AccountModule),
  },
  {
    path: 'basket',
    loadChildren: () => import('./basket/basket.module').then(m => m.BasketModule),
    // canLoad: [AuthGuard]
  },
  {
    path: 'orders',
    loadChildren: () => import('./orders/orders.module').then(m => m.OrdersModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'checkout',
    loadChildren: () => import('./checkout/checkout.module').then(m => m.CheckoutModule),
    canActivate: [AuthGuard],
    canLoad: [CheckoutGuard]
  },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
