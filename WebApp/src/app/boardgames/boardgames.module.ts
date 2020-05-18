import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateBoardgameComponent } from './create-boardgame/create-boardgame.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ShowBoardgameComponent } from './show-boardgame/show-boardgame.component';
import { ShowBoardgamesListComponent } from './show-boardgames-list/show-boardgames-list.component';
import { EditBoardgameComponent } from './edit-boardgame/edit-boardgame.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    CreateBoardgameComponent, 
    ShowBoardgameComponent, 
    ShowBoardgamesListComponent, 
    EditBoardgameComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  exports:[
    CreateBoardgameComponent,
    ShowBoardgameComponent,
    ShowBoardgamesListComponent,
    EditBoardgameComponent
  ]
})
export class BoardgamesModule { }
