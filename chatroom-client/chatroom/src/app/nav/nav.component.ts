import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountQuery } from '../shared/states/account/account.query';
import { AccountService } from '../shared/states/account/account.service';
import { AccountStore } from '../shared/states/account/account.store';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {

  constructor(
    private router: Router,
    private accountService: AccountService,
    public accountQuery: AccountQuery) { }

  ngOnInit(): void {
    this.accountQuery.isAuth;
  }

  logout() {
    console.log('logout');
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

}
