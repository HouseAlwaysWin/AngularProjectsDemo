import { Component, OnInit, AfterViewInit, AfterContentInit, Output, EventEmitter, OnDestroy, HostListener } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { TranslateService } from '@ngx-translate/core';
import { Observable, of, ReplaySubject, Subscription } from 'rxjs';
import { debounceTime, map, startWith } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { BasketService } from 'src/app/basket/basket.service';
import { IApiResponse } from 'src/app/models/apiResponse';
import { Basket, IBasket } from 'src/app/models/basket';
import { IProduct } from 'src/app/models/product';
import { IUser } from 'src/app/models/user';
import { ShopService } from 'src/app/shop/shop.service';

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
    private basketService: BasketService,
    private accountService: AccountService,
    private shopService: ShopService
  ) {
  }

  onNavSideToggle() {
    this.navSideToggle.emit();
  }

  getBasket() {
    this.basketSub = this.basketService.basket$.subscribe((basket: Basket) => {

      this.basketCount = '';
      if (basket) {
        if (basket.basketItems.length > 0) {
          this.basketCount = basket.basketItems.length.toString();
        }
      }
    });
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
    // this.fbAuthService.logout();
  }

  ngOnDestroy() {
    this.accountService.currrentUser.unsubscribe();
    this.basketSub.unsubscribe();
    // this.authSubscription.unsubscribe();
  }

  changeLang(lang: string) {
    this.translate.use(lang);
  }


}
