import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-star-rating',
  templateUrl: './star-rating.component.html',
  styleUrls: ['./star-rating.component.scss']
})
export class StarRatingComponent implements OnInit {
  @Input() rating: number;
  @Input() amountOfRates: number = 0;
  @Input() disabled: boolean = false;
  @Input() disableAmmount: boolean = false;
  @Input('starCount') starCount: number = 5;
  @Input('color') color: string = 'accent';
  @Input() itemId: number;
  @Output() ratingClick: EventEmitter<any> = new EventEmitter<any>();
  
  ratingArr = [];

  constructor() {
  }


  ngOnInit() {
    for (let index = 0; index < this.starCount; index++) {
      this.ratingArr.push(index);
    }
  }

  onClick(rating: number): void {
    this.rating = rating;

    this.ratingClick.emit({
      itemId: this.itemId,
      rating: rating
    });
  }

  showIcon(index:number) {
    if (this.rating >= index + 1) {
      return 'star';
    } else {
      return 'star_border';
    }
  }

}
export enum StarRatingColor {
  primary = "primary",
  accent = "accent",
  warn = "warn"
}