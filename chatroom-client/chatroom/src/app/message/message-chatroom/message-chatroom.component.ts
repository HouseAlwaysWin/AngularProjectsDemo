import { Parser } from '@angular/compiler/src/ml_parser/parser';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { take } from 'rxjs/operators';
import { Message, MessageWithPageIndex } from 'src/app/shared/models/message';
import { UserShortInfo } from 'src/app/shared/models/user';
import { AccountService } from 'src/app/shared/services/account.service';
import { MessageService } from 'src/app/shared/services/message.service';
import { DataService } from 'src/app/shared/states/data.service';

@Component({
  selector: 'app-message-chatroom',
  templateUrl: './message-chatroom.component.html',
  styleUrls: ['./message-chatroom.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MessageChatroomComponent implements OnInit {

  autoLoadMsg: boolean = true;
  messageContent: string;

  @Input() recipientUserName: string;
  @Input() messageGroupId: string;

  // @Output() sendMessages: EventEmitter<{ content: string, recipientUser: string }>;

  currentUser: UserShortInfo;
  otherUser: UserShortInfo;
  pageIndex: number;
  pageSize: number;

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

  checkScroll() {
    console.log(this.messageListContent.nativeElement.scrollTop);
    if (this.messageListContent.nativeElement.scrollTop === 0) {
      console.log('call message');
      if (this.state.query.messagesPageIndex > 0) {
        this.pageIndex = this.state.query.messagesPageIndex - 1;
      }
      this.state.store.update({
        messagesPageIndex: this.pageIndex
      });
      this.pageSize = 10;
      if (this.pageIndex > 0) {
        this.messageService.getMessagesListPaged(this.pageIndex, this.pageSize, parseInt(this.messageGroupId))
          .subscribe((res: any) => {
            console.log(res);
            this.messageListContent.nativeElement.scrollTop = 1;
            this.state.query.messagesThread$.pipe(take(1)).subscribe(msg => {
              this.state.store.update({
                messagesThread: [...res.data.data, ...msg]
              })
            })
          })
      }

    }
  }


  initProps() {
    this.currentUser = this.state.query.user;
    setTimeout(() => {
      this.goDown();
    }, 100);
  }

  sendMessage() {
    if (this.messageContent && this.recipientUserName) {
      let groupId = parseInt(this.messageGroupId);
      this.messageService.sendMessage(this.recipientUserName, groupId, this.messageContent).then(() => {
        this.messageContent = '';
        console.log(this.messageContent);
      }).finally(() => {
        this.autoGoMessageDown();
        this.messageContent = '';
      });
    }
  }

  autoGoMessageDown() {
    setTimeout(() => {
      this.messageListContent.nativeElement.scrollTop = this.messageListContent.nativeElement.scrollHeight;
    }, 100);
  }


}
