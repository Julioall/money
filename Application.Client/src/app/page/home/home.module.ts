import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { NavBarComponent } from '../../shared/components/nav-bar/nav-bar.component';

@NgModule({
  declarations: [
    HomeComponent,
    NavBarComponent
  ],
  imports: [
    CommonModule,
  ],
  exports: [
    HomeComponent
  ]
})
export class HomeModule { }
