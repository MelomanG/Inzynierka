import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { PubsService } from '../pubs.service';
import { environment } from 'src/environments/environment';
import { PubModel } from 'src/app/shared/models/pub';

@Component({
  selector: 'app-edit-pub',
  templateUrl: './edit-pub.component.html',
  styleUrls: ['./edit-pub.component.scss']
})
export class EditPubComponent implements OnInit {

  pub: PubModel;
  imageUrl: string;
  fileToUpload: File;
  formGroup: FormGroup;
  
  @ViewChild('Image')
  Image;
  
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private pubService: PubsService) { }

  ngOnInit() {
    this.createForm();
  }

  createForm(){
    this.pubService.getPub(this.route.snapshot.params.id)
      .subscribe(res => {
          this.pub = <PubModel> res;
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

  onSubmit() {
    this.pubService.updatePub(this.formGroup.value, this.fileToUpload)
      .subscribe(data => {
        this.router.navigate([`show/pub/${this.formGroup.value.id}`]);
    })
  }

  deletePub() {
    this.pubService.deletePub(this.pub.id)
      .subscribe(res => {
        this.router.navigate([`show/pubs`]);
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
