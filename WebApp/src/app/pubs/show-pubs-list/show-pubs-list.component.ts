import { Component, OnInit, Renderer2, ViewChild, ElementRef } from '@angular/core';
import { PaginationResult } from 'src/app/shared/models/paginationresult';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { PubModel } from 'src/app/shared/models/pub';
import { PubsService } from '../pubs.service';
import { AuthenticationService } from 'src/app/authentication/authentication.service';
import { MatDialog } from '@angular/material/dialog';
import { RateModel } from 'src/app/shared/models/rate';
import { PubRateDialogComponent } from '../pub-rate-dialog/pub-rate-dialog.component';
import { PubQuery, BoardGameQuery } from 'src/app/shared/models/query';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs/internal/Observable';
import { startWith, map } from 'rxjs/operators';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { BoardGameService } from 'src/app/boardgames/boardgame.service';

@Component({
  selector: 'app-show-pubs-list',
  templateUrl: './show-pubs-list.component.html',
  styleUrls: ['./show-pubs-list.component.scss']
})
export class ShowPubsListComponent implements OnInit {
  serverUrl: string;
  isAuthenticated: any;
  sortOptions = [
    { name: 'Nazwa A-Z', value: 'name' },
    { name: 'Nazwa Z-A', value: 'name desc'},
    { name: 'Ocena rosnąco', value: 'rate' },
    { name: 'Ocena malejaco', value: 'rate desc' },
    { name: 'Popularne', value: 'like desc' },
    { name: 'Mało popularne', value: 'like' }
  ];

  @ViewChild('search', {static: false}) searchTerm: ElementRef;

  citiesControl = new FormControl();
  boardGameControl = new FormControl();

  pubList: PaginationResult<PubModel>;
  pubQuery: PubQuery;
  boardGameQuery: BoardGameQuery;
  cities: string[];
  filteredCities: Observable<string[]>;
  loadedBoardGames: BoardGameModel[];
  filteredBoardGames: Observable<BoardGameModel[]>;

  constructor(
    private pubService: PubsService,
    private authService: AuthenticationService,
    private dialog: MatDialog,
    private router: Router,
    private renderer2: Renderer2,
    private boardGameService: BoardGameService) {
      this.pubQuery = new PubQuery();
      this.pubService.setPubQuery(this.pubQuery);
      this.boardGameQuery = new BoardGameQuery()
      this.boardGameService.setBoardGameQuery(this.boardGameQuery);
    }

  ngOnInit() {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.serverUrl = environment.serverUrl;
    this.loadPubs();
    this.loadCities();
    this.loadBoardgames();
  }

  loadPubs() {
    this.pubService.getPubs().
      subscribe(res => {
        this.pubList = <PaginationResult<PubModel>> res;
    });
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
      this.pubQuery = this.pubService.getPubQuery();
      this.pubQuery.city = null;
      this.pubService.setPubQuery(this.pubQuery);
      this.loadPubs();
      return;
    }

    if(!this.cities)
    {
      this.pubService.getCities()
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
      this.boardGameQuery = this.boardGameService.getBoardGameQuery();
      this.boardGameQuery.search = null;
      this.boardGameService.setBoardGameQuery(this.boardGameQuery);
      this.pubQuery = this.pubService.getPubQuery();
      this.pubQuery.boardGameId = null;
      this.pubService.setPubQuery(this.pubQuery);
      this.loadPubs()
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
    const query = this.pubService.getPubQuery();
    if (query.page !== event)
    {
      query.page = event;
       this.pubService.setPubQuery(query);
       this.loadPubs();
    }
   }

  showPub(pub: PubModel) {
    this.router.navigate([`show/pub/${pub.id}`]);
  }

  onCitySelected(city: string) {
    const query = this.pubService.getPubQuery();
    query.city = city;
    query.page = 1;
    this.pubService.setPubQuery(query);
    this.loadPubs();
  }

  onBoardGameSelected(boardGameName: string)
  {
    const boardGameId = this.loadedBoardGames.find(bg => bg.name == boardGameName).id;
    const query = this.pubService.getPubQuery();
    query.boardGameId = boardGameId;
    this.pubService.setPubQuery(query);
    this.loadPubs();
  }

  onSearch() {
    const query = this.pubService.getPubQuery();
    query.search = this.searchTerm.nativeElement.value;
    query.page = 1;
    this.pubService.setPubQuery(query);
    this.loadPubs();
  }

  onSortChange(value) {
    const query = this.pubService.getPubQuery();
    query.sort = value;
    this.pubService.setPubQuery(query);
    this.loadPubs();
  }
 
  onReset() {
    this.citiesControl.setValue('');
    this.boardGameControl.setValue('');
    this.searchTerm.nativeElement.value = '';
    this.pubQuery = new PubQuery();
    this.pubService.setPubQuery(this.pubQuery);
    this.boardGameQuery = new BoardGameQuery();
    this.boardGameService.setBoardGameQuery(this.boardGameQuery);
    this.loadPubs();
  }

  thumbClicked(pub) {
    if(!this.isAuthenticated)
        return;

    pub.isLikedByUser = !pub.isLikedByUser
    if(pub.isLikedByUser)
    {
      pub.amountOfLikes += 1;
      this.pubService.likePub(pub.id).subscribe();
    }
    else
    {
      this.pubService.unLikePub(pub.id).subscribe();
      pub.amountOfLikes -= 1;
    }
  }

  getPubRate(rates: RateModel[]) {
    if(rates.length <=0 )
      return 0;
    var sum = 0;
    for (var i = 0; i < rates.length; i++) {
      sum += rates[i].userRate
    }
    return Math.round(sum/rates.length)
  }

  toggleCreateRate(pub: PubModel) {
    var dialogRef = this.dialog.open(PubRateDialogComponent, {
      width: "450px",
      data: {
        pub
      }
    });

    dialogRef.afterClosed().subscribe(() => {
      this.loadPubs();
    }
    )
  }

  mouseenter (event) {
    this.renderer2.addClass(event.target, 'mat-elevation-z5')
 }
 
 mouseleave (event) {
   this.renderer2.removeClass(event.target, 'mat-elevation-z5')
 }
}
