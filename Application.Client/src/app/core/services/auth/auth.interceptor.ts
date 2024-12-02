import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, finalize, Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { BusyService } from '../busy/busy.service';
import { TranslateService } from '@ngx-translate/core';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(
    private busyService: BusyService,
    private authService: AuthService,
    private translate: TranslateService
  ) {
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    this.busyService.show();
    const token = this.authService.getToken();
    const authReq = request.clone({
      headers: request.headers.set('Authorization', `Bearer ${token}`)
    });

    return next.handle(authReq).pipe(
      finalize(() => {
        this.busyService.hide();
      }),
      catchError((error) => {
        if (error.status === 401) {
          this.authService.logout();
          throw error(this.translate.instant('services.auth.errorUnauthorizedRequest'));
        }
        throw error;
      })
    );
  }
}
