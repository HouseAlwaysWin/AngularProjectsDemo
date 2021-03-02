import { Injectable } from '@angular/core';
import { ILoginForm } from '../models/loginForm';
import { AngularFireAuth } from '@angular/fire/auth';
import { IRegisterForm } from '../models/registerForm';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private afAuth: AngularFireAuth) { }

  regiaterByFB(registerData: IRegisterForm) {
    this.afAuth.createUserWithEmailAndPassword(
      registerData.email, registerData.password
    ).then(result => {
      console.log(result);
    }).catch(error => {

    });
  }

  loginByFB(loginData: ILoginForm) {
    this.afAuth.signInWithEmailAndPassword(
      loginData.email,
      loginData.password
    ).then(result => {
      console.log(result);
    }).catch(error => {

    });
  }
}