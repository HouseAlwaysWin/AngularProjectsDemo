import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { FbAuthService } from 'src/app/account/fb-auth.service';

@Component({
  selector: 'app-nav-top',
  templateUrl: './nav-top.component.html',
  styleUrls: ['./nav-top.component.scss']
})
export class NavTopComponent implements OnInit {
  @Output() navSideToggle = new EventEmitter();

  isAuth = false;
  authSubscription: Subscription;

  constructor(
    public translate: TranslateService,
    private fbAuthService: FbAuthService
  ) {
    translate.addLangs(['en']);
    const browserLang = navigator.language;
    translate.use(browserLang.match(/en|zh-TW/) ? browserLang : 'en');
  }

  onNavSideToggle() {
    this.navSideToggle.emit();
  }

  ngOnInit(): void {
    this.authSubscription = this.fbAuthService
      .authChange.subscribe(authStatus => {
        this.isAuth = authStatus;
      });
  }

  onLogout() {
    this.fbAuthService.logout();
  }



}
