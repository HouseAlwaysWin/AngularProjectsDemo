import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserInfo } from 'os';
import { of, Subject, Subscription } from 'rxjs';
import { last, map, switchMap, take, takeLast, takeUntil } from 'rxjs/operators';
import { MessageGroup } from '../shared/models/message';
import { Res } from '../shared/models/response';
import { UserShortInfo } from '../shared/models/user';
import { AccountService } from '../shared/services/account.service';
import { MessageService } from '../shared/services/message.service';
import { AccountQuery } from '../shared/states/account/account.query';
import { SharedStore } from '../shared/states/shared/shared.store';
import { MessageChatroomComponent } from './message-chatroom/message-chatroom.component';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss']
})
export class MessageComponent implements OnInit, OnDestroy {
  messageGroupList: MessageGroup[] = [];
  currentUser: UserShortInfo;
  currentChatroomUser: string;
  currentChatroomGroupId: string;

  @ViewChild('chatroom') chatroom: MessageChatroomComponent;

  constructor(
    private router: Router,
    private activeRoute: ActivatedRoute,
    private accountService: AccountService,
    private messageService: MessageService,
    public accountQuery: AccountQuery,
    private sharedStore: SharedStore) {
  }

  private _onDestroy = new Subject();
  ngOnDestroy(): void {
    this._onDestroy.next();
    this.messageService.stopHubConnection();
  }


  ngOnInit() {
    this.currentUser = this.accountQuery.user;
    this.currentChatroomUser = this.activeRoute.snapshot.paramMap.get('username');
    this.currentChatroomGroupId = this.activeRoute.snapshot.paramMap.get('messageGroupId');
    if (this.currentChatroomUser || this.currentChatroomGroupId) {
      this.getMessageThread(this.currentChatroomUser, this.currentChatroomGroupId);
    }
    this.getMessageGroups();
  }

  getMessageGroups() {
    this.messageService.getMessageFriendsGroups().subscribe((res: Res<MessageGroup[]>) => {
      this.messageGroupList = res.data;
    });
  }


  getMessageThread(username: string, groupId: string) {
    this.currentChatroomUser = username;
    this.currentChatroomGroupId = groupId;
    this.messageService.createHubConnection(username, groupId);
  }
}
