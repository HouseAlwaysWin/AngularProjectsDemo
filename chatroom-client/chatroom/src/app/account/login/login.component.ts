import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormFieldService } from 'src/app/shared/services/form-field.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;

  constructor(
    public formService: FormFieldService,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.initForm();
    console.log(this.loginForm.get('password').errors);
  }

  initForm() {
    this.loginForm = this.formBuilder.group({
      email: [null, [Validators.required, Validators.email]],
      password: [null, [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit() {

  }

}
