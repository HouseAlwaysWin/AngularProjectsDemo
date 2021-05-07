import { Injectable } from '@angular/core';
import { ILoginForm } from '../models/loginForm';
import { AngularFireAuth } from '@angular/fire/auth';
import { IRegisterForm } from '../models/registerForm';
import { BehaviorSubject, Observable, of, ReplaySubject, Subject } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { IApiResponse } from '../models/apiResponse';
import { IUser } from '../models/user';
import { catchError, map } from 'rxjs/operators';
import { IAddress } from '../models/address';
import { MatDialog } from '@angular/material/dialog';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  currrentUser = new BehaviorSubject<IUser>(null);
  currentUser$ = this.currrentUser.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router) { }


  GetUserState(token: string) {
    if (token === null) {
      this.currrentUser.next(null);
      return of(null);
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.get(this.baseUrl + 'account/getuser', { headers }).pipe(
      map((res: IApiResponse<IUser>) => {
        if (res.isSuccessed) {
          localStorage.setItem('token', res.data.token);
          this.currrentUser.next(res.data);
        }
      }),
      catchError(error => {
        localStorage.removeItem('token');
        this.currrentUser.next(null);
        return of(error);
      })
    );
  }

  register(registerData: IRegisterForm) {
    return this.http.post(this.baseUrl + 'account/register', registerData).pipe(
      map((res: IApiResponse<IUser>) => {
        console.log(res);
        if (res.isSuccessed) {
          localStorage.setItem('token', res.data.token);
          this.currrentUser.next(res.data);
        }
        return res;
      })
    )
  }

  login(loginData: ILoginForm) {
    return this.http.post(this.baseUrl + 'account/login', loginData).pipe(
      map((res: IApiResponse<IUser>) => {
        if (res.isSuccessed) {
          localStorage.setItem('token', res.data.token);
          this.currrentUser.next(res.data);
        }
        return res;
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.currrentUser.next(null);
    this.router.navigateByUrl('/shop');
  }

  getUserAddress() {
    return this.http.get(this.baseUrl + 'account/address');
  }

  updateUserAddress(address: IAddress) {
    return this.http.put(this.baseUrl + 'account/address', address);
  }

}
