import { Component, OnInit, Renderer2 } from '@angular/core';
import { AuthenticationService } from 'src/app/authentication/authentication.service';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss']
})
export class SideNavComponent implements OnInit {
  isAdmin: boolean = null;

  constructor(
    private authService: AuthenticationService,
    private renderer2: Renderer2) { }

  ngOnInit(): void {
  }

  mouseenter (event) {
    this.renderer2.addClass(event.target, 'mat-elevation-z5')
 }
 
 mouseleave (event) {
   this.renderer2.removeClass(event.target, 'mat-elevation-z5')
 }
  
 onToggle(sidenav) {
  this.authService.isAdmin()
  .subscribe(res => {
    this.isAdmin = true;
    sidenav.open();
  }, err => {
    this.isAdmin = false
    sidenav.open();
  })
 }
}
