import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../core/service/auth/auth.service';
import { Router } from '@angular/router';
import { NotificationService } from '../../../core/service/notification/notification.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private router: Router,
    private notificationService: NotificationService,
  ) { }

  ngOnInit() {
  }

  onClickLogout() {
    this.notificationService.showSuccess('Logged out successfully');
    this._logOut()

  }
  private _logOut() {
    this.authService.removeToken()
    this.router.navigate(['/auth']);
  }
}
