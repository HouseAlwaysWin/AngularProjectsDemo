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
    this.currentChatroomUser = this.state.query.friendRedirectParam;;
    if (this.currentChatroomUser) {
      this.messageService.stopHubConnection();
      this.messageService.createHubConnection(this.currentChatroomUser, this.currentChatroomGroupId);
    }

    this.getMessageGroups();
    this.autoGoDown();
  }


  autoGoDown() {
    this.state.query.messagesThread$.subscribe(m => {
      this.messageThread = m;
      if (this.chatroom) {
        setTimeout(() => {
          this.chatroom.goDown();
        });
      }
    });
  }


  getMessageGroups() {
    this.state.query.messageGroups$.subscribe(groups => {
      this.messageGroupList = groups;
    })
    this.messageService.getMessageGroups().subscribe((res: Res<MessageGroup[]>) => {
      console.log('getMessageFriendsGroups');
      console.log(res);
      this.messageGroupList = res.data;
      this.state.store.update({
        messagesGroups: res.data
      });
    });
  }


  getMessageThread(group: MessageGroup) {

    this.messageService.stopHubConnection();
    this.currentChatroomUser = (this.currentUser.userName === group.groupName) ? group.groupOtherName : group.groupName;
    this.currentChatroomGroupId = group.id.toString();
    this.messageService.createHubConnection(this.currentChatroomUser, this.currentChatroomGroupId);
  }

}
