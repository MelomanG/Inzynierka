import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from '../app-routing.module';
import { CreatePubComponent } from './create-pub/create-pub.component';
import { ShowPubComponent } from './show-pub/show-pub.component';
import { EditPubComponent } from './edit-pub/edit-pub.component';
import { ShowPubsListComponent } from './show-pubs-list/show-pubs-list.component';
import { ShowUserPubsComponent } from './show-user-pubs/show-user-pubs.component';
import { ShowUserFavoritePubsComponent } from './show-user-favorite-pubs/show-user-favorite-pubs.component';



@NgModule({
  declarations: [
    CreatePubComponent,
    ShowPubComponent,
    EditPubComponent,
    ShowPubsListComponent,
    ShowUserPubsComponent,
    ShowUserFavoritePubsComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  exports: [
    CreatePubComponent,
    ShowPubComponent,
    EditPubComponent,
    ShowPubsListComponent,
    ShowUserPubsComponent,
    ShowUserFavoritePubsComponent
  ]
})
export class PubsModule { }
