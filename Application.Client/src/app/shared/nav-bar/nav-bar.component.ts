import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth/auth.service';
import { NotificationService } from '../../core/services/notification/notification.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.scss'
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
