import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MaterialModule } from '../material.module';
import { TranslateModule } from '@ngx-translate/core';



@NgModule({
  declarations: [],
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
