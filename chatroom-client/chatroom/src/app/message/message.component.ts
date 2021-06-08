import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { UserInfo } from 'os';
import { of, Subject, Subscription } from 'rxjs';
import { last, map, switchMap, take, takeLast, takeUntil } from 'rxjs/operators';
import { MessageGroup } from '../shared/models/message';
import { UserShortInfo } from '../shared/models/user';
import { AccountService } from '../shared/services/account.service';
import { MessageService } from '../shared/services/message.service';
import { AccountQuery } from '../shared/states/account/account.query';
import { SharedStore } from '../shared/states/shared/shared.store';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss']
})
export class MessageComponent implements OnInit, OnDestroy {
  messageGroupList: MessageGroup[];
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
    this.getMessageGroups();
  }

  getMessageGroups() {
    this.messageService.getMessageGroups().subscribe((groups: MessageGroup[]) => {
      console.log(groups);
      this.messageGroupList = groups;
    });
  }

}
