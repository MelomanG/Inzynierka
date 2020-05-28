import { Component, OnInit, Renderer2 } from '@angular/core';
import { PubModel } from 'src/app/shared/models/pub';
import { PubsService } from '../pubs.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { MatDialog } from '@angular/material/dialog';
import { RateModel } from 'src/app/shared/models/rate';
import { PubRateDialogComponent } from '../pub-rate-dialog/pub-rate-dialog.component';
import { FormControl } from '@angular/forms';
import { startWith, map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-show-user-favorite-pubs',
  templateUrl: './show-user-favorite-pubs.component.html',
  styleUrls: ['./show-user-favorite-pubs.component.scss']
})
export class ShowUserFavoritePubsComponent implements OnInit {
  searchControl = new FormControl();

  pubList: PubModel[];
  filteredPubList: Observable<PubModel[]>;
  serverUrl: string;

  constructor(
    private pubService: PubsService,
    private dialog: MatDialog,
    private router: Router,
    private renderer2: Renderer2) { }

  ngOnInit() {
    this.serverUrl = environment.serverUrl;
    this.loadPubs();
    this.filterPubs();
  }

  loadPubs() {
    this.pubService.getLikedPubs().
      subscribe(res => {
        this.pubList = <PubModel[]> res;
    });
  }

  filterPubs() {
    this.filteredPubList = this.searchControl.valueChanges
      .pipe(
        startWith(''),
        map(value => this._filter(value))
      )
  }

  private _filter(value: string) {
    if(value.length < 2)
    {
      this.loadPubs();
      return this.pubList;
    }
    return this.pubList
      .filter(bg =>
        bg.name.toLowerCase().includes(value.toLowerCase())
        || bg.address.city.toLowerCase().includes(value.toLowerCase()) );
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

