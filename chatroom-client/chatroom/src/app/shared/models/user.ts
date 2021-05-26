export class UserToken {
  email: string = '';
  userName: string = '';
  token: string = '';
  photos: UserPhoto[]
}

export class UserPhoto {
  url: string;
  isMain: string;
}
