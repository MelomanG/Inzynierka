import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { RegisterComponent } from './register/register.component';
import { AuthInterceptor } from "./services/auth/auth.interceptor";
import { AuthService } from "./services/auth/auth.service";

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { LayoutModule } from '@angular/cdk/layout';

import { MatSidenavModule, MatIconModule, MatButtonModule, MatInputModule, MatCardModule, MatListModule, MatToolbarModule, MatRadioModule } from '@angular/material';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from "@angular/router";
const routers = [
  // { path: "", component: HomeComponent },
  // { path: "question", component: QuestionComponent },
  // { path: "question/:quizId", component: QuestionComponent },
  { path: "register", component: RegisterComponent },
]

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    LayoutModule,
    MatToolbarModule, MatButtonModule, MatSidenavModule, MatIconModule, MatListModule, MatInputModule, MatCardModule, MatRadioModule,
    HttpClientModule,
    FormsModule, ReactiveFormsModule,
    RouterModule.forRoot(routers),
  ],
  providers: [
    AuthService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi:true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
