import { Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UtilitiesService } from './shared/services/utilities.service';
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
    this.checkLogin();
    this.globalLoading();
  }

  globalLoading() {
    this.sharedQuery.gLoading$.subscribe(res => {
      console.log('trigger loading');
      this.loading = res;
    });
  }

  constructor(
    private router: Router,
    private utilitiesService: UtilitiesService,
    public sharedQuery: SharedQuery,
    private accountService: AccountService) {
  }

  @HostListener('document:click', ['$event'])
  documentClick(event: any): void {
    this.utilitiesService.documentClickedTarget.next(event.target)
  }

  title = 'chatroom';

  checkLogin() {
    const token: string = JSON.parse(localStorage.getItem('token'));

    if (token) {
      this.accountService.getUserDetail().subscribe(res => {
        if (res) {
          console.log('already login')
        }
        else {
          this.router.navigate(['/account/login']);
        }
      });
    }
  }



}


