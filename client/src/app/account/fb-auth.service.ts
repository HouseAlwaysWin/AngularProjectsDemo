import { Injectable } from '@angular/core';
import { AngularFireAuth } from '@angular/fire/auth';
import { BehaviorSubject, ReplaySubject, Subject } from 'rxjs';
import { ILoginForm } from '../models/loginForm';
import { IRegisterForm } from '../models/registerForm';

@Injectable({
  providedIn: 'root'
})
export class FbAuthService {
  isAuth$ = new ReplaySubject<boolean>();
  constructor(private afAuth: AngularFireAuth) { }

  regiater(registerData: IRegisterForm) {
    this.afAuth.createUserWithEmailAndPassword(
      registerData.email, registerData.password
    ).then(result => {
      console.log(result);
    }).catch(error => {
      console.log(error);
    });
  }

  login(loginData: ILoginForm) {
    this.afAuth.signInWithEmailAndPassword(
      loginData.email,
      loginData.password
    ).then(result => {
      console.log(result.user);
      this.isAuth$.next(true);
    }).catch(error => {
      this.isAuth$.next(false);
      console.log(error);
    });
    return this.isAuth$;
  }

  logout() {
    console.log('logout');
    this.afAuth.signOut();
  }

  authListener() {
    this.afAuth.authState.subscribe(user => {
      console.log(user);
      if (user) {
        this.isAuth$.next(true);
      } else {
        this.isAuth$.next(false);
      }
    });
  }



}
