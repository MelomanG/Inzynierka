import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from "@angular/forms";
import { AuthService } from '../services/auth/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  form

  constructor(private fb: FormBuilder, private auth: AuthService) {
    this.form = fb.group({
      userName: ["", Validators.required, ],
      email: ["", Validators.email],
      password: ["", Validators.required],
      confirmationPassword: ["", Validators.required]
    })
   }

  ngOnInit() {
  }
}
