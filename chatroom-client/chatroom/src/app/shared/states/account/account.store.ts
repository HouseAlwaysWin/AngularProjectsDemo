import { Injectable } from "@angular/core";
import { Store, StoreConfig } from "@datorama/akita";
import { Friend } from "../../models/friend";
import { Message } from "../../models/message";
import { Notify } from "../../models/notification";
import { UserDetail, UserPhoto, UserShortInfo } from "../../models/user";




export interface AccountState {
  user: UserShortInfo;
  mainPhoto: string;
  userPhotos: UserPhoto[];
  usersOnline: string[];
  loading: boolean;
  friendList: Friend[];
  messagesThread: Message[];
  notifies: Notify[],
  notifyNotReadCount: number,
  isAuth: boolean;
}

export function createInitialState(): AccountState {
  return {
    user: new UserShortInfo(),
    mainPhoto: '',
    userPhotos: [],
    usersOnline: [],
    friendList: [],
    loading: false,
    messagesThread: [],
    notifies: [],
    notifyNotReadCount: 0,
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
