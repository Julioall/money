import { TestBed } from '@angular/core/testing';

import { CategoriesRepositoryService } from './categories-repository.service';

describe('CategoriesRepositoryService', () => {
  let service: CategoriesRepositoryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CategoriesRepositoryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
