import { Injectable } from '@angular/core';
import { AngularFireAuth } from '@angular/fire/auth';
import { Subject } from 'rxjs';
import { ILoginForm } from '../models/loginForm';
import { IRegisterForm } from '../models/registerForm';

@Injectable({
  providedIn: 'root'
})
export class FbAuthService {
  authChange = new Subject<boolean>();
  private isAuthenticated = false;
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
      console.log(result);
    }).catch(error => {
      console.log(error);
    });
  }

  logout() {
    this.afAuth.signOut();
  }

  authListener() {
    this.afAuth.authState.subscribe(user => {
      if (user) {
        this.isAuthenticated = true;
        this.authChange.next(true);
      } else {
        this.isAuthenticated = false;
        this.authChange.next(false);
      }
    });
  }

  isAuth() {
    return this.isAuthenticated;
  }

}
