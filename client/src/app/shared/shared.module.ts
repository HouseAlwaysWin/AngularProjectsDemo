import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MaterialModule } from '../material/material.module';
import { TranslateModule } from '@ngx-translate/core';
import { MatCarouselComponent } from './components/mat-carousel/mat-carousel.component';
import { DialogComfirm } from './components/dialog-comfirm/dialog-comfirm.component';
import { CdkStepperModule } from '@angular/cdk/stepper';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DialogMessage } from './components/dialog-message/dialog-message.component';
import { TrucateTextPipe } from './pipes/trucate-text.pipe';
import { ForLoopDirective } from './directives/for-loop/for-loop.directive';



@NgModule({
  declarations: [MatCarouselComponent, DialogComfirm, DialogMessage, TrucateTextPipe, ForLoopDirective],
  imports: [
    CommonModule,
    FlexLayoutModule,
    MaterialModule,
    TranslateModule,
    CdkStepperModule,
    ReactiveFormsModule,
    FormsModule
  ],
  exports: [
    CommonModule,
    FlexLayoutModule,
    MaterialModule,
    TranslateModule,
    CdkStepperModule,
    ReactiveFormsModule,
    FormsModule,
    MatCarouselComponent,
    TrucateTextPipe,
    DialogComfirm
  ]
})
export class SharedModule { }
