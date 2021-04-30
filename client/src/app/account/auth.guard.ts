import { Injectable, OnDestroy } from '@angular/core';
import { AngularFireAuth } from '@angular/fire/auth';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router, CanLoad, Route, UrlSegment } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs/operators';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, OnDestroy {
  isAuth: boolean;
  private _onDestroy = new Subject();
  constructor(
    private afAuth: AngularFireAuth,
    private accountService: AccountService,
    private router: Router) {
    this.accountService.currentUser$
      .pipe(takeUntil(this._onDestroy))
      .subscribe(user => {
        if (user) {
          this.isAuth = true;
        }
        else {
          this.isAuth = false;
        }
      });
  }

  ngOnDestroy(): void {
    this._onDestroy.next();
  }


  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (this.isAuth) {
      return true;
    }
    this.router.navigate(['account/login'],
      { queryParams: { returnUrl: state.url } });
    return false;
  }

}
