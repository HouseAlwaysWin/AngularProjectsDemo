import { Component, OnInit } from '@angular/core';
import { UserToken } from './shared/models/user';
import { AccountService } from './shared/states/account/account.service';
import { SharedQuery } from './shared/states/shared/shared.query';
import { SharedStore } from './shared/states/shared/shared.store';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  loading: boolean = false;

  ngOnInit(): void {
    this.globalLoading();
    this.setCurrentUser();
  }

  globalLoading() {
    this.sharedQuery.gLoading$.subscribe(res => {
      this.loading = res;
    });
  }

  constructor(
    public sharedQuery: SharedQuery,
    private accountService: AccountService) {
  }

  title = 'chatroom';

  setCurrentUser() {
    const token: UserToken = JSON.parse(localStorage.getItem('user'));

    if (token) {
      this.accountService.setCurrentUser(token);
    }
  }



}


