import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  returnUrl: string;
  registerForm: FormGroup;
  errors: string[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private activatedRoute: ActivatedRoute,
    private router: Router) {
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

    // this.registerForm = new FormGroup({
    //   email: new FormControl('', {
    //     validators: [Validators.required, Validators.email]
    //   }),
    //   password: new FormControl('', {
    //     validators: [Validators.required]
    //   }),
    //   passwordConfirm: new FormControl('', {
    //     validators: [Validators.required]
    //   })
    // })
  }

  onSubmit() {
    console.log(this.registerForm);
    this.accountService.regiater({
      displayName: this.registerForm.value.displayName,
      email: this.registerForm.value.email,
      password: this.registerForm.value.password
    }).subscribe(res => {
      if (res.data) {
        if (!this.returnUrl) {
          this.router.navigateByUrl(this.returnUrl);
        } else {
          this.router.navigateByUrl('/home');
        }
      } else {
        this.errors[0] = res.message;
        console.log(this.errors);
      }
    }, error => {
      console.log(error);
      this.errors = error.errors;
    });
  }

}
