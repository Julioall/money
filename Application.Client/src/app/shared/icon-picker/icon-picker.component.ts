import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-icon-picker',
  templateUrl: './icon-picker.component.html',
  styleUrls: ['./icon-picker.component.scss']
})
export class IconPickerComponent {
  @Input() icons: string[] = [];
  @Output() iconSelected = new EventEmitter<string>();

  selectedIcon: string = '';

  selectIcon(icon: string): void {
    this.selectedIcon = icon;
    this.iconSelected.emit(icon);
  }

  isIconActive(icon: string): string {
    return this.selectedIcon === icon ? 'icon-active' : '';
  }

  resetSelection(): void {
    this.selectedIcon = '';
  }
}
