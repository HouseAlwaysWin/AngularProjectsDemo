import { Injectable } from "@angular/core";
import { Query } from "@datorama/akita";
import { AccountState, AccountStore } from "./account.store";


@Injectable({
  providedIn: 'root'
})
export class AccountQuery extends Query<AccountState> {

  token$ = this.select('token');
  email$ = this.select('email');
  username$ = this.select('username');
  isAuth$ = this.select('isAuth');
  loading$ = this.select('loading');

  get token() {
    return this.getValue().token;
  }

  get email() {
    return this.getValue().email;
  }

  get username() {
    return this.getValue().username;
  }

  get isAuth() {
    return this.getValue().isAuth;
  }

  get loading() {
    return this.getValue().loading;
  }

  constructor(protected store: AccountStore) {
    super(store);
  }

}
