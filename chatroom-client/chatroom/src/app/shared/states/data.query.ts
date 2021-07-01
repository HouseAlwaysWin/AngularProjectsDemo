import { Injectable } from "@angular/core";
import { Query } from "@datorama/akita";
import { DataState, DataStore } from "./data.store";


@Injectable({
  providedIn: 'root'
})
export class DataQuery extends Query<DataState> {
  all$ = this.select();
  user$ = this.select(state => state.user);;
  mainPhoto$ = this.select(state => state.user.photos.filter(p => p.isMain)[0]?.url);
  userPhotos = this.select('userPhotos');
  token$ = this.select(state => state.user.token);
  email$ = this.select(state => state.user.email);
  username$ = this.select(state => state.user.userName);
  photos$ = this.select(state => state.user.photos);
  isAuth$ = this.select('isAuth');
  friendList$ = this.select('friendList');
  usersOnline$ = this.select('usersOnline');
  loading$ = this.select('loading');
  messagesThread$ = this.select('messagesThread');
  messageGoBottom$ = this.select('messageGoBottom');
  messagesPageIndex$ = this.select('messagesPageIndex');
  messageGroups$ = this.select('messagesGroups');
  messageUnreadCount$ = this.select('messageUnreadCount');
  notifies$ = this.select('notifies');
  notifyNotReadCount$ = this.select('notifyNotReadCount');
  gLoading$ = this.select('gLoading');
  friendRedirectParam$ = this.select('friendRedirectParam');

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

  get notifyNotReadCount() {
    return this.getValue().notifyNotReadCount;
  }

  get gLoading() {
    return this.getValue().gLoading;
  }

  get messageGroups() {
    return this.getValue().messagesGroups;
  }

  get friendRedirectParam() {
    return this.getValue().friendRedirectParam;
  }

  get messageUnreadCount() {
    return this.getValue().messageUnreadCount;
  }

  get messagesPageIndex() {
    return this.getValue().messagesPageIndex;
  }

  get messageGoBottom() {
    return this.getValue().messageGoBottom;
  }

  constructor(protected store: DataStore) {
    super(store);
  }

}
