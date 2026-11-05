import { firstValueFrom } from 'rxjs';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Component, Inject, OnInit, PLATFORM_ID, ViewChild } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Account } from '../../core/models/account';
import { ModalComponent } from '../../shared/modal/modal.component';
import { NotificationService } from '../../core/services/notification/notification.service';
import { IconPickerComponent } from '../../shared/icon-picker/icon-picker.component';
import { AccountsService } from '../../core/services/accounts/accounts.service';

const EMPTY_STRING = '';

@Component({
  selector: 'app-accounts',
  standalone: true,
  imports: [
    CommonModule,
    IconPickerComponent,
    ModalComponent,
    ReactiveFormsModule,
    TranslateModule,
  ],
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.scss'],
})
export class AccountsComponent implements OnInit {
  @ViewChild(IconPickerComponent) iconPicker?: IconPickerComponent;

  icons: string[] = this._getLogos();
  selectedIcon = EMPTY_STRING;
  accounts: Account[] = [];
  accountForm: FormGroup;
  idAccountModal = 'newAccountModal';
  textButtonActionAccountModal: string;
  isSubmitted = false;
  isLoadingAccounts = false;
  isSaving = false;
  isDeletingId?: string;
  editingAccountId?: string;

  constructor(
    private _fb: FormBuilder,
    private _notificationService: NotificationService,
    private translate: TranslateService,
    private _accountsService: AccountsService,
    @Inject(PLATFORM_ID) private platformId: object
  ) {
    this.accountForm = this._fb.group({
      bankName: [EMPTY_STRING, [Validators.maxLength(100)]],
      accountType: ['checking', Validators.required],
      accountName: [
        EMPTY_STRING,
        [Validators.required, Validators.minLength(3), Validators.pattern(/\S/)],
      ],
      balance: [0, [Validators.min(0)]],
      hideBalance: [false],
      iconUrl: [EMPTY_STRING, Validators.required],
    });

    this.textButtonActionAccountModal = this.translate.instant(
      'components.accounts.textActionButtonModal'
    );
  }

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      void this._getAllAccounts();
    }
  }

  onNewAccount(): void {
    this.onResetSelections();
  }

  onResetSelections(): void {
    this.editingAccountId = undefined;
    this.selectedIcon = EMPTY_STRING;
    this.accountForm.reset({
      accountName: EMPTY_STRING,
      bankName: EMPTY_STRING,
      accountType: 'checking',
      balance: 0,
      hideBalance: false,
      iconUrl: EMPTY_STRING,
    });
    this.iconPicker?.resetSelection();
    this.isSubmitted = false;
    this.textButtonActionAccountModal = this.translate.instant(
      'components.accounts.textActionButtonModal'
    );
  }

  async onActionModal(): Promise<void> {
    this.isSubmitted = true;

    if (this.accountForm.invalid) {
      this._notificationService.showError(
        this.translate.instant('components.accounts.errorFormInvalid')
      );
      return;
    }

    if (this.isSaving) return;
    const isEditing = !!this.editingAccountId;
    this.isSaving = true;

    try {
      const formValue = this.accountForm.getRawValue();
      const account: Account = {
        id: this.editingAccountId,
        accountName: formValue.accountName.trim(),
        balance: Number(formValue.balance ?? 0),
        hideBalance: Boolean(formValue.hideBalance),
        iconUrl: this.selectedIcon,
        bankName: String(formValue.bankName ?? '').trim(),
        accountType: formValue.accountType,
      };
      if (!Number.isFinite(account.balance) || account.balance < 0) {
        this._notificationService.showError(this.translate.instant('components.accounts.errorFormInvalid'));
        return;
      }
      const savedAccount = await firstValueFrom(
        isEditing
          ? this._accountsService.update(account)
          : this._accountsService.create(account)
      );

      if (!savedAccount) {
        // The base service already reports HTTP errors and returns an empty value.
        return;
      }

      await this._getAllAccounts();
      this._closeModal();
      this.onResetSelections();
      this._notificationService.showSuccess(
        this.translate.instant('components.accounts.accountSaved', {
          accountName: savedAccount.accountName ?? account.accountName,
        })
      );
    } catch {
      this._notificationService.showError(
        this.translate.instant(
          isEditing
            ? 'components.accounts.errorUpdateAccount'
            : 'components.accounts.errorCreateAccount'
        )
      );
    } finally {
      this.isSaving = false;
    }
  }

  onSelectIcon(icon: string): void {
    this.selectedIcon = icon;
    this.accountForm.get('iconUrl')?.setValue(icon);
  }

  async onRemove(id: string | undefined): Promise<void> {
    if (!id) {
      this._notificationService.showError(
        this.translate.instant('components.accounts.errorRemoveAccount')
      );
      return;
    }

    if (!window.confirm(this.translate.instant('components.accounts.confirmRemoveAccount'))) {
      return;
    }

    this.isDeletingId = id;
    try {
      const removed = await firstValueFrom(this._accountsService.delete(id));
      if (removed === false) {
        // The base service already reports HTTP errors and returns false.
        return;
      }

      await this._getAllAccounts();
      this._notificationService.showSuccess(
        this.translate.instant('components.accounts.accountRemoved')
      );
    } catch {
      this._notificationService.showError(
        this.translate.instant('components.accounts.errorRemoveAccount')
      );
    } finally {
      this.isDeletingId = undefined;
    }
  }

  onEdit(account: Account): void {
    if (!account.id || !account.accountName || !account.iconUrl) {
      this._notificationService.showError(
        this.translate.instant('components.accounts.errorLoadAccount')
      );
      return;
    }

    this.editingAccountId = account.id;
    this.selectedIcon = account.iconUrl;
    this.accountForm.reset({
      accountName: account.accountName,
      bankName: account.bankName ?? EMPTY_STRING,
      accountType: account.accountType ?? 'checking',
      balance: account.balance ?? 0,
      hideBalance: account.hideBalance ?? false,
      iconUrl: account.iconUrl,
    });
    if (this.iconPicker) {
      this.iconPicker.selectedIcon = account.iconUrl;
    }
    this.isSubmitted = false;
    this.textButtonActionAccountModal = this.translate.instant(
      'components.accounts.textEditActionButtonModal'
    );
  }

  formatBalance(balance: number | null | undefined): string {
    const isPortuguese = !this.translate.currentLang?.startsWith('en');
    return new Intl.NumberFormat(isPortuguese ? 'pt-BR' : 'en-US', {
      style: 'currency',
      currency: isPortuguese ? 'BRL' : 'USD',
    }).format(balance ?? 0);
  }

  private _closeModal(): void {
    const modal = document.getElementById(this.idAccountModal);
    const closeButton = modal?.querySelector('.btn-close');
    (closeButton as HTMLElement | null)?.click();
  }

  private async _getAllAccounts(): Promise<void> {
    this.isLoadingAccounts = true;
    try {
      this.accounts = (await firstValueFrom(this._accountsService.getAll())) ?? [];
    } finally {
      this.isLoadingAccounts = false;
    }
  }

  private _getLogos(): string[] {
    return Array.from({ length: 276 }, (_, index) => `img/logo-${index + 1}.svg`);
  }
}
