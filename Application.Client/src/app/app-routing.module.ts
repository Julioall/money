import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OverviewComponent } from './page/overview/overview.component';
import { AuthComponent } from './page/auth/auth.component';
import { SettingsComponent } from './page/settings/settings.component';
import { CategoriesComponent } from './shared/components/categories/categories.component';
import { AccountsComponent } from './shared/components/accounts/accounts.component';
import { CardsComponent } from './shared/components/cards/cards.component';

const routes: Routes = [
  { path: '', redirectTo: 'overview', pathMatch: 'full' },
  { path: 'overview', component: OverviewComponent },
  { path: 'auth', component: AuthComponent },
  { path: 'settings', component: SettingsComponent, children: [
    { path: 'categories', component: CategoriesComponent },
    { path: 'accounts', component: AccountsComponent },
    { path: 'cards', component: CardsComponent },
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
