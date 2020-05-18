import { Component, OnInit } from '@angular/core';
import { BoardGameService } from '../boardgame.service';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-show-boardgame',
  templateUrl: './show-boardgame.component.html',
  styleUrls: ['./show-boardgame.component.scss']
})
export class ShowBoardgameComponent implements OnInit {
  boardGame: BoardGameModel;
  serverUrl: string;

  constructor(
    private boardGameService: BoardGameService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
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

}
