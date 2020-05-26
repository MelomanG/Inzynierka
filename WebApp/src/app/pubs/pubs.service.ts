import { Injectable } from '@angular/core';
import { CreatePubModel, PubModel } from '../shared/models/pub';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { CreateRateModel, RateModel } from '../shared/models/rate';

@Injectable({
  providedIn: 'root'
})
export class PubsService {

  constructor(private http: HttpClient) {}

  createPub(pub: CreatePubModel, image: File) {
    return this.http.post(`${environment.apiUrl}/Pub`, pub)
        .pipe(switchMap(resp => {
            const data = <PubModel>resp;
            return this.uploadImage(data.id, image)
        }));
  }

  getPubs() {
      return this.http.get(`${environment.apiUrl}/Pub`);
  }

  getUsersPubs() {
      return this.http.get(`${environment.apiUrl}/Pub/users`);
  }

  getLikedPubs() {
      return this.http.get(`${environment.apiUrl}/Pub/like`)
  }

  getPub(id: string) {
      return this.http.get(`${environment.apiUrl}/Pub/${id}`);
  }

  updatePub(pub: PubModel, image: File) {
      return this.http.put(`${environment.apiUrl}/Pub/${pub.id}`, pub)
          .pipe(switchMap(resp => {
              const data = <PubModel>resp;
              if(image) 
                  return this.uploadImage(data.id, image)
              else 
                  return new Observable(observer => observer.next(data));
          }));
  }

  deletePub(id: string) {
      return this.http.delete(`${environment.apiUrl}/Pub/${id}`);
  }

  likePub(id: string) {
      return this.http.post(`${environment.apiUrl}/Pub/${id}/like`, {})
  }

  unLikePub(id: string) {
      return this.http.delete(`${environment.apiUrl}/Pub/${id}/unlike`)
  }

  ratePub(id: string, rate: CreateRateModel) {
      return this.http.post(`${environment.apiUrl}/Pub/${id}/rate`, rate)
  }

  getUserPubRate(id: string) {
      return this.http.get(`${environment.apiUrl}/Pub/${id}/rate`)
  }

  updatePubRate(id: string, rate: RateModel) {
      return this.http.put(`${environment.apiUrl}/Pub/${id}/rate/${rate.id}`, rate)
  }

  private uploadImage(id: string, image: File) {
      let formData = new FormData();
      formData.append('image', image, image.name);
      return this.http.post(`${environment.apiUrl}/Pub/${id}/image`, formData);
  }
}
