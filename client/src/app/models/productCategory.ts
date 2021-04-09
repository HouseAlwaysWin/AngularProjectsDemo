export interface IProductCategory {
  id: number;
  name: string;
  langCode: string;
  level: number;
  parentId?: number;
  hasChild: boolean;
  index: number;
  children?: IProductCategory[];
}
