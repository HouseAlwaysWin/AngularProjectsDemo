import { Injectable } from "@angular/core";
import { Store, StoreConfig } from "@datorama/akita";
import { Friend } from "../models/friend";
import { Message, MessageGroup, MessageWithPageIndex } from "../models/message";
import { Notify } from "../models/notification";
import { UserPhoto, UserShortInfo } from "../models/user";




export interface DataState {
  user: UserShortInfo;
  mainPhoto: string;
  userPhotos: UserPhoto[];
  usersOnline: string[];
  loading: boolean;
  friendList: Friend[];
  messagesThread: Message[];
  messageGoBottom: boolean;
  messagesPageIndex: number;
  messagesGroups: MessageGroup[];
  messageUnreadCount: number;
  notifies: Notify[],
  notifyNotReadCount: number,
  isAuth: boolean;
  gLoading: boolean;
  friendRedirectParam: Friend;
}

export function createInitialState(): DataState {
  return {
    user: new UserShortInfo(),
    mainPhoto: '',
    userPhotos: [],
    usersOnline: [],
    friendList: [],
    loading: false,
    messagesThread: null,
    messageGoBottom: false,
    messagesPageIndex: 0,
    messagesGroups: [],
    messageUnreadCount: 0,
    notifies: [],
    notifyNotReadCount: 0,
    isAuth: false,
    gLoading: false,
    friendRedirectParam: new Friend()
  }
}

@Injectable({
  providedIn: 'root'
})
@StoreConfig({ name: 'account' })
export class DataStore extends Store<DataState>{
  constructor() {
    super(createInitialState());
  }
}
