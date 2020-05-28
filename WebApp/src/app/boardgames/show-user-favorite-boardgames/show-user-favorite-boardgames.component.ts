import { Component, OnInit, Renderer2, ViewChild, ElementRef } from '@angular/core';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { BoardGameService } from '../boardgame.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { RateModel } from 'src/app/shared/models/rate';
import { BoardGameRateDialogComponent } from '../board-game-rate-dialog/board-game-rate-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { FormControl } from '@angular/forms';
import { startWith, map } from 'rxjs/operators';

@Component({
  selector: 'app-show-user-favorite-boardgames',
  templateUrl: './show-user-favorite-boardgames.component.html',
  styleUrls: ['./show-user-favorite-boardgames.component.scss']
})
export class ShowUserFavoriteBoardgamesComponent implements OnInit {
  searchControl = new FormControl();

  boardGameList: BoardGameModel[];
  filteredBoardGameList: Observable<BoardGameModel[]>;
  serverUrl: string;

  constructor(
    private boardGameService: BoardGameService,
    private dialog: MatDialog,
    private router: Router,
    private renderer2: Renderer2) { }

  ngOnInit() {
    this.serverUrl = environment.serverUrl;
    this.loadBoardGames();
    this.filterBoardGames();
  }

  loadBoardGames() {
    this.boardGameService.getLikedBoardGames().
      subscribe(res => {
        this.boardGameList = <BoardGameModel[]> res;
    });
  }

  filterBoardGames() {
    this.filteredBoardGameList = this.searchControl.valueChanges
      .pipe(
        startWith(''),
        map(value => this._filter(value))
      )
  }

  private _filter(value: string) {
    if(value.length < 2)
    {
      this.loadBoardGames();
      return this.boardGameList;
    }
    return this.boardGameList
      .filter(bg =>
        bg.name.toLowerCase().includes(value.toLowerCase())
        || bg.category.name.toLowerCase().includes(value.toLowerCase()) );
  }

  showBoardGame(boardgame: BoardGameModel) {
    this.router.navigate([`show/boardgame/${boardgame.id}`]);
  }

  thumbClicked(boardGame) {
    boardGame.isLikedByUser = !boardGame.isLikedByUser
    if(boardGame.isLikedByUser)
    {
      boardGame.amountOfLikes += 1;
      this.boardGameService.likeBoardGame(boardGame.id).subscribe(
        res => this.loadBoardGames()
      );
    }
    else
    {
      this.boardGameService.unLikeBoardGame(boardGame.id).subscribe(
        res => this.loadBoardGames()
      );
      boardGame.amountOfLikes -= 1;
    }
  }

  getBoardGameRate(rates: RateModel[]) {
    if(rates.length <=0 )
      return 0;
    var sum = 0;
    for (var i = 0; i < rates.length; i++) {
      sum += rates[i].userRate
    }
    return Math.round(sum/rates.length)
  }

  toggleCreateRate(boardGame: BoardGameModel) {
    var dialogRef = this.dialog.open(BoardGameRateDialogComponent, {
      width: "450px",
      data: {
        boardGame
      }
    });

    dialogRef.afterClosed().subscribe(() => {
      this.loadBoardGames();
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
