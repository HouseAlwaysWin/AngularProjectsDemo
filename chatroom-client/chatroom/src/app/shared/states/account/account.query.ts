import { Injectable } from "@angular/core";
import { Query } from "@datorama/akita";
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
  loading$ = this.select('loading');

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

  constructor(protected store: AccountStore) {
    super(store);
  }

}