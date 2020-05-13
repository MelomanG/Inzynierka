import { Component, OnInit } from '@angular/core';
import { BoardGameService } from '../boardgame.service';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-show-boardgame',
  templateUrl: './show-boardgame.component.html',
  styleUrls: ['./show-boardgame.component.scss']
})
export class ShowBoardgameComponent implements OnInit {

  boardGame: BoardGameModel;

  constructor(private boardGameService: BoardGameService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.loadBoardGame();
  }

  loadBoardGame(){
    this.boardGameService.getBoardGame(this.route.snapshot.params.id)
      .subscribe(res => {
        this.boardGame = <BoardGameModel> res;
        console.log(this.boardGame);
      });
  }
}
