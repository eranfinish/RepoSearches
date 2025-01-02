// interceptors/auth.interceptor.ts
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router) {
    console.log('AuthInterceptor constructed'); // Verify it's being created
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    console.log('AuthInterceptor - Starting interception');
    console.log('Original Request:', request.url);

    // Try to get token from cookie
    const token = this.getCookie('token');
    console.log('Token found:', token ? 'Yes (${token})' : 'No');

    let modifiedRequest = request;

    if (token) {
      console.log('Adding Bearer token to request');
      modifiedRequest = request.clone({
        headers: request.headers.set('Authorization', `Bearer ${token}`)
      });
    }

    // Always add withCredentials
    modifiedRequest = modifiedRequest.clone({
      withCredentials: true
    });

 //   console.log('Modified Request Headers:', modifiedRequest.headers.keys());
   // console.log('Final Authorization Header:', modifiedRequest.headers.get('Authorization'));

    return next.handle(modifiedRequest).pipe(
      tap(event => {
        console.log('Response in interceptor:', event);
      }),
      catchError((error: HttpErrorResponse) => {
        console.error('Error in interceptor:', error);
        if (error.status === 401) {
          console.log('Unauthorized - redirecting to login');
          this.router.navigate(['/login']);
        }
        return throwError(() => error);
      })
    );
  }

  private getCookie(name: string): string | null {
    console.log('Searching for cookie:', name);
    console.log('All cookies:', document.cookie);

    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) {
      const token = parts.pop()?.split(';').shift() || null;
      console.log('Found token in cookie:', token);
      return token;
    }
    console.log('No token found in cookies');
    return null;
  }
}
