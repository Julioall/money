import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Account } from '../../core/models/account';
import { ModalComponent } from '../../shared/modal/modal.component';
import { NotificationService } from '../../core/services/notification/notification.service';

@Component({
  selector: 'app-accounts',
  standalone: true,
  imports: [CommonModule, ModalComponent, ReactiveFormsModule, TranslateModule],
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.scss'],
})
export class AccountsComponent implements OnInit {
  accountForm!: FormGroup;
  accounts: Account[] = [];
  accountModal: string | undefined;
  textButtonActionAccountModal: string | undefined;

  constructor(
    private _fb: FormBuilder,
    private _notificationService: NotificationService,
    private translate: TranslateService
  ) {
    this.accountForm = this._fb.group({
      accountName: ['', [Validators.required, Validators.minLength(3)]],
      balance: [0, [Validators.required, Validators.min(0)]],
      hideBalance: [false, Validators.required],
      iconUrl: ['', Validators.required],
    });

    this.textButtonActionAccountModal = this.translate.instant('accounts.textActionButtonModal');
  }

  ngOnInit(): void {
    this.accounts = [
      {
        id: '1',
        accountName: 'Poupança',
        balance: 1000,
        hideBalance: false,
        iconUrl: 'https://assets.organizze.com.br/institutions/logos/bb.png',
      },
      {
        id: '2',
        accountName: 'Conta Corrente',
        balance: 2500,
        hideBalance: false,
        iconUrl: 'https://assets.organizze.com.br/institutions/logos/itau.png',
      },
      {
        id: '3',
        accountName: 'Fundo de Emergência',
        balance: 5000,
        hideBalance: true,
        iconUrl: 'https://assets.organizze.com.br/institutions/logos/bradesco.png',
      },
      {
        id: '4',
        accountName: 'Conta de Investimento',
        balance: 15000,
        hideBalance: false,
        iconUrl: 'https://assets.organizze.com.br/institutions/logos/santander.png',
      },
      {
        id: '5',
        accountName: 'Economias para Viagem',
        balance: 1200,
        hideBalance: false,
        iconUrl: 'https://assets.organizze.com.br/institutions/logos/nubank.png',
      },
      {
        id: '6',
        accountName: 'Fundo de Aposentadoria',
        balance: 25000,
        hideBalance: true,
        iconUrl: 'https://assets.organizze.com.br/institutions/logos/caixa.png',
      },
      {
        id: '7',
        accountName: 'Carteira Pessoal',
        balance: 200,
        hideBalance: false,
        iconUrl: 'https://assets.organizze.com.br/institutions/logos/picpay.png',
      },
    ];
  }

  onResetSelections(): void {
    this.accountForm.reset({
      accountName: '',
      balance: 0,
      hideBalance: false,
      iconUrl: '',
    });
  }

  onActionModal(): void {
    if (this.accountForm.valid) {
      const account: Account = this.accountForm.value;

      this._notificationService.showSuccess(
        this.translate.instant('accounts.accountSaved', { accountName: account.accountName })
      );

      this.onResetSelections();
    } else {
      this._notificationService.showError(this.translate.instant('accounts.errorFormInvalid'));
    }
  }
}
