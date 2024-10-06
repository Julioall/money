import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../core/model/entities/user';
import { NotificationService } from '../../core/service/notification/notification.service';
import { AuthService } from '../../core/service/auth/auth.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UserFinance } from '../../core/model/entities/userFinance';
import { firstValueFrom } from 'rxjs';  // Importa firstValueFrom

@Component({
  selector: 'app-auth',
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

  showSignUp(): void {
    this.rightPanelActive = 'right-panel-active';
    setTimeout(() => {
      this.signupFormVisible = true;
      this.loginFormVisible = false;
      this.loginSubmitted = false;
      this.loginForm.reset();
    }, 350);
  }

  showLogin(): void {
    this.rightPanelActive = '';
    setTimeout(() => {
      this.loginFormVisible = true;
      this.signupFormVisible = false;
      this.signupSubmitted = false;
      this.signupForm.reset();
    }, 350);
  }

  async onLogin(): Promise<void> {
    this.loginSubmitted = true;
    if (this.loginForm.invalid) {
      this.notificationService.showError('Please fill out the form correctly.');
      return;
    }

    const { email, password } = this.loginForm.value;
    try {
      const response = await firstValueFrom(await this.authService.login(email, password));
      if (response?.token) {
        this.authService.saveToken(response.token);
        this.notificationService.showSuccess('Login successfully!');
        this.loginForm.reset();
        this.router.navigate(['/overview']);
      }
    } catch (error) {
      this.notificationService.showError('Login error. Please check your credentials.');
    }
  }

  onSignup(): void {
    this.signupSubmitted = true;
    if (this.signupForm.invalid) {
      this.notificationService.showError('Please fill out the form correctly.');
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
      next: () => {
        this.notificationService.showSuccess('Registration successful!');
        setTimeout(() => {
          this.showLogin();
          this.loginForm.patchValue({ email, password });
        }, 2000);
      },
      error: () => {
        this.notificationService.showError('Error during registration. Please try again.');
      }
    });
  }
}
