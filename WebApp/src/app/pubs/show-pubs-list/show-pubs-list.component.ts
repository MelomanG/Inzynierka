import { Component, OnInit, Renderer2 } from '@angular/core';
import { PaginationResult } from 'src/app/shared/models/paginationresult';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { PubModel } from 'src/app/shared/models/pub';
import { PubsService } from '../pubs.service';
import { AuthenticationService } from 'src/app/authentication/authentication.service';
import { MatDialog } from '@angular/material/dialog';
import { RateModel } from 'src/app/shared/models/rate';
import { PubRateDialogComponent } from '../pub-rate-dialog/pub-rate-dialog.component';

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
    private dialog: MatDialog,
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
    if(!this.isAuthenticated)
        return;

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

  getPubRate(rates: RateModel[]) {
    if(rates.length <=0 )
      return 0;
    var sum = 0;
    for (var i = 0; i < rates.length; i++) {
      sum += rates[i].userRate
    }
    return Math.round(sum/rates.length)
  }

  toggleCreateRate(pub: PubModel) {
    var dialogRef = this.dialog.open(PubRateDialogComponent, {
      width: "450px",
      data: {
        pub
      }
    });

    dialogRef.afterClosed().subscribe(() => {
      this.loadPubs();
    }
    )
  }

  mouseenter (event) {
    this.renderer2.addClass(event.target, 'mat-elevation-z5')
 }
 
 mouseleave (event) {
   this.renderer2.removeClass(event.target, 'mat-elevation-z5')
 }
}
