export interface IApiResponse<T> {
  isSuccessed: boolean;
  message: string;
  data: T;
}

export interface IApiPagingResponse<T> {
  totalCount: number;
  isSuccessed: boolean;
  message: string;
  data: T;
}

