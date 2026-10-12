import { TestBed } from '@angular/core/testing';
import { configureCommonTestBed } from '../../../testing/test-bed';

import { CategoriesService } from './categories.service';

describe('CategoriesService', () => {
  let service: CategoriesService;

  beforeEach(async () => {
    await configureCommonTestBed();
    service = TestBed.inject(CategoriesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
