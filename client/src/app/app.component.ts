import { Component, ElementRef, HostListener, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AccountService } from './account/account.service';
import { BasketService } from './basket/basket.service';
import * as BasketActions from './basket/store/basket.actions';
import * as appReducer from './store/app.reducer';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  private _onDestroy = new Subject();

  @ViewChild('mainContent') mainContent: ElementRef;

  constructor(
    public translate: TranslateService,
    private store: Store<appReducer.AppState>,
    private accountService: AccountService,
  ) {
    translate.addLangs(['en-US', 'zh-TW']);
    const browserLang = navigator.language;
    translate.use(browserLang.match(/zh-TW|en-US/) ? browserLang : 'en-US');
    translate.use('zh-TW');
  }

  ngOnDestroy(): void {
    this._onDestroy.next();
  }

  ngOnInit(): void {
    this.loadCurrentUser();
    this.loadBasket();
  }

  loadCurrentUser() {
    const token = localStorage.getItem('token');
    this.accountService.GetUserState(token)
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        console.log('get user');
      }, error => {
        console.log(error);
      })
  }

  loadBasket() {
    this.store.dispatch(BasketActions.GetBasket());
  }

  watchScroll() {
    var sh = Math.trunc(this.mainContent.nativeElement.scrollHeight);
    var st = Math.trunc(this.mainContent.nativeElement.scrollTop);
    var ht = Math.trunc(this.mainContent.nativeElement.offsetHeight);
  }


}
