import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateBoardgameComponent } from './create-boardgame/create-boardgame.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ShowBoardgameComponent } from './show-boardgame/show-boardgame.component';
import { ShowBoardgamesListComponent } from './show-boardgames-list/show-boardgames-list.component';
import { EditBoardgameComponent } from './edit-boardgame/edit-boardgame.component';
import { SharedModule } from '../shared/shared.module';
import { AppRoutingModule } from '../app-routing.module';
import { ShowUserFavoriteBoardgamesComponent } from './show-user-favorite-boardgames/show-user-favorite-boardgames.component';
import { CreateBoardgameCategoryComponent } from './create-boardgame-category/create-boardgame-category.component';
import { BoardGameRateDialogComponent } from './board-game-rate-dialog/board-game-rate-dialog.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@NgModule({
  declarations: [
    CreateBoardgameComponent, 
    ShowBoardgameComponent, 
    ShowBoardgamesListComponent, 
    EditBoardgameComponent,
    ShowUserFavoriteBoardgamesComponent,
    CreateBoardgameCategoryComponent,
    BoardGameRateDialogComponent
  ],
  imports: [
    PaginationModule.forRoot(),
    SharedModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
  ],
  exports:[
    CreateBoardgameComponent,
    ShowBoardgameComponent,
    ShowBoardgamesListComponent,
    EditBoardgameComponent,
    ShowUserFavoriteBoardgamesComponent,
    CreateBoardgameCategoryComponent,
    BoardGameRateDialogComponent
  ]
})
export class BoardgamesModule { }
