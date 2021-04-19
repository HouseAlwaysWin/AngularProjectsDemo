import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AccountService } from './account/account.service';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  /**
   *
   */
  constructor(
    public translate: TranslateService,
    private accountService: AccountService,
    private basketService: BasketService,
  ) {
    translate.addLangs(['en-US', 'zh-TW']);
    const browserLang = navigator.language;
    translate.use(browserLang.match(/en-US|zh-TW/) ? browserLang : 'en-US');
    translate.use('zh-TW');

  }
  ngOnInit(): void {
    // this.authService.authListener();
    this.loadCurrentUser();
    this.basketService.loadBasket();
  }

  loadCurrentUser() {
    const token = localStorage.getItem('token');
    this.accountService.GetUserState(token).subscribe(() => {
      console.log('get user');
    }, error => {
      console.log(error);
    })

  }

}
