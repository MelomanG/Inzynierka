import { Component, OnInit, Renderer2 } from '@angular/core';
import { PubModel } from 'src/app/shared/models/pub';
import { PaginationResult } from 'src/app/shared/models/paginationresult';
import { PubsService } from '../pubs.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-show-user-favorite-pubs',
  templateUrl: './show-user-favorite-pubs.component.html',
  styleUrls: ['./show-user-favorite-pubs.component.scss']
})
export class ShowUserFavoritePubsComponent implements OnInit {

  pubList: PubModel[];
  serverUrl: string;

  constructor(
    private pubService: PubsService,
    private router: Router,
    private renderer2: Renderer2) { }

  ngOnInit() {
    this.serverUrl = environment.serverUrl;
    this.loadPubs();
  }

  loadPubs() {
    this.pubService.getLikedPubs().
      subscribe(res => {
        this.pubList = <PubModel[]> res;
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
      this.pubService.likePub(pub.id).subscribe(() => this.loadPubs());
    }
    else
    {
      this.pubService.unLikePub(pub.id).subscribe(() => this.loadPubs());
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

