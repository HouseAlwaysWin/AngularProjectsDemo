import { Component, OnInit, AfterViewInit, AfterContentInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Observable, Subscription } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { FbAuthService } from 'src/app/account/fb-auth.service';
import { IUser } from 'src/app/models/user';

@Component({
  selector: 'app-nav-top',
  templateUrl: './nav-top.component.html',
  styleUrls: ['./nav-top.component.scss']
})
export class NavTopComponent implements OnInit, OnDestroy {
  @Output() navSideToggle = new EventEmitter();
  showBar = false;
  isAuth = false;
  // authSubscription: Subscription;

  constructor(
    public translate: TranslateService,
    private fbAuthService: FbAuthService,
    private accountService: AccountService
  ) {
    translate.addLangs(['en']);
    const browserLang = navigator.language;
    translate.use(browserLang.match(/en|zh-TW/) ? browserLang : 'en');
  }

  onNavSideToggle() {
    this.navSideToggle.emit();
  }

  ngOnInit(): void {
    this.showBar = false;
    this.accountService.isAuth.subscribe(state => {
      console.log(state);
      this.isAuth = state;
      this.showBar = true;
    });
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
    this.accountService.isAuth.unsubscribe();
    // this.authSubscription.unsubscribe();
  }



}
