import { Component, OnInit, AfterViewInit, AfterContentInit, Output, EventEmitter, OnDestroy, HostListener } from '@angular/core';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { ReplaySubject, Subscription } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { BasketService } from 'src/app/basket/basket.service';
import { Basket, IBasket } from 'src/app/models/basket';
import { ShopParams } from 'src/app/models/shopParams';
import { IUser } from 'src/app/models/user';
import { ShopService } from 'src/app/shop/shop.service';
import * as appReducer from '../../store/app.reducer';
import * as ShopActions from '../../shop/store/shop.actions';
import * as CheckoutActions from '../../checkout/store/checkout.actions';
import { CheckoutService } from 'src/app/checkout/checkout.service';

@Component({
  selector: 'app-nav-top',
  templateUrl: './nav-top.component.html',
  styleUrls: ['./nav-top.component.scss']
})
export class NavTopComponent implements OnInit, OnDestroy {
  @Output() navSideToggle = new EventEmitter();


  langChange = new ReplaySubject();
  showBar = false;
  basketCount: string = '';
  basketSub: Subscription;
  productSub: Subscription;
  isAuth = false;
  userInfo: IUser;


  constructor(
    public translate: TranslateService,
    private accountService: AccountService,
    private store: Store<appReducer.AppState>
  ) {
  }

  onNavSideToggle() {
    this.navSideToggle.emit();
  }

  getBasket() {
    this.basketSub = this.store.select('basket')
      .subscribe(res => {
        if (res.basket) {
          this.basketCount = res.basket.basketItems.length === 0 ? '' : res.basket.basketItems.length.toString();
        }
        else {
          this.basketCount = '';
        }
      })
  }


  ngOnInit(): void {
    this.showBar = false;
    this.accountService.currrentUser.subscribe(user => {
      console.log(user);
      this.userInfo = user;
      this.isAuth = user ? true : false;
      this.showBar = true;
    });
    this.getBasket();
  }

  onLogout() {
    this.accountService.logout();
  }

  ngOnDestroy() {
    this.accountService.currrentUser.unsubscribe();
    this.basketSub.unsubscribe();
  }

  changeLang(lang: string) {
    this.translate.use(lang);
    this.store.dispatch(ShopActions.GetCategories());
    this.store.dispatch(ShopActions.GetProductList(new ShopParams()));
    this.store.dispatch(CheckoutActions.GetDeliveryMethod());
  }


}
