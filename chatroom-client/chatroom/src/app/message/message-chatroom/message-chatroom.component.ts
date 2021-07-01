import { ChangeDetectionStrategy, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { UntilDestroy } from '@ngneat/until-destroy';
import { Subject } from 'rxjs';
import { take } from 'rxjs/operators';
import { UserShortInfo } from 'src/app/shared/models/user';
import { MessageService } from 'src/app/shared/services/message.service';
import { DataService } from 'src/app/shared/states/data.service';

@UntilDestroy({ checkProperties: true })
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
  @Input() messageGroupId: number;

  // @Output() sendMessages: EventEmitter<{ content: string, recipientUser: string }>;

  currentUser: UserShortInfo;
  otherUser: UserShortInfo;
  pageIndex: number;
  pageSize: number;
  showGoBottomBtn: boolean = false;
  hideGoBottomBtn: boolean = false;
  initHide: boolean = false;

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
    this.autoGoDown();
  }


  ngAfterViewInit() {
    this.autoGoDown();
  }

  autoGoDown() {
    this.state.query.messageGoBottom$.subscribe(res => {
      console.log('autoGoDown');
      console.log(res);
      if (res) {
        this.autoGoMessageDown();
        this.state.store.update({
          messageGoBottom: false
        });
      }
    })
  }


  goBottom() {
    this.messageListContent.nativeElement.scrollTop =
      this.messageListContent.nativeElement.scrollHeight - this.messageListContent.nativeElement.offsetHeight;
  }

  checkGoBottomBtn() {
    var sh = Math.trunc(this.messageListContent.nativeElement.scrollHeight);
    var st = Math.trunc(this.messageListContent.nativeElement.scrollTop);
    var ht = Math.trunc(this.messageListContent.nativeElement.offsetHeight);
    if ((st !== sh - ht)) {
      this.showGoBottomBtn = true;
      // after first time init then add hide class.
      if (this.initHide) {
        console.log(this.initHide);
        this.hideGoBottomBtn = false;
      }
    }
    else {
      this.showGoBottomBtn = false;
      if (this.initHide) {
        this.hideGoBottomBtn = true;
      }
    }
  }

  checkScroll() {
    this.checkGoBottomBtn();
    if (this.messageListContent.nativeElement.scrollTop === 0) {
      if (this.state.query.messagesPageIndex > 0) {
        this.pageIndex = this.state.query.messagesPageIndex - 1;
      }
      this.state.store.update({
        messagesPageIndex: this.pageIndex
      });
      this.pageSize = 10;
      if (this.pageIndex > 0) {
        this.messageService.getMessagesListPaged(this.pageIndex, this.pageSize, this.messageGroupId)
          .subscribe((res: any) => {
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
    this.autoGoMessageDown();
  }

  sendMessage() {
    if (this.messageContent && this.recipientUserName) {
      this.messageService.sendMessage(this.recipientUserName, this.messageGroupId, this.messageContent).then(() => {
        this.messageContent = '';
      }).finally(() => {
        this.messageContent = '';
        this.autoGoMessageDown();
      });
    }
  }

  autoGoMessageDown() {
    setTimeout(() => {
      this.goBottom();
    }, 100);

    this.initHide = false;
    setTimeout(() => {
      this.initHide = true;
    }, 1000);
  }
}
