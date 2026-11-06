import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { Account } from '../../core/models/account';
import { AccountsService } from '../../core/services/accounts/accounts.service';
import {
  BankCatalogService, BankInstitution,
} from '../../core/services/banks/bank-catalog.service';
import { NotificationService } from '../../core/services/notification/notification.service';
import { configureCommonTestBed } from '../../testing/test-bed';
import { AccountsComponent } from './accounts.component';

describe('AccountsComponent', () => {
  const bank: BankInstitution = {
    ispb: '18236120',
    compe: '260',
    compe4: '0260',
    name: 'NU PAGAMENTOS S.A.',
    shortName: 'NU PAGAMENTOS',
    displayName: 'Nubank',
    logo: { png: 'logos/png/18236120.png', svg: 'logos/svg/18236120.svg' },
  };
  const logoUrl = 'https://cdn.jsdelivr.net/npm/logos-bancos-br@0.8.4/logos/svg/18236120.svg';

  let component: AccountsComponent;
  let fixture: ComponentFixture<AccountsComponent>;
  let accountsService: jasmine.SpyObj<AccountsService>;
  let notifications: jasmine.SpyObj<NotificationService>;
  let bankCatalog: jasmine.SpyObj<BankCatalogService>;

  beforeEach(async () => {
    accountsService = jasmine.createSpyObj<AccountsService>(
      'AccountsService', ['getAll', 'create', 'update', 'delete']
    );
    notifications = jasmine.createSpyObj<NotificationService>(
      'NotificationService', ['showSuccess', 'showError']
    );
    bankCatalog = jasmine.createSpyObj<BankCatalogService>(
      'BankCatalogService', ['load', 'find', 'get', 'logoUrl']
    );

    accountsService.getAll.and.returnValue(of([]));
    bankCatalog.load.and.returnValue(Promise.resolve([bank]));
    bankCatalog.find.and.returnValue([bank]);
    bankCatalog.get.and.callFake(ispb => ispb === bank.ispb ? bank : undefined);
    bankCatalog.logoUrl.and.callFake(institution =>
      institution?.ispb === bank.ispb ? logoUrl : null
    );

    await configureCommonTestBed({
      imports: [AccountsComponent],
      providers: [
        { provide: AccountsService, useValue: accountsService },
        { provide: NotificationService, useValue: notifications },
        { provide: BankCatalogService, useValue: bankCatalog },
      ],
    });
    fixture = TestBed.createComponent(AccountsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    await fixture.whenStable();
  });

  it('renders the account form and searchable institution field', () => {
    expect(component).toBeTruthy();
    expect(fixture.nativeElement.querySelector('#bankSearch')).toBeTruthy();
    expect(fixture.nativeElement.querySelector('#bankName')).toBeTruthy();
  });

  it('searches the catalog by name or banking code', () => {
    component.onBankQueryChange({ target: { value: '260' } } as unknown as Event);

    expect(bankCatalog.find).toHaveBeenCalledWith('260');
    expect(component.matchingBanks).toEqual([bank]);
    expect(component.selectedBankIspb).toBeNull();
  });

  it('creates an account using the selected ISPB and official CDN logo', async () => {
    accountsService.create.and.returnValue(of({
      id: 'account-1', accountName: 'Minha conta', balance: 125.50,
      bankIspb: bank.ispb, bankName: 'Nubank', iconUrl: logoUrl,
    }));
    component.accountForm.patchValue({ accountName: 'Minha conta', balance: '125.50' });
    component.selectBank(bank);

    await component.onActionModal();

    expect(accountsService.create).toHaveBeenCalledWith(jasmine.objectContaining({
      accountName: 'Minha conta',
      bankIspb: '18236120',
      bankName: 'Nubank',
      balance: 125.5,
      iconUrl: logoUrl,
    }));
    expect(notifications.showSuccess).toHaveBeenCalled();
  });

  it('can create a manual institution without a logo or ISPB', async () => {
    accountsService.create.and.returnValue(of({
      id: 'account-1', accountName: 'Dinheiro', bankName: 'Carteira',
      balance: 20, iconUrl: 'bank-default',
    }));
    component.accountForm.patchValue({
      accountName: 'Dinheiro', bankName: 'Carteira', balance: 20,
    });

    await component.onActionModal();

    expect(accountsService.create).toHaveBeenCalledWith(jasmine.objectContaining({
      bankName: 'Carteira', bankIspb: null, iconUrl: 'bank-default',
    }));
  });

  it('edits a bank account preserving the selected ISPB', async () => {
    const account: Account = {
      id: 'account-1', accountName: 'Principal', bankName: 'Nubank',
      bankIspb: bank.ispb, balance: 50, hideBalance: true, iconUrl: logoUrl,
    };
    accountsService.update.and.returnValue(of({ ...account, accountName: 'Diária' }));
    component.onEdit(account);
    component.accountForm.get('accountName')?.setValue('Diária');

    await component.onActionModal();

    expect(accountsService.update).toHaveBeenCalledWith(jasmine.objectContaining({
      id: 'account-1',
      bankIspb: '18236120',
      accountName: 'Diária',
      hideBalance: true,
    }));
    expect(accountsService.create).not.toHaveBeenCalled();
  });

  it('does not load deleted legacy static logo URLs', () => {
    const legacy: Account = {
      id: 'legacy-1', accountName: 'Antiga', bankName: 'Banco antigo',
      balance: 55, iconUrl: 'img/logo-142.svg',
    };
    expect(component.accountLogoUrl(legacy)).toBeNull();
    component.onEdit(legacy);
    expect(component.accountForm.get('bankName')?.value).toBe('Banco antigo');
  });

  it('rejects invalid form data and a negative balance', async () => {
    await component.onActionModal();
    expect(accountsService.create).not.toHaveBeenCalled();

    component.accountForm.patchValue({
      accountName: 'Principal', bankName: 'Banco manual', balance: -1,
    });
    await component.onActionModal();

    expect(component.accountForm.get('balance')?.hasError('min')).toBeTrue();
    expect(accountsService.create).not.toHaveBeenCalled();
    expect(notifications.showError).toHaveBeenCalled();
  });

  it('masks balances for hidden accounts', () => {
    component.accounts = [
      { id: 'hidden', accountName: 'Oculta', balance: 125.5, hideBalance: true, iconUrl: 'bank-default' },
      { id: 'visible', accountName: 'Normal', balance: 1234.5, hideBalance: false, iconUrl: 'bank-default' },
    ];
    fixture.detectChanges();
    const text = fixture.nativeElement.textContent;

    expect(text).toContain('••••••');
    expect(text).toContain(component.formatBalance(1234.5));
    expect(text).not.toContain(component.formatBalance(125.5));
  });

  it('resets search and form for a new account', () => {
    component.selectBank(bank);
    component.accountForm.patchValue({ accountName: 'Anterior', balance: 42 });
    component.onNewAccount();

    expect(component.selectedBankIspb).toBeNull();
    expect(component.bankSearch).toBe('');
    expect(component.accountForm.get('bankName')?.value).toBe('');
    expect(component.accountForm.get('balance')?.value).toBe(0);
  });

  it('does not delete an account when the user cancels', async () => {
    spyOn(window, 'confirm').and.returnValue(false);
    await component.onRemove('account-1');
    expect(accountsService.delete).not.toHaveBeenCalled();
  });

  it('deletes and refreshes the account list', async () => {
    spyOn(window, 'confirm').and.returnValue(true);
    accountsService.delete.and.returnValue(of(null as unknown as boolean));
    await component.onRemove('account-1');
    expect(accountsService.delete).toHaveBeenCalledWith('account-1');
    expect(accountsService.getAll).toHaveBeenCalledTimes(2);
    expect(notifications.showSuccess).toHaveBeenCalled();
  });
});
