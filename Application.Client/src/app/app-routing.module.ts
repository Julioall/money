import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OverviewComponent } from './page/overview/overview.component';
import { AuthComponent } from './page/auth/auth.component';
import { SettingsComponent } from './page/settings/settings.component';
import { CategoriesComponent } from './shared/components/categories/categories.component';
import { AccountsComponent } from './shared/components/accounts/accounts.component';
import { CreditCardComponent } from './shared/components/credit-card/credit-card.component';

const routes: Routes = [
  { path: '', redirectTo: 'overview', pathMatch: 'full' },
  { path: 'overview', component: OverviewComponent },
  { path: 'auth', component: AuthComponent },
  { path: 'settings', component: SettingsComponent, children: [
    { path: 'categories', component: CategoriesComponent },
    { path: 'accounts', component: AccountsComponent },
    { path: 'credit-cards', component: CreditCardComponent },
    { path: '', redirectTo: 'categories', pathMatch: 'full' },
  ] },
];


@NgModule({
  imports: [
    RouterModule.forRoot(routes),
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
