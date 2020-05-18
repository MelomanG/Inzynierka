import { Component, OnInit } from '@angular/core';
import { BoardGameService } from '../boardgame.service';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { PaginationResult } from 'src/app/shared/models/paginationresult';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-show-boardgames-list',
  templateUrl: './show-boardgames-list.component.html',
  styleUrls: ['./show-boardgames-list.component.scss']
})
export class ShowBoardgamesListComponent implements OnInit {

  boardGameList: PaginationResult<BoardGameModel>;
  serverUrl: string;

  constructor(
    private boardGameService: BoardGameService,
    private router: Router) { }

  ngOnInit() {
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
}
