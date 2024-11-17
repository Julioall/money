import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  constructor(private toast: ToastrService) {}

  showSuccess(message: string, title: string = 'Success') {
    this.toast.success(message, title, {
      timeOut: 3000,
      positionClass: 'toast-top-right',
    });
  }

  showError(message: string, title: string = 'Error') {
    this.toast.error(message, title, {
      timeOut: 3000,
      progressBar: true,
      positionClass: 'toast-top-right'
    });
  }

  showWarning(message: string, title: string = 'Warning') {
    this.toast.warning(message, title, {
      timeOut: 3000,
      progressBar: true,
      positionClass: 'toast-top-right'
    });
  }
}
