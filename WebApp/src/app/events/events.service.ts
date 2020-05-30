import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { EventQuery } from '../shared/models/query';
import { CreateEventModel, EventModel } from '../shared/models/events';
import { environment } from 'src/environments/environment';
import { switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventsService {
  eventQuery = new EventQuery();

  constructor(private http: HttpClient) {}

  createEvent(eventModel: CreateEventModel, image: File) {
    return this.http.post(`${environment.apiUrl}/Event`, eventModel)
        .pipe(switchMap(resp => {
            const data = <EventModel>resp;
            return this.uploadImage(data.id, image)
        }));
  }

  getEvents() {
    let params = new HttpParams();
    if(this.eventQuery.boardGameId)
        params =params.append("board_game_id", this.eventQuery.boardGameId.toString())
    if(this.eventQuery.pubId)
      params =params.append("pub_id", this.eventQuery.boardGameId.toString())
    if(this.eventQuery.city)
        params =params.append("city", this.eventQuery.city.toString())
    if(this.eventQuery.search)
        params =params.append("search", this.eventQuery.search.toString())

    params = params.append("sort", this.eventQuery.sort.toString())
    params = params.append("page", this.eventQuery.page.toString())
    params = params.append("page_size", this.eventQuery.pageSize.toString())

    return this.http.get(`${environment.apiUrl}/Event`, { params: params });
  }

  getUsersEvents() {
    return this.http.get(`${environment.apiUrl}/Event/user`);
  }

  getEvent(id: string) {
      return this.http.get(`${environment.apiUrl}/Event/${id}`);
  }

  updateEvent(event: EventModel, image: File) {
      return this.http.put(`${environment.apiUrl}/Event/${event.id}`, event)
          .pipe(switchMap(resp => {
              const data = <EventModel>resp;
              if(image) 
                  return this.uploadImage(data.id, image)
              else 
                  return new Observable(observer => observer.next(data));
          }));
  }

  deleteEvent(id: string) {
      return this.http.delete(`${environment.apiUrl}/Event/${id}`);
  }

  addParticipant(eventId: string, participantId: string) {
      return this.http.post(`${environment.apiUrl}/Event/${eventId}/participant/${participantId}`, {});
  }

  deleteParticipant(eventId: string, participantId: string) {
      return this.http.delete(`${environment.apiUrl}/Event/${eventId}/participant/${participantId}`);
  }

  joinEvent(id: string) {
      return this.http.post(`${environment.apiUrl}/Event/${id}/join`, {})
  }

  quitEvent(id: string) {
      return this.http.delete(`${environment.apiUrl}/Event/${id}/quit`)
  }

  getCities() {
    return this.http.get(`${environment.apiUrl}/Event/cities`)
  }

  setEventQuery(query: EventQuery) {
    this.eventQuery = query;
  }

  getEventQuery() {
    return this.eventQuery;
  }

  private uploadImage(id: string, image: File) {
      let formData = new FormData();
      formData.append('image', image, image.name);
      return this.http.post(`${environment.apiUrl}/Event/${id}/image`, formData);
  }
}
