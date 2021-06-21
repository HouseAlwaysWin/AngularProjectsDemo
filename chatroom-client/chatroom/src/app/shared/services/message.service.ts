import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { MessageGroup } from '../models/message';
import { Res } from '../models/response';
import { DataService } from '../states/data.service';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  apiUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;

  constructor(
    private http: HttpClient,
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
        // let currentGroup = this.state.query.messageGroups;
        // let targetGroup = currentGroup.filter(g => g.id === res.group.id)[0];
        // /// check group is exist.
        // if (targetGroup) {
        //   targetGroup.lastMessages = res.group.lastMessages;
        //   console.log(currentGroup);
        //   this.state.store.update({
        //     messagesGroups: currentGroup
        //   });
        // }
        // else {
        //   this.state.query.messageGroups$.pipe(take(1)).subscribe(group => {
        //     console.log(group);
        //     this.state.store.update({
        //       messagesGroups: [...group, res.group]
        //     });
        //   });
        // }

        this.state.store.update({
          messagesThread: res.messages,
        })

      });


      this.hubConnection.on('NewMessage', res => {
        // console.log('newMessage')
        // console.log(res);
        // let currentGroup = this.state.query.messageGroups;
        // let targetGroup = currentGroup.filter(g => g.id === res.group.id)[0];
        // /// check group is exist.
        // if (targetGroup) {
        //   targetGroup.lastMessages = res.group.lastMessages;
        //   console.log(currentGroup);
        //   this.state.store.update({
        //     messagesGroups: currentGroup
        //   });
        // }
        // else {
        //   this.state.query.messageGroups$.pipe(take(1)).subscribe(group => {
        //     console.log(group);
        //     this.state.store.update({
        //       messagesGroups: [...group, res.group]
        //     });
        //   });
        // }

        this.state.query.messagesThread$.pipe(take(1)).subscribe(messages => {
          console.log(messages);
          this.state.store.update({
            messagesThread: [...messages, res.message],
          })
        })
      });


      // this.hubConnection.on('UpdatedGroup', (group: MessageGroup) => {
      //   console.log('UpdatedGroup')
      //   this.state.query.messagesThread$.pipe(take(1)).subscribe(messages => {
      //     messages.forEach(message => {
      //       if (!message.dateRead) {
      //         message.dateRead = new Date(Date.now())
      //       }
      //     })
      //     this.state.store.update({
      //       messagesThread: messages
      //     });
      //   })
      // });
    }
  }

  stopHubConnection() {
    if (this.hubConnection) {
      console.log('stop');
      this.hubConnection.stop();
    }
  }

  async sendMessage(recipientUserName: string, messageGroupId: number, content: string) {
    return this.hubConnection.invoke('SendMessageAsync', { recipientUserName, content, messageGroupId })
      .catch(error => console.log(error));
  }

  deleteMessage(id: number) {
    return this.http.delete(`${this.apiUrl}message/${id}`);
  }

  getMessageGroups() {
    return this.http.get(`${this.apiUrl}messages/get-messages-groups-list`);
  }

}
