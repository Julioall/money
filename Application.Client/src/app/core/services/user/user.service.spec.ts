import { TestBed } from '@angular/core/testing';
import { configureCommonTestBed } from '../../../testing/test-bed';

import { UserService } from './user.service';

describe('UserService', () => {
  let service: UserService;

  beforeEach(async () => {
    await configureCommonTestBed();
    service = TestBed.inject(UserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
