import { TestBed, inject } from '@angular/core/testing';

import { Category } from '../models/category';
import { BaseRepositoryService, ENTITY_NAME_TOKEN } from './base-repository.service';
import { configureCommonTestBed } from '../../testing/test-bed';

describe('Service: BaseRepository', () => {
  beforeEach(async () => {
    await configureCommonTestBed({
      providers: [
        BaseRepositoryService,
        { provide: ENTITY_NAME_TOKEN, useValue: 'category' },
      ]
    });
  });

  it('should ...', inject([BaseRepositoryService], (service: BaseRepositoryService<Category>) => {
    expect(service).toBeTruthy();
  }));
});
