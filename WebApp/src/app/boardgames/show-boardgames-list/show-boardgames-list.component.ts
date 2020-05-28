import { Component, OnInit, Renderer2, ViewChild, ElementRef } from '@angular/core';
import { BoardGameService } from '../boardgame.service';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { PaginationResult } from 'src/app/shared/models/paginationresult';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AuthenticationService } from 'src/app/authentication/authentication.service';
import { RateModel } from 'src/app/shared/models/rate';
import { MatDialog } from '@angular/material/dialog';
import { BoardGameRateDialogComponent } from '../board-game-rate-dialog/board-game-rate-dialog.component';
import { BoardGameQuery } from 'src/app/shared/models/query';
import { BoardGameCategoryService } from '../boardgamecategory.service';
import { BoardGameCategoryModel } from 'src/app/shared/models/boardgamecategory';
import { Observable } from 'rxjs';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-show-boardgames-list',
  templateUrl: './show-boardgames-list.component.html',
  styleUrls: ['./show-boardgames-list.component.scss']
})
export class ShowBoardgamesListComponent implements OnInit {
  serverUrl: string;
  isAuthenticated: boolean;
  sortOptions = [
    { name: 'Nazwa A-Z', value: 'name' },
    { name: 'Nazwa Z-A', value: 'name desc'},
    { name: 'Ocena rosnąco', value: 'rate' },
    { name: 'Ocena malejaco', value: 'rate desc' },
    { name: 'Popularne', value: 'like desc' },
    { name: 'Mało popularne', value: 'like' }
  ];
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  
  minPlayers = new FormControl();
  maxPlayers = new FormControl();
  minAge = new FormControl();
  maxAge = new FormControl();

  boardGameList: PaginationResult<BoardGameModel>;
  boardGameQuery: BoardGameQuery;

  boardGameCategories: BoardGameCategoryModel[];

  constructor(
    private boardGameService: BoardGameService,
    private boardGameCategoryService: BoardGameCategoryService,
    private authService: AuthenticationService,
    private dialog: MatDialog,
    private router: Router,
    private renderer2: Renderer2) {
      this.boardGameQuery = new BoardGameQuery()
      this.boardGameService.setBoardGameQuery(this.boardGameQuery);
     }

  ngOnInit() {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.serverUrl = environment.serverUrl;
    this.loadBoardGameCategories();
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

  getBoardGameRate(rates: RateModel[]) {
    if(rates.length <=0 )
      return 0;
    var sum = 0;
    for (var i = 0; i < rates.length; i++) {
      sum += rates[i].userRate
    }
    return Math.round(sum/rates.length)
  }

  loadBoardGameCategories() {
    this.boardGameCategoryService.getBoardGameCategories()
      .subscribe(res => {
        this.boardGameCategories = <BoardGameCategoryModel[]>res
      });
    }
  
  onPageChanged(event: any) {
    const query = this.boardGameService.getBoardGameQuery();
    if (query.page !== event)
    {
      query.page = event;
       this.boardGameService.setBoardGameQuery(query);
       this.loadBoardGames();
    }
   }

   onApplyFilters(){
      const query = this.boardGameService.getBoardGameQuery();
      query.minPlayers = parseInt(this.minPlayers.value, 10);
      query.maxPlayers = parseInt(this.maxPlayers.value, 10);
      query.minAge = parseInt(this.minAge.value, 10);
      query.maxAge = parseInt(this.maxAge.value, 10);
      console.log(query);
      this.boardGameService.setBoardGameQuery(query);
      this.loadBoardGames();
   }

   onSearch() {
     const query = this.boardGameService.getBoardGameQuery();
     query.search = this.searchTerm.nativeElement.value;
     query.page = 1;
     this.boardGameService.setBoardGameQuery(query);
     this.loadBoardGames();
   }

   onSortChange(value) {
     const query = this.boardGameService.getBoardGameQuery();
     query.sort = value;
     this.boardGameService.setBoardGameQuery(query);
     this.loadBoardGames();
   }

   onBoardGameCategorySelected(categoryId) {
     console.log(categoryId)
    const query = this.boardGameService.getBoardGameQuery();
    query.category = categoryId;
    this.boardGameService.setBoardGameQuery(query);
    this.loadBoardGames();
   }
 
   onReset() {
     this.searchTerm.nativeElement.value = '';      
     this.minPlayers.setValue('');
     this.maxPlayers.setValue('');
     this.minAge.setValue('');
     this.maxAge.setValue('');
     this.boardGameQuery = new BoardGameQuery();
     this.boardGameService.setBoardGameQuery(this.boardGameQuery);
     this.loadBoardGames();
   }

   thumbClicked(boardGame) {
     if(!this.isAuthenticated)
       return;
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
