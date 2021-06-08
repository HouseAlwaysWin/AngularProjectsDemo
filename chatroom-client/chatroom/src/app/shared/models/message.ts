export class Message {
  id: number;
  senderId: number;
  senderUsername: string;
  senderPhotoUrl: string;
  recipientUsers: MessageRecivedUser[];
  content: string;
  dateRead?: Date;
  messageSent: Date;
}

export interface MessageGroup {
  id: number;
  alternateId: string;
  groupName: string;
  groupOtherName: string;
  groupImg: string;
  groupOtherImg: string;
  groupType: number;
  messages: Message[];
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
