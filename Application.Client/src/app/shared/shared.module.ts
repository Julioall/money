import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreditCardComponent } from '../components/credit-card/credit-card.component';
import { ColorPickerComponent } from './color-picker/color-picker.component';
import { IconPickerComponent } from './icon-picker/icon-picker.component';
import { ModalComponent } from './modal/modal.component';

@NgModule({
  declarations: [
    ModalComponent,
    ColorPickerComponent,
    IconPickerComponent,
    CreditCardComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    ModalComponent,
    ColorPickerComponent,
    IconPickerComponent,
    CreditCardComponent
  ]
})
export class SharedModule { }
