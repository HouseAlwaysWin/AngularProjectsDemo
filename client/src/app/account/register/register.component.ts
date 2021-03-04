import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;
  errors: string;

  constructor(
    private formBuilder: FormBuilder,
    private accountService: AccountService) {
  }

  ngOnInit(): void {
    this.validateRegisterForm();
  }

  validateRegisterForm() {
    // this.registerForm = this.formBuilder.group({
    //   email: [null, [Validators.required, Validators.email]
    //   ],
    //   password: [null, [Validators.required]],
    //   passwordConfirm: [null, [Validators.required]]
    // });
    this.registerForm = new FormGroup({
      email: new FormControl('', {
        validators: [Validators.required, Validators.email]
      }),
      password: new FormControl('', {
        validators: [Validators.required]
      }),
      passwordConfirm: new FormControl('', {
        validators: [Validators.required]
      })
    })
  }

  onSubmit() {
    console.log(this.registerForm);
    this.accountService.regiaterByFB({
      email: this.registerForm.value.email,
      password: this.registerForm.value.password
    });
  }

}
