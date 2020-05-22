import { Component, OnInit, ViewChild } from '@angular/core';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { BoardGameService } from '../boardgame.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { BoardGameCategoryService } from '../boardgamecategory.service';
import { BoardGameCategoryModel } from 'src/app/shared/models/boardgamecategory';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-edit-boardgame',
  templateUrl: './edit-boardgame.component.html',
  styleUrls: ['./edit-boardgame.component.scss']
})
export class EditBoardgameComponent implements OnInit {

  boardGame: BoardGameModel;
  formGroup: FormGroup;
  boardGamesCategories: BoardGameCategoryModel[]; 
  @ViewChild('Image')
  Image;
  fileToUpload: File;
  imageUrl: any;

  constructor(
    private fb: FormBuilder,
    private boardGameService: BoardGameService,
    private boardGameCategoryService: BoardGameCategoryService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.createForm();
  }

  createForm(){
    this.boardGameService.getBoardGame(this.route.snapshot.params.id)
      .subscribe(res => {
        this.boardGame = <BoardGameModel> res;
        this.imageUrl = `${environment.serverUrl}/${this.boardGame.imagePath}`;
        this.boardGameCategoryService.getBoardGameCategories()
          .subscribe(res =>{
              this.boardGamesCategories = <BoardGameCategoryModel[]>res;
              this.formGroup = this.fb.group({
                id:[this.boardGame.id],
                name:[this.boardGame.name, Validators.required],
                description:[this.boardGame.description, Validators.compose([Validators.required, Validators.minLength(30), Validators.maxLength(1000)])],
                minPlayers:[this.boardGame.minPlayers, Validators.required],
                maxPlayers:[this.boardGame.maxPlayers, Validators.required],
                fromAge:[this.boardGame.fromAge, Validators.required],
                categoryId:[this.boardGame.categoryId, Validators.required],
                imagePath:[this.boardGame.imagePath]
              });
            });
      });
  }

  onSubmit() {
    this.boardGameService.updateBoardGame(this.formGroup.value, this.fileToUpload)
      .subscribe(data => {
        this.router.navigate([`show/boardgame/${this.formGroup.value.id}`]);
    })
  }

  deleteBoardGame() {
    this.boardGameService.deleteBoardGame(this.boardGame.id)
      .subscribe(res => {
        this.router.navigate([`show/boardgames`]);
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