import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-icon-picker',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './icon-picker.component.html',
  styleUrl: './icon-picker.component.scss'
})
export class IconPickerComponent {
  @Input() iconsClass: string[] = [];
  @Input() iconsImages: string[] = [];
  @Output() iconSelected = new EventEmitter<string>();

  selectedIcon: string = '';

  selectIcon(icon: string) {
    this.selectedIcon = icon;
    this.iconSelected.emit(icon);
  }

  isIconSelected(icon: string): string {
    return this.selectedIcon === icon ? 'icon-active' : '';
  }

  isLogoSelected(icon: string): string {
    return this.selectedIcon === icon ? 'logo-active' : '';
  }

  resetSelection() {
    this.selectedIcon = '';
  }
}
