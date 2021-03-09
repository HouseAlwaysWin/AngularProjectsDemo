import { Injectable } from '@angular/core';
import { AngularFireAuth } from '@angular/fire/auth';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router, CanLoad, Route, UrlSegment } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { FbAuthService } from './fb-auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanLoad {
  /**
   *
   */
  constructor(
    private afAuth: AngularFireAuth,
    private authService: FbAuthService,
    private router: Router) { }
  canLoad(route: Route, segments: UrlSegment[]): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    return this.authService.isAuth$.pipe(map(
      isAuth => {
        console.log(isAuth);
        if (isAuth) {
          this.router.navigate(['/home']);
        }
        return true;
      }
    ));
  }


  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.authService.isAuth$.pipe(map(
      isAuth => {
        if (isAuth) {
          return true;
        }
        this.router.navigate(['account/login'],
          { queryParams: { returnUrl: state.url } });
        return false;
      }
    ));
  }

}
