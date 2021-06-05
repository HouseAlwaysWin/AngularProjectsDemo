import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { UserInfo } from 'os';
import { of, Subject, Subscription } from 'rxjs';
import { last, map, switchMap, take, takeLast, takeUntil } from 'rxjs/operators';
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
  autoLoadMsg: boolean = true;
  messageContent: string;
  recipientUserName: string;
  currentUser: UserShortInfo;
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
    this.recipientUserName = this.activeRoute.snapshot.paramMap.get('username');
    // this.getMessageThreads();
    console.log('init');
  }

  ngAfterContentInit(): void {
    //Called after ngOnInit when the component's or directive's content has been initialized.
    //Add 'implements AfterContentInit' to the class.
    console.log('afterContentInit');
  }

  ngAfterViewInit(): void {
    //Called after ngAfterContentInit when the component's view has been initialized. Applies to components only.
    //Add 'implements AfterViewInit' to the class.
    console.log('afterViewInit');
    this.getCurrentUser();
  }



  getCurrentUser() {
    console.log(this.accountQuery.user);
    // this.accountQuery.user$
    //   .subscribe(user => {
    //     console.log(user);
    //     this.getMessageThreads();
    //   });
  }

  sendMessage() {
    if (this.messageContent && this.recipientUserName) {
      this.sharedStore.update({
        gLoading: true
      });
      this.messageService.sendMessage(this.recipientUserName, this.messageContent).then(() => {
        this.messageContent = '';
      }).finally(() => {
        this.sharedStore.update({
          gLoading: false
        });
      });
    }

  }



  getMessageThreads() {
    if (this.currentUser && this.recipientUserName) {
      console.log('messageThreads');
      this.messageService.createHubConnection(this.currentUser, this.recipientUserName);
    }
  }

}
