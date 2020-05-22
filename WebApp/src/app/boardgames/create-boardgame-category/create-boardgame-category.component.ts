import { Component, OnInit, ViewChild } from '@angular/core';
import { BoardGameCategoryModel, CreateBoardGameCategoryModel } from 'src/app/shared/models/boardgamecategory';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BoardGameCategoryService } from '../boardgamecategory.service';
import { AuthenticationService } from 'src/app/authentication/authentication.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-create-boardgame-category',
  templateUrl: './create-boardgame-category.component.html',
  styleUrls: ['./create-boardgame-category.component.scss']
})
export class CreateBoardgameCategoryComponent implements OnInit {
  isAdmin: boolean = null;

  editTitle: string;

  displayedColumns: string[] = ['name', 'actions'];
  boardGamesCategories: MatTableDataSource<BoardGameCategoryModel>; 
  formGroup: FormGroup;
  showError: boolean = false;

  toEditBoardGameCategory: BoardGameCategoryModel;

  constructor(
    private fb: FormBuilder,
    private boardGameCategoryService: BoardGameCategoryService,
    private authService: AuthenticationService) { }
    
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  ngOnInit() {
    this.authService.isAdmin().subscribe(res => this.isAdmin = true, err => this.isAdmin = false);
    this.createForm();
    this.loadBardGameCategories();
  }
  
  loadBardGameCategories() {
      this.boardGameCategoryService.getBoardGameCategories()
        .subscribe(res =>{
          const data = <BoardGameCategoryModel[]>res;
          this.boardGamesCategories = new MatTableDataSource<BoardGameCategoryModel>(data);
          this.boardGamesCategories.paginator = this.paginator;
          this.boardGamesCategories.sort = this.sort;
        });
  }

  createForm(){
    this.formGroup = this.fb.group({
      id:[this.toEditBoardGameCategory?.id],
      name:[this.toEditBoardGameCategory?.name, Validators.required]
    });
  }

  onSubmit(category: CreateBoardGameCategoryModel) {
    this.boardGameCategoryService.createCategory(category)
      .subscribe(() => {
        this.loadBardGameCategories();
        this.cancel();
      });
    }

    editMode(row) {
      this.toEditBoardGameCategory = row;
      this.createForm();
      this.createEditTitle(row.name);
    }

    edit() {
      this.boardGameCategoryService.updateCategory(this.formGroup.value)
      .subscribe(() => {
        this.loadBardGameCategories();
        this.cancel();
      });
    }

    delete(row) {
      this.boardGameCategoryService.deleteCategory(row.id)
        .subscribe(() => this.loadBardGameCategories());
    }

    cancel() {
      this.toEditBoardGameCategory = null;
      this.createForm();
    }

    createEditTitle(name) {
      this.editTitle = `Edytujesz kategorie: ${name}`;
    }
}
