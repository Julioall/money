import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/service/auth/auth.service';
import { NotificationService } from '../../core/service/notification/notification.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {
  isLoggedIn: boolean = false;
  constructor(
    private authService: AuthService,
    private router: Router,
    private notificationService: NotificationService,
  ) { }

  ngOnInit() {
    this.authService.loggedIn$.subscribe(loggedIn => {
      this.isLoggedIn = loggedIn;
    });
  }

  onClickLogout() {
    this.notificationService.showSuccess('Logged out successfully');
    this._logOut()

  }
  private _logOut() {
    this.authService.removeToken()
    this.isLoggedIn = false;
    this.router.navigate(['/auth']);
  }
}
