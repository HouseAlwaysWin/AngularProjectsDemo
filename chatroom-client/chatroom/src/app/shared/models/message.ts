export class Message {
  id: number;
  senderId: number;
  senderUsername: string;
  senderPhotoUrl: string;
  // recipientId: number;
  // recipientUsername: string;
  // recipientPhotoUrl: string;
  recipientUsers: MessageRecivedUser[];
  content: string;
  dateRead?: Date;
  messageSent: Date;
}

export interface MessageGroup {
  id: number;
  name: string;
  connections: MessageConnection[]
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
