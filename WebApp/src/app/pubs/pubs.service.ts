import { Injectable } from '@angular/core';
import { CreatePubModel, PubModel } from '../shared/models/pub';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { CreateRateModel, RateModel } from '../shared/models/rate';
import { PubQuery } from '../shared/models/query';

@Injectable({
  providedIn: 'root'
})
export class PubsService {
  pubQuery = new PubQuery();

  constructor(private http: HttpClient) {}

  createPub(pub: CreatePubModel, image: File) {
    return this.http.post(`${environment.apiUrl}/Pub`, pub)
        .pipe(switchMap(resp => {
            const data = <PubModel>resp;
            return this.uploadImage(data.id, image)
        }));
  }

  getPubs() {
    let params = new HttpParams();
    if(this.pubQuery.boardGameId)
        params =params.append("board_game_id", this.pubQuery.boardGameId.toString())
    if(this.pubQuery.city)
        params =params.append("city", this.pubQuery.city.toString())
    if(this.pubQuery.search)
        params =params.append("search", this.pubQuery.search.toString())

    params = params.append("sort", this.pubQuery.sort.toString())
    params = params.append("page", this.pubQuery.page.toString())
    params = params.append("page_size", this.pubQuery.pageSize.toString())

    return this.http.get(`${environment.apiUrl}/Pub`, { params: params });
  }

  setPubQuery(query: PubQuery) {
    this.pubQuery = query;
  }

  getPubQuery() {
    return this.pubQuery;
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

  getCities() {
    return this.http.get(`${environment.apiUrl}/Pub/cities`)
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

  addBoardGames(pubId: string, boardGameId: string) {
      return this.http.post(`${environment.apiUrl}/Pub/${pubId}/BoardGame/${boardGameId}`, {});
  }

  deleteBoardGames(pubId: string, boardGameId: string) {
      return this.http.delete(`${environment.apiUrl}/Pub/${pubId}/BoardGame/${boardGameId}`);
  }

  private uploadImage(id: string, image: File) {
      let formData = new FormData();
      formData.append('image', image, image.name);
      return this.http.post(`${environment.apiUrl}/Pub/${id}/image`, formData);
  }
}
