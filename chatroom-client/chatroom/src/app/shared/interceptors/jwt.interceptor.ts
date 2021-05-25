import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountQuery } from '../states/account/account.query';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountQuery: AccountQuery) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let token = this.accountQuery.token;
    console.log(token);
    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    })
    return next.handle(request);
  }
}
