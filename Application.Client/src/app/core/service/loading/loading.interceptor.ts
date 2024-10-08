import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { delay, finalize } from 'rxjs/operators';
import { LoadingService } from './loading.service';
import { AuthService } from '../auth/auth.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  constructor(
    private loadingService: LoadingService,
    private authService: AuthService
  ) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const authHeaders = this.authService.getAuthHeaders();
    const authReq = req.clone({ headers: authHeaders });

    this.loadingService.startLoading();
    return next.handle(authReq).pipe(
      delay(300),
      finalize(() => {
        this.loadingService.stopLoading();
      })
    );
  }
}
