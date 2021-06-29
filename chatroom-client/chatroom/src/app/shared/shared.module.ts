import { Injector, NgModule } from '@angular/core';
import { FormFieldInvalidDirective } from './directives/form-field-invalid.directive';
import { CalendarModule } from 'primeng/calendar';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { GalleriaModule } from 'primeng/galleria';
import { FileUploadModule } from 'primeng/fileupload';
import { DragAndDropDirective } from './directives/drag-and-drop.directive';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BadgeModule } from 'primeng/badge';
import { TrucateTextPipe } from './pipes/trucate-text.pipe';

export let InjectorInstance: Injector;
@NgModule({
  declarations: [
    FormFieldInvalidDirective,
    DragAndDropDirective,
    TrucateTextPipe
  ],
  exports: [
    ReactiveFormsModule,
    FormsModule,
    FormFieldInvalidDirective,
    CalendarModule,
    ProgressSpinnerModule,
    GalleriaModule,
    FileUploadModule,
    DragAndDropDirective,
    BadgeModule,
    TrucateTextPipe
  ]
})
export class SharedModule {

  constructor(private injector: Injector) {
    InjectorInstance = injector;
  }
}
