import { Component, OnInit, ViewChild, ElementRef, Renderer2 } from '@angular/core';
import { FormControl } from '@angular/forms';
import { PaginationResult } from 'src/app/shared/models/paginationresult';
import { EventModel } from 'src/app/shared/models/events';
import { PubQuery, BoardGameQuery, EventQuery } from 'src/app/shared/models/query';
import { Observable } from 'rxjs';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { PubsService } from 'src/app/pubs/pubs.service';
import { AuthenticationService } from 'src/app/authentication/authentication.service';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { BoardGameService } from 'src/app/boardgames/boardgame.service';
import { environment } from 'src/environments/environment';
import { PubModel } from 'src/app/shared/models/pub';
import { startWith, map } from 'rxjs/operators';
import { EventsService } from '../events.service';

@Component({
  selector: 'app-show-events',
  templateUrl: './show-events.component.html',
  styleUrls: ['./show-events.component.scss']
})
export class ShowEventsComponent implements OnInit {
  serverUrl: string;
  isAuthenticated: any;
  sortOptions = [
    { name: 'Nazwa A-Z', value: 'name' },
    { name: 'Nazwa Z-A', value: 'name desc'},
    { name: 'Data rosnąco', value: 'date' },
    { name: 'Data malejaco', value: 'date desc' },
    { name: 'Uczestnicy', value: 'users' },
    { name: 'Uczestnicy malejaco', value: 'users desc' }
  ];

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  pubControl = new FormControl();
  boardGameControl = new FormControl();
  citiesControl = new FormControl();

  selectedSort;

  eventList: PaginationResult<EventModel>;

  pubQuery: PubQuery;
  boardGameQuery: BoardGameQuery;
  eventQuery: EventQuery;

  cities: string[];
  pubs: PubModel[];
  loadedBoardGames: BoardGameModel[];

  filteredCities: Observable<string[]>;
  filteredBoardGames: Observable<BoardGameModel[]>;
  filteredPubs: Observable<PubModel[]>;

  constructor(
    private eventService: EventsService,
    private pubService: PubsService,
    private authService: AuthenticationService,
    private router: Router,
    private renderer2: Renderer2,
    private boardGameService: BoardGameService) {
      this.pubQuery = new PubQuery();
      this.pubService.setPubQuery(this.pubQuery);
      this.boardGameQuery = new BoardGameQuery()
      this.boardGameService.setBoardGameQuery(this.boardGameQuery);
      this.eventQuery = new EventQuery()
      this.eventService.setEventQuery(this.eventQuery);
    }

  ngOnInit() {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.serverUrl = environment.serverUrl;
    this.loadBoardgames();
    this.loadEvents();
    this.loadCities();
    this.loadPubs();
  }

  loadEvents() {
    this.eventService.getEvents().
      subscribe(res => {
        this.eventList = <PaginationResult<EventModel>> res;
    });
  } 

  loadPubs() {
    this.filteredPubs = this.pubControl.valueChanges
    .pipe(
      startWith(''),
      map(value => this._filterPubs(value))
    );
  }

  private _filterPubs(value: string): PubModel[] {
    if(value.length < 2)
    {
      this.pubs = null;
      this.eventQuery = this.eventService.getEventQuery();
      this.eventQuery.pubId = null;
      this.eventService.setEventQuery(this.eventQuery);
      this.loadEvents();
      return;
    }

    if(!this.pubs)
    {
      this.pubQuery = this.pubService.getPubQuery();
      this.pubQuery.search = value
      this.boardGameQuery.pageSize = 100;
      this.pubService.setPubQuery(this.pubQuery);

      this.pubService.getPubs()
        .subscribe(res => {
          this.pubs = (<PaginationResult<PubModel>>res).results;
          const filterValue = value.toLowerCase();
          return this.pubs.filter(option => option.name.toLowerCase().includes(filterValue));
      })
    }
    else {
      const filterValue = value.toLowerCase();
      return this.pubs.filter(option => option.name.toLowerCase().includes(filterValue));
    }
  }

  loadCities() {
    this.filteredCities = this.citiesControl.valueChanges
    .pipe(
      startWith(''),
      map(value => this._filterCities(value))
    );
  }

