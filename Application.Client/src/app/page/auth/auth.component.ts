import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth/auth.service';
import { Router, RouterModule } from '@angular/router';
import { NotificationService } from '../../core/services/notification/notification.service';
import { firstValueFrom } from 'rxjs';
import { User } from '../../core/models/user';
import { UserFinance } from '../../core/models/userFinance';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {TranslateModule} from "@ngx-translate/core";
import { TranslateService } from '@ngx-translate/core';


@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, TranslateModule],
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss'],
})
export class AuthComponent implements OnInit {
  loginForm!: FormGroup;
  loginSubmitted: boolean = false;
  signupForm!: FormGroup;
  signupSubmitted: boolean = false;
  rightPanelActive: string = '';
  loginFormVisible: boolean = true;
  signupFormVisible: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private notificationService: NotificationService,
    private translate: TranslateService
  ) {}

  ngOnInit() {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });

    this.signupForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  showSignUp() {
    this.rightPanelActive = 'right-panel-active';
    this.signupFormVisible = true;
    this.loginFormVisible = false;
    this.loginSubmitted = false;
    this.loginForm.reset();
  }

  showLogin() {
    this.rightPanelActive = '';
    this.loginFormVisible = true;
    this.signupFormVisible = false;
    this.signupSubmitted = false;
    this.signupForm.reset();
  }

  async onLogin(): Promise<void> {
    this.loginSubmitted = true;
    if (this.loginForm.invalid) {
      return;
    }

    const { email, password } = this.loginForm.value;
    try {
      const response = await firstValueFrom(await this.authService.login(email, password));
      if (response?.token) {
        this.authService.saveToken(response.token);
        const successMessage = await firstValueFrom(this.translate.get('components.auth.loginSuccess'));
        this.notificationService.showSuccess(successMessage);
        this.loginForm.reset();
        this.router.navigate(['/overview']);
      }
    } catch (error) {
      const errorMessage = await firstValueFrom(this.translate.get('components.auth.loginError'));
      this.notificationService.showError(errorMessage);
    }
  }

  onSignup() {
    this.signupSubmitted = true;
    if (this.signupForm.invalid) {
      return;
    }

    const { fullName, email, password } = this.signupForm.value;
    const newUser: User = {
      account: {
        email: email,
        password: password,
        role: 'User',
        dateJoined: new Date(),
      },
      profile: {
        name: fullName,
        dateOfBirth: undefined,
        profilePictureUrl: '',
      },
      finance: new UserFinance(),
    };

    this.authService.signup(newUser).subscribe({
      next: async () => {
        const successMessage = await firstValueFrom(this.translate.get('components.auth.signupSuccess'));
        this.notificationService.showSuccess(successMessage);
        this.showLogin();
        this.loginForm.patchValue({ email, password });
      },
      error: async () => {
        const errorMessage = await firstValueFrom(this.translate.get('components.auth.signupError'));
        this.notificationService.showError(errorMessage);
      }
    });
  }
}
