import { Component, OnInit } from '@angular/core';
import { PubModel } from 'src/app/shared/models/pub';
import { PubsService } from '../pubs.service';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AuthenticationService } from 'src/app/authentication/authentication.service';
import { MatDialog } from '@angular/material/dialog';
import { RateModel } from 'src/app/shared/models/rate';
import { PubRateDialogComponent } from '../pub-rate-dialog/pub-rate-dialog.component';

@Component({
  selector: 'app-show-pub',
  templateUrl: './show-pub.component.html',
  styleUrls: ['./show-pub.component.scss']
})
export class ShowPubComponent implements OnInit {
  pub: PubModel;
  serverUrl: string;
  isAuthenticated: boolean;

  constructor(
    private pubService: PubsService,
    private authService: AuthenticationService,
    private dialog: MatDialog,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.serverUrl = environment.serverUrl;
    this.loadPub();
  }

  loadPub(){
    this.pubService.getPub(this.route.snapshot.params.id)
      .subscribe(res => {
        this.pub = <PubModel> res;
      });
  }

    editPub(pub: PubModel) {
      this.router.navigate([`edit/pub/${pub.id}`]);
    }

    getFullStreet() {
      let fullStreet = `${this.pub.address.street} ${this.pub.address.buildingNumber}`
      if(this.pub.address.localNumber)
        fullStreet += `/${this.pub.address.localNumber}`;
        return fullStreet;
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
        this.loadPub();
      }
      )
    }
  
    thumbClicked() {
      if(!this.isAuthenticated)
        return;
        
      this.pub.isLikedByUser = !this.pub.isLikedByUser
      if(this.pub.isLikedByUser)
      {
        this.pub.amountOfLikes += 1;
        this.pubService.likePub(this.pub.id).subscribe();
      }
      else
      {
        this.pubService.unLikePub(this.pub.id).subscribe();
        this.pub.amountOfLikes -= 1;
      }
    }
  }
