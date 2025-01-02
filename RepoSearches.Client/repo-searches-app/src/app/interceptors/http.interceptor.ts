import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable()// For rerouting when unauthorized access
export class AuthInterceptor implements HttpInterceptor {

    constructor(private router: Router) {}

    intercept(req: HttpRequest<any>, next: HttpHandler) {
        return next.handle(req).pipe(
            catchError(err => {
                if (err.status === 401) {
                    // If User is not authenticated, redirect to login page
                    localStorage.removeItem('isLoggedIn');
                    this.router.navigate(['/login']);
                }
                return throwError(err);
            })
        );
    }
}
