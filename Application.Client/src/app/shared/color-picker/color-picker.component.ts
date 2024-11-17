import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-color-picker',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './color-picker.component.html',
  styleUrl: './color-picker.component.scss'
})
export class ColorPickerComponent {
  @Input() colors: string[] = [];
  @Output() colorSelected = new EventEmitter<string>();

  selectedColor: string = '';

  selectColor(color: string) {
    this.selectedColor = color;
    this.colorSelected.emit(color);
  }

  isColorActive(color: string): string {
    return this.selectedColor === color ? 'color-active' : '';
  }

  getColorStyle(color: string): { [key: string]: string } {
    return { 'background-color': color };
  }

  resetSelection() {
    this.selectedColor = '';
  }
}
