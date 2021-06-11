import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { MessageComponent } from './message.component';
import { MessageChatroomComponent } from './message-chatroom/message-chatroom.component';

const routes: Routes = [
  { path: 'list', component: MessageComponent },
  // { path: 'chatroom', component: MessageChatroomComponent },
];


@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ]
})
export class MessageRoutingModule { }
