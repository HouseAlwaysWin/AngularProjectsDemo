import { Injectable } from "@angular/core";
import { Store, StoreConfig } from "@datorama/akita";


export interface AccountState {
  token: string;
  email: string;
  username: string;
  loading: boolean;
  isAuth: boolean;
}

export function createInitialState(): AccountState {
  return {
    token: '',
    email: '',
    username: '',
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
