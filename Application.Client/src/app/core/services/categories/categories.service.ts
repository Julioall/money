import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { NotificationService } from '../notification/notification.service';
import { Category } from '../../models/category';
import { BaseRepositoryService } from '../base-repository.service';

@Injectable({
  providedIn: 'root',
  useFactory: (http: HttpClient, notificationService: NotificationService) => {
    return new CategoriesService(http, notificationService);
  },
  deps: [HttpClient, NotificationService]
})
export class CategoriesService extends BaseRepositoryService<Category> {
  constructor(http: HttpClient, notificationService: NotificationService) {
    super(notificationService, http, 'Category');
  }

  getCategoriesByName(name: string): Observable<Category[]> {
    return this.getAll().pipe(
      map(categories => categories.filter(category => category.name.includes(name)))
    );
  }
}
