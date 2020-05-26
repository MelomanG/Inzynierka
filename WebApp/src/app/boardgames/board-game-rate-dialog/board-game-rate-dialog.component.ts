import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import { CreateRateModel, RateModel } from 'src/app/shared/models/rate';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { BoardGameService } from '../boardgame.service';
import { AuthenticationService } from 'src/app/authentication/authentication.service';
import { catchError, map} from 'rxjs/operators';
import { Observable } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-board-game-rate-dialog',
  templateUrl: './board-game-rate-dialog.component.html',
  styleUrls: ['./board-game-rate-dialog.component.scss']
})
export class BoardGameRateDialogComponent implements OnInit {
  rate: RateModel;
  bg: BoardGameModel;
  isAuthenticated: boolean;

  constructor(
    private authService: AuthenticationService,
    private boardGameService: BoardGameService,
    private dialogRef: MatDialogRef<BoardGameRateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public boardGame: any)
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

      this.boardGameService.getUserBoardGameRate(this.boardGame.boardGame.id)
        .subscribe(res => {
            this.rate = <RateModel>res;
        });
        
        this.bg = this.boardGame.boardGame
    }
  }
  
  ratingComponentClick($event) {
    this.rate.userRate = $event.rating;
  }

  save() {
    this.boardGameService.rateBoardGame(this.bg.id, this.rate).subscribe();
    this.dialogRef.close()
  }

  update() {
    this.boardGameService.updateBoardGameRate(this.bg.id, this.rate).subscribe();
    this.dialogRef.close()
  }

  dismiss() {
    this.dialogRef.close(null);
  }
}
