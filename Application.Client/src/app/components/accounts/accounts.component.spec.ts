import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { Account } from '../../core/models/account';
import { AccountsService } from '../../core/services/accounts/accounts.service';
import { NotificationService } from '../../core/services/notification/notification.service';
import { configureCommonTestBed } from '../../testing/test-bed';
import { AccountsComponent } from './accounts.component';

describe('AccountsComponent', () => {
  let component: AccountsComponent;
  let fixture: ComponentFixture<AccountsComponent>;
  let accountsService: jasmine.SpyObj<AccountsService>;
  let notificationService: jasmine.SpyObj<NotificationService>;

  beforeEach(async () => {
    accountsService = jasmine.createSpyObj<AccountsService>(
      'AccountsService',
      ['getAll', 'create', 'update', 'delete']
    );
    notificationService = jasmine.createSpyObj<NotificationService>(
      'NotificationService',
      ['showSuccess', 'showError']
    );
    accountsService.getAll.and.returnValue(of([]));

    await configureCommonTestBed({
      imports: [AccountsComponent],
      providers: [
        { provide: AccountsService, useValue: accountsService },
        { provide: NotificationService, useValue: notificationService },
      ],
    });

    fixture = TestBed.createComponent(AccountsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('creates an account with a numeric balance and selected icon', async () => {
    const savedAccount: Account = {
      id: 'account-1',
      accountName: 'Checking',
      balance: 125.5,
      hideBalance: false,
      iconUrl: 'img/logo-1.svg',
    };
    accountsService.create.and.returnValue(of(savedAccount));
    component.accountForm.patchValue({ accountName: 'Checking', balance: '125.5' });
    component.onSelectIcon('img/logo-1.svg');

    await component.onActionModal();

    expect(accountsService.create).toHaveBeenCalledWith(jasmine.objectContaining({
      accountName: 'Checking',
      balance: 125.5,
      hideBalance: false,
      iconUrl: 'img/logo-1.svg',
    }));
    expect(accountsService.update).not.toHaveBeenCalled();
    expect(notificationService.showSuccess).toHaveBeenCalled();
  });

  it('updates the selected account when editing', async () => {
    const account: Account = {
      id: 'account-1',
      accountName: 'Checking',
      balance: 50,
      hideBalance: true,
      iconUrl: 'img/logo-2.svg',
    };
    accountsService.update.and.returnValue(of({ ...account, accountName: 'Daily account' }));

    component.onEdit(account);
    component.accountForm.get('accountName')?.setValue('Daily account');
    await component.onActionModal();

    expect(accountsService.update).toHaveBeenCalledWith(jasmine.objectContaining({
      id: 'account-1',
      accountName: 'Daily account',
      hideBalance: true,
      iconUrl: 'img/logo-2.svg',
    }));
    expect(accountsService.create).not.toHaveBeenCalled();
  });

  it('does not save an invalid account form', async () => {
    await component.onActionModal();

    expect(accountsService.create).not.toHaveBeenCalled();
    expect(accountsService.update).not.toHaveBeenCalled();
    expect(notificationService.showError).toHaveBeenCalled();
  });

  it('rejects a negative balance before sending the account', async () => {
    component.accountForm.patchValue({ accountName: 'Checking', balance: -1 });
    component.onSelectIcon('img/logo-1.svg');

    await component.onActionModal();

    expect(accountsService.create).not.toHaveBeenCalled();
    expect(component.accountForm.get('balance')?.hasError('min')).toBeTrue();
  });

  it('shows a masked balance only for accounts configured to hide it', () => {
    component.accounts = [
      {
        id: 'hidden',
        accountName: 'Savings',
        balance: 125.5,
        hideBalance: true,
        iconUrl: 'img/logo-1.svg',
      },
      {
        id: 'visible',
        accountName: 'Checking',
        balance: 1234.5,
        hideBalance: false,
        iconUrl: 'img/logo-2.svg',
      },
    ];
    fixture.detectChanges();
    const renderedText = fixture.nativeElement.textContent;

    expect(renderedText).toContain('••••••');
    expect(renderedText).toContain(component.formatBalance(1234.5));
    expect(renderedText).not.toContain(component.formatBalance(125.5));
  });

  it('keeps the balance input and hide-balance checkbox in their own layout styles', () => {
    const balanceInput: HTMLInputElement = fixture.nativeElement.querySelector('#balance');
    const checkbox: HTMLInputElement = fixture.nativeElement.querySelector('#hideBalance');
    const checkboxLabel = checkbox.closest('label');

    expect(balanceInput.classList).toContain('account-input');
    expect(balanceInput.parentElement?.classList).not.toContain('account-input');
    expect(checkbox.classList).toContain('account-hide-balance-checkbox');
    expect(checkboxLabel?.classList).toContain('account-hide-balance');
    expect(window.getComputedStyle(checkbox).width).toBe(
      window.getComputedStyle(document.documentElement).fontSize
    );
  });

  it('resets account fields and icon selection when starting a new account', () => {
    component.accountForm.patchValue({
      accountName: 'Old account',
      balance: 42,
      hideBalance: true,
    });
    component.onSelectIcon('img/logo-4.svg');

    component.onNewAccount();

    expect(component.editingAccountId).toBeUndefined();
    expect(component.accountForm.getRawValue()).toEqual({
      accountName: '',
      balance: 0,
      hideBalance: false,
      iconUrl: '',
    });
    expect(component.selectedIcon).toBe('');
    expect(component.iconPicker?.selectedIcon).toBe('');
  });

  it('removes an account and reloads the list', async () => {
    accountsService.delete.and.returnValue(of(null as unknown as boolean));

    await component.onRemove('account-1');

    expect(accountsService.delete).toHaveBeenCalledWith('account-1');
    expect(accountsService.getAll).toHaveBeenCalledTimes(2);
    expect(notificationService.showSuccess).toHaveBeenCalled();
  });

  it('does not reload or show success when deletion fails', async () => {
    accountsService.delete.and.returnValue(of(false));

    await component.onRemove('account-1');

    expect(accountsService.getAll).toHaveBeenCalledTimes(1);
    expect(notificationService.showSuccess).not.toHaveBeenCalled();
  });
});
