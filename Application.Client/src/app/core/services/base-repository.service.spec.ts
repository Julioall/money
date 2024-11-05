import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ToastrModule } from 'ngx-toastr';

import { NotificationService } from './notification/notification.service';
import { Category } from '../models/category';
import { BaseRepositoryService, ENTITY_NAME_TOKEN } from './base-repository.service';

describe('Service: BaseRepository', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        ToastrModule.forRoot(),
      ],
      providers: [
        BaseRepositoryService,
        NotificationService,
        { provide: ENTITY_NAME_TOKEN, useValue: 'category' },
      ]
    });
  });

  it('should ...', inject([BaseRepositoryService], (service: BaseRepositoryService<Category>) => {
    expect(service).toBeTruthy();
  }));
});
