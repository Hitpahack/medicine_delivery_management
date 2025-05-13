export interface ProductDto {
  id: number;
  PharmacyId: number;
  SupplierId?: number;
  Quantity: number;
  Priority?: string;
  Status?: string;
  AddedDate: Date;
  name: string;
  unit: string;
  manufacturer: string;
  distributor?: string;
  stock: number;
}