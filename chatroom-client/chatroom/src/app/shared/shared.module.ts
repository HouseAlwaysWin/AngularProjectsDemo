import { NgModule } from '@angular/core';
import { FormFieldInvalidDirective } from './directives/form-field-invalid.directive';



@NgModule({
  declarations: [
    FormFieldInvalidDirective,
  ],
  exports: [
    FormFieldInvalidDirective,
  ]
})
export class SharedModule { }
