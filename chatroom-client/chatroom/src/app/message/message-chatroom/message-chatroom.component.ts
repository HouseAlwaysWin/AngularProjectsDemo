import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { UserShortInfo } from 'src/app/shared/models/user';
import { AccountService } from 'src/app/shared/services/account.service';
import { MessageService } from 'src/app/shared/services/message.service';
import { AccountQuery } from 'src/app/shared/states/account/account.query';
import { SharedStore } from 'src/app/shared/states/shared/shared.store';

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
    private activeRoute: ActivatedRoute,
    private accountService: AccountService,
    private messageService: MessageService,
    public accountQuery: AccountQuery,
    private sharedStore: SharedStore) {
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

  goDown() {
    this.messageListContent.nativeElement.scrollTop = this.messageListContent.nativeElement.scrollHeight;
  }

  goTop() {
    this.messageListContent.nativeElement.scrollTop = 0;
  }



  initProps() {
    // this.recipientUserName = this.activeRoute.snapshot.paramMap.get('username');
    // this.messageGroupId = this.activeRoute.snapshot.paramMap.get('messageGroupId');
    this.currentUser = this.accountQuery.user;
  }

  sendMessage() {
    if (this.messageContent && this.recipientUserName) {
      this.sharedStore.update({
        gLoading: true
      });
      this.messageService.sendMessage(this.recipientUserName, this.messageGroupId, this.messageContent).then(() => {
        this.messageContent = '';
      }).finally(() => {
        setTimeout(() => {
          this.messageListContent.nativeElement.scrollTop = this.messageListContent.nativeElement.scrollHeight;
        }, 100);
        this.sharedStore.update({
          gLoading: false
        });
      });
    }

  }



  // getMessageThreads() {
  //   console.log('chatRoom-getMessageThreads');
  //   this.messageService.createHubConnection(this.currentUser, this.recipientUserName);
  // }

  // stopConnection() {
  //   this.messageService.stopHubConnection();
  // }

}
