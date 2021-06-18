import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { Message } from 'src/app/shared/models/message';
import { UserShortInfo } from 'src/app/shared/models/user';
import { AccountService } from 'src/app/shared/services/account.service';
import { MessageService } from 'src/app/shared/services/message.service';
import { DataService } from 'src/app/shared/states/data.service';

@Component({
  selector: 'app-message-chatroom',
  templateUrl: './message-chatroom.component.html',
  styleUrls: ['./message-chatroom.component.scss']
})
export class MessageChatroomComponent implements OnInit {

  autoLoadMsg: boolean = true;
  messageContent: string;

  @Input() recipientUserName: string;
  @Input() messageGroupId: string;

  // @Output() sendMessages: EventEmitter<{ content: string, recipientUser: string }>;

  currentUser: UserShortInfo;
  otherUser: UserShortInfo;

  @ViewChild('messageListContent') messageListContent: ElementRef;
  constructor(
    public state: DataService,
    private messageService: MessageService,
  ) {
  }

  private _onDestroy = new Subject();
  ngOnDestroy(): void {
    this._onDestroy.next();
  }

  ngOnInit() {
    this.initProps();
  }

  goDown() {
    this.messageListContent.nativeElement.scrollTop = this.messageListContent.nativeElement.scrollHeight;
  }

  goTop() {
    this.messageListContent.nativeElement.scrollTop = 0;
  }



  initProps() {
    this.currentUser = this.state.query.user;
  }

  sendMessage() {
    if (this.messageContent && this.recipientUserName) {
      let groupId = parseInt(this.messageGroupId);
      this.messageService.sendMessage(this.recipientUserName, groupId, this.messageContent).then(() => {
        this.messageContent = '';
      }).finally(() => {
        this.autoGoMessageDown();
      });
    }
  }

  autoGoMessageDown() {
    setTimeout(() => {
      this.messageListContent.nativeElement.scrollTop = this.messageListContent.nativeElement.scrollHeight;
    }, 100);
  }


}
