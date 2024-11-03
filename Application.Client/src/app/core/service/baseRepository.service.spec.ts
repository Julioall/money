import { TestBed, waitForAsync, inject } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ToastrModule } from 'ngx-toastr';

import { BaseRepositoryService } from './baseRepository.service';
import { NotificationService } from '../service/notification/notification.service';
import { ENTITY_NAME_TOKEN } from './baseRepository.service';
import { Category } from '../model/entities/category';

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
