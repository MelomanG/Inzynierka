import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BoardgamesModule } from './boardgames/boardgames.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BoardGameService } from './boardgames/boardgame.service';
import { BoardGameCategoryService } from './boardgames/boardgamecategory.service';
import { SharedModule } from './shared/shared.module';
import { RegisterComponent } from './authentication/register/register.component';
import { AuthenticationService } from './authentication/authentication.service';
import { LoginComponent } from './authentication/login/login.component';
import { AuthenticationInterceptor } from './authentication/autnentication.interceptor';
import { PubsModule } from './pubs/pubs.module';
import { ShowUserEventsComponent } from './events/show-user-events/show-user-events.component';

@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    LoginComponent,
    ShowUserEventsComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    BoardgamesModule,
    PubsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    SharedModule
  ],
  providers: [
    BoardGameService, 
    BoardGameCategoryService,
    AuthenticationService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
