import { Component, OnInit } from '@angular/core';
import { BoardGameService } from '../boardgame.service';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AuthenticationService } from 'src/app/authentication/authentication.service';
import { RateModel } from 'src/app/shared/models/rate';
import { BoardGameRateDialogComponent } from '../board-game-rate-dialog/board-game-rate-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-show-boardgame',
  templateUrl: './show-boardgame.component.html',
  styleUrls: ['./show-boardgame.component.scss']
})
export class ShowBoardgameComponent implements OnInit {
  boardGame: BoardGameModel;
  serverUrl: string;
  isAdmin: boolean;
  isAuthenticated: boolean;

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
  }

  loadBoardGame(){
    this.boardGameService.getBoardGame(this.route.snapshot.params.id)
      .subscribe(res => {
        this.boardGame = <BoardGameModel> res;
      });
  }

  editBoardGame(boardgame: BoardGameModel) {
    this.router.navigate([`edit/boardgame/${boardgame.id}`]);
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
      this.loadBoardGame();
    }
    )
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
