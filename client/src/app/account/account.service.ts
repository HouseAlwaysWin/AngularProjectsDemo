import { Injectable } from '@angular/core';
import { ILoginForm } from '../models/loginForm';
import { AngularFireAuth } from '@angular/fire/auth';
import { IRegisterForm } from '../models/registerForm';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  authChange = new Subject<boolean>();

  constructor(private afAuth: AngularFireAuth) { }

  regiaterByFB(registerData: IRegisterForm) {
    this.afAuth.createUserWithEmailAndPassword(
      registerData.email, registerData.password
    ).then(result => {
      console.log(result);
    }).catch(error => {
      console.log(error);
    });
  }

  loginByFB(loginData: ILoginForm) {
    this.afAuth.signInWithEmailAndPassword(
      loginData.email,
      loginData.password
    ).then(result => {
      console.log(result);
    }).catch(error => {
      console.log(error);
    });
  }

  logoutByFB() {
    this.afAuth.signOut();
  }

  isAuth() {
    this.afAuth.authState.subscribe(user => {
      console.log(user);
      if (user) {
        this.authChange.next(true);
      }
      else {
        this.authChange.next(false);
      }
    });
  }
}
