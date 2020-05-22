import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CreateBoardgameComponent } from './boardgames/create-boardgame/create-boardgame.component';
import { ShowBoardgamesListComponent } from './boardgames/show-boardgames-list/show-boardgames-list.component';
import { ShowBoardgameComponent } from './boardgames/show-boardgame/show-boardgame.component';
import { EditBoardgameComponent } from './boardgames/edit-boardgame/edit-boardgame.component';
import { RegisterComponent } from './authentication/register/register.component';
import { LoginComponent } from './authentication/login/login.component';
import { CreatePubComponent } from './pubs/create-pub/create-pub.component';
import { ShowPubComponent } from './pubs/show-pub/show-pub.component';
import { EditPubComponent } from './pubs/edit-pub/edit-pub.component';
import { ShowPubsListComponent } from './pubs/show-pubs-list/show-pubs-list.component';
import { ShowUserPubsComponent } from './pubs/show-user-pubs/show-user-pubs.component';
import { ShowUserFavoritePubsComponent } from './pubs/show-user-favorite-pubs/show-user-favorite-pubs.component';
import { ShowUserFavoriteBoardgamesComponent } from './boardgames/show-user-favorite-boardgames/show-user-favorite-boardgames.component';
import { CreateBoardgameCategoryComponent } from './boardgames/create-boardgame-category/create-boardgame-category.component';
import { ShowUserEventsComponent } from './events/show-user-events/show-user-events.component';


const routes: Routes = [
  {path: "", component: ShowBoardgamesListComponent},
  {path: "home", component: ShowBoardgamesListComponent},
  {path: "login", component: LoginComponent},
  {path: "register", component: RegisterComponent},
  {path: "create/boardgame", component: CreateBoardgameComponent},
  {path: "show/boardgames", component: ShowBoardgamesListComponent},
  {path: "user/favorite/boardgames", component: ShowUserFavoriteBoardgamesComponent},
  {path: "show/boardgame/:id", component: ShowBoardgameComponent},
  {path: "edit/boardgame/:id", component: EditBoardgameComponent},
  {path: "create/boardgamecategory", component: CreateBoardgameCategoryComponent},
  {path: "create/pub", component: CreatePubComponent},
  {path: "show/pubs", component: ShowPubsListComponent},
  {path: "user/pubs", component: ShowUserPubsComponent},
  {path: "user/favorite/pubs", component: ShowUserFavoritePubsComponent},
  {path: "user/events", component: ShowUserEventsComponent},
  {path: "show/pub/:id", component: ShowPubComponent},
  {path: "edit/pub/:id", component: EditPubComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
