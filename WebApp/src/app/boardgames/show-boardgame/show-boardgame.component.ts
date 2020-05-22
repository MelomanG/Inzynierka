import { Component, OnInit } from '@angular/core';
import { BoardGameService } from '../boardgame.service';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AuthenticationService } from 'src/app/authentication/authentication.service';

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

  thumbClicked() {
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
