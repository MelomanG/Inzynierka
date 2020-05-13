import { Component, OnInit } from '@angular/core';
import { BoardGameService } from '../boardgame.service';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { PaginationResult } from 'src/app/shared/models/paginationresult';
import { Router } from '@angular/router';

@Component({
  selector: 'app-show-boardgames-list',
  templateUrl: './show-boardgames-list.component.html',
  styleUrls: ['./show-boardgames-list.component.scss']
})
export class ShowBoardgamesListComponent implements OnInit {

  boardGameList: PaginationResult<BoardGameModel>;

  constructor(
    private boardGameService: BoardGameService,
    private router: Router) { }

  ngOnInit() {
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
}
