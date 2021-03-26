import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { NgModule } from '@angular/core';
import { AccountRoutingModule } from './account.routing.module';
import { MaterialModule } from '../material/material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [LoginComponent, RegisterComponent],
  imports: [
    AccountRoutingModule,
    MaterialModule,
    SharedModule
  ]
})
export class AccountModule { }
