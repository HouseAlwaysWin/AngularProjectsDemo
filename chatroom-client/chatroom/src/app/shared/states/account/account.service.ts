import { Injectable } from "@angular/core";
import { AccountStore } from "./account.store";
import { HttpClient } from '@angular/common/http';
import { Login } from "../../models/login";
import { environment } from "src/environments/environment";
import { Res } from "../../models/response";
import { UserToken } from "../../models/user";
import { catchError, map } from 'rxjs/operators'
import { of } from "rxjs";
import { AccountQuery } from "./account.query";
import { Register } from "../../models/register";
import { SharedStore } from "../shared/shared.store";

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  apiUrl = environment.apiUrl;
  constructor(
    private http: HttpClient,
    private query: AccountQuery,
    private store: AccountStore,
    private sharedStore: SharedStore) {
  }

  register(model: Register) {
    this.sharedStore.update({ gLoading: true });
    return this.http.post(`${this.apiUrl}account/register`, model).pipe(
      map((res: Res<UserToken>) => {
        if (res.isSuccessed) {
          this.setCurrentUser(res.data);
        }
        this.sharedStore.update({ gLoading: false });
        return res;
      }),
      catchError(error => {
        this.sharedStore.update({ gLoading: false });
        localStorage.removeItem('user');
        console.log(error);
        return of(error);
      })
    )
  }


  login(model: Login) {
    this.sharedStore.update({ gLoading: true });
    return this.http.post(`${this.apiUrl}account/login`, model).pipe(
      map((res: Res<UserToken>) => {
        console.log(res);
        if (res.isSuccessed) {
          this.setCurrentUser(res.data);
        }
        this.sharedStore.update({ gLoading: false });
        return res;
      }),
      catchError(error => {
        this.sharedStore.update({ gLoading: false });
        localStorage.removeItem('user');
        console.log(error);
        return of(error);
      })
    )
  }

  logout() {
    this.sharedStore.update({ gLoading: true });
    localStorage.removeItem('user');
    this.store.update(null);
    this.sharedStore.update({ gLoading: false });
  }

  setCurrentUser(user: UserToken) {
    localStorage.setItem('user', JSON.stringify(user));
    this.store.update({
      ...user,
      isAuth: true
    })
  }
}


