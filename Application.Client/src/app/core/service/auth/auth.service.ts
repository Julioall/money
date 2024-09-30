import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom, Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { User } from '../../model/entities/user';
import { jwtDecode } from 'jwt-decode'; 
import { environment } from '../../../environment/environment';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/User/add`;

  constructor(private http: HttpClient, private router: Router) {}

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
      body.IpAddress = ipResponse.ip; // Adiciona o IP se obtido
    } catch (error) {
      console.error('Could not retrieve IP address, proceeding without it.', error);
    }
    
    return this.http.post<any>(url, body).pipe(
      map((response) => {
        if (response && response.token) {
          this.saveToken(response.token);
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

  logout(): void {
    this.removeToken();
    this.router.navigate(['/login']);
  }

  saveToken(token: string): void {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  removeToken(): void {
    localStorage.removeItem('token');
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

  getAuthHeaders(): HttpHeaders {
    const token = this.getToken();
    if (token && !this.isTokenExpired(token)) {
      return new HttpHeaders().set('Authorization', `Bearer ${token}`);
    }
    return new HttpHeaders();
  }

  getDeviceInfo() {
    const userAgent = navigator.userAgent;
    const browser = this.getBrowserInfo(userAgent);
    const os = this.getOSInfo(userAgent);
    return { browser, os };
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
