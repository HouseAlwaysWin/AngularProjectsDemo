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


    this.hubConnection.on('UpdateGroup', (newGroup) => {
      console.log('updateGroup');
      let currentGroup = this.state.query.messageGroups;
      let targetGroup = currentGroup.filter(g => g.id === newGroup.id)[0];
      /// check group is exist.
      if (targetGroup) {
        targetGroup.lastMessages = newGroup.lastMessages;
        targetGroup.unreadCount = newGroup.unreadCount;
        this.state.store.update({
          messagesGroups: currentGroup
        });
      }
      else {
        this.state.query.messageGroups$.pipe(take(1)).subscribe(group => {
          this.state.store.update({
            messagesGroups: [...group, newGroup]
          });
        });
      }
    });


    this.hubConnection.on('GetMessagesUnreadTotalCount', totalCount => {
      this.state.store.update({
        messageUnreadCount: totalCount
      });
    });


    this.hubConnection.on('GetOnlineUsers', (usernames: string[]) => {
      this.state.store.update({
        usersOnline: usernames
      })
    });

    this.hubConnection.on('UserIsOnline', username => {
      this.state.query.usersOnline$.pipe(
        take(1)
      ).subscribe(usernames => {
        this.state.store.update({
          usersOnline: [...usernames, username]
        })
      })

    })

    this.hubConnection.on('UserIsOfflineAsync', username => {
      this.state.query.usersOnline$.pipe(
        take(1)
      ).subscribe(usernames => {
        this.state.store.update({
          usersOnline: [...usernames.filter(x => x !== username)]
        })
      })
    })

    this.hubConnection.on('GetNotifications', (notifies) => {
      this.state.store.update({
        notifies: notifies.notifications.data,
        notifyNotReadCount: notifies.notReadTotalCount
      });
    });

  }

  async sendFriendRequest(friendId: number) {
    return this.hubConnection.invoke('SendFriendRequestAsync', friendId)
      .catch(error => console.log(error));
  }

  stopHubConnection() {
    this.hubConnection.stop().catch(error => console.log(error));
  }
}
