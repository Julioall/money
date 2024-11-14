import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { finalize, Observable, catchError } from 'rxjs';
import { LoadingService } from '../loading/loading.service';
import { AuthService } from './auth.service';


@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(
    private loadingService: LoadingService,
    private authService: AuthService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const authHeaders = this.authService.getAuthHeaders();
    const clonedRequest = request.clone({ headers: authHeaders });

    this.loadingService.startLoading();

    return next.handle(clonedRequest).pipe(
      finalize(() => {
        this.loadingService.stopLoading();
      }),
      catchError((error) => {
        if (error.status === 401) {
          throw error('Unauthorized request');
        }
        console.error('Error on requisition:', error);
        throw error;
      })
    );
  }
}
