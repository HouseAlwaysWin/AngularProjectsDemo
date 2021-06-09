import { UserShortInfo } from "./user";

export interface Notify {
  id: number;
  appUserId: number;
  appUser: UserShortInfo,
  requestUserId: number;
  requestUser: UserShortInfo,
  mainPhoto: string,
  content: string,
  isRead: boolean,
  notificationType: number,
  createdDate: Date
}
