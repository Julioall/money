import { Routes } from '@angular/router';
import { AuthComponent } from './page/auth/auth.component';
import { OverviewComponent } from './page/overview/overview.component';
import { SettingsComponent } from './page/settings/settings.component';
import { CreditCardComponent } from './components/credit-card/credit-card.component';
import { CategoriesComponent } from './components/categories/categories.component';

export const routes: Routes = [
  { path: '', redirectTo: 'overview', pathMatch: 'full' },
  { path: 'overview', component: OverviewComponent },
  { path: 'auth', component: AuthComponent },
  { path: 'settings', component: SettingsComponent, children: [
    { path: 'categories', component: CategoriesComponent },
    { path: 'accounts', component: AuthComponent },
    { path: 'credit-cards', component: CreditCardComponent },
    { path: '', redirectTo: 'categories', pathMatch: 'full' },
  ] },
];
