import { TestBed } from '@angular/core/testing';
import { configureCommonTestBed } from '../../../testing/test-bed';

import { NotificationService } from './notification.service';

describe('NotificationService', () => {
  let service: NotificationService;

  beforeEach(async () => {
    await configureCommonTestBed();
    service = TestBed.inject(NotificationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
