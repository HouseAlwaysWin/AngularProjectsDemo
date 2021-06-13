import { Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UtilitiesService } from './shared/services/utilities.service';
import { AccountService } from './shared/services/account.service';
import { PresenceService } from './shared/services/presence.service';
import { DataService } from './shared/states/data.service';

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
    this.state.query.gLoading$.subscribe(res => {
      this.loading = res;
    });
  }

  constructor(
    private router: Router,
    private state: DataService,
    private utilitiesService: UtilitiesService,
    private accountService: AccountService,
    private presenceService: PresenceService) {
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
          // this.presenceService.createHubConnection();
        }
        else {
          this.router.navigate(['/account/login']);
        }
      });
    }
  }



}


