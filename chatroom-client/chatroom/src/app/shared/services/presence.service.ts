import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AccountQuery } from '../states/account/account.query';
import { AccountStore } from '../states/account/account.store';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {

  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  constructor(
    private accountStore: AccountStore,
    private accountQuery: AccountQuery) { }


  createHubConnection() {
    let token = localStorage.getItem('token');
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('GetOnlineUsers', (usernames: string[]) => {
      this.accountStore.update({
        usersOnline: usernames
      })
    });

    this.hubConnection.on('UserIsOnline', username => {
      console.log(username);
      this.accountQuery.usersOnline$.pipe(
        take(1)
      ).subscribe(usernames => {
        this.accountStore.update({
          usersOnline: [...usernames, username]
        })
      })

    })

    this.hubConnection.on('UserIsOffline', username => {
      console.log(username);
      this.accountQuery.usersOnline$.pipe(
        take(1)
      ).subscribe(usernames => {
        this.accountStore.update({
          usersOnline: [...usernames.filter(x => x !== username)]
        })
      })

    })
  }
}
