import { Injectable } from "@angular/core";
import { Query } from "@datorama/akita";
import { BehaviorSubject } from "rxjs";
import { UserShortInfo } from "../../models/user";
import { AccountState, AccountStore } from "./account.store";


@Injectable({
  providedIn: 'root'
})
export class AccountQuery extends Query<AccountState> {
  all$ = this.select();
  user$ = this.select(state => state.user);;
  mainPhoto$ = this.select(state => state.user.photos.filter(p => p.isMain)[0]?.url);
  userPhotos = this.select('userPhotos');
  token$ = this.select(state => state.user.token);
  email$ = this.select(state => state.user.email);
  username$ = this.select(state => state.user.userName);
  photos$ = this.select(state => state.user.photos);
  isAuth$ = this.select('isAuth');
  friends$ = this.select('friendList');
  usersOnline$ = this.select('usersOnline');
  loading$ = this.select('loading');
  messagesThread$ = this.select('messagesThread');
  notifies$ = this.select('notifies');

  get token() {
    return this.getValue().user.token;
  }

  get email() {
    return this.getValue().user.email;
  }

  get username() {
    return this.getValue().user.userName;
  }

  get isAuth() {
    return this.getValue().isAuth;
  }


  get user() {
    return this.getValue().user;
  }

  get photos() {
    return this.getValue().user.photos;
  }

  get loading() {
    return this.getValue().loading;
  }

  get friendList() {
    return this.getValue().friendList;
  }

  get notified() {
    return this.getValue().notifies;
  }

  constructor(protected store: AccountStore) {
    super(store);
  }

}
