import { Injectable, OnDestroy } from '@angular/core';
import { AngularFireAuth } from '@angular/fire/auth';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router, CanLoad, Route, UrlSegment } from '@angular/router';
import { Observable, of, Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs/operators';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, OnDestroy {
  private _onDestroy = new Subject();
  constructor(
    private afAuth: AngularFireAuth,
    private accountService: AccountService,
    private router: Router) {
  }

  ngOnDestroy(): void {
    this._onDestroy.next();
  }


  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.accountService.currentUser$
      .pipe(
        takeUntil(this._onDestroy),
        map(user => {
          if (user) {
            return true;
          }
          else {
            this.router.navigate(['account/login'], { queryParams: { returnUrl: state.url } });
            return false;
          }
        }));
  }

}
