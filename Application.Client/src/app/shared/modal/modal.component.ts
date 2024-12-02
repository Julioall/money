import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [CommonModule,TranslateModule],
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.scss'
})
export class ModalComponent {
  @Input() idModal?: string;
  @Input() textButton?: string;
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
