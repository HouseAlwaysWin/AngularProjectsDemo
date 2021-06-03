import { Injectable } from "@angular/core";
import { AccountStore } from "./account.store";
import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { Login } from "../../models/login";
import { environment } from "src/environments/environment";
import { Res } from "../../models/response";
import { UserDetail, UserPhoto, UserShortInfo } from "../../models/user";
import { catchError, map } from 'rxjs/operators'
import { of } from "rxjs";
import { AccountQuery } from "./account.query";
import { Register } from "../../models/register";
import { SharedStore } from "../shared/shared.store";
import { Friend } from "../../models/friend";

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  apiUrl = environment.apiUrl;
  constructor(
    private http: HttpClient,
    private accountQuery: AccountQuery,
    private accountStore: AccountStore,
    private sharedStore: SharedStore) {
  }

  private setCurrentUser(userDetail: UserShortInfo) {
    localStorage.setItem('token', JSON.stringify(userDetail.token));
    this.accountStore.update({
      user: userDetail,
      userPhotos: userDetail.photos,
      mainPhoto: userDetail.photos.filter(p => p.isMain)[0]?.url,
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
        console.log(res);
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

  logout() {
    this.sharedStore.update({ gLoading: true });
    localStorage.removeItem('token');
    this.accountStore.update(null);
    this.sharedStore.update({ gLoading: false });
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
        console.log(res);
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

  addNewFriend(id: number) {
    return this.http.post(`${this.apiUrl}user/add-friend/${id}`, null).pipe(
      map((res: Res<Friend[]>) => {
        this.accountStore.update({
          friendList: res.data
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






}


