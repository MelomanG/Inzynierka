import { Component, OnInit, Renderer2 } from '@angular/core';
import { FormControl } from '@angular/forms';
import { EventModel } from 'src/app/shared/models/events';
import { Observable } from 'rxjs';
import { EventsService } from '../events.service';
import { AuthenticationService } from 'src/app/authentication/authentication.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { startWith, map } from 'rxjs/operators';

@Component({
  selector: 'app-show-user-events',
  templateUrl: './show-user-events.component.html',
  styleUrls: ['./show-user-events.component.scss']
})
export class ShowUserEventsComponent implements OnInit {
  searchControl = new FormControl();

  eventList: EventModel[];
  filteredEventList: Observable<EventModel[]>;
  serverUrl: string;
  isAuthenticated: any;

  constructor(
    private eventService: EventsService,
    private authService: AuthenticationService,
    private router: Router,
    private renderer2: Renderer2) { }

  ngOnInit() {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.serverUrl = environment.serverUrl;
    this.loadEvents();
    this.filterEvents();
  }

  loadEvents() {
    this.eventService.getUsersEvents().
      subscribe(res => {
        this.eventList = <EventModel[]> res;
    });
  }

  filterEvents() {
    this.filteredEventList = this.searchControl.valueChanges
      .pipe(
        startWith(''),
        map(value => this._filter(value))
      )
  }

  private _filter(value: string) {
    if(value.length < 2)
    {
      this.loadEvents();
      return this.eventList;
    }
    return this.eventList
      .filter(bg =>
        bg.name.toLowerCase().includes(value.toLowerCase()) );
  }

  showEvent(event: EventModel) {
    this.router.navigate([`show/event/${event.id}`]);
  }

  mouseenter (event) {
    this.renderer2.addClass(event.target, 'mat-elevation-z5')
 }
 
 mouseleave (event) {
   this.renderer2.removeClass(event.target, 'mat-elevation-z5')
 }
}
