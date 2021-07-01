import { ResPaging } from "./response";


export class Message {
  id: number;
  senderId: number;
  senderUsername: string;
  mainPhoto: string;
  recipientUsers: MessageRecivedUser[];
  content: string;
  dateRead?: Date;
  messageSent: Date;
}

export class MessageWithPageIndex {
  messages: ResPaging<Message>;
  pageIndex: 0
}

export interface MessageGroup {
  id: number;
  // alternateId: string;
  groupName: string;
  groupOtherName: string;
  groupImg: string;
  groupOtherImg: string;
  groupType: number;
  messages: Message[];
  lastMessages: string;
  unreadCount: number;
}

export interface MessageConnection {
  messageConnectionId: string;
  userName: string;
}

export interface MessageRecivedUser {
  id: number;
  appUserId: number;
  userName: string;
  userMainPhoto: string;
  dateRead: Date
}
