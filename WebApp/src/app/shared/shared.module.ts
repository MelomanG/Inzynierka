import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './material/material.module';
import { FormsModule } from '@angular/forms';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { AppRoutingModule } from '../app-routing.module';
import { SideNavComponent } from './components/side-nav/side-nav.component';
import { StarRatingComponent } from './components/star-rating/star-rating.component';
import { ShowOpinionsTabComponent } from './components/show-opinions-tab/show-opinions-tab.component';


@NgModule({
  declarations: [
  NavBarComponent,
  SideNavComponent,
  StarRatingComponent,
  ShowOpinionsTabComponent,
],
  imports: [
    CommonModule,
    FormsModule,
    MaterialModule,
    AppRoutingModule,
  ],
  exports: [
    MaterialModule,
    AppRoutingModule,
    NavBarComponent,
    SideNavComponent,
    StarRatingComponent,
    ShowOpinionsTabComponent
  ]
})
export class SharedModule { }
