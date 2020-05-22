import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './material/material.module';
import { FormsModule } from '@angular/forms';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { AppRoutingModule } from '../app-routing.module';
import { SideNavComponent } from './components/side-nav/side-nav.component';



@NgModule({
  declarations: [
  NavBarComponent,
  SideNavComponent,
],
  imports: [
    CommonModule,
    FormsModule,
    MaterialModule,
    AppRoutingModule
  ],
  exports: [
    MaterialModule,
    AppRoutingModule,
    NavBarComponent,
    SideNavComponent
  ]
})
export class SharedModule { }
