import { Injector, NgModule } from '@angular/core';
import { FormFieldInvalidDirective } from './directives/form-field-invalid.directive';
import { CalendarModule } from 'primeng/calendar';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { GalleriaModule } from 'primeng/galleria';


export let InjectorInstance: Injector;
@NgModule({
  declarations: [
    FormFieldInvalidDirective,
  ],
  exports: [
    FormFieldInvalidDirective,
    CalendarModule,
    ProgressSpinnerModule,
    GalleriaModule
  ]
})
export class SharedModule {

  constructor(private injector: Injector) {
    InjectorInstance = injector;
  }
}
