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
    this.messageService.stopHubConnection(this.currentChatroomGroupId);
  }


  ngOnInit() {
    this.currentUser = this.state.query.user;
    this.currentChatroomUser = this.state.query.friendRedirectParam;;
    if (this.currentChatroomUser) {
      this.messageService.stopHubConnection(this.currentChatroomGroupId);
      this.messageService.createHubConnection(this.currentChatroomUser, this.currentChatroomGroupId)
        .finally(() => {
          this.autoGoDown();
        });
    }

    this.getMessageGroups();
  }


  autoGoDown() {
    if (this.chatroom) {
      setTimeout(() => {
        this.chatroom.goDown();
      }, 100);
    }
  }


  getMessageGroups() {
    // this.state.query.messageGroups$.subscribe(groups => {
    //   this.messageGroupList = groups;
    // })
    this.messageService.getMessageGroups().subscribe((res: Res<MessageGroup[]>) => {
      console.log('getMessageGroups');
      console.log(res);
      this.messageGroupList = res.data;
      this.state.store.update({
        messagesGroups: res.data
      });
    });
  }


  getMessageThread(group: MessageGroup) {

    if (this.messageService.hubConnection) {
      this.messageService.stopHubConnection(this.currentChatroomGroupId).then(() => {
        this.currentChatroomUser = (this.currentUser.userName === group.groupName) ? group.groupOtherName : group.groupName;
        this.currentChatroomGroupId = group.alternateId;
        this.messageService.createHubConnection(this.currentChatroomUser, this.currentChatroomGroupId)
          .finally(() => {
            this.autoGoDown();
          });
      });
    }
    else {
      this.currentChatroomUser = (this.currentUser.userName === group.groupName) ? group.groupOtherName : group.groupName;
      this.currentChatroomGroupId = group.alternateId;
      this.messageService.createHubConnection(this.currentChatroomUser, this.currentChatroomGroupId)
        .finally(() => {
          this.autoGoDown();
        });;

    }

  }

}
