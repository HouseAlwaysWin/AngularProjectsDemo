import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }

  initRegisterForm() {
    this.registerForm = this.fb.group({
      displayName: [null, Validators.required],
      email: [null,
        [Validators.required, Validators.email]
      ],
      password: [null, [Validators.required]]
    })
  }

}
