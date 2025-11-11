
import { computed, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

export interface AuthResponse {
  message: string;
  token: string;
  userId: number;
  expiresAt: string;
}

export interface RegisterPayload {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export interface LoginPayload {
  email: string;
  password: string;
}

export interface UserProfile {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly isBrowser = typeof window !== 'undefined';
  private readonly apiUrl = '/api/auth';
  private readonly tokenKey = 'auth_token';
  private readonly expiresAtKey = 'auth_expires_at';
  private readonly loggedIn = signal(this.hasValidToken());

  readonly isAuthenticated = computed(() => this.loggedIn());

  constructor(private http: HttpClient) {}

  register(user: RegisterPayload): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, user).pipe(
      tap(response => this.persistAuth(response))
    );
  }

  login(credentials: LoginPayload): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, credentials).pipe(
      tap(response => this.persistAuth(response))
    );
  }

  getProfile(): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${this.apiUrl}/me`);
  }

  getToken(): string | null {
    if (!this.isBrowser) {
      return null;
    }
    return this.hasValidToken() ? localStorage.getItem(this.tokenKey) : null;
  }

  isLoggedIn(): boolean {
    return this.hasValidToken();
  }

  logout(): void {
    this.clearStoredCredentials();
    this.loggedIn.set(false);
  }

  private persistAuth(response: AuthResponse): void {
    if (!this.isBrowser) {
      return;
    }
    localStorage.setItem(this.tokenKey, response.token);
    localStorage.setItem(this.expiresAtKey, response.expiresAt);
    this.loggedIn.set(true);
  }

  private hasValidToken(): boolean {
    if (!this.isBrowser) {
      return false;
    }
    const token = localStorage.getItem(this.tokenKey);
    const expiresAt = localStorage.getItem(this.expiresAtKey);
    if (!token || !expiresAt) {
      this.clearStoredCredentials();
      return false;
    }

    const expiresAtTime = Date.parse(expiresAt);
    if (Number.isNaN(expiresAtTime) || expiresAtTime <= Date.now()) {
      this.clearStoredCredentials();
      return false;
    }

    return true;
  }

  private clearStoredCredentials(): void {
    if (!this.isBrowser) {
      return;
    }
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.expiresAtKey);
  }
}
