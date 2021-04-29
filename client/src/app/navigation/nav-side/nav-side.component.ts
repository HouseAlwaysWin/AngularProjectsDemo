import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { BasketService } from 'src/app/basket/basket.service';
import { Basket } from 'src/app/models/basket';
import { ShopParams } from 'src/app/models/shopParams';
import { IUser } from 'src/app/models/user';
import { ShopService } from 'src/app/shop/shop.service';
import * as appReducer from '../../store/app.reducer';
import * as ShopActions from '../../shop/store/shop.actions';
import * as BasketActions from '../../basket/store/basket.actions';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-nav-side',
  templateUrl: './nav-side.component.html',
  styleUrls: ['./nav-side.component.scss']
})
export class NavSideComponent implements OnInit, OnDestroy {
  @Output() navSideClose = new EventEmitter();
  isAuth = false;
  userInfo: IUser;
  showLangs: boolean = false;

  basketCount: string = '';
  basketSub: Subscription;

  constructor(
    private store: Store<appReducer.AppState>,
    public translate: TranslateService,
    private basketService: BasketService,
    private accountService: AccountService,
    private shopService: ShopService) { }

  ngOnDestroy(): void {
    this.accountService.currrentUser.unsubscribe();
    this.basketSub.unsubscribe();
  }

  onNavSideClose() {
    this.navSideClose.emit();
  }

  getBasket() {
    // this.basketSub = this.basketService.basket$.subscribe((basket: Basket) => {

    //   this.basketCount = '';
    //   if (basket) {
    //     if (basket.basketItems.length > 0) {
    //       this.basketCount = basket.basketItems.length.toString();
    //     }
    //   }
    // });
    this.basketSub = this.store.select('basket').subscribe(res => {
      if (res.basket) {
        this.basketCount = res.basket.basketItems.length === 0 ? '' : res.basket.basketItems.length.toString();
      }
      else {
        this.basketCount = '';
      }
    })
  }

  ngOnInit(): void {
    this.accountService.currrentUser.subscribe(user => {
      this.userInfo = user;
      this.isAuth = user ? true : false;
    });
    this.getBasket();
  }

  onLogout() {
    this.accountService.logout();
    this.onNavSideClose();
  }

  showLangList() {
    this.showLangs = !this.showLangs;
  }

  changeLang(lang: string) {
    this.translate.use(lang);
    // this.shopService.getCategories().subscribe();
    // this.shopService.getProducts(new ShopParams()).subscribe();
    this.store.dispatch(ShopActions.GetCategories());
    this.store.dispatch(ShopActions.GetProductList(new ShopParams()));
  }

}
