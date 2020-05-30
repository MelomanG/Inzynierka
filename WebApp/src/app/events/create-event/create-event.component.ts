import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { EventsService } from '../events.service';
import { CreateEventModel, EventModel } from 'src/app/shared/models/events';
import { PubsService } from 'src/app/pubs/pubs.service';
import { BoardGameService } from 'src/app/boardgames/boardgame.service';
import { PubQuery, BoardGameQuery } from 'src/app/shared/models/query';
import { PubModel } from 'src/app/shared/models/pub';
import { Observable } from 'rxjs';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { startWith, map } from 'rxjs/operators';
import { PaginationResult } from 'src/app/shared/models/paginationresult';
import * as moment from 'moment';

@Component({
  selector: 'app-create-event',
  templateUrl: './create-event.component.html',
  styleUrls: ['./create-event.component.scss']
})
export class CreateEventComponent implements OnInit {
  pubControl = new FormControl();
  boardGameControl = new FormControl();
  startDateFormControll = new FormControl();
  hoursFormControl = new FormControl();
  minutesFormControl = new FormControl();
  checkBosFormControl = new FormControl();

  pubQuery: PubQuery;
  boardGameQuery: BoardGameQuery;
  pubs: PubModel[];
  filteredPubs: Observable<PubModel[]>;
  loadedBoardGames: BoardGameModel[];
  filteredBoardGames: Observable<BoardGameModel[]>;

  constructor(
    private fb: FormBuilder,
    private router: Router, 
    private eventService: EventsService,
    private pubService: PubsService,
    private boardGameService: BoardGameService) {
      this.pubQuery = new PubQuery();
      this.pubService.setPubQuery(this.pubQuery);
      this.boardGameQuery = new BoardGameQuery()
      this.boardGameService.setBoardGameQuery(this.boardGameQuery);
     }

  formGroup: FormGroup;
  showError: boolean = false;
  @ViewChild('Image')
  Image;
  imageUrl: string = "assets/images/default-image.png";
  fileToUpload: File;

  ngOnInit() {
    this.createForm();
    this.loadPubs();
    this.loadBoardgames();
  }

  createForm(){
    this.checkBosFormControl.setValue(true);
    this.formGroup = this.fb.group({
      name:[null, Validators.required],
      description:[null, Validators.compose([Validators.required, Validators.minLength(30), Validators.maxLength(1000)])],
      pubId:[null],
      boardGameId:[null, Validators.required],
      address: this.fb.group({
        street:[null],
        buildingNumber:[null],
        localNumber:[null],
        postalCode:[null],
        city:[null],
      })
    });
  }

  loadPubs() {
    this.filteredPubs = this.pubControl.valueChanges
    .pipe(
      startWith(''),
      map(value => this._filterPubs(value))
    );
  }

  private _filterPubs(value: string): PubModel[] {
    if(value.length < 2)
    {
      this.pubs = null;
      this.formGroup.value.pubId = null;
      this.pubQuery = this.pubService.getPubQuery();
      this.pubQuery.search = null;
      this.pubService.setPubQuery(this.pubQuery);
      return;
    }

    if(!this.pubs)
    {
      this.pubQuery = this.pubService.getPubQuery();
      this.pubQuery.search = value;
      this.boardGameQuery.pageSize = 100;
      this.pubService.setPubQuery(this.pubQuery);

      this.pubService.getPubs()
        .subscribe(res => {
          this.pubs = (<PaginationResult<PubModel>>res).results;
          const filterValue = value.toLowerCase();
          return this.pubs.filter(option => option.name.toLowerCase().includes(filterValue));
      })
    }
    else {
      const filterValue = value.toLowerCase();
      return this.pubs.filter(option => option.name.toLowerCase().includes(filterValue));
    }
  }

  loadBoardgames() {
    this.filteredBoardGames = this.boardGameControl.valueChanges
    .pipe(
      startWith(''),
      map(value => this._filterBoardGame(value))
    );
  }
  
  private _filterBoardGame(value: string): BoardGameModel[] {
    if(value.length < 2)
    {
      this.loadedBoardGames = null;
      this.boardGameQuery = this.boardGameService.getBoardGameQuery();
      this.boardGameQuery.search = null;
      this.formGroup.value.boardGameId = null;
      this.boardGameService.setBoardGameQuery(this.boardGameQuery);
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
          const temp = this.loadedBoardGames.filter(option => option.name.toLowerCase().includes(filterValue));
          return temp;
        });
    }
    else {
      const filterValue = value.toLowerCase();
      const temp = this.loadedBoardGames.filter(option => option.name.toLowerCase().includes(filterValue));
      return temp;
    }
  }

  onPubSelected(pubName: string) {
    const pubId = this.pubs.find(bg => bg.name == pubName).id;
    this.formGroup.value.pubId = pubId;
  }

  onBoardGameSelected(boardGameName: string)
  {
    const boardGameId = this.loadedBoardGames.find(bg => bg.name == boardGameName).id;
    this.formGroup.value.boardGameId = boardGameId;
  }

  onSubmit(eventModel: CreateEventModel) {
    eventModel.startDate = moment.utc(this.startDateFormControll.value).add(1, 'd').toISOString()
    if(this.hoursFormControl.value && this.minutesFormControl.value)
      eventModel.startDate = `${eventModel.startDate.substring(0, 11)}${this.hoursFormControl.value}:${this.minutesFormControl.value}:00`;
    
    eventModel.isPublic = this.checkBosFormControl.value;
    console.log(eventModel);
    this.eventService.createEvent(eventModel, this.fileToUpload)
      .subscribe(data => {
        this.router.navigate([`show/event/${(<EventModel>data).id}`]);
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
