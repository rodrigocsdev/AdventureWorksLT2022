export interface Product {
  productID?: number;
  name: string;
  productNumber: string;
  color?: string;
  standardCost: number;
  listPrice: number;
  size?: string;
  weight?: number;
  productCategoryID?: number;
  productModelID?: number;
  sellStartDate: string;
  sellEndDate?: string;
  discontinuedDate?: string;
  thumbNailPhoto?: Uint8Array;
  thumbnailPhotoFileName?: string;
  rowGuid: string;
  modifiedDate: string;
}
