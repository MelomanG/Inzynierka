import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateBoardgameComponent } from './boardgames/create-boardgame/create-boardgame.component';
import { ShowBoardgamesListComponent } from './boardgames/show-boardgames-list/show-boardgames-list.component';
import { ShowBoardgameComponent } from './boardgames/show-boardgame/show-boardgame.component';


const routes: Routes = [
  {path: "create/boardgame", component: CreateBoardgameComponent},
  {path: "show/boardgames", component: ShowBoardgamesListComponent},
  {path: "show/boardgame/:id", component: ShowBoardgameComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
