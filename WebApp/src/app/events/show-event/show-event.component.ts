import { Component, OnInit, ViewChild, Renderer2 } from '@angular/core';
import { RateModel } from 'src/app/shared/models/rate';
import { PubRateDialogComponent } from 'src/app/pubs/pub-rate-dialog/pub-rate-dialog.component';
import { BoardGameRateDialogComponent } from 'src/app/boardgames/board-game-rate-dialog/board-game-rate-dialog.component';
import { UserModel } from 'src/app/shared/models/contacts';
import { EventsService } from '../events.service';
import { EventModel } from 'src/app/shared/models/events';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { PubsService } from 'src/app/pubs/pubs.service';
import { AuthenticationService } from 'src/app/authentication/authentication.service';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { PubModel } from 'src/app/shared/models/pub';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { BoardGameService } from 'src/app/boardgames/boardgame.service';

@Component({
  selector: 'app-show-event',
  templateUrl: './show-event.component.html',
  styleUrls: ['./show-event.component.scss']
})
export class ShowEventComponent implements OnInit {
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  displayedColumns: string[] = ['name'];

  users: MatTableDataSource<UserModel>; 
  serverUrl: string;
  isAuthenticated: boolean;
  eventModel: EventModel;

  constructor(
    private boardGameService: BoardGameService,
    private eventService: EventsService,
    private pubService: PubsService,
    private authService: AuthenticationService,
    private dialog: MatDialog,
    private route: ActivatedRoute,
    private router: Router,
    private renderer2: Renderer2) { }

  ngOnInit() {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.serverUrl = environment.serverUrl;
    this.loadPub();
  }

  loadPub(){
    this.eventService.getEvent(this.route.snapshot.params.id)
      .subscribe(res => {
        this.eventModel = <EventModel> res;
        console.log(res)
        this.users = new MatTableDataSource<UserModel>(this.eventModel.participants);
        this.users.paginator = this.paginator;
        this.users.sort = this.sort;
      });
  }

    editEvent() {
      this.router.navigate([`edit/event/${this.eventModel.id}`]);
    }

    getFullStreet() {
      let fullStreet = `${this.eventModel.address.street} ${this.eventModel.address.buildingNumber}`
      if(this.eventModel.address.localNumber)
        fullStreet += `/${this.eventModel.address.localNumber}`;
        return fullStreet;
    }

    loadBoardGamesTable()
    {
      this.pubService.getPub(this.route.snapshot.params.id)
        .subscribe(res => {
            this.users = new MatTableDataSource<UserModel>((<EventModel> res).participants);
            this.users.paginator = this.paginator;
            this.users.sort = this.sort;
      })
    }

    applyFilter(event: Event) {
      this.users.filterPredicate = 
        (data: UserModel, filter: string) => {
          return data.userName.toLowerCase().includes(filter.toLowerCase())};
      const filterValue = (event.target as HTMLInputElement).value;
      this.users.filter = filterValue;
    }

    onBoardGameClick() {
      this.router.navigate([`show/boardgame/${this.eventModel.boardGame.id}`]);
    }

    onPubClick() {
      this.router.navigate([`show/pub/${this.eventModel.pub.id}`]);
    }

    getRate(rates: RateModel[]) {
      if(rates.length <=0 )
        return 0;
      var sum = 0;
      for (var i = 0; i < rates.length; i++) {
        sum += rates[i].userRate
      }
      return Math.round(sum/rates.length)
    }

    toggleCreatePubRate() {
      const pub = this.eventModel.pub;
      var dialogRef = this.dialog.open(PubRateDialogComponent, {
        width: "450px",
        data: {
          pub
        }
      });
  
      dialogRef.afterClosed().subscribe(() => {
        this.loadPub();
      }
      )
    }

    toggleCreateBoardGameRate() {
      const boardGame = this.eventModel.boardGame;
      var dialogRef = this.dialog.open(BoardGameRateDialogComponent, {
        width: "450px",
        data: {
          boardGame
        }
      });
  
      dialogRef.afterClosed().subscribe(() => {
        this.loadPub();
      }
      )
    }
  
    joinClicked() {
      if(!this.isAuthenticated)
        return;
        
      this.eventModel.isUserParticipant = !this.eventModel.isUserParticipant
      if(this.eventModel.isUserParticipant)
      {
        this.eventService.joinEvent(this.eventModel.id).subscribe();
      }
      else
      {
        this.eventService.quitEvent(this.eventModel.id).subscribe();
      }

      this.loadBoardGamesTable();
    }
  
    thumbPubClicked() {
      if(!this.isAuthenticated)
        return;
        
      this.eventModel.pub.isLikedByUser = !this.eventModel.pub.isLikedByUser
      if(this.eventModel.pub.isLikedByUser)
      {
        this.eventModel.pub.amountOfLikes += 1;
        this.pubService.likePub(this.eventModel.pub.id).subscribe();
      }
      else
      {
        this.pubService.unLikePub(this.eventModel.pub.id).subscribe();
        this.eventModel.pub.amountOfLikes -= 1;
      }
    }
  
    thumbBoardGameClicked() {
      if(!this.isAuthenticated)
        return;
        
      this.eventModel.boardGame.isLikedByUser = !this.eventModel.boardGame.isLikedByUser
      if(this.eventModel.boardGame.isLikedByUser)
      {
        this.eventModel.boardGame.amountOfLikes += 1;
        this.boardGameService.likeBoardGame(this.eventModel.boardGame.id).subscribe();
      }
      else
      {
        this.boardGameService.unLikeBoardGame(this.eventModel.boardGame.id).subscribe();
        this.eventModel.boardGame.amountOfLikes -= 1;
      }
    }

    showPub(pub: PubModel) {
      this.router.navigate([`show/pub/${pub.id}`]);
    }

    showBoardGame(boardGame: BoardGameModel) {
      this.router.navigate([`show/boardgame/${boardGame.id}`]);
    }

    mouseenter (event) {
      this.renderer2.addClass(event.target, 'mat-elevation-z5')
    }
    
    mouseleave (event) {
      this.renderer2.removeClass(event.target, 'mat-elevation-z5')
    }
  }