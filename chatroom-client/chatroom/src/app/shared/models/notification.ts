import { ResPaging } from "./response";
import { UserShortInfo } from "./user";

export enum QAStatus {
  NoResponse,
  Accept,
  Reject
}

export interface Notify {
  id: number;
  appUserId: number;
  appUser: UserShortInfo,
  requestUserId: number;
  requestUser: UserShortInfo,
  mainPhoto: string,
  content: string,
  isAnswerQuestion: boolean,
  readDate: Date,
  notificationType: number,
  qAStatus: QAStatus,
  createdDate: Date
}

export interface NotifyList {
  notifications: ResPaging<Notify>;
  notReadTotalCount: number;
}
