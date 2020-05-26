import { Component, OnInit, Inject } from '@angular/core';
import { PubsService } from '../pubs.service';
import { CreateRateModel, RateModel } from 'src/app/shared/models/rate';
import { PubModel } from 'src/app/shared/models/pub';
import { AuthenticationService } from 'src/app/authentication/authentication.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-pub-rate-dialog',
  templateUrl: './pub-rate-dialog.component.html',
  styleUrls: ['./pub-rate-dialog.component.scss']
})
export class PubRateDialogComponent implements OnInit {
  rate: RateModel;
  p: PubModel;
  isAuthenticated: boolean;

  constructor(
    private authService: AuthenticationService,
    private pubService: PubsService,
    private dialogRef: MatDialogRef<PubRateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public pub: any)
     { }

  ngOnInit(): void {
    this.isAuthenticated = this.authService.isAuthenticated();
    if(this.isAuthenticated)
    {
      this.rate = {
        id: null,
        userRate: 1,
        comment: "",
        userName: null
      }      
      
      this.pubService.getUserPubRate(this.pub.pub.id)
        .subscribe(res => {
            this.rate = <RateModel>res;
        });
      
      this.p = this.pub.pub
    }
  }
  
  ratingComponentClick($event) {
    this.rate.userRate = $event.rating;
  }

  save() {
    this.pubService.ratePub(this.p.id, this.rate).subscribe();
    this.dialogRef.close()
  }

  update() {
    this.pubService.updatePubRate(this.p.id, this.rate).subscribe();
    this.dialogRef.close()
  }

  dismiss() {
    this.dialogRef.close(null);
  }
}
