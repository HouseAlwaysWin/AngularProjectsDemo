import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Res } from 'src/app/shared/models/response';
import { UserToken } from 'src/app/shared/models/user';
import { FormFieldService } from 'src/app/shared/services/form-field.service';
import { AccountService } from 'src/app/shared/states/account/account.service';

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
  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    public formService: FormFieldService,
    private accountService: AccountService,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit(): void {
    this.returnUrl = this.activatedRoute.snapshot.queryParams.returnUrl || '/home'
    this.initForm();
  }

  initForm() {
    this.registerForm = this.formBuilder.group({
      username: [null, [Validators.required]],
      email: [null, [Validators.required, Validators.email]],
      dateOfBirth: [null, [Validators.required, Validators.min(1)]],
      password: [null, [Validators.required, Validators.minLength(6)]],
      passwordConfirm: [null, [Validators.required, Validators.minLength(6)]],
      gender: ['male', [Validators.required]]
    });
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
