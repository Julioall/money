import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SettingsComponent } from './settings.component';
import { RouterModule } from '@angular/router';
import { CategoriesComponent } from '../../components/categories/categories.component';
import { AccountsComponent } from '../../components/accounts/accounts.component';
import { CreditCardComponent } from '../../components/credit-card/credit-card.component';
import { ModalComponent } from '../../shared/modal/modal.component';

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
