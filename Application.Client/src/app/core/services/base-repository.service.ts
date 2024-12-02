import { Injectable, Inject, InjectionToken } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environment/environment';
import { NotificationService } from './notification/notification.service';

export const ENTITY_NAME_TOKEN = new InjectionToken<string>('entityName');

@Injectable({
  providedIn: 'root'
})
export class BaseRepositoryService<T> {
  public baseUrl = environment.apiUrl;

  constructor(
    private notificationService: NotificationService,
    protected http: HttpClient,
    @Inject(ENTITY_NAME_TOKEN) protected entityName: string
  ) {}

  getAll(): Observable<T[]> {
    return this.http.get<T[]>(`${this.baseUrl}/${this.entityName}`)
      .pipe(catchError(this.handleError<T[]>('getAll', [])));
  }

  getById(id: string): Observable<T> {
    return this.http.get<T>(`${this.baseUrl}/${this.entityName}/${id}`)
      .pipe(catchError(this.handleError<T>('getById')));
  }

  create(item: T): Observable<T> {
    return this.http.post<T>(`${this.baseUrl}/${this.entityName}`, item)
      .pipe(catchError(this.handleError<T>('create')));
  }

  update(item: T): Observable<T> {
    return this.http.put<T>(`${this.baseUrl}/${this.entityName}`, item)
      .pipe(catchError(this.handleError<T>('update')));
  }

  delete(id: string): Observable<boolean> {
    return this.http.delete<boolean>(`${this.baseUrl}/${this.entityName}/${id}`)
      .pipe(catchError(this.handleError<boolean>('delete', false)));
  }

  handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      this.notificationService.showError(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }
}
