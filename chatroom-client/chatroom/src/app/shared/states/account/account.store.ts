import { Injectable } from "@angular/core";
import { Store, StoreConfig } from "@datorama/akita";
import { UserToken } from "../../models/user";




export interface AccountState {
  user: UserToken;
  loading: boolean;
  isAuth: boolean;
}

export function createInitialState(): AccountState {
  return {
    user: new UserToken(),
    loading: false,
    isAuth: false
  }
}

@Injectable({
  providedIn: 'root'
})
@StoreConfig({ name: 'account' })
export class AccountStore extends Store<AccountState>{
  constructor() {
    super(createInitialState());
  }
}
