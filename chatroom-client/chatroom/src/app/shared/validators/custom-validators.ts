import { HttpClient } from "@angular/common/http";
import { CoreEnvironment } from "@angular/compiler/src/compiler_facade_interface";
import { Component, Directive, Injector } from "@angular/core";
import { AbstractControl, AsyncValidatorFn, ValidatorFn } from "@angular/forms";
import { of, timer } from "rxjs";
import { catchError, debounceTime, map, switchMap } from "rxjs/operators";
import { environment } from "src/environments/environment";
import { Res } from "../models/response";
import { InjectorInstance } from "../shared.module";
import { AccountStore } from "../states/account/account.store";




export class CustomValidators {
  static apiUrl = environment.apiUrl;
  constructor() {
  }



  static isMatch(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value
        ? null : { isMatching: true }
    }
  }

  static isUserDuplicated(control: AbstractControl) {
    return CustomValidators.userDuplicated()(control);
  }

  static userDuplicated(): AsyncValidatorFn {
    return (control: AbstractControl) => {
      if (!control.value) {
        return of(null);
      }
      return CustomValidators.checkUserDuplicate(control.value);
    };
  }
  private static checkUserDuplicate(emailOrUserName: string) {
    const accountStore = InjectorInstance.get(AccountStore);
    const http = InjectorInstance.get(HttpClient);
    accountStore.update({
      loading: true
    });
    return http.get(`${this.apiUrl}account/checkUserDuplicated?emailOrUsername=${emailOrUserName}`)
      .pipe(
        debounceTime(200),
        map((res: Res<boolean>) => {
          accountStore.update({
            loading: false
          });
          return res.data ? { userDuplicated: true } : null;
        }),
        catchError(error => {
          accountStore.update({
            loading: false
          });
          console.log(error);
          return of(error);
        })
      );
  }



}
