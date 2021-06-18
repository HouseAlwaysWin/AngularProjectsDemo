import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { NotifyList } from '../models/notification';
import { DataService } from '../states/data.service';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {

  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  constructor(
    private state: DataService
  ) { }


  createHubConnection() {
    let user = this.state.query.user;
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
      this.state.store.update({
        usersOnline: usernames
      })
    });

    this.hubConnection.on('UserIsOnline', username => {
      this.state.query.usersOnline$.pipe(
        take(1)
      ).subscribe(usernames => {
        console.log('userIsOnline');
        console.log(usernames);
        this.state.store.update({
          usersOnline: [...usernames, username]
        })
      })

    })

    this.hubConnection.on('UserIsOffline', username => {
      this.state.query.usersOnline$.pipe(
        take(1)
      ).subscribe(usernames => {
        this.state.store.update({
          usersOnline: [...usernames.filter(x => x !== username)]
        })
      })
    })

    this.hubConnection.on('GetNotifications', (notifies) => {
      console.log('GetNotifications')
      console.log(notifies);
      console.log(notifies.notifications.data);
      console.log(notifies.notReadTotalCount);
      this.state.store.update({
        notifies: notifies.notifications.data,
        notifyNotReadCount: notifies.notReadTotalCount
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
