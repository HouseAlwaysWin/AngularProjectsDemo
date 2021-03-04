import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../account.service';
import { FbAuthService } from '../fb-auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private fbauth: FbAuthService) { }

  ngOnInit(): void {
    this.validateLoginForm();
  }

  validateLoginForm() {
    // this.loginForm = new FormGroup({
    //   email: new FormControl('', [Validators.required, Validators.email]),
    //   password: new FormControl('', Validators.required)
    // });
    this.loginForm = this.formBuilder.group({
      email: [null, [Validators.required, Validators.email]
      ],
      password: [null, [Validators.required]]
    });
  }

  onSubmit() {
    this.fbauth.login({
      email: this.loginForm.value.email,
      password: this.loginForm.value.password
    });
  }


}
