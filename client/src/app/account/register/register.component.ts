import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit, OnDestroy {

  returnUrl: string;
  registerForm: FormGroup;
  error: string;
  private _onDestroy = new Subject();

  constructor(
    public translate: TranslateService,
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private activatedRoute: ActivatedRoute,
    private router: Router) {
  }
  ngOnDestroy(): void {
    this._onDestroy.next();
  }

  get email() {
    return this.registerForm.get('email');
  }

  get password() {
    return this.registerForm.get('password');
  }

  get passwordConfirm() {
    return this.registerForm.get('passwordConfirm');
  }

  get displayName() {
    return this.registerForm.get('displayName');
  }

  ngOnInit(): void {
    this.returnUrl = this.activatedRoute.snapshot.queryParams.returnUrl || '/shop';
    this.accountService.currentUser$
      .pipe(takeUntil(this._onDestroy))
      .subscribe(user => {
        if (user) {
          this.router.navigate(['/']);
        }
      });
    this.validateRegisterForm();
  }

  validateRegisterForm() {
    this.registerForm = this.formBuilder.group({
      displayName: [null, [Validators.required]],
      email: [null, [Validators.required, Validators.email]
      ],
      password: [null, [Validators.required]],
      passwordConfirm: [null, Validators.required]
    });

  }

  onSubmit() {
    this.accountService.register(this.registerForm.value)
      .pipe(takeUntil(this._onDestroy))
      .subscribe(res => {
        if (res.data) {
          if (!this.returnUrl) {
            this.router.navigateByUrl(this.returnUrl);
          } else {
            this.router.navigateByUrl('/home');
          }
        } else {
          if (res.message) {
            this.error = res.message;
          }
          else {
            this.error = this.translate.instant('AccountForm.RegisterFailed');
          }
        }
      }, error => {
        console.log(error);
        this.error = error.error.message;
      });
  }

}
