export class Message {
  id: number;
  senderId: number;
  senderUsername: string;
  senderPhotoUrl: string;
  recipientId: number;
  recipientUsername: string;
  recipientPhotoUrl: string;
  content: string;
  dateRead?: Date;
  messageSent: Date;
}

export interface MessageGroup {
  name: string;
  connections: MessageConnection[]
}

export interface MessageConnection {
  messageConnectionId: string;
  userName: string;
}
