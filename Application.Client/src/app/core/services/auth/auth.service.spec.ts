import { TestBed } from '@angular/core/testing';
import { configureCommonTestBed } from '../../../testing/test-bed';

import { AuthService } from './auth.service';

describe('AuthService', () => {
  let service: AuthService;

  beforeEach(async () => {
    await configureCommonTestBed();
    service = TestBed.inject(AuthService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
