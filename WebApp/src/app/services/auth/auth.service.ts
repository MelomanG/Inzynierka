import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from "@angular/router"
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private api: HttpClient,
    private router: Router) { }

    get isAuthenticated() {
      return !!localStorage.getItem("token");
    }

  register(credentials) {
    this.api.post(`${environment.hexadoApiEndpoint}/account/register`, credentials,
      { responseType: "text" })
      .subscribe(res => {
        this.router.navigate(['/']);
      })
  }

  login(credentials) {
    this.api.post(`${environment.hexadoApiEndpoint}/login`, credentials,
      { responseType: "text" })
      .subscribe(res => {
        localStorage.setItem("token", res);
        this.router.navigate(['/']);
      })
  }

  authenticate(res) {
    localStorage.setItem("token", res);
    this.router.navigate(['/']);
  }

  logout() {
    localStorage.removeItem("token");
  }
}
