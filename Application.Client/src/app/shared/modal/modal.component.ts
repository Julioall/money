import { Component, Input, Output, EventEmitter, input } from '@angular/core';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent {
  @Input() title?: string;
  @Input() idModal?: string;
  @Input() textButton?: string = "Save";
  @Input() withCancel?: boolean = false;
  @Output() action = new EventEmitter<void>();

  onActionClick() {
    this.action.emit();
  }
}
