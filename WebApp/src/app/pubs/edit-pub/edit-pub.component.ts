import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { PubsService } from '../pubs.service';
import { environment } from 'src/environments/environment';
import { PubModel } from 'src/app/shared/models/pub';
import { MatTableDataSource } from '@angular/material/table';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { BoardGameService } from 'src/app/boardgames/boardgame.service';
import { BoardGameQuery } from 'src/app/shared/models/query';
import { PaginationResult } from 'src/app/shared/models/paginationresult';

@Component({
  selector: 'app-edit-pub',
  templateUrl: './edit-pub.component.html',
  styleUrls: ['./edit-pub.component.scss']
})
export class EditPubComponent implements OnInit {
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  displayedColumns: string[] = ['name', 'category','actions'];
  boardGames: MatTableDataSource<BoardGameModel>; 
  addButtonDisabled: boolean = true;
  loadedBoardGames: BoardGameModel[];
  filteredBoardGames: Observable<BoardGameModel[]>;
  boardGameControl = new FormControl();
  boardGameQuery: BoardGameQuery;

  pub: PubModel;
  imageUrl: string;
  fileToUpload: File;
  formGroup: FormGroup;
  
  @ViewChild('Image')
  Image;
  boardGameToAdd: BoardGameModel;
  
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private pubService: PubsService,
    private boardGameService: BoardGameService) {
      this.boardGameQuery = this.boardGameService.getBoardGameQuery();
     }

  ngOnInit() {
    this.createForm();
    this.loadFilteredBoardGames();
  }

  createForm(){
    this.pubService.getPub(this.route.snapshot.params.id)
      .subscribe(res => {
          this.pub = <PubModel> res;
          this.boardGames = new MatTableDataSource<BoardGameModel>(this.pub.pubBoardGames);
          this.boardGames.paginator = this.paginator;
          this.boardGames.sort = this.sort;
          this.imageUrl = `${environment.serverUrl}/${this.pub.imagePath}`;
          this.formGroup = this.fb.group({
            id:[this.pub.id],
            name:[this.pub.name, Validators.required],
            description:[this.pub.description, Validators.compose([Validators.required, Validators.minLength(30), Validators.maxLength(1000)])],
            address: this.fb.group({
              street:[this.pub.address.street, Validators.required],
              buildingNumber:[this.pub.address.buildingNumber, Validators.required],
              localNumber:[this.pub.address.localNumber],
              postalCode:[this.pub.address.postalCode, Validators.required],
              city:[this.pub.address.city, Validators.required],
            }),
            imagePath:[this.pub.imagePath]
      });
    })
  }

  loadFilteredBoardGames() {
    this.filteredBoardGames = this.boardGameControl.valueChanges
    .pipe(
      startWith(''),
      map(value => this._filter(value))
    );
  }

  loadBoardGamesTable()
  {
    this.pubService.getPub(this.route.snapshot.params.id)
      .subscribe(res => {
          this.boardGames = new MatTableDataSource<BoardGameModel>((<PubModel> res).pubBoardGames);
          this.boardGames.paginator = this.paginator;
          this.boardGames.sort = this.sort;
    })
  }

  private _filter(value: string): BoardGameModel[] {
    if(value.length < 2)
    {
      this.boardGameToAdd = null;
      this.loadedBoardGames = null;
      return;
    }
    
    if(!this.loadedBoardGames)
    {
      this.boardGameQuery.search = value;
      this.boardGameQuery.pageSize = 100;
      this.boardGameService.setBoardGameQuery(this.boardGameQuery)
  
      this.boardGameService.getBoardGames()
        .subscribe(res => {
          this.loadedBoardGames = (<PaginationResult<BoardGameModel>>res).results;
          const filterValue = value.toLowerCase();
          return this.loadedBoardGames.filter(option => option.name.toLowerCase().includes(filterValue));
        });
    }
    else {
      const filterValue = value.toLowerCase();
      return this.loadedBoardGames.filter(option => option.name.toLowerCase().includes(filterValue));
    }
  }

  onBoardGameSelected(boardGameName: string)
  {
    this.boardGameToAdd = this.loadedBoardGames.find(bg => bg.name == boardGameName);
  }

  addBoardGame()
  {
    this.pubService.addBoardGames(this.pub.id, this.boardGameToAdd.id)
      .subscribe(() => this.loadBoardGamesTable());
  }

  onSubmit() {
    this.pubService.updatePub(this.formGroup.value, this.fileToUpload)
      .subscribe(data => {
        this.router.navigate([`show/pub/${this.formGroup.value.id}`]);
    })
  }

  onCancel() {
    this.router.navigate([`show/pub/${this.formGroup.value.id}`])
  }

  deletePub() {
    this.pubService.deletePub(this.pub.id)
      .subscribe(res => {
        this.router.navigate([`show/pubs`]);
      })
  }

  deleteBoardGame(row) {
    this.pubService.deleteBoardGames(this.pub.id, row.id)
      .subscribe(() => this.loadBoardGamesTable());
  }

  applyFilter(event: Event) {
    this.boardGames.filterPredicate = 
      (data: BoardGameModel, filter: string) => {
        return data.name.toLowerCase().includes(filter.toLowerCase()) 
        || data.category.name.toLowerCase().includes(filter.toLowerCase())
      };
    const filterValue = (event.target as HTMLInputElement).value;
    this.boardGames.filter = filterValue;
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
