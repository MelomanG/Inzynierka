import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { AppRoutingModule } from '../app-routing.module';
import { SideNavComponent } from './components/side-nav/side-nav.component';
import { StarRatingComponent } from './components/star-rating/star-rating.component';
import { ShowOpinionsTabComponent } from './components/show-opinions-tab/show-opinions-tab.component';
import { PagerComponent } from './components/pager/pager.component';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { ContactsSidenavComponent } from './components/contacts-sidenav/contacts-sidenav.component';
import { AddContactDialogComponent } from './components/add-contact-dialog/add-contact-dialog.component';

@NgModule({
  declarations: [
  NavBarComponent,
  SideNavComponent,
  StarRatingComponent,
  ShowOpinionsTabComponent,
  PagerComponent,
  PagingHeaderComponent,
  ContactsSidenavComponent,
  AddContactDialogComponent,
],
  imports: [
    PaginationModule.forRoot(),
    CommonModule,
    FormsModule,
    MaterialModule,
    AppRoutingModule,
    ReactiveFormsModule
  ],
  exports: [
    PaginationModule,
    MaterialModule,
    AppRoutingModule,
    NavBarComponent,
    SideNavComponent,
    StarRatingComponent,
    PagerComponent,
    ShowOpinionsTabComponent,
    PagingHeaderComponent,
    ContactsSidenavComponent,
  ]
})
export class SharedModule { }
