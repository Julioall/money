import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { delay, finalize } from 'rxjs/operators';
import { LoadingService } from './loading.service'; // Atualize o caminho conforme necessário
import { AuthService } from '../auth/auth.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  constructor(
    private loadingService: LoadingService,
    private authService: AuthService // Adicione o AuthService aqui
  ) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    // Obtém o cabeçalho de autenticação
    const authHeaders = this.authService.getAuthHeaders();

    // Clona a requisição e adiciona os cabeçalhos de autenticação
    const authReq = req.clone({ headers: authHeaders });

    this.loadingService.startLoading();
    return next.handle(authReq).pipe(
      delay(1000),
      finalize(() => {
        this.loadingService.stopLoading();
      })
    );
  }
}
