import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { BaseRepositoryService } from '../baseRepository.service';
import { Category } from '../../model/entities/category';
import { NotificationService } from '../../service/notification/notification.service';

@Injectable({
  providedIn: 'root',
  useFactory: (http: HttpClient, notificationService: NotificationService) => {
    return new CategoriesRepositoryService(http, notificationService);
  },
  deps: [HttpClient, NotificationService]
})
export class CategoriesRepositoryService extends BaseRepositoryService<Category> {
  constructor(http: HttpClient, notificationService: NotificationService) {
    super(notificationService, http, 'Category');
  }

  getCategoriesByName(name: string): Observable<Category[]> {
    return this.getAll().pipe(
      map(categories => categories.filter(category => category.name.includes(name)))
    );
  }
}
