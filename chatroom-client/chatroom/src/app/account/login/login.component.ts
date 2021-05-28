import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Login } from 'src/app/shared/models/login';
import { Res } from 'src/app/shared/models/response';
import { UserDetail } from 'src/app/shared/models/user';
import { FormFieldService } from 'src/app/shared/services/form-field.service';
import { AccountService } from 'src/app/shared/states/account/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  returnUrl: string;
  error: string;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private accountService: AccountService,
    public formService: FormFieldService,
    private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.returnUrl = this.activatedRoute.snapshot.queryParams.returnUrl || '/home'
    this.initForm();
  }

  initForm() {
    this.loginForm = this.formBuilder.group({
      emailOrUserName: [null, [Validators.required]],
      password: [null, [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit() {

    if (this.loginForm.valid) {
      this.accountService.login(this.loginForm.value).subscribe((res: Res<UserDetail>) => {
        if (res.isSuccessed) {
          if (this.returnUrl) {
            this.router.navigateByUrl(this.returnUrl);
          }
          else {
            this.router.navigateByUrl('/home');
          }
        }
        else {
          if (res.message) {
            this.error = res.message;
          }
          else {
            this.error = 'error';
          }
        }
      }, error => {
        var message = error.error.message;
        this.error = message;
      })
    }
    else {
      this.loginForm.markAllAsTouched();
    }
  }

}
