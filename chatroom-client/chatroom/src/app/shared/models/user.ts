export class UserToken {
  email: string = '';
  userName: string = '';
  token: string = '';
  photos: UserPhoto[]
}

export class UserPhoto {
  id: number;
  url: string;
  isMain: string;
}
