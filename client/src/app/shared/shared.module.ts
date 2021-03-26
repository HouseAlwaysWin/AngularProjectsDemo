import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MaterialModule } from '../material/material.module';
import { TranslateModule } from '@ngx-translate/core';
import { MatCarouselComponent } from './components/mat-carousel/mat-carousel.component';
import { DialogComfirm } from './components/dialog-comfirm/dialog-comfirm.component';
import { CdkStepperModule } from '@angular/cdk/stepper';
import { ReactiveFormsModule } from '@angular/forms';
import { DialogMessage } from './components/dialog-message/dialog-message.component';



@NgModule({
  declarations: [MatCarouselComponent, DialogComfirm, DialogMessage],
  imports: [
    CommonModule,
    FlexLayoutModule,
    MaterialModule,
    TranslateModule,
    CdkStepperModule,
    ReactiveFormsModule
  ],
  exports: [
    CommonModule,
    FlexLayoutModule,
    MaterialModule,
    TranslateModule,
    CdkStepperModule,
    ReactiveFormsModule,
    MatCarouselComponent
  ]
})
export class SharedModule { }
