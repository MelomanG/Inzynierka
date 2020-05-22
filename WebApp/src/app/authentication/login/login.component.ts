import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
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
      email:[null, Validators.compose([Validators.required, Validators.email])],
      password:[null, Validators.required],
    });
  }

  onSubmit() {
    this.authService.login(this.formGroup.value)
      .subscribe(data => {
        this.router.navigate([`home`]);
    })
  }

}
