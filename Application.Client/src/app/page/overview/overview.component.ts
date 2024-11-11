import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth/auth.service';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-overview',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './overview.component.html',
  styleUrl: './overview.component.scss'
})
export class OverviewComponent implements OnInit {
  token: string | null | undefined;
  isLoggedIn = false;

  constructor(
    private authService: AuthService,
    private router: Router

  ) {}

  ngOnInit() {
    this.isLoggedIn = this.authService.isLoggedIn();
    this.token = this.authService.getToken();
    if (!this.isLoggedIn) {
      this.router.navigate(['/auth']);
    }
  }
}
