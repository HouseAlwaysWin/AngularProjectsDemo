import { Component, ElementRef, OnDestroy, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserInfo } from 'os';
import { of, Subject, Subscription } from 'rxjs';
import { last, map, switchMap, take, takeLast, takeUntil } from 'rxjs/operators';
import { Message, MessageGroup } from '../shared/models/message';
import { Res } from '../shared/models/response';
import { UserShortInfo } from '../shared/models/user';
import { AccountService } from '../shared/services/account.service';
import { MessageService } from '../shared/services/message.service';
import { DataService } from '../shared/states/data.service';
import { MessageChatroomComponent } from './message-chatroom/message-chatroom.component';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss']
})
export class MessageComponent implements OnInit, OnDestroy {
  messageGroupList: MessageGroup[] = [];
  messageThread: Message[];
  currentUser: UserShortInfo;
  currentChatroomUser: string;
  currentChatroomGroupId: string;


  @ViewChild('chatroom') chatroom: MessageChatroomComponent;

  constructor(
    private router: Router,
    private activeRoute: ActivatedRoute,
    private accountService: AccountService,
    private messageService: MessageService,
    private state: DataService
  ) {
  }

  private _onDestroy = new Subject();
  ngOnDestroy(): void {
    this._onDestroy.next();
    this.messageService.stopHubConnection();
  }


  ngOnInit() {
    this.currentUser = this.state.query.user;
    this.currentChatroomUser = this.activeRoute.snapshot.paramMap.get('username');
    this.currentChatroomGroupId = this.activeRoute.snapshot.paramMap.get('messageGroupId');
    if (this.currentChatroomUser || this.currentChatroomGroupId) {
      this.getMessageThread(this.currentChatroomUser, this.currentChatroomGroupId);
    }
    this.getMessageGroups();
    this.autoGoDown();
  }

  autoGoDown() {
    this.state.query.messagesThread$.subscribe(m => {
      if (this.chatroom) {
        setTimeout(() => {
          this.chatroom.goDown();
        });
      }
    });
  }


  getMessageGroups() {
    this.state.query.messageGrouops$.subscribe(res => {
      this.messageGroupList = res;
    })
    this.messageService.getMessageFriendsGroups().subscribe((res: Res<MessageGroup[]>) => {
      console.log(res);
      this.messageGroupList = res.data;
    });
  }


  getMessageThread(username: string, groupId: string) {
    this.messageService.stopHubConnection();

    this.currentChatroomUser = username;
    this.currentChatroomGroupId = groupId;
    this.messageService.createHubConnection(username, groupId);
  }
}
