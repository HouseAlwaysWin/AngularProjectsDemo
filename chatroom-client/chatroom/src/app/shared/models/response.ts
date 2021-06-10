
export class Res<T>{
  isSuccessed: boolean = false;
  message: string = '';
  data: T = null;
}

export class ResPaging<T>{
  totalCount: number;
  data: T[] = null;
}
