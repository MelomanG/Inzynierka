import { Component, OnInit, Renderer2 } from '@angular/core';
import { PaginationResult } from 'src/app/shared/models/paginationresult';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { BoardGameService } from '../boardgame.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-show-user-favorite-boardgames',
  templateUrl: './show-user-favorite-boardgames.component.html',
  styleUrls: ['./show-user-favorite-boardgames.component.scss']
})
export class ShowUserFavoriteBoardgamesComponent implements OnInit {

  boardGameList: BoardGameModel[];
  serverUrl: string;

  constructor(
    private boardGameService: BoardGameService,
    private router: Router,
    private renderer2: Renderer2) { }

  ngOnInit() {
    this.serverUrl = environment.serverUrl;
    this.loadBoardGames();
  }

  loadBoardGames() {
    this.boardGameService.getLikedBoardGames().
      subscribe(res => {
        this.boardGameList = <BoardGameModel[]> res;
    });
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
  mouseenter (event) {
    this.renderer2.addClass(event.target, 'mat-elevation-z5')
 }
 
 mouseleave (event) {
   this.renderer2.removeClass(event.target, 'mat-elevation-z5')
 }
}
