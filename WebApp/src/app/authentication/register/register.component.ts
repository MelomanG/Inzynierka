import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AuthenticationService } from '../authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  formGroup: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthenticationService,
    private router: Router
  ) 
  { }

  ngOnInit(): void {
    this.createForm();
  }

  createForm(){
    this.formGroup = this.fb.group({
      username:[null, Validators.required],
      email:[null, Validators.compose([Validators.required, Validators.email])],
      password:[null, Validators.required],
      confirmationPassword:[null, Validators.compose([Validators.required])],
    });
  }

  onSubmit() {
    this.authService.register(this.formGroup.value)
      .subscribe(data => {
        this.router.navigate([`login`]);
    })
  }
}
