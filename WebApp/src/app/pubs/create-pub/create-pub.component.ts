import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PubsService } from '../pubs.service';
import { PubModel, CreatePubModel } from 'src/app/shared/models/pub';

@Component({
  selector: 'app-create-pub',
  templateUrl: './create-pub.component.html',
  styleUrls: ['./create-pub.component.scss']
})
export class CreatePubComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    private router: Router, 
    private pubService: PubsService) { }

  formGroup: FormGroup;
  showError: boolean = false;

  imageUrl: string = "assets/images/default-image.png";
  fileToUpload: File;

  ngOnInit() {
    this.createForm();
  }

  createForm(){
    this.formGroup = this.fb.group({
      name:[null, Validators.required],
      description:[null, Validators.compose([Validators.required, Validators.minLength(30), Validators.maxLength(1000)])],
      address: this.fb.group({
        street:[null, Validators.required],
        buildingNumber:[null, Validators.required],
        localNumber:[null],
        postalCode:[null, Validators.required],
        city:[null, Validators.required],
      })
    });
  }

  onSubmit(pub: CreatePubModel) {
    console.log(pub);
    this.pubService.createPub(pub, this.fileToUpload)
      .subscribe(data => {
        this.router.navigate([`show/pub/${(<PubModel>data).id}`]);
    })
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
