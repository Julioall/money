import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.scss'
})
export class ModalComponent {
  @Input() idModal?: string;
  @Input() textButton: string = "Save";
  @Input() isCancelEnabled: boolean = false;
  @Output() action = new EventEmitter<void>();
  @Output() close = new EventEmitter<void>();

  onActionClick() {
    this.action.emit();
  }

  onClosedClick(){
    this.close.emit();
  }
}
