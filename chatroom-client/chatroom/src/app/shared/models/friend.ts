import { Message } from "./message";
import { UserShortInfo } from "./user";

export class Friend {
  friendId: number;
  friend: UserShortInfo;
  createdDate: string;
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
