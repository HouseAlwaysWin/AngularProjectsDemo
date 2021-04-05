import { Component, OnInit, AfterViewInit, AfterContentInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { TranslateService } from '@ngx-translate/core';
import { Observable, ReplaySubject, Subscription } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { BasketService } from 'src/app/basket/basket.service';
import { IApiResponse } from 'src/app/models/apiResponse';
import { Basket, IBasket } from 'src/app/models/basket';
import { IUser } from 'src/app/models/user';

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
  isAuth = false;

  constructor(
    public translate: TranslateService,
    private basketService: BasketService,
    private accountService: AccountService,
    private matePage: MatPaginatorIntl
  ) {
    // translate.addLangs(['en']);
    // const browserLang = navigator.language;
    // translate.use(browserLang.match(/en|zh-TW/) ? browserLang : 'en');
    // translate.use('en');
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
      this.isAuth = user ? true : false;
      this.showBar = true;
    });
    this.getBasket();

    // this.basket$ = this.basketService.basket$;


    // this.showBar = false;
    // this.authSubscription = this.fbAuthService
    //   .isAuth$.subscribe(authStatus => {
    //     this.isAuth = authStatus;
    //     this.showBar = true;
    //   });
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
