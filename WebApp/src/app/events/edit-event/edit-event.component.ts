import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { BoardGameModel } from 'src/app/shared/models/boardgame';
import { Observable } from 'rxjs';
import { FormControl, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BoardGameQuery, PubQuery } from 'src/app/shared/models/query';
import { PubModel } from 'src/app/shared/models/pub';
import { Router, ActivatedRoute } from '@angular/router';
import { PubsService } from 'src/app/pubs/pubs.service';
import { BoardGameService } from 'src/app/boardgames/boardgame.service';
import { environment } from 'src/environments/environment';
import { startWith, map } from 'rxjs/operators';
import { PaginationResult } from 'src/app/shared/models/paginationresult';
import { UserModel } from 'src/app/shared/models/contacts';
import { EventModel } from 'src/app/shared/models/events';
import { EventsService } from '../events.service';
import * as moment from 'moment';
import { ContactService } from 'src/app/shared/services/contact.service';

@Component({
  selector: 'app-edit-event',
  templateUrl: './edit-event.component.html',
  styleUrls: ['./edit-event.component.scss']
})

export class EditEventComponent implements OnInit {
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
  displayedColumns: string[] = ['name','actions'];
  users: MatTableDataSource<UserModel>;
  userNameControl = new FormControl();

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
  loadedUsers: UserModel[];
  filteredUsers: Observable<UserModel[]>;

  eventModel: EventModel;
  imageUrl: string;
  fileToUpload: File;
  formGroup: FormGroup;
  
  @ViewChild('Image')
  Image;
  userToAdd: UserModel;
  
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private contactService: ContactService,
    private pubService: PubsService,
    private boardGameService: BoardGameService,
    private eventService: EventsService) {
      this.pubQuery = new PubQuery();
      this.pubService.setPubQuery(this.pubQuery);
      this.boardGameQuery = new BoardGameQuery()
      this.boardGameService.setBoardGameQuery(this.boardGameQuery);
     }

  ngOnInit() {
    this.createForm();
    this.loadPubs();
    this.loadBoardgames();
    this.filterContacts();
  }

  createForm(){
    this.eventService.getEvent(this.route.snapshot.params.id)
      .subscribe(res => {
          this.eventModel = <EventModel> res;
          this.startDateFormControll.setValue(new Date(this.eventModel.startDate));
          this.hoursFormControl.setValue(this.eventModel.startDate.substring(11, 13));  
          this.minutesFormControl.setValue(this.eventModel.startDate.substring(14, 16));
          this.checkBosFormControl.setValue(this.eventModel.isPublic);
          this.boardGameControl.setValue(this.eventModel.boardGame.name);
          this.pubControl.setValue(this.eventModel.pub.name);
          this.users = new MatTableDataSource<UserModel>(this.eventModel.participants);
          this.users.paginator = this.paginator;
          this.users.sort = this.sort;
          this.imageUrl = `${environment.serverUrl}/${this.eventModel.imagePath}`;
          this.formGroup = this.fb.group({
            id:[this.eventModel.id],
            name:[this.eventModel.name, Validators.required],
            pubId:[this.eventModel.pub.id],
            boardGameId:[this.eventModel.boardGame.id, Validators.required],
            description:[this.eventModel.description, Validators.compose([Validators.required, Validators.minLength(30), Validators.maxLength(1000)])],
            address: this.fb.group({
              street:[this.eventModel.address.street, Validators.required],
              buildingNumber:[this.eventModel.address.buildingNumber, Validators.required],
              localNumber:[this.eventModel.address.localNumber],
              postalCode:[this.eventModel.address.postalCode, Validators.required],
              city:[this.eventModel.address.city, Validators.required],
            }),
            imagePath:[this.eventModel.imagePath]
      });
    })
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

  filterContacts() {
    this.filteredUsers = this.userNameControl.valueChanges
    .pipe(
      startWith(''),
      map(value => this._filterCities(value))
    );
  }

  private _filterCities(value: string): UserModel[] {
    if(value.length < 2)
    {
      this.loadedUsers = null;
      this.userToAdd = null;
      return;
    }
    if(!this.loadedUsers)
    {
      this.contactService.getUsers(value)
        .subscribe(res => {
          this.loadedUsers = <UserModel[]>res;
          const filterValue = value.toLowerCase();
          return this.loadedUsers.filter(option => option.userName.toLowerCase().includes(filterValue));
      });
    }
    else {
      const filterValue = value.toLowerCase();
      return this.loadedUsers.filter(option => option.userName.toLowerCase().includes(filterValue));

    }
  }

  onUserNameSelected(userName: string) {
    this.userToAdd = this.loadedUsers.find(c => c.userName == userName);
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

  loadUsersTable()
  {
    this.eventService.getEvent(this.route.snapshot.params.id)
      .subscribe(res => {
          this.users = new MatTableDataSource<UserModel>((<EventModel> res).participants);
          this.users.paginator = this.paginator;
          this.users.sort = this.sort;
    })
  }

  addUser()
  {
    console.log(this.eventModel)
    this.eventService.addParticipant(this.eventModel.id, this.userToAdd.id)
      .subscribe(() => this.loadUsersTable());
  }

  onSubmit() {
    var eventModel = <EventModel>this.formGroup.value;
    eventModel.startDate = moment.utc(this.startDateFormControll.value).add(1, 'd').toISOString()
    if(this.hoursFormControl.value && this.minutesFormControl.value)
      eventModel.startDate = `${eventModel.startDate.substring(0, 11)}${this.hoursFormControl.value}:${this.minutesFormControl.value}:00`;
    
    eventModel.isPublic = this.checkBosFormControl.value;
    console.log(eventModel);
    
    this.eventService.updateEvent(eventModel, this.fileToUpload)
      .subscribe(data => {
        this.router.navigate([`show/event/${(<EventModel>data).id}`]);
    })
  }

  onCancel() {
    this.router.navigate([`show/event/${this.formGroup.value.id}`])
  }

  deleteEvent() {
    this.eventService.deleteEvent(this.eventModel.id)
      .subscribe(res => {
        this.router.navigate([`show/events`]);
      })
  }

  deleteUser(row) {
    this.eventService.deleteParticipant(this.eventModel.id, this.userToAdd.id)
      .subscribe(() => this.loadUsersTable());
  }

  applyFilter(event: Event) {
    this.users.filterPredicate = 
      (data: UserModel, filter: string) => {
        return data.userName.toLowerCase().includes(filter.toLowerCase()) };
    const filterValue = (event.target as HTMLInputElement).value;
    this.users.filter = filterValue;
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