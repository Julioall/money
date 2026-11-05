import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { firstValueFrom } from 'rxjs';
import { Account } from '../../core/models/account';
import { AccountsService } from '../../core/services/accounts/accounts.service';
import {
  BankCatalogService,
  BankInstitution,
  institutionName,
} from '../../core/services/banks/bank-catalog.service';
import { NotificationService } from '../../core/services/notification/notification.service';
import { ModalComponent } from '../../shared/modal/modal.component';

const EMPTY_STRING = '';
const FALLBACK_ICON = 'bank-default';
const CATALOG_LOGO_URL = /^https:\/\/cdn\.jsdelivr\.net\/npm\/logos-bancos-br@[\d.]+\/logos\/(?:png|svg)\/\d{8}\.(?:png|svg)$/;

@Component({
  selector: 'app-accounts',
  standalone: true,
  imports: [CommonModule, ModalComponent, ReactiveFormsModule, TranslateModule],
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.scss'],
})
export class AccountsComponent implements OnInit {
  accounts: Account[] = [];
  accountForm: FormGroup;
  idAccountModal = 'newAccountModal';
  textButtonActionAccountModal: string;
  isSubmitted = false;
  isLoadingAccounts = false;
  isSaving = false;
  isDeletingId?: string;
  editingAccountId?: string;

  bankSearch = EMPTY_STRING;
  matchingBanks: BankInstitution[] = [];
  selectedBankIspb: string | null = null;
  isLoadingCatalog = false;
  catalogError = false;

