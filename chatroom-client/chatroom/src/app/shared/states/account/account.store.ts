import { Injectable } from "@angular/core";
import { Store, StoreConfig } from "@datorama/akita";
import { UserDetail, UserPhoto } from "../../models/user";




export interface AccountState {
  // userToken: UserToken;

  user: UserDetail;
  mainPhoto: string;
  userPhotos: UserPhoto[];
  loading: boolean;
  isAuth: boolean;
}

export function createInitialState(): AccountState {
  return {
    // userToken: new UserToken(),
    user: new UserDetail(),
    mainPhoto: '',
    userPhotos: [],
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
