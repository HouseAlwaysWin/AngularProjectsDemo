import { Component, OnInit } from '@angular/core';
import { UserPhoto } from '../shared/models/user';
import { AccountQuery } from '../shared/states/account/account.query';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  headerImgs: UserPhoto[];
  selectImg: UserPhoto;

  constructor(private accountQuery: AccountQuery) { }

  ngOnInit(): void {
    this.headerImgs = this.accountQuery.user.photos;
    console.log(this.headerImgs)
  }

}
