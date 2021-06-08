import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Friend } from '../shared/models/friend';
import { UserPhoto, UserShortInfo } from '../shared/models/user';
import { FormFieldService } from '../shared/services/form-field.service';
import { AccountService } from '../shared/services/account.service';
import { Router } from '@angular/router';
import { AccountQuery } from '../shared/states/account/account.query';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.scss']
})
export class FriendsComponent implements OnInit {
  friendList: Friend[];
  searchForm: FormGroup;
  constructor(
    private router: Router,
    public formService: FormFieldService,
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private accountQuery: AccountQuery) { }

  ngOnInit(): void {
    this.getAllFriends();
    this.initSearchForm();
  }

  initSearchForm() {
    this.searchForm = this.formBuilder.group({
      search: [null, [Validators.required]],
    });
  }

  getAllFriends() {
    this.accountService.getFriends().subscribe(res => {
      this.friendList = res.data;
    });
  }

  getFriendMainPhoto(photos: UserPhoto[]) {
    let mainUrl = photos.filter(p => p.isMain)[0].url;
    return mainUrl == null ? '' : mainUrl;
  }

  removeFriend(friend: Friend) {
    this.accountService.removeFriend(friend).subscribe(res => {
      this.friendList = res.data;
    });
  }

  goMessage(friend: UserShortInfo) {
    this.router.navigate([`/message/chatroom`, { username: friend.userName }]);
  }
}
