import { NotFoundComponent } from './core/not-found/not-found.component';
import { HomeComponent } from './home/home.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './account/auth.guard';


const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'not-found', component: NotFoundComponent },
  {
    path: 'shop',
    loadChildren: () => import('./shop/shop.module').then(m => m.ShopModule),
  },
  {
    path: 'account',
    loadChildren: () => import('./account/account.module').then(m => m.AccountModule),
    canLoad: [AuthGuard]
  },
  {
    path: 'basket',
    loadChildren: () => import('./basket/basket.module').then(m => m.BasketModule),
    canLoad: [AuthGuard]
  },
  {
    path: 'orders',
    loadChildren: () => import('./orders/orders.module').then(m => m.OrdersModule),
    canLoad: [AuthGuard]
  },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
