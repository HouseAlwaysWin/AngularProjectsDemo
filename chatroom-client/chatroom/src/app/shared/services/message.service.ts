import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { MessageGroup } from '../models/message';
import { UserShortInfo } from '../models/user';
import { AccountQuery } from '../states/account/account.query';
import { AccountStore } from '../states/account/account.store';
import { SharedStore } from '../states/shared/shared.store';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  apiUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;

  constructor(private http: HttpClient,
    private accountQuery: AccountQuery,
    private accountStore: AccountStore,
    private sharedStore: SharedStore
  ) { }

  createHubConnection(user: UserShortInfo, otherUsername: string) {
    if (user?.token && otherUsername) {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(`${this.hubUrl}message?username=${otherUsername}`, {
          accessTokenFactory: () => user.token
        })
        .withAutomaticReconnect()
        .build();

      this.hubConnection.start()
        .catch(error => console.log(error))
        .finally(() => {
          this.sharedStore.update({ gLoading: false });
        });

      this.hubConnection.on('ReceiveMessageThread', messages => {
        console.log('receiveMessageThread')
        console.log(messages[messages.length - 1]);
        this.accountStore.update({
          messagesThread: messages
        })
      });


      this.hubConnection.on('NewMessage', message => {
        console.log('newMessage')
        this.accountQuery.messagesThread$.pipe(take(1)).subscribe(messages => {
          console.log(messages[messages.length - 1])
          this.accountStore.update({
            messagesThread: [...messages, message]
          })
        })
      });

      this.hubConnection.on('UpdatedGroup', (group: MessageGroup) => {
        console.log('UpdatedGroup')
        console.log(group)
        // if (group.connections.some(x => x.userName === otherUsername)) {
        this.accountQuery.messagesThread$.pipe(take(1)).subscribe(messages => {
          messages.forEach(message => {
            if (!message.dateRead) {
              message.dateRead = new Date(Date.now())
            }
          })
          this.accountStore.update({
            messagesThread: messages
          });
        })
        // }
      });
    }
  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }

  async sendMessage(recipientUserName: string, content: string) {
    return this.hubConnection.invoke('SendMessage', { recipientUserName, content })
      .catch(error => console.log(error));
  }

  deleteMessage(id: number) {
    return this.http.delete(`${this.apiUrl}message/${id}`);
  }

}
