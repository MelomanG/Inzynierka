import { Component, OnInit, ViewChild } from '@angular/core';
import { BoardGameService } from '../boardgame.service';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AuthenticationService } from 'src/app/authentication/authentication.service';
import { RateModel } from 'src/app/shared/models/rate';
import { BoardGameRateDialogComponent } from '../board-game-rate-dialog/board-game-rate-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { PubModel } from 'src/app/shared/models/pub';
import { PubRateDialogComponent } from 'src/app/pubs/pub-rate-dialog/pub-rate-dialog.component';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-show-boardgame',
  templateUrl: './show-boardgame.component.html',
  styleUrls: ['./show-boardgame.component.scss']
})

export class ShowBoardgameComponent implements OnInit {
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  displayedColumns: string[] = ['name', 'city', 'raiting'];

  boardGame: BoardGameModel;
  serverUrl: string;
  isAdmin: boolean;
  isAuthenticated: boolean;
  pubs: MatTableDataSource<PubModel>;

  constructor(
    private boardGameService: BoardGameService,
    private authService: AuthenticationService,
    private dialog: MatDialog,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.authService.isAdmin().subscribe(res => this.isAdmin = true, err => this.isAdmin = false);
    this.isAuthenticated = this.authService.isAuthenticated();
    this.serverUrl = environment.serverUrl;
    this.loadBoardGame();
    this.loadPubsWithBoardGame();
  }

  loadBoardGame(){
    this.boardGameService.getBoardGame(this.route.snapshot.params.id)
      .subscribe(res => {
        this.boardGame = <BoardGameModel> res;
      });
  }

  loadPubsWithBoardGame(){
    this.boardGameService.getPubsWithBoardGame(this.route.snapshot.params.id)
      .subscribe(res => {
        const data = <PubModel[]> res;
        this.pubs = new MatTableDataSource<PubModel>(data);
        this.pubs.paginator = this.paginator;
        this.pubs.sort = this.sort;
      });
  }

  editBoardGame(boardgame: BoardGameModel) {
    this.router.navigate([`edit/boardgame/${boardgame.id}`]);
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

  toggleCreateBoardGameRate(boardGame: BoardGameModel) {
    var dialogRef = this.dialog.open(BoardGameRateDialogComponent, {
      width: "450px",
      data: {
        boardGame
      }
    });

    dialogRef.afterClosed().subscribe(() => {
      this.loadBoardGame();
    }
    )
  }

  onPubClick(pub: PubModel) {
    this.router.navigate([`show/pub/${pub.id}`]);
  }

  toggleCreatePubRate(pub: PubModel) {
    var dialogRef = this.dialog.open(PubRateDialogComponent, {
      width: "450px",
      data: {
        pub
      }
    });

    dialogRef.afterClosed().subscribe(() => {
      this.loadBoardGame();
    }
    )
  }

  applyFilter(event: Event) {
    this.pubs.filterPredicate = 
      (data: PubModel, filter: string) => {
        return data.name.toLowerCase().includes(filter.toLowerCase()) 
        || data.address.city.toLowerCase().includes(filter.toLowerCase())
      };
    const filterValue = (event.target as HTMLInputElement).value;
    this.pubs.filter = filterValue;
  }

  thumbClicked() {
    if(!this.isAuthenticated)
      return;
      
    this.boardGame.isLikedByUser = !this.boardGame.isLikedByUser
    if(this.boardGame.isLikedByUser)
    {
      this.boardGame.amountOfLikes += 1;
      this.boardGameService.likeBoardGame(this.boardGame.id).subscribe();
    }
    else
    {
      this.boardGameService.unLikeBoardGame(this.boardGame.id).subscribe();
      this.boardGame.amountOfLikes -= 1;
    }
  }
}
