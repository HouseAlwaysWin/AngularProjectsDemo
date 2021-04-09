import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { BasketService } from 'src/app/basket/basket.service';
import { Basket } from 'src/app/models/basket';
import { IUser } from 'src/app/models/user';

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
    public translate: TranslateService,
    private basketService: BasketService,
    private accountService: AccountService) { }

  ngOnDestroy(): void {
    this.accountService.currrentUser.unsubscribe();
    this.basketSub.unsubscribe();
  }

  onNavSideClose() {
    this.navSideClose.emit();
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
    this.accountService.currrentUser.subscribe(user => {
      console.log(user);
      this.userInfo = user;
      this.isAuth = user ? true : false;
    });
  }

  onLogout() {
    this.accountService.logout();
  }

  showLangList() {
    this.showLangs = !this.showLangs;
  }

}
