import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessageComponent } from './message.component';
import { SharedModule } from '../shared/shared.module';
import { MessageRoutingModule } from './message-routing.module';



@NgModule({
  declarations: [
    MessageComponent
  ],
  imports: [
    MessageRoutingModule,
    CommonModule,
    SharedModule,
  ]
})
export class MessageModule { }
