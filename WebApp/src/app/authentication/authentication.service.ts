import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { TokenStorageKey, RefreshTokenStorageKey } from '../shared/constants';
import { LoginResponseModel } from '../shared/models/authentication';
import { Observable } from 'rxjs';
import { switchMap, mergeMap } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(
    private http: HttpClient,
    private router: Router
  ) {}

  register(registerModel) {
    return this.http.post(`${environment.apiUrl}/Account/Register`, registerModel);
  }

  login(loginModel) {
    return this.http.post(`${environment.apiUrl}/Auth/Login`, loginModel)
      .pipe(switchMap(res => {
        const data = <LoginResponseModel>res;
        localStorage.setItem(TokenStorageKey, data.accessToken.token);
        localStorage.setItem(RefreshTokenStorageKey, data.refreshToken);
        return new Observable(observer => observer.next(loginModel.userName));
      }));
  }

  logout() {
    localStorage.removeItem(TokenStorageKey);
    localStorage.removeItem(RefreshTokenStorageKey);
  }

  refreshToken() {
    var token = localStorage.getItem(TokenStorageKey);
    var refToken = localStorage.getItem(RefreshTokenStorageKey);
    if(!token || !refToken)
      this.router.navigate['login'];

    var headers: HttpHeaders = new HttpHeaders()
    headers.set("Authorization", `Bearer ${token}`)
    return this.http.post(`${environment.apiUrl}/Auth/Refresh`, {refreshToken: refToken} , { headers })
      .pipe(mergeMap( res => {
        const data = <LoginResponseModel>res;
        localStorage.setItem(TokenStorageKey, data.accessToken.token);
        localStorage.setItem(RefreshTokenStorageKey, data.refreshToken);
        return new Observable(observer => observer.next());
      }));
  }

  isAuthenticated() {
    return !!localStorage.getItem(TokenStorageKey);
  }

  isAdmin() {
    return this.http.get(`${environment.apiUrl}/Account/IsAdmin`)
  }
}
