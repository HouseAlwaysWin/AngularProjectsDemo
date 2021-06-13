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

  currentUser: UserShortInfo;
  otherUser: UserShortInfo;

  @ViewChild('messageListContent') messageListContent: ElementRef;
  constructor(
    public state: DataService,
    private activeRoute: ActivatedRoute,
    private accountService: AccountService,
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

  // ngAfterViewChecked() {
  //   this.messageListContent.nativeElement.scrollTop = this.messageListContent.nativeElement.scrollHeight;
  // }

  public goDown() {
    this.messageListContent.nativeElement.scrollTop = this.messageListContent.nativeElement.scrollHeight;
  }

  goTop() {
    this.messageListContent.nativeElement.scrollTop = 0;
  }



  initProps() {
    // this.recipientUserName = this.activeRoute.snapshot.paramMap.get('username');
    // this.messageGroupId = this.activeRoute.snapshot.paramMap.get('messageGroupId');
    this.currentUser = this.state.query.user;
  }

  sendMessage() {
    if (this.messageContent && this.recipientUserName) {
      this.messageService.sendMessage(this.recipientUserName, this.messageGroupId, this.messageContent).then(() => {
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



  // getMessageThreads() {
  //   console.log('chatRoom-getMessageThreads');
  //   this.messageService.createHubConnection(this.currentUser, this.recipientUserName);
  // }

  // stopConnection() {
  //   this.messageService.stopHubConnection();
  // }

}
