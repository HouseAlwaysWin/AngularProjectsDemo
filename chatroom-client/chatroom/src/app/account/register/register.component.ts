import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { of, timer } from 'rxjs';
import { catchError, debounceTime, map, switchMap, tap } from 'rxjs/operators';
import { Res } from 'src/app/shared/models/response';
import { UserToken } from 'src/app/shared/models/user';
import { FormFieldService } from 'src/app/shared/services/form-field.service';
import { AccountQuery } from 'src/app/shared/states/account/account.query';
import { AccountService } from 'src/app/shared/states/account/account.service';
import { AccountStore } from 'src/app/shared/states/account/account.store';
import { CustomValidators } from 'src/app/shared/validators/custom-validators';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  returnUrl: string;
  error: string;
  startDate = new Date(1990, 0, 1);
  emailLoading: boolean = false;
  userLoading: boolean = false;
  emailFocus: boolean = false;
  userNameFocus: boolean = false;

  constructor(
    private http: HttpClient,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    public formService: FormFieldService,
    private accountService: AccountService,
    private accountQuery: AccountQuery,
    private formBuilder: FormBuilder,
    private cdRef: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.returnUrl = this.activatedRoute.snapshot.queryParams.returnUrl || '/home'
    this.initForm();
    this.loading();
  }

  onUserNameFocus() {
    this.userNameFocus = true;
  }

  onEmailFocus() {
    this.emailFocus = true;
  }

  loading() {
    this.accountQuery.loading$.subscribe(res => {
      if (this.emailFocus) {
        this.emailLoading = res;
        if (!res) {
          this.emailFocus = false;
        }

      }

      if (this.userNameFocus) {

        this.userLoading = res;
        if (!res) {
          this.userNameFocus = false;
        }
      }
    });

  }


  initForm() {
    this.registerForm = this.formBuilder.group({
      username: [null, [Validators.required], [CustomValidators.isUserDuplicated]],
      email: [null, [Validators.required, Validators.email], [CustomValidators.isUserDuplicated]],
      dateOfBirth: [null, [Validators.required, Validators.min(1)]],
      password: [null, [Validators.required, Validators.minLength(6)]],
      confirmPassword: [null, [Validators.required, Validators.minLength(6), CustomValidators.isMatch('password')]],
      gender: ['male', [Validators.required]]
    }, { updateOn: 'blur' });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      this.accountService.register(this.registerForm.value).subscribe((res: Res<UserToken>) => {
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
      this.registerForm.markAllAsTouched();
    }
  }

}
