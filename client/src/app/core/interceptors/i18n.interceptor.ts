import { Injectable, Injector } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

@Injectable()
export class I18nInterceptor implements HttpInterceptor {
  constructor(private router: Router,
    private translate: TranslateService) {
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let lang = this.translate.currentLang;
    request = request.clone({
      setHeaders: {
        'Accept-Language': lang,
      }
    })
    return next.handle(request);
  }
}
