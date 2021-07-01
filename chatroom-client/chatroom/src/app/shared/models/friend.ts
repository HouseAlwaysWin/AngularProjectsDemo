import { Message } from "./message";
import { Notify, NotifyList } from "./notification";
import { ResPaging } from "./response";
import { UserShortInfo } from "./user";

export class Friend {
  friendId: number;
  friend: UserShortInfo;
  messageGroupId: number;
  createdDate: string;
}

export interface AcceptFriend {
  notifications: NotifyList;
  friends: Friend[];
}

// export class FriendInfo {
//   id: number = 0;
//   userName: string = '';
//   email: string = ''
//   userPublicId: '';
//   userInfoDto: UserInfo = null;
//   messageSent: Message[] = [];
//   messagesReceived: Message[] = [];
//   photos: UserPhoto[] = [];
// }
