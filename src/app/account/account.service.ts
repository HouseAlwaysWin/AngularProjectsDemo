import { Injectable } from '@angular/core';
import { ILoginForm } from '../models/loginForm';
import { AngularFireAuth } from '@angular/fire/auth';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private afAuth: AngularFireAuth) { }

  regiaterByFB() {

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
