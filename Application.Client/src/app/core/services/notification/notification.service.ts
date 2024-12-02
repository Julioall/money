import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  constructor(
    private toast: ToastrService,
    private translate: TranslateService
  ) {
  }

  showSuccess(message: string, title?: string) {
    if (!title) {
      title = this.translate.instant('services.notifications.success');
    }
    this.toast.success(message, title);
  }

  showError(message: string, title?: string) {
    if (!title) {
      title = this.translate.instant('services.notifications.error');
    }
    this.toast.error(message, title);
  }

  showWarning(message: string, title?: string) {
    if (!title) {
      title = this.translate.instant('services.notifications.warning');
    }
    this.toast.warning(message, title);
  }
}
