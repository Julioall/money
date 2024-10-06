/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { BaseRepositoryService } from './baseRepository.service';

describe('Service: BaseRepository', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BaseRepositoryService]
    });
  });

  it('should ...', inject([BaseRepositoryService], (service: BaseRepositoryService) => {
    expect(service).toBeTruthy();
  }));
});
