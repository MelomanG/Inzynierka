import { Component, OnInit, Input, Output, EventEmitter, Renderer2 } from '@angular/core';
import { ContactService } from '../../services/contact.service';
import { ContactModel } from '../../models/contacts';
import { FormControl } from '@angular/forms';
import { startWith, map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { AddContactDialogComponent } from '../add-contact-dialog/add-contact-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-contacts-sidenav',
  templateUrl: './contacts-sidenav.component.html',
  styleUrls: ['./contacts-sidenav.component.scss']
})
export class ContactsSidenavComponent implements OnInit {
  @Output() sideNavToggle = new EventEmitter<void>();

  userNameControl = new FormControl();

  contacts: ContactModel[];
  filteredContacts: Observable<ContactModel[]>;
  
  constructor(
    private contactService: ContactService,
    private dialog: MatDialog,
    private renderer2: Renderer2) { }

  ngOnInit(): void {
    this.loadContacts();
    this.filterContacts();
  }

  loadContacts() {
    this.contactService.getContacts()
    .subscribe(res => {
      this.contacts = <ContactModel[]>res;
    });
  }

  filterContacts() {
    this.filteredContacts = this.userNameControl.valueChanges
      .pipe(
        startWith(''),
        map(value => this._filter(value))
      )
  }

  private _filter(value: string) {
    if(value.length < 2)
    {
      this.loadContacts();
      return this.contacts;
    }
    return this.contacts
      .filter(contact =>
          contact.contactUserName.toLowerCase().includes(value.toLowerCase()));
  }

  deleteContact(contact) {
    this.contactService.deleteContact(contact.id).subscribe();
    this.clear();
  }


  onAddContact() {
    var dialogRef = this.dialog.open(AddContactDialogComponent, {
      width: "450px"
    });

    dialogRef.afterClosed().subscribe(() => {
      this.clear();
    })
  }

 onToggleContacts(contactsSidenav) {
  this.clear();
  contactsSidenav.toggle()
 }

 private clear() {
  this.filteredContacts = null;
  this.contacts = null;
  this.loadContacts();
  this.filterContacts();
 }

 onToggle() {
  this.sideNavToggle.emit();
 }

 mouseenter (event) {
   this.renderer2.addClass(event.target, 'mat-elevation-z5')
}

mouseleave (event) {
  this.renderer2.removeClass(event.target, 'mat-elevation-z5')
}
}
