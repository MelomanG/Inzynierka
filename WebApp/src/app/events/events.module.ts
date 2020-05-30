import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { SharedModule } from '../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from '../app-routing.module';
import { MatSortModule } from '@angular/material/sort';
import { ShowEventComponent } from './show-event/show-event.component';
import { EditEventComponent } from './edit-event/edit-event.component';
import { ShowEventsComponent } from './show-events/show-events.component';
import { ShowUserEventsComponent } from './show-user-events/show-user-events.component';
import { CreateEventComponent } from './create-event/create-event.component';
import { MatInputModule } from '@angular/material/input';



@NgModule({
  declarations: [
    ShowEventComponent,
    EditEventComponent,
    ShowEventsComponent,
    ShowUserEventsComponent,
    CreateEventComponent
  ],
  imports: [
    PaginationModule.forRoot(),
    SharedModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    MatSortModule,
    MatInputModule 
  ],
  exports: [
    ShowEventComponent,
    EditEventComponent,
    ShowEventsComponent,
    ShowUserEventsComponent
  ]
})
export class EventsModule { }
