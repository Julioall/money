import { HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { Account } from '../../models/account';
import { configureCommonTestBed } from '../../../testing/test-bed';
import { AccountsService } from './accounts.service';

describe('AccountsService', () => {
  let service: AccountsService;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await configureCommonTestBed();
    service = TestBed.inject(AccountsService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpMock.verify());

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('loads accounts from the Account API endpoint', () => {
    const accounts: Account[] = [{
      id: 'account-1',
      accountName: 'Checking',
      balance: 25,
      hideBalance: false,
      iconUrl: 'img/logo-1.svg',
    }];

    service.getAll().subscribe((result) => expect(result).toEqual(accounts));

    const request = httpMock.expectOne(`${service.baseUrl}/Account`);
    expect(request.request.method).toBe('GET');
    request.flush(accounts);
  });

  it('creates an account with POST', () => {
    const account: Account = {
      accountName: 'Savings',
      balance: 100,
      hideBalance: true,
      iconUrl: 'img/logo-2.svg',
    };

    service.create(account).subscribe((result) => expect(result).toEqual(account));

    const request = httpMock.expectOne(`${service.baseUrl}/Account`);
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual(account);
    request.flush(account);
  });

  it('updates an account with PUT', () => {
    const account: Account = {
      id: 'account-1',
      accountName: 'Daily account',
      balance: 250,
      hideBalance: false,
      iconUrl: 'img/logo-3.svg',
    };

    service.update(account).subscribe((result) => expect(result).toEqual(account));

    const request = httpMock.expectOne(`${service.baseUrl}/Account`);
    expect(request.request.method).toBe('PUT');
    expect(request.request.body).toEqual(account);
    request.flush(account);
  });

  it('removes an account by id with DELETE', () => {
    service.delete('account-1').subscribe((result) => expect(result).toBeNull());

    const request = httpMock.expectOne(`${service.baseUrl}/Account/account-1`);
    expect(request.request.method).toBe('DELETE');
    request.flush(null, { status: 204, statusText: 'No Content' });
  });
});
