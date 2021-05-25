import { Component, OnInit } from '@angular/core';
import { AccountQuery } from '../shared/states/account/account.query';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(public accountQuery: AccountQuery) { }

  ngOnInit(): void {
  }

}
