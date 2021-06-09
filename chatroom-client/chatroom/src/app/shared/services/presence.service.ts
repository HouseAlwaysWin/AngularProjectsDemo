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
    // let token = localStorage.getItem('token');
    console.log('presence create connection');
    let user = this.accountQuery.user;
    console.log(user);
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .catch(error => console.log(error));


    this.hubConnection.on('GetOnlineUsers', (usernames: string[]) => {
      console.log('getOnlineUsers')
      console.log(usernames)
      this.accountStore.update({
        usersOnline: usernames
      })
    });

    this.hubConnection.on('UserIsOnline', username => {
      console.log('UserIsOnline')
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
      console.log('UserIsOffline')
      console.log(username);
      this.accountQuery.usersOnline$.pipe(
        take(1)
      ).subscribe(usernames => {
        this.accountStore.update({
          usersOnline: [...usernames.filter(x => x !== username)]
        })
      })
    })

    this.hubConnection.on('GetNotifications', notifies => {
      console.log(notifies);
      this.accountStore.update({
        notifies: notifies
      });
    });

  }

  async sendFriendRequest(friendId: number) {
    return this.hubConnection.invoke('SendFriendRequest', friendId)
      .catch(error => console.log(error));
  }

  stopHubConnection() {
    this.hubConnection.stop().catch(error => console.log(error));
  }
}
