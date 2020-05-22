import { Component, OnInit, ViewChild } from '@angular/core';
import { BoardGameCategoryModel } from 'src/app/shared/models/boardgamecategory';
import { CreateBoardGameModel, BoardGameModel } from 'src/app/shared/models/boardgame';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BoardGameService } from '../boardgame.service';
import { BoardGameCategoryService } from '../boardgamecategory.service';
import { AuthenticationService } from 'src/app/authentication/authentication.service';

@Component({
  selector: 'app-create-boardgame',
  templateUrl: './create-boardgame.component.html',
  styleUrls: ['./create-boardgame.component.scss']
})

export class CreateBoardgameComponent implements OnInit {
    isAdmin: boolean = null;

    boardGamesCategories: BoardGameCategoryModel[]; 
    formGroup: FormGroup;
    showError: boolean = false;

    imageUrl: string = "assets/images/default-image.png";
    @ViewChild('Image')
    Image;
    fileToUpload: File;

    constructor(
      private fb: FormBuilder,
      private router: Router, 
      private boardGameService: BoardGameService,
      private boardGameCategoryService: BoardGameCategoryService,
      private authService: AuthenticationService) { }

    ngOnInit() {
      this.authService.isAdmin().subscribe(res => this.isAdmin = true, err => this.isAdmin = false);
      this.createForm();
      this.loadBardGameCategories();
    }
    
    loadBardGameCategories() {
        this.boardGameCategoryService.getBoardGameCategories()
          .subscribe(res =>
             this.boardGamesCategories = <BoardGameCategoryModel[]>res);
    }

    createForm(){
      this.formGroup = this.fb.group({
        name:[null, Validators.required],
        description:[null, Validators.compose([Validators.required, Validators.minLength(30), Validators.maxLength(1000)])],
        minPlayers:[null, Validators.required],
        maxPlayers:[null, Validators.required],
        fromAge:[null, Validators.required],
        categoryId:[null, Validators.required]
      });
    }

    onSubmit(boardGame: CreateBoardGameModel) {
      this.boardGameService.createBoardGame(boardGame, this.fileToUpload)
        .subscribe(data => {
          this.router.navigate([`show/boardgame/${(<BoardGameModel>data).id}`]);
      })
    }

    onClickFileInputButton(): void {
      this.Image.nativeElement.click();
    }
  
    handleFileInput(file: FileList) {
      this.fileToUpload = file.item(0);

      var reader = new FileReader();
      reader.onload = (event: any) => {
        this.imageUrl = event.target.result;
      }
      reader.readAsDataURL(this.fileToUpload);
    }
}
