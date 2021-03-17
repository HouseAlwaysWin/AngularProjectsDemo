import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../account.service';
import { FbAuthService } from '../fb-auth.service';

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
    private formBuilder: FormBuilder,
    private fbauth: FbAuthService,
    private accountService: AccountService,
    private activatedRoute: ActivatedRoute,
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
    this.router.navigateByUrl('/shop');
    this.accountService.login(this.loginForm.value).subscribe(res => {
      console.log(res);
      if (res.data) {
        if (!this.returnUrl) {
          this.router.navigateByUrl(this.returnUrl);
        } else {
          this.router.navigateByUrl('/home');
        }
      } else {
        this.error = res.message;
      }

    }, error => {
      console.log(error);
      this.error = error.error.message;
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
