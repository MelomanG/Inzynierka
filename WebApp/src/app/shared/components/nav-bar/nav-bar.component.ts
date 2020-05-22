import { Component, OnInit, Output, EventEmitter, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/authentication/authentication.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {

  @Output() toggleSidenav = new EventEmitter();
  
  constructor(
    private authService: AuthenticationService,
    private router: Router,
    private renderer2: Renderer2)
    { }

  ngOnInit(): void {
  }

  logout() {
    this.authService.logout();
    this.router.navigate([`home`]);
  }

  isAuthenticated() {
    return this.authService.isAuthenticated()
  }

  onMenuClicked() {
    this.toggleSidenav.emit();
  }

  mouseenter (event) {
    this.renderer2.addClass(event.target, 'mat-elevation-z5')
 }
 
 mouseleave (event) {
   this.renderer2.removeClass(event.target, 'mat-elevation-z5')
 }
}
