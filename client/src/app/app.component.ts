import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';
import { FbAuthService } from './account/fb-auth.service';

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
    private accountService: AccountService,
    private authService: FbAuthService) {

  }
  ngOnInit(): void {
    const token = localStorage.getItem('token');

    this.accountService.GetUserState(token).subscribe(() => {
      console.log('get user');
    }, error => {
      console.log(error);
    })
    // this.authService.authListener();
  }
}
