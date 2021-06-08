import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessageComponent } from './message.component';
import { SharedModule } from '../shared/shared.module';
import { MessageRoutingModule } from './message-routing.module';
import { MessageChatroomComponent } from './message-chatroom/message-chatroom.component';



@NgModule({
  declarations: [
    MessageComponent,
    MessageChatroomComponent
  ],
  imports: [
    MessageRoutingModule,
    CommonModule,
    SharedModule,
  ]
})
export class MessageModule { }
