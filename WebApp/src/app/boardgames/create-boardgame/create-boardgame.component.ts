import { Component, OnInit } from '@angular/core';
import { BoardGameCategoryModel } from 'src/app/shared/models/boardgamecategory';
import { CreateBoardGameModel } from 'src/app/shared/models/boardgame';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BoardGameService } from '../boardgame.service';
import { BoardGameCategoryService } from '../boardgamecategory.service';

@Component({
  selector: 'app-create-boardgame',
  templateUrl: './create-boardgame.component.html',
  styleUrls: ['./create-boardgame.component.scss']
})

export class CreateBoardgameComponent implements OnInit {

    constructor(private fb: FormBuilder, private router: Router, 
      private boardGameService: BoardGameService, private boardGameCategoryService: BoardGameCategoryService) { }

    boardGamesCategories: BoardGameCategoryModel[]; 
    formGroup: FormGroup;
    showError: boolean = false;

    ngOnInit() {
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
        minPlayers:[null],
        maxPlayers:[null],
        fromAge:[null],
        categoryId:[null]
      });
    }

    onSubmit(boardGame: CreateBoardGameModel) {
      this.boardGameService.createBoardGame(boardGame);
    }
}
