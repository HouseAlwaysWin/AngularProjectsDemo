import { IPicture } from "./picture";

export interface IProduct {
  id: number;
  name: string;
  description: string;
  price: number;
  productCategory: IProductCategory;
  productAttributes: IProductAttribute[];
  productPictures: IPicture[];
}

export interface IProductAttribute {
  id: number;
  name: string;
  productAttributeValue: IProductAttributeValue[];
}

export interface IProductAttributeValue {
  id: number;
  name: string;
  priceAdjustment: number;
  Quantity: number;
  seqIndex: number;
}

export interface IProductCategory {
  id: number;
  name: string;
  level: number;
  parentId?: number;
  hasChild: boolean;
  seqNo: number;
  children: IProductCategory[];
}
