import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Friend } from '../shared/models/friend';
import { UserPhoto, UserShortInfo } from '../shared/models/user';
import { FormFieldService } from '../shared/services/form-field.service';
import { AccountService } from '../shared/services/account.service';
import { Router } from '@angular/router';
import { DataService } from '../shared/states/data.service';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.scss']
})
export class FriendsComponent implements OnInit {
  friendList: Friend[];
  userOnline: string[];
  searchForm: FormGroup;
  constructor(
    private router: Router,
    public formService: FormFieldService,
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private state: DataService
  ) { }

  ngOnInit(): void {
    this.getAllFriends();
    this.initSearchForm();
    this.state.query.usersOnline$.subscribe(user => {
      console.log(user);
      this.userOnline = user;
    })
  }

  initSearchForm() {
    this.searchForm = this.formBuilder.group({
      search: [null, [Validators.required]],
    });
  }

  getAllFriends() {
    this.accountService.getFriends().subscribe(res => {
      console.log(res.data);
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

  goMessage(friend: Friend) {
    console.log(friend);
    this.state.store.update({
      friendRedirectParam: friend
    });
    this.router.navigate([`/message/list`]);
  }
}
