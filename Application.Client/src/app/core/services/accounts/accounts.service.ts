import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NotificationService } from '../notification/notification.service';
import { BaseRepositoryService } from '../base-repository.service';
import { Account } from '../../models/account';

@Injectable({
  providedIn: 'root',
  useFactory: (http: HttpClient, notificationService: NotificationService) => {
    return new AccountsService(http, notificationService);
  },
  deps: [HttpClient, NotificationService]
})
export class AccountsService extends BaseRepositoryService<Account> {
  constructor(http: HttpClient, notificationService: NotificationService) {
    super(notificationService, http, 'Account');
  }
}
