export interface IApiResponse<T> {
  isSuccessed: boolean;
  message: string;
  data: T;
}

export interface IApiPagingResponse<T> {
  pageIndex: number;
  pageSize: number;
  totalCount: number;
  isSuccessed: boolean;
  message: string;
  data: T;
}

