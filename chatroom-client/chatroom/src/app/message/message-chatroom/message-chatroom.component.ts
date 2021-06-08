import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
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

  recipientUserName: string;
  messageGroupId: string;
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
    this.messageService.stopHubConnection();
    this._onDestroy.next();
  }


  ngOnInit() {
    this.initProps();
    this.getMessageThreads();
  }

  ngAfterViewChecked() {
    this.messageListContent.nativeElement.scrollTop = this.messageListContent.nativeElement.scrollHeight;

  }

  initProps() {
    this.recipientUserName = this.activeRoute.snapshot.paramMap.get('username');
    this.messageGroupId = this.activeRoute.snapshot.paramMap.get('messageGroupId');
    this.currentUser = this.accountQuery.user;
  }

  sendMessage() {
    if (this.messageContent && this.recipientUserName) {
      this.sharedStore.update({
        gLoading: true
      });
      this.messageService.sendMessage(this.recipientUserName, this.messageContent, this.messageGroupId).then(() => {
        this.messageContent = '';
      }).finally(() => {
        this.sharedStore.update({
          gLoading: false
        });
      });
    }

  }



  getMessageThreads() {
    console.log('messageThreads');
    this.messageService.createHubConnection(this.currentUser, this.recipientUserName);
  }

}