  constructor(
    private fb: FormBuilder,
    private notification: NotificationService,
    private translate: TranslateService,
    private accountsService: AccountsService,
    private banks: BankCatalogService,
    @Inject(PLATFORM_ID) private platformId: object
  ) {
    this.accountForm = this.fb.group({
      accountName: [
        EMPTY_STRING,
        [Validators.required, Validators.minLength(3), Validators.maxLength(100), Validators.pattern(/\S/)],
      ],
      bankName: [
        EMPTY_STRING,
        [Validators.required, Validators.maxLength(100), Validators.pattern(/\S/)],
      ],
      accountType: ['checking', Validators.required],
      balance: [0, [Validators.required, Validators.min(0)]],
      hideBalance: [false],
    });
    this.textButtonActionAccountModal = this.translate.instant('components.accounts.textActionButtonModal');
  }

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      void this.loadCatalog();
      void this.loadAccounts();
    }
  }

  async loadCatalog(): Promise<void> {
    this.isLoadingCatalog = true;
    this.catalogError = false;
    try {
      await this.banks.load();
      this.refreshSearch();
    } catch {
      this.catalogError = true;
      // A manual institution remains available when the external CDN is unreachable.
    } finally {
      this.isLoadingCatalog = false;
    }
  }

  onBankQueryChange(event: Event): void {
    this.bankSearch = (event.target as HTMLInputElement).value;
    // Changing the search does not silently change the selected institution.
    this.selectedBankIspb = null;
    this.accountForm.get('bankName')?.setValue(EMPTY_STRING);
    this.refreshSearch();
  }

  selectBank(bank: BankInstitution): void {
    this.selectedBankIspb = bank.ispb;
    this.bankSearch = institutionName(bank);
    this.accountForm.get('bankName')?.setValue(institutionName(bank));
    this.matchingBanks = [];
  }

  clearBankSelection(): void {
    this.selectedBankIspb = null;
    this.bankSearch = EMPTY_STRING;
    this.accountForm.get('bankName')?.setValue(EMPTY_STRING);
    this.refreshSearch();
  }

  bankLogoUrl(bank: BankInstitution): string | null {
    return this.banks.logoUrl(bank);
  }

  accountLogoUrl(account: Account): string | null {
    const fromCatalog = this.banks.logoUrl(this.banks.get(account.bankIspb));
    if (fromCatalog) return fromCatalog;
    // Legacy accounts may contain img/logo-123.svg. Never request deleted files;
    // show a neutral bank icon until the user chooses the correct institution.
    return account.iconUrl && CATALOG_LOGO_URL.test(account.iconUrl) ? account.iconUrl : null;
  }

  hideBrokenLogo(event: Event): void {
    const image = event.target as HTMLImageElement;
    image.style.display = 'none';
  }

  onNewAccount(): void {
    this.onResetSelections();
  }

  onResetSelections(): void {
    this.editingAccountId = undefined;
    this.selectedBankIspb = null;
    this.bankSearch = EMPTY_STRING;
    this.accountForm.reset({
      accountName: EMPTY_STRING,
      bankName: EMPTY_STRING,
      accountType: 'checking',
      balance: 0,
      hideBalance: false,
    });
    this.isSubmitted = false;
    this.textButtonActionAccountModal = this.translate.instant('components.accounts.textActionButtonModal');
    this.refreshSearch();
  }

  async onActionModal(): Promise<void> {
    this.isSubmitted = true;
    if (this.accountForm.invalid || this.isSaving) {
      if (this.accountForm.invalid) {
        this.notification.showError(this.translate.instant('components.accounts.errorFormInvalid'));
      }
      return;
    }

    const isEditing = !!this.editingAccountId;
    const form = this.accountForm.getRawValue();
    const institution = this.banks.get(this.selectedBankIspb);
    const name = String(form.accountName ?? '').trim();
    const bankName = institution ? institutionName(institution) : String(form.bankName ?? '').trim();
    const balance = Number(form.balance);

    if (name.length < 3 || name.length > 100 || !bankName || !Number.isFinite(balance) || balance < 0) {
      this.notification.showError(this.translate.instant('components.accounts.errorFormInvalid'));
      return;
    }

    this.isSaving = true;
    const account: Account = {
      id: this.editingAccountId,
      accountName: name,
      bankName,
      bankIspb: this.selectedBankIspb,
      accountType: form.accountType,
      balance,
      hideBalance: Boolean(form.hideBalance),
      iconUrl: this.banks.logoUrl(institution) || FALLBACK_ICON,
    };

    try {
      const saved = await firstValueFrom(isEditing
        ? this.accountsService.update(account)
        : this.accountsService.create(account));
      if (!saved) return;

      await this.loadAccounts();
      this.closeModal();
      this.onResetSelections();
      this.notification.showSuccess(this.translate.instant('components.accounts.accountSaved', {
        accountName: saved.accountName ?? name,
      }));
    } catch {
      this.notification.showError(this.translate.instant(isEditing
        ? 'components.accounts.errorUpdateAccount'
        : 'components.accounts.errorCreateAccount'));
    } finally {
      this.isSaving = false;
    }
  }

  async onRemove(id: string | undefined): Promise<void> {
    if (!id || this.isDeletingId) return;
    if (!window.confirm(this.translate.instant('components.accounts.confirmRemoveAccount'))) return;

    this.isDeletingId = id;
    try {
      const removed = await firstValueFrom(this.accountsService.delete(id));
      if (removed === false) return;
      await this.loadAccounts();
      this.notification.showSuccess(this.translate.instant('components.accounts.accountRemoved'));
    } catch {
      this.notification.showError(this.translate.instant('components.accounts.errorRemoveAccount'));
    } finally {
      this.isDeletingId = undefined;
    }
  }

  onEdit(account: Account): void {
    if (!account.id || !account.accountName) {
      this.notification.showError(this.translate.instant('components.accounts.errorLoadAccount'));
      return;
    }
    this.editingAccountId = account.id;
    this.selectedBankIspb = account.bankIspb ?? null;
    this.bankSearch = account.bankIspb
      ? institutionName(this.banks.get(account.bankIspb) || {
          ispb: account.bankIspb,
          compe: null,
          compe4: null,
          name: account.bankName || '',
          shortName: account.bankName || '',
          displayName: null,
          logo: null,
        })
      : EMPTY_STRING;

    this.accountForm.reset({
      accountName: account.accountName,
      bankName: account.bankName || this.bankSearch,
      accountType: account.accountType || 'checking',
      balance: account.balance ?? 0,
      hideBalance: account.hideBalance ?? false,
    });
    this.isSubmitted = false;
    this.textButtonActionAccountModal = this.translate.instant('components.accounts.textEditActionButtonModal');
    this.refreshSearch();
  }

  formatBalance(balance: number | null | undefined): string {
    const isPortuguese = !this.translate.currentLang?.startsWith('en');
    return new Intl.NumberFormat(isPortuguese ? 'pt-BR' : 'en-US', {
      style: 'currency',
      currency: isPortuguese ? 'BRL' : 'USD',
    }).format(balance ?? 0);
  }

  private refreshSearch(): void {
    this.matchingBanks = this.selectedBankIspb ? [] : this.banks.find(this.bankSearch);
  }

  private closeModal(): void {
    const modal = document.getElementById(this.idAccountModal);
    (modal?.querySelector('.btn-close') as HTMLElement | null)?.click();
  }

  private async loadAccounts(): Promise<void> {
    this.isLoadingAccounts = true;
    try {
      this.accounts = (await firstValueFrom(this.accountsService.getAll())) ?? [];
    } finally {
      this.isLoadingAccounts = false;
    }
  }
}
