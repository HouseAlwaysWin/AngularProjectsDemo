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
  messagesPageIndex: number;
  messagesGroups: MessageGroup[];
  messageUnreadCount: number;
  notifies: Notify[],
  notifyNotReadCount: number,
  isAuth: boolean;
  gLoading: boolean;
  friendRedirectParam: string;
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
    messagesPageIndex: 0,
    messagesGroups: [],
    messageUnreadCount: 0,
    notifies: [],
    notifyNotReadCount: 0,
    isAuth: false,
    gLoading: false,
    friendRedirectParam: ''
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
