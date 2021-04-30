import { Injectable, OnInit } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router, CanLoad, Route, UrlSegment } from '@angular/router';
import { Observable } from 'rxjs';
import * as appReducer from '../store/app.reducer'
import * as BasketActions from '../basket/store/basket.actions';
import { Store } from '@ngrx/store';
import { IBasketItem } from '../models/basket';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CheckoutGuard implements CanLoad, OnInit {
  basketItemsCount: number = 0;
  constructor(
    private router: Router,
    private store: Store<appReducer.AppState>
  ) {

  }
  canLoad(route: Route, segments: UrlSegment[]): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    this.store.dispatch(BasketActions.GetBasket());
    return this.store.select('basket').pipe(
      map(res => {
        let count = res.basket ? res.basket.basketItems.length : 0;
        console.log(count);
        if (count === 0) {
          this.router.navigate(['/shop']);
          return false;
        }
        return true
      })
    )
  }

  ngOnInit(): void {

  }


}
