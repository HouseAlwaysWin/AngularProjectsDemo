import { Injector, NgModule } from '@angular/core';
import { FormFieldInvalidDirective } from './directives/form-field-invalid.directive';
import { CalendarModule } from 'primeng/calendar';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { GalleriaModule } from 'primeng/galleria';
import { FileUploadModule } from 'primeng/fileupload';
import { DragAndDropDirective } from './directives/drag-and-drop.directive';


export let InjectorInstance: Injector;
@NgModule({
  declarations: [
    FormFieldInvalidDirective,
    DragAndDropDirective,
  ],
  exports: [
    FormFieldInvalidDirective,
    CalendarModule,
    ProgressSpinnerModule,
    GalleriaModule,
    FileUploadModule,
    DragAndDropDirective
  ]
})
export class SharedModule {

  constructor(private injector: Injector) {
    InjectorInstance = injector;
  }
}
