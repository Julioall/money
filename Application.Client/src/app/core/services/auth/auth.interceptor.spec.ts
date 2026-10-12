import { TestBed } from '@angular/core/testing';
import { configureCommonTestBed } from '../../../testing/test-bed';

import { AuthInterceptor } from './auth.interceptor';

describe('AuthInterceptor', () => {
  beforeEach(async () => {
    await configureCommonTestBed({ providers: [AuthInterceptor] });
  });

  it('should be created', () => {
    const interceptor: AuthInterceptor = TestBed.inject(AuthInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
