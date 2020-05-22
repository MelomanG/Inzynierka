import { Component, OnInit, Renderer2 } from '@angular/core';
import { PaginationResult } from 'src/app/shared/models/paginationresult';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { PubModel } from 'src/app/shared/models/pub';
import { PubsService } from '../pubs.service';
import { AuthenticationService } from 'src/app/authentication/authentication.service';

@Component({
  selector: 'app-show-pubs-list',
  templateUrl: './show-pubs-list.component.html',
  styleUrls: ['./show-pubs-list.component.scss']
})
export class ShowPubsListComponent implements OnInit {

  pubList: PaginationResult<PubModel>;
  serverUrl: string;
  isAuthenticated: any;

  constructor(
    private pubService: PubsService,
    private authService: AuthenticationService,
    private router: Router,
    private renderer2: Renderer2) { }

  ngOnInit() {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.serverUrl = environment.serverUrl;
    this.loadPubs();
  }

  loadPubs() {
    this.pubService.getPubs().
      subscribe(res => {
        this.pubList = <PaginationResult<PubModel>> res;
    });
  }

  showPub(pub: PubModel) {
    this.router.navigate([`show/pub/${pub.id}`]);
  }

  thumbClicked(pub) {
    pub.isLikedByUser = !pub.isLikedByUser
    if(pub.isLikedByUser)
    {
      pub.amountOfLikes += 1;
      this.pubService.likePub(pub.id).subscribe();
    }
    else
    {
      this.pubService.unLikePub(pub.id).subscribe();
      pub.amountOfLikes -= 1;
    }
  }

  mouseenter (event) {
    this.renderer2.addClass(event.target, 'mat-elevation-z5')
 }
 
 mouseleave (event) {
   this.renderer2.removeClass(event.target, 'mat-elevation-z5')
 }
}
