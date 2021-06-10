import { Injectable } from "@angular/core";
import { AccountStore } from "../states/account/account.store";
import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { Login } from "../models/login";
import { environment } from "src/environments/environment";
import { Res, ResPaging } from "../models/response";
import { UserDetail, UserPhoto, UserShortInfo } from "../models/user";
import { catchError, map } from 'rxjs/operators'
import { BehaviorSubject, of } from "rxjs";
import { AccountQuery } from "../states/account/account.query";
import { Register } from "../models/register";
import { SharedStore } from "../states/shared/shared.store";
import { AcceptFriend, Friend } from "../models/friend";
import { PresenceService } from "./presence.service";
import { Notify, NotifyList } from "../models/notification";

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  apiUrl = environment.apiUrl;
  constructor(
    private http: HttpClient,
    private accountQuery: AccountQuery,
    private accountStore: AccountStore,
    private sharedStore: SharedStore,
    private presenceService: PresenceService
  ) {
  }


  private setCurrentUser(user: UserShortInfo) {
    localStorage.setItem('token', JSON.stringify(user.token));
    this.accountStore.update({
      user: user,
      userPhotos: user.photos,
      mainPhoto: user.photos.filter(p => p.isMain)[0]?.url,
      isAuth: true
    })
  }

  getUserDetail() {
    return this.http.get(`${this.apiUrl}account/getUserInfo`)
      .pipe(
        map((res: Res<UserShortInfo>) => {
          if (res.isSuccessed) {
            this.setCurrentUser(res.data);
          }
          return res.isSuccessed;
        }),
        catchError(error => {
          console.log(error);
          return of(error);
        })
      );
  }

  register(model: Register) {
    this.sharedStore.update({ gLoading: true });
    return this.http.post(`${this.apiUrl}account/register`, model).pipe(
      map((res: Res<UserShortInfo>) => {
        if (res.isSuccessed) {
          this.setCurrentUser(res.data);
        }
        this.sharedStore.update({ gLoading: false });
        return res;
      }),
      catchError(error => {
        this.sharedStore.update({ gLoading: false });
        localStorage.removeItem('token');
        console.log(error);
        return of(error);
      })
    )
  }


  login(model: Login) {
    this.sharedStore.update({ gLoading: true });
    return this.http.post(`${this.apiUrl}account/login`, model).pipe(
      map((res: Res<UserShortInfo>) => {
        if (res.isSuccessed) {
          this.setCurrentUser(res.data);
        }
        this.sharedStore.update({ gLoading: false });
        this.presenceService.createHubConnection();
        return res;
      }),
      catchError(error => {
        this.sharedStore.update({ gLoading: false });
        localStorage.removeItem('token');
        console.log(error);
        return of(error);
      })
    )
  }

  logout() {
    this.sharedStore.update({ gLoading: true });
    localStorage.removeItem('token');
    this.accountStore.update(null);
    this.sharedStore.update({ gLoading: false });
    this.presenceService.stopHubConnection();
  }

  setUserPhotoAsMain(photo: UserPhoto) {
    this.sharedStore.update({ gLoading: true });
    return this.http.put(`${this.apiUrl}user/set-main-photo/${photo.id}`, {})
      .pipe(
        map((res: Res<any>) => {
          if (res.isSuccessed) {
            this.accountStore.update(state => {
              let statePhotos = state.user.photos;
              for (let i = 0; i < statePhotos.length; i++) {
                statePhotos[i].isMain = false;
                if (statePhotos[i].id === photo.id) {
                  statePhotos[i].isMain = true;
                }
              }
              state.user.photos = statePhotos;

              return ({
                user: state.user
              })
            });
          }
          this.sharedStore.update({ gLoading: false });
          return res;
        }),
        catchError(error => {
          this.sharedStore.update({ gLoading: false });
          console.log(error);
          return of(error);
        }));
  }

  uploadUserPhoto(files: any) {
    const formData: FormData = new FormData();
    for (let i = 0; i < files.length; i++) {
      formData.append('files', files[i]);
    }

    return this.http.post(`${this.apiUrl}user/add-photos`, formData,
      {
        reportProgress: true,
        observe: 'events',
      }).pipe(
        catchError(error => {
          this.sharedStore.update({ gLoading: false });
          console.log(error);
          return of(error);
        })
      );

  }

  deleteUserPhoto(photo: UserPhoto) {
    this.sharedStore.update({ gLoading: true });
    console.log(photo);
    return this.http.delete(`${this.apiUrl}user/delete-photo/${photo.id}`, {})
      .pipe(
        map((res: Res<any>) => {
          if (res.isSuccessed) {

            this.accountStore.update(state => {
              let index = state.user.photos.indexOf(photo);
              if (index > -1) {
                state.user.photos.splice(index, 1);
              }
            });
          }
          this.sharedStore.update({ gLoading: false });
          return res;
        }),
        catchError(error => {
          this.sharedStore.update({ gLoading: false });
          console.log(error);
          return of(error);
        }));
  }


  getFriends() {
    return this.http.get(`${this.apiUrl}user/get-friends`).pipe(
      map((res: Res<Friend[]>) => {
        if (res.isSuccessed) {
          this.accountStore.update({
            friendList: res.data
          });
        }
        return res;
      }),
      catchError(error => {
        console.log(error);
        return of(error);
      })
    )
  }

  findFriendByPublicId(publicId: string) {
    return this.http.get(`${this.apiUrl}user/get-by-publicId/${publicId}`);
  }

  acceptFriend(id: number, notifyId: number) {
    return this.http.post(`${this.apiUrl}user/accept-friend/${id}/${notifyId}`, null).pipe(
      map((res: AcceptFriend) => {
        console.log(res);
        this.accountStore.update({
          friendList: res.friends,
          notifies: res.notifications.notifications.data,
          notifyNotReadCount: res.notifications.notReadTotalCount
        })
        return res;
      })
    );
  }

  rejectFriend(notifyId: number) {
    console.log(notifyId)
    return this.http.post(`${this.apiUrl}user/reject-friend/${notifyId}`, null).pipe(
      map((res: NotifyList) => {
        console.log(res);
        this.accountStore.update({
          notifies: res.notifications.data,
          notifyNotReadCount: res.notReadTotalCount
        })
        return res;
      })
    );
  }

  updateNotify() {
    console.log('update');
    return this.http.put(`${this.apiUrl}user/update-readall-notifications`, null).pipe(
      map((res: NotifyList) => {
        console.log(res);
        this.accountStore.update({
          notifies: res.notifications.data,
          notifyNotReadCount: res.notReadTotalCount
        })
        return res;
      })
    );
  }

  removeFriend(friend: Friend) {
    return this.http.delete(`${this.apiUrl}user/remove-friend/${friend.friendId}`).pipe(
      map((res: Res<Friend[]>) => {
        this.accountStore.update({
          friendList: res.data
        })
        return res;
      })
    );
  }

  updatePublicId(publicId: string) {
    return this.http.put(`${this.apiUrl}user/update-publicId/${publicId}`, null);
  }


  getNotifies() {
    return this.http.get(`${this.apiUrl}user/get-notifications`);
  }





}


