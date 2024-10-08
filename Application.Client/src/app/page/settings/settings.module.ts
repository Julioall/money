import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SettingsComponent } from './settings.component';
import { CategoriesComponent } from '../../shared/components/categories/categories.component';
import { AccountsComponent } from '../../shared/components/accounts/accounts.component';
import { CreditCardComponent } from '../../shared/components/credit-card/credit-card.component';
import { RouterModule } from '@angular/router';
import { ModalComponent } from '../../shared/components/modal/modal.component';

@NgModule({
  declarations: [
    SettingsComponent,
    CategoriesComponent,
    AccountsComponent,
    CreditCardComponent,
    ModalComponent
  ],
  imports: [
    CommonModule,
    RouterModule
  ]
})
export class SettingsModule { }
