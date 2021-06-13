import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { MessageGroup } from '../models/message';
import { DataService } from '../states/data.service';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  apiUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;

  constructor(private http: HttpClient,
    // private accountQuery: AccountQuery,
    // private accountStore: AccountStore,
    private state: DataService,
  ) { }

  createHubConnection(otherUsername: string, groupId: string) {
    let user = this.state.query.user;
    if (!groupId) groupId = '';
    if (user?.token && otherUsername) {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(`${this.hubUrl}message?username=${otherUsername}&groupId=${groupId}`, {
          accessTokenFactory: () => user.token
        })
        .withAutomaticReconnect()
        .build();

      this.hubConnection.start()
        .catch(error => console.log(error))
        .finally(() => {
        });

      this.hubConnection.on('ReceiveMessageThread', res => {
        console.log('receiveMessageThread')
        console.log(res);
        this.state.store.update({
          messagesThread: res.messages,
          messagesGroups: res.groups
        })
      });


      this.hubConnection.on('NewMessage', message => {
        console.log('newMessage')
        this.state.query.messagesThread$.pipe(take(1)).subscribe(messages => {
          this.state.store.update({
            messagesThread: [...messages, message]
          })
        })
      });


      this.hubConnection.on('UpdatedGroup', (group: MessageGroup) => {
        console.log('UpdatedGroup')
        this.state.query.messagesThread$.pipe(take(1)).subscribe(messages => {
          messages.forEach(message => {
            if (!message.dateRead) {
              message.dateRead = new Date(Date.now())
            }
          })
          this.state.store.update({
            messagesThread: messages
          });
        })
      });
    }
  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }

  async sendMessage(recipientUserName: string, messageGroupId: string, content: string) {
    console.log('SendmessageService')
    return this.hubConnection.invoke('SendMessage', { recipientUserName, content, messageGroupId })
      .catch(error => console.log(error));
  }

  deleteMessage(id: number) {
    return this.http.delete(`${this.apiUrl}message/${id}`);
  }

  getMessageFriendsGroups() {
    return this.http.get(`${this.apiUrl}messages/get-messages-friends-list`);
  }

}
