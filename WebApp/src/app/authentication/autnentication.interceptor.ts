import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { TokenStorageKey, RefreshTokenStorageKey } from '../shared/constants';
import { catchError, mergeMap, switchMap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationInterceptor implements HttpInterceptor {

  constructor(
    private router: Router,
    private authService: AuthenticationService
  )
  { }

  intercept(req, next) {
    var token = localStorage.getItem(TokenStorageKey);
    var authRequest = req.clone({
        headers: req.headers.set("Authorization", `Bearer ${token}`)
    });

    return next.handle(authRequest).pipe(
      catchError(
        err => {
          if(err instanceof HttpErrorResponse) {
            if(err.status === 401 || err.status === 403) {
              if(req.url.toLowerCase().includes('/api/auth/refresh') || req.url.toLowerCase().includes('/api/account/register') || req.url.toLowerCase().includes('/api/auth/login')) {
                localStorage.removeItem(TokenStorageKey);
                localStorage.removeItem(RefreshTokenStorageKey);
                this.router.navigate['login'];
              }
              else {
                return this.authService.refreshToken().pipe(
                  switchMap(() => {
                    var secAuthRequest = req.clone({
                      headers: req.headers.set("Authorization", `Bearer ${localStorage.get(TokenStorageKey)}`)});
                    return next.handle(secAuthRequest);
                  })
                )
              }
            }
          }
        this.router.navigate['login'];
      })
    );
  }
}
