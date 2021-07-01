import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { MessageGroup, MessageWithPageIndex } from '../models/message';
import { Res } from '../models/response';
import { DataService } from '../states/data.service';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  apiUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  public hubConnection: HubConnection;

  constructor(
    private http: HttpClient,
    private state: DataService,
  ) { }

  createHubConnection(otherUsername: string, groupId: number) {
    let user = this.state.query.user;

    if (user?.token && otherUsername) {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(`${this.hubUrl}message?username=${otherUsername}&groupId=${groupId}`, {
          accessTokenFactory: () => user.token
        })
        .withAutomaticReconnect()
        .build();

      this.hubConnection.on('ReceiveMessageThread', (res: MessageWithPageIndex) => {
        console.log('receiveMessageThread')
        this.state.store.update({
          messagesThread: res.messages.data,
          messagesPageIndex: res.pageIndex,
        })

      });

      this.hubConnection.on('MessageGoDown', res => {
        console.log('messageGoBottom')
        console.log(res);
        this.state.store.update({
          messageGoBottom: res
        });
      })


      this.hubConnection.on('NewMessage', res => {
        this.state.query.messagesThread$.pipe(take(1)).subscribe(messages => {
          this.state.store.update({
            messagesThread: [...messages, res.message],
          })
        })
      });

      return this.hubConnection.start()
        .catch(error => console.log(error));

    }
  }

  stopHubConnection(messageGroupId: number) {
    if (this.hubConnection) {
      return this.hubConnection.invoke('OnDisconnectChatRoom', { groupId: messageGroupId })
        .then(() => {
          this.hubConnection.stop();
        });
      // .catch(error => console.log(error));
    }
  }

  async sendMessage(recipientUsername: string, messageGroupId: number, content: string) {
    return this.hubConnection.invoke('SendMessageAsync', { recipientUsername, content, messageGroupId: messageGroupId.toString() })
      .catch(error => console.log(error));
  }

  deleteMessage(id: number) {
    return this.http.delete(`${this.apiUrl}message/${id}`);
  }

  getMessageGroups() {
    return this.http.get(`${this.apiUrl}messages/get-messages-groups-list`);
  }

  getMessagesListPaged(pageIndex: number, pageSize: number, groupId: number) {
    return this.http.get(`${this.apiUrl}messages/get-user-messages-paged?pageIndex=${pageIndex}&pageSize=${pageSize}&groupId=${groupId}`)
  }

}
