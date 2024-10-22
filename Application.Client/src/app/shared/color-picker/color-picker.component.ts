import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-color-picker',
  templateUrl: './color-picker.component.html',
  styleUrls: ['./color-picker.component.scss']
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
