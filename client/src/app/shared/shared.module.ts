import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MaterialModule } from '../material/material.module';
import { TranslateModule } from '@ngx-translate/core';
import { MatCarouselComponent } from './components/mat-carousel/mat-carousel.component';



@NgModule({
  declarations: [MatCarouselComponent],
  imports: [
    CommonModule,
    FlexLayoutModule,
    MaterialModule,
    TranslateModule
  ],
  exports: [
    CommonModule,
    FlexLayoutModule,
    MaterialModule,
    TranslateModule
  ]
})
export class SharedModule { }
