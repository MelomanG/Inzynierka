import { Component, OnInit } from '@angular/core';
import { PubModel } from 'src/app/shared/models/pub';
import { PubsService } from '../pubs.service';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AuthenticationService } from 'src/app/authentication/authentication.service';

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

    thumbClicked() {
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
