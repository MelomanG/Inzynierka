import { Component, OnInit, ViewChild, Renderer2 } from '@angular/core';
import { PubModel } from 'src/app/shared/models/pub';
import { PubsService } from '../pubs.service';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AuthenticationService } from 'src/app/authentication/authentication.service';
import { MatDialog } from '@angular/material/dialog';
import { RateModel } from 'src/app/shared/models/rate';
import { PubRateDialogComponent } from '../pub-rate-dialog/pub-rate-dialog.component';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { BoardGameRateDialogComponent } from 'src/app/boardgames/board-game-rate-dialog/board-game-rate-dialog.component';

@Component({
  selector: 'app-show-pub',
  templateUrl: './show-pub.component.html',
  styleUrls: ['./show-pub.component.scss']
})
export class ShowPubComponent implements OnInit {
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  displayedColumns: string[] = ['name', 'category', 'raiting'];
  boardGames: MatTableDataSource<BoardGameModel>; 
  pub: PubModel;
  serverUrl: string;
  isAuthenticated: boolean;

  constructor(
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
    this.pubService.getPub(this.route.snapshot.params.id)
      .subscribe(res => {
        this.pub = <PubModel> res;
        this.boardGames = new MatTableDataSource<BoardGameModel>(this.pub.pubBoardGames);
        this.boardGames.paginator = this.paginator;
        this.boardGames.sort = this.sort;
      });
  }

    editPub(pub: PubModel) {
      this.router.navigate([`edit/pub/${pub.id}`]);
    }

    getFullStreet() {
      let fullStreet = `${this.pub.address.street} ${this.pub.address.buildingNumber}`
      if(this.pub.address.localNumber)
        fullStreet += `/${this.pub.address.localNumber}`;
        return fullStreet;
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

    loadBoardGamesTable()
    {
      this.pubService.getPub(this.route.snapshot.params.id)
        .subscribe(res => {
            this.boardGames = new MatTableDataSource<BoardGameModel>((<PubModel> res).pubBoardGames);
            this.boardGames.paginator = this.paginator;
            this.boardGames.sort = this.sort;
      })
    }

    applyFilter(event: Event) {
      this.boardGames.filterPredicate = 
        (data: BoardGameModel, filter: string) => {
          return data.name.toLowerCase().includes(filter.toLowerCase()) 
          || data.category.name.toLowerCase().includes(filter.toLowerCase())
        };
      const filterValue = (event.target as HTMLInputElement).value;
      this.boardGames.filter = filterValue;
    }

    onBoardGameClick(boardGame: BoardGameModel) {
      this.router.navigate([`show/boardgame/${boardGame.id}`]);
    }

    toggleCreatePubRate(pub: PubModel) {
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

    toggleCreateBoardGameRate(boardGame: BoardGameModel) {
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
  
    thumbClicked() {
      if(!this.isAuthenticated)
        return;
        
      this.pub.isLikedByUser = !this.pub.isLikedByUser
      if(this.pub.isLikedByUser)
      {
        this.pub.amountOfLikes += 1;
        this.pubService.likePub(this.pub.id).subscribe();
      }
      else
      {
        this.pubService.unLikePub(this.pub.id).subscribe();
        this.pub.amountOfLikes -= 1;
      }
    }
  }
