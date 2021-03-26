import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { DialogMessage } from 'src/app/shared/components/dialog-message/dialog-message.component';
import { AccountService } from '../account.service';

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
    public dialog: MatDialog,
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private activatedRoute: ActivatedRoute,
    public translate: TranslateService,
    private router: Router) { }

  ngOnInit(): void {
    this.returnUrl = this.activatedRoute.snapshot.queryParams.returnUrl || '/shop';
    this.validateLoginForm();
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }

  validateLoginForm() {

    this.loginForm = this.formBuilder.group({
      email: [null, [Validators.required, Validators.email]
      ],
      password: [null, [Validators.required]]
    });
  }

  onSubmit() {
    this.accountService.login(this.loginForm.value).subscribe(res => {
      console.log(res);
      if (res.isSuccessed) {
        if (!this.returnUrl) {
          this.router.navigateByUrl(this.returnUrl);
        } else {
          this.router.navigateByUrl('/home');
        }
      } else {
        this.error = res.message;
      }

    }, error => {
      var message = error.error.message ? error.error.message : this.translate.instant('AccountForm.LoginFailed');
      this.dialog.open(DialogMessage, {
        data: {
          message: message
        }
      });
    });
    // this.fbauth.login({
    //   email: this.loginForm.value.email,
    //   password: this.loginForm.value.password
    // }).subscribe(isAuth => {
    //   if (isAuth) {
    //     if (!this.returnUrl) {
    //       this.router.navigateByUrl(this.returnUrl);
    //     } else {
    //       this.router.navigateByUrl('/home');
    //     }
    //   }
    //   else {
    //     this.router.navigateByUrl('/account/login');
    //   }
    // });
  }


}
