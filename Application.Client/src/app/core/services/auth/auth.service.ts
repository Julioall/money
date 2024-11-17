import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { BehaviorSubject, lastValueFrom, Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { jwtDecode } from 'jwt-decode';
import { environment } from '../../../../environment/environment';
import { Router } from '@angular/router';
import { User } from '../../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/User/add`;
  private loggedInSubject = new BehaviorSubject<boolean>(this.isLoggedIn());
  public loggedIn$ = this.loggedInSubject.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    this.checkLoginStatus();
  }

  async login(email: string, password: string): Promise<Observable<any>> {
    const url = `${environment.apiUrl}/Authentication/login`;
    const deviceInfo = this.getDeviceInfo();

    const body = {
      Email: email,
      Password: password,
      IpAddress: "",
      OperatingSystem: deviceInfo.os,
      Browser: deviceInfo.browser,
    };

    try {
      const ipResponse = await lastValueFrom(this.getIpAddress());
      body.IpAddress = ipResponse.ip;
    } catch (error) {
      console.error('Could not retrieve IP address, proceeding without it.', error);
    }

    return this.http.post<any>(url, body).pipe(
      map((response) => {
        if (response && response.token) {
          this.saveToken(response.token);
          this.loggedInSubject.next(true);
        }
        return response;
      }),
      catchError(error => {
        if (error.status === 401) {
          return throwError(() => new Error('Invalid credentials'));
        }
        return throwError(() => new Error('Login failed'));
      })
    );
  }

  signup(user: User): Observable<any> {
    return this.http.post<any>(this.apiUrl, user).pipe(
      catchError(() => throwError(() => new Error('Sign up failed')))
    );
  }

  logout() {
    this.removeToken();
    this.router.navigate(['/login']);
  }

  saveToken(token: string) {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('token', token);
    }
  }

  getToken(): string | null {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem('token');
    }
    return null;
  }

  removeToken() {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('token');
    }
  }

  isTokenExpired(token: string): boolean {
    try {
      const decoded: any = jwtDecode(token);
      if (decoded.exp === undefined) return false;
      const date = new Date(0);
      date.setUTCSeconds(decoded.exp);
      return date.valueOf() < new Date().valueOf();
    } catch {
      return true;
    }
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    return token !== null && !this.isTokenExpired(token);
  }

  private checkLoginStatus() {
    const isLoggedIn = this.isLoggedIn();
    this.loggedInSubject.next(isLoggedIn);
  }

  updateLoginStatus() {
    this.checkLoginStatus();
  }

  getDeviceInfo() {
    if (isPlatformBrowser(this.platformId)) {
      const userAgent = navigator.userAgent;
      const browser = this.getBrowserInfo(userAgent);
      const os = this.getOSInfo(userAgent);
      return { browser, os };
    }
    return { browser: 'Unknown Browser', os: 'Unknown OS' };
  }

  private getBrowserInfo(userAgent: string): string {
    if (userAgent.includes('Firefox')) {
      return 'Firefox';
    } else if (userAgent.includes('Chrome')) {
      return 'Chrome';
    } else if (userAgent.includes('Safari')) {
      return 'Safari';
    } else if (userAgent.includes('Edge')) {
      return 'Edge';
    } else {
      return 'Unknown Browser';
    }
  }

  private getOSInfo(userAgent: string): string {
    if (userAgent.includes('Win')) {
      return 'Windows';
    } else if (userAgent.includes('Mac')) {
      return 'MacOS';
    } else if (userAgent.includes('Linux')) {
      return 'Linux';
    } else if (userAgent.includes('Android')) {
      return 'Android';
    } else if (userAgent.includes('like Mac')) {
      return 'iOS';
    } else {
      return 'Unknown OS';
    }
  }

  getIpAddress(): Observable<any> {
    const apiUrl = 'https://api.ipify.org?format=json';
    return this.http.get<any>(apiUrl);
  }
}
