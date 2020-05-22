import { Component, OnInit, Renderer2 } from '@angular/core';
import { BoardGameService } from '../boardgame.service';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { PaginationResult } from 'src/app/shared/models/paginationresult';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AuthenticationService } from 'src/app/authentication/authentication.service';

@Component({
  selector: 'app-show-boardgames-list',
  templateUrl: './show-boardgames-list.component.html',
  styleUrls: ['./show-boardgames-list.component.scss']
})
export class ShowBoardgamesListComponent implements OnInit {

  boardGameList: PaginationResult<BoardGameModel>;
  serverUrl: string;
  isAuthenticated: boolean;

  constructor(
    private boardGameService: BoardGameService,
    private authService: AuthenticationService,
    private router: Router,
    private renderer2: Renderer2) { }

  ngOnInit() {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.serverUrl = environment.serverUrl;
    this.loadBoardGames();
  }

  loadBoardGames() {
    this.boardGameService.getBoardGames().
      subscribe(res => {
        this.boardGameList = <PaginationResult<BoardGameModel>> res;
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
      this.boardGameService.likeBoardGame(boardGame.id).subscribe();
    }
    else
    {
      this.boardGameService.unLikeBoardGame(boardGame.id).subscribe();
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
