import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ContactService {

  constructor(private http: HttpClient) {}

  getUsers(search: string = null) {
    let params = new HttpParams()
    if(search)
      params.append('search', search);

    return this.http.get(`${environment.apiUrl}/User/users`, {params: params});
  }

  getContacts() {
    return this.http.get(`${environment.apiUrl}/User/contacts`);
  }

  addContact(id: string) {
    return this.http.post(`${environment.apiUrl}/User/${id}/contact`, {});
  }

  deleteContact(id: string) {
    return this.http.delete(`${environment.apiUrl}/User/contact/${id}`,);
  }
}
