import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  
  /**
   * Interceptor to attach JWT token to outgoing requests.
   * @param authService The authentication service to retrieve the token.
   */
  constructor(private authService: AuthService, private router: Router) {}

  /**
   * Intercepts HTTP requests and adds the Authorization header if a token is present.
   * @param request The outgoing request object.
   * @param next The next interceptor in the chain.
   * @returns An observable of the HTTP event.
   */
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.getToken();

    // Only attach token if it exists and the request is for the API
    if (token && request.url.startsWith('/api')) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          this.authService.logout();
          this.router.navigate(['/auth']);
        }
        return throwError(() => error);
      })
    );
  }
}