  private _filterCities(value: string): string[] {
    if(value.length < 2)
    {
      this.cities = null;      
      this.eventQuery = this.eventService.getEventQuery();
      this.eventQuery.city = null;
      this.eventService.setEventQuery(this.eventQuery);
      this.loadEvents();
      return;
    }

    if(!this.cities)
    {
      this.eventService.getCities()
        .subscribe(res => {
          this.cities = <string[]>res;
          const filterValue = value.toLowerCase();
          return this.cities.filter(option => option.toLowerCase().includes(filterValue));
      })
    }
    else {
      const filterValue = value.toLowerCase();
      return this.cities.filter(option => option.toLowerCase().includes(filterValue));
    }
  }

  loadBoardgames() {
    this.filteredBoardGames = this.boardGameControl.valueChanges
    .pipe(
      startWith(''),
      map(value => this._filterBoardGame(value))
    );
  }
  
  private _filterBoardGame(value: string): BoardGameModel[] {
    if(value.length < 2)
    {
      this.loadedBoardGames = null;
      this.eventQuery = this.eventService.getEventQuery();
      this.eventQuery.boardGameId = null;
      this.eventService.setEventQuery(this.eventQuery);
      this.loadEvents()
      return;
    }
    
    if(!this.loadedBoardGames)
    {
      this.boardGameQuery.search = value;
      this.boardGameQuery.pageSize = 100;
      this.boardGameService.setBoardGameQuery(this.boardGameQuery)
  
      this.boardGameService.getBoardGames()
        .subscribe(res => {
          this.loadedBoardGames = (<PaginationResult<BoardGameModel>>res).results;
          const filterValue = value.toLowerCase();
          const temp = this.loadedBoardGames.filter(option => option.name.toLowerCase().includes(filterValue));
          return temp;
        });
    }
    else {
      const filterValue = value.toLowerCase();
      const temp = this.loadedBoardGames.filter(option => option.name.toLowerCase().includes(filterValue));
      return temp;
    }
  }
  
  onPageChanged(event: any) {
    const query = this.eventService.getEventQuery();
    if (query.page !== event)
    {
      query.page = event;
       this.eventService.setEventQuery(query);
       this.loadEvents();
    }
   }

  showEvent(eventModel: EventModel) {
    this.router.navigate([`show/event/${eventModel.id}`]);
  }

  onCitySelected(city: string) {
    const query = this.eventService.getEventQuery();
    query.city = city;
    query.page = 1;
    this.eventService.setEventQuery(query);
    this.loadEvents();
  }

  onBoardGameSelected(boardGameName: string)
  {
    const boardGameId = this.loadedBoardGames.find(bg => bg.name == boardGameName).id;
    const query = this.eventService.getEventQuery();
    query.boardGameId = boardGameId;
    query.page = 1;
    this.eventService.setEventQuery(query);
    this.loadEvents();
  }

  onPubSelected(pubName: string)
  {
    const pubId = this.pubs.find(p => p.name == pubName).id;
    const query = this.eventService.getEventQuery();
    query.pubId = pubId;
    query.page = 1;
    this.eventService.setEventQuery(query);
    this.loadEvents();
  }

  onSearch() {
    const query = this.eventService.getEventQuery();
    query.search = this.searchTerm.nativeElement.value;
    query.page = 1;
    this.eventService.setEventQuery(query);
    this.loadEvents();
  }

  onSortChange(value) {
    const query = this.eventService.getEventQuery();
    query.sort = value;
    query.page = 1;
    this.eventService.setEventQuery(query);
    this.loadEvents();
  }
 
  onReset() {
    this.citiesControl.setValue('');
    this.boardGameControl.setValue('');
    this.pubControl.setValue('');
    this.selectedSort = null;
    this.searchTerm.nativeElement.value = '';
    this.eventQuery = new EventQuery();
    this.eventService.setEventQuery(this.eventQuery);
    this.pubQuery = new PubQuery();
    this.pubService.setPubQuery(this.pubQuery);
    this.boardGameQuery = new BoardGameQuery();
    this.boardGameService.setBoardGameQuery(this.boardGameQuery);
    this.loadEvents();
  }

  mouseenter (event) {
    this.renderer2.addClass(event.target, 'mat-elevation-z5')
 }
 
 mouseleave (event) {
   this.renderer2.removeClass(event.target, 'mat-elevation-z5')
 }
}
