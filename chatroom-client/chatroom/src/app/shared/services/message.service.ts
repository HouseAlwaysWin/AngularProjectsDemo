import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Message } from 'primeng/api';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  apiUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;

  constructor(private http: HttpClient) { }


  getMessagethread(username: string) {
    return this.http.get<Message[]>(`${this.apiUrl}message/thread/${username}`)
  }
}
