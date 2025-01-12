import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import {Router } from '@angular/router';
import { tap, catchError } from 'rxjs/operators';  // Make sure to import it

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/auth`; // Replace with your API URL
isAuthenticated:boolean = false;



private getInitialStatus(): boolean {
  return localStorage.getItem('isLoggedIn') === 'true';
}
private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.getInitialStatus());
public isAuthenticated$: Observable<boolean> = this.isAuthenticatedSubject.asObservable();

  constructor(private http: HttpClient, private router:Router) {}

  checkAuth(): Observable<boolean> {
    return this.http.get<boolean>(
      `${this.apiUrl}/check-auth`,{ withCredentials: true })
      .pipe(
              map(response => {
                this.isAuthenticatedSubject.next(true);
                        this.isAuthenticated = true;
        return true;
      }),
      catchError(() => {
        this.isAuthenticatedSubject.next(false);
        this.isAuthenticated = false;
        return of(false);
      })
    );
  }

  setAuthenticationStatus(isAuthenticated: boolean): void {
    this.isAuthenticatedSubject.next(isAuthenticated);
  }

  // Optional: Function to get the current value (if needed)
  getAuthenticationStatus(): boolean {
    return this.isAuthenticatedSubject.getValue();
  }
  login(credentials: any): Observable<{ token: string }> {
    console.log('Attempting login...');
    return this.http.post(`${this.apiUrl}/login`, credentials,
      {
        withCredentials: true,
        observe: 'response'
      }).pipe(
        tap((response: any) => {
          console.log('Login Response:', response);
          console.log('Response Headers:', response.headers.keys());
          console.log('Set-Cookie Header:', response.headers.get('set-cookie'));
          console.log('Cookies after login:', document.cookie);

          // If token is in response body, set it manually
          if (response.body && response.body['token']) {
            this.setCookie('token', response.body.token);
            this.isAuthenticated = true;
            this.setAuthenticationStatus(true);
            //this.isAuthenticatedSubject.next(true);
          }
        }),
        catchError(error => {
          console.error('Login error:', error);
          throw error;
        })
      );
}
private setCookie(name: string, value: string): void {
  const secure = window.location.protocol === 'https:';
  document.cookie = `${name}=${value}; path=/; ${secure ? 'secure;' : ''} samesite=none;`;
  console.log('Cookie set manually:', document.cookie);
}
    logout() {
      return this.http.post(`${this.apiUrl}/logout`, {}).subscribe({
        next: () => {console.log('Logged out successfully');
        this.isAuthenticated = false;
        this.isAuthenticatedSubject.next(false);
        this.setAuthenticationStatus(false);

        },
        error: (err) => console.error('Logout failed:', err)
      });
    }

    isLoggedIn(): boolean {
      return this.isAuthenticated || localStorage.getItem('isLoggedIn') === 'true';
    }
    setLoggedIn(status: boolean): void {
      localStorage.setItem('isLoggedIn', status.toString());
      this.isAuthenticatedSubject.next(status); // מעדכן את ה-BehaviorSubject
    }
  register(user: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, user,
      {
        withCredentials: true,  // This goes here
        observe: 'response'     // This goes here
      } ).pipe(
        tap(response => {
          console.log('Register Response:', response);
          console.log('Cookies after register:', document.cookie);
          this.isAuthenticatedSubject.next(true);
        })
      );
  }


}


