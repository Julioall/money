import { firstValueFrom } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
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
import { ColorPickerComponent } from '../../shared/color-picker/color-picker.component';
import { AccountsService } from '../../core/services/accounts/accounts.service';

const STRING_VAZIA = '';

@Component({
  selector: 'app-accounts',
  standalone: true,
  imports: [
    CommonModule,
    IconPickerComponent,
    ColorPickerComponent,
    ModalComponent,
    ReactiveFormsModule,
    TranslateModule,
  ],
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.scss'],
})
export class AccountsComponent implements OnInit {
  @ViewChild(ModalComponent) modal!: ModalComponent;

  colors: string[] = [
    '#ED1B25',
    '#A41A44',
    '#7A2990',
    '#392B8A',
    '#2F5AA8',
    '#018CCD',
    '#60AF46',
    '#CDDB26',
    '#F6ED24',
    '#F7BA12',
    '#F9971D',
    '#F14B21',
    '#F26A6A',
    '#C94B67',
    '#A158B5',
    '#5D4FB1',
    '#4B7EC2',
    '#42B8E1',
    '#88C876',
    '#E1ED6C',
    '#F8F17D',
    '#F8CD5C',
  ];
  icons: string[] = this._getLogos();
  selectedColor: string = '';
  selectedIcon: string = '';

  accounts: Account[] = [];

  accountForm!: FormGroup;
  idAccountModal: string = 'newAccountModal';
  textButtonActionAccountModal?: string;
  isSubmitted: boolean = false;

  constructor(
    private _fb: FormBuilder,
    private _notificationService: NotificationService,
    private translate: TranslateService,
    private _accountsService: AccountsService
  ) {
    this.accountForm = this._fb.group({
      accountName: [
        STRING_VAZIA,
        [Validators.required, Validators.minLength(3)],
      ],
      balance: [STRING_VAZIA, [Validators.min(0)]],
      iconUrl: [STRING_VAZIA, Validators.required],
    });

    this.textButtonActionAccountModal = this.translate.instant(
      'components.accounts.textActionButtonModal'
    );
  }

  ngOnInit(): void {
    this._getAllAccounts();
  }

  onResetSelections(): void {
    this.accountForm.reset({
      accountName: STRING_VAZIA,
      balance: STRING_VAZIA,
      hideBalance: false,
      iconUrl: STRING_VAZIA,
    });
    this.isSubmitted = false;
  }

  async onActionModal(): Promise<void> {
    try {
      this.isSubmitted = true;
      if (this.accountForm.valid) {
        const newAccount: Account = this.accountForm.value;
        if (!newAccount.balance) {
          newAccount.balance = 0;
        }
        await firstValueFrom(this._accountsService.create(newAccount));
        await this._getAllAccounts();
        this.onResetSelections();
        this._closeModal();
        this._notificationService.showSuccess(
          this.translate.instant('components.accounts.accountSaved')
        );
      } else {
        this._notificationService.showError(
          this.translate.instant('components.accounts.errorFormInvalid')
        );
      }
    } catch (error: any) {
      this._notificationService.showError(
        error,
        this.translate.instant('components.accounts.errorCreateAccount')
      );
    }
  }

  onSelectColor(color: string) {
    this.selectedColor = color;
  }

  onSelectIcon(icon: string) {
    this.accountForm.get('iconUrl')?.setValue(icon);
  }

  onRemove(arg0: string) {
    throw new Error('Method not implemented.');
  }

  onEdit(_t12: Account) {
    throw new Error('Method not implemented.');
  }

  private _closeModal() {
    const modal = document.getElementById(this.idAccountModal);

    if (modal != null) {
      const closeButton = modal.querySelector('.btn-close');

      if (closeButton != null) {
        (closeButton as HTMLElement).click();
      }
    }
  }

  private async _getAllAccounts() {
    this.accounts = await firstValueFrom(this._accountsService.getAll());
  }

  private _getLogos(): string[] {
    const logos: string[] = [];

    for (let i = 1; i <= 276; i++) {
      logos.push(`img/logo-${i}.svg`);
    }

    return logos;
  }
}
