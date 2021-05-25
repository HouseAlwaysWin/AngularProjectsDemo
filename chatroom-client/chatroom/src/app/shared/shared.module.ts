import { NgModule } from '@angular/core';
import { FormFieldInvalidDirective } from './directives/form-field-invalid.directive';
import { CalendarModule } from 'primeng/calendar';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
@NgModule({
  declarations: [
    FormFieldInvalidDirective,
  ],
  exports: [
    FormFieldInvalidDirective,
    CalendarModule,
    ProgressSpinnerModule,
  ]
})
export class SharedModule { }
