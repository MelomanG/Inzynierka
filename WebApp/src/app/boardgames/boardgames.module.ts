import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateBoardgameComponent } from './create-boardgame/create-boardgame.component';
import { MaterialModule } from '../shared/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ShowBoardgameComponent } from './show-boardgame/show-boardgame.component';
import { ShowBoardgamesListComponent } from './show-boardgames-list/show-boardgames-list.component';


@NgModule({
  declarations: [CreateBoardgameComponent, ShowBoardgameComponent, ShowBoardgamesListComponent],
  imports: [
    MaterialModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  exports:[
    CreateBoardgameComponent,
    ShowBoardgameComponent,
    ShowBoardgamesListComponent
  ]
})
export class BoardgamesModule { }
