// export class UserToken {
//   email: string = '';
//   userName: string = '';
//   token: string = '';
// }

export class UserDetail {
  email: string = '';
  userName: string = '';
  token: string = '';
  photos: UserPhoto[];
  userInfo: UserInfo;
  address: UserAddress;
  emailConfirmed: boolean;
  phoneNumber: string;
  phoneNumberConfirm: boolean;
  twoFactorEnabled: boolean;
  lockoutEnd: boolean;
  accessFailedCount: number;
  userPublicId: string;
}

export class UserAddress {
  firstName: string;
  lastName: string;
  street: string;
  city: string;
  state: string;
  zipCode: string;
}

export class UserPhoto {
  id: number;
  url: string;
  isMain: boolean;
}

export class UserInfo {
  dateOfBirth: string;
  gender: string;
  introduction: string;
  knownAs: string;
  lastActive: string;
}
