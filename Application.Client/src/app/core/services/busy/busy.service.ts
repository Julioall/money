import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root',
})
export class BusyService {
  busyRequestCount = 0;

  constructor(private spinnerService: NgxSpinnerService) {}

  show() {
    this.busyRequestCount ++;
    this.spinnerService;
    this.spinnerService.show(undefined,
      {
        type: "line-spin-fade",
        bdColor: "rgba(251, 247, 234, 0.25)",
        size: "small",
        color: "#008440",
        fullScreen: true
      });
  }

  hide() {
    this.busyRequestCount--;
    if(this.busyRequestCount<=0){
      this.busyRequestCount=0;
      this.spinnerService.hide();
    }
  }
}
