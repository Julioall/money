import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { NotificationService } from '../notification/notification.service';
import { User } from '../../models/user';
import { BaseRepositoryService } from '../base-repository.service';

@Injectable({
  providedIn: 'root',
  useFactory: (http: HttpClient, notificationService: NotificationService) => {
    return new UserService(http, notificationService);
  },
  deps: [HttpClient, NotificationService]
})
export class UserService extends BaseRepositoryService<User> {
  constructor(
    http: HttpClient,
    notificationService: NotificationService
  ) {
    super(notificationService, http, 'user');
  }

  getUsersByRole(role: string): Observable<User[]> {
    return this.http.get<User[]>(`${this.baseUrl}/user/get-by-role/${role}`)
      .pipe(
        catchError(this.handleError<User[]>('getUsersByRole', []))
      );
  }

  getUserByEmail(email: string): Observable<User> {
    return this.http.get<User>(`${this.baseUrl}/user/get-by-email/${email}`)
      .pipe(
        catchError(this.handleError<User>('getUserByEmail'))
      );
  }
}
