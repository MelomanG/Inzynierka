import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormControl } from '@angular/forms';
import { UserModel } from '../../models/contacts';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { ContactService } from '../../services/contact.service';

@Component({
  selector: 'app-add-contact-dialog',
  templateUrl: './add-contact-dialog.component.html',
  styleUrls: ['./add-contact-dialog.component.scss']
})
export class AddContactDialogComponent implements OnInit {
  selectedUser: UserModel;
  userNameControl = new FormControl();
  users: UserModel[];
  filteredUsers: Observable<UserModel[]>;

  constructor(
    private contactService: ContactService,
    private dialogRef: MatDialogRef<AddContactDialogComponent>) { }

  ngOnInit(): void {
    this.loadUsers();
    this.filterContacts();
  }

  loadUsers() {
    this.contactService.getUsers()
    .subscribe(res => {
      this.users = <UserModel[]>res;
    });
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
      this.loadUsers();
      return;
    }
    if(!this.users)
    {
      this.contactService.getUsers(value)
        .subscribe(res => {
          this.users = <UserModel[]>res;
          const filterValue = value.toLowerCase();
          return this.users.filter(option => option.userName.toLowerCase().includes(filterValue));
      });
    }
    else {
      const filterValue = value.toLowerCase();
      return this.users.filter(option => option.userName.toLowerCase().includes(filterValue));

    }
  }

  onUserNameSelected(userName: string) {
    this.selectedUser = this.users.find(c => c.userName == userName);
  }

  save() {
    this.contactService.addContact(this.selectedUser.id).subscribe();
    this.dialogRef.close()
  }

  dismiss() {
    this.dialogRef.close(null);
  }
}
