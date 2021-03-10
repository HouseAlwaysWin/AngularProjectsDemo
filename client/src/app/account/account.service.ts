import { Injectable } from '@angular/core';
import { ILoginForm } from '../models/loginForm';
import { AngularFireAuth } from '@angular/fire/auth';
import { IRegisterForm } from '../models/registerForm';
import { Observable, of, ReplaySubject, Subject } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { IApiResponse } from '../models/apiResponse';
import { IUser } from '../models/user';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currrentUser = new ReplaySubject<IUser>();
  isAuth = new ReplaySubject<boolean>();
  currentUser$ = this.currrentUser.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router) { }


  GetUserState(token: string) {
    if (token === null) {
      this.currrentUser.next(null);
      this.isAuth.next(false);
      return of(null);
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
    return this.http.get(this.baseUrl + 'account/gettoken', { headers }).pipe(
      map((res: IApiResponse<IUser>) => {
        if (res.statusCode === 200) {
          localStorage.setItem('token', res.data.token);
          this.currrentUser.next(res.data);
          this.isAuth.next(true);
        }

      })
    );
  }

  regiater(registerData: IRegisterForm) {
    return this.http.post(this.baseUrl + 'account/register', registerData).pipe(
      map((res: IApiResponse<IUser>) => {
        if (res.statusCode === 200) {
          localStorage.setItem('token', res.data.token);
          this.currrentUser.next(res.data);
          this.isAuth.next(true);
        }
      })
    )
  }

  login(loginData: ILoginForm) {
    return this.http.post(this.baseUrl + 'account/login', loginData).pipe(
      map((res: IApiResponse<IUser>) => {
        if (res.statusCode === 200) {
          console.log(res);
          localStorage.setItem('token', res.data.token);
          this.currrentUser.next(res.data);
          this.isAuth.next(true);
        }
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.currrentUser.next(null);
    this.isAuth.next(false);
    this.router.navigateByUrl('/');
  }

}
