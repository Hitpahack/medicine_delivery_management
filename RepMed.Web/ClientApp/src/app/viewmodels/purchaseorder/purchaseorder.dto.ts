export interface CreatePODto  {

    po: BasePODto;
    items: BasePOItemDto[];
}

export interface BasePODto {
    pharmacyId: number;
    supplierId: number;
    poNumber: string;
    eddate?: Date;
    totalAmount?: number;
    remarks?: string;
}
export interface BasePOItemDto {
    //purchaseOrderId?: number;
    productId: number;
    quantity: number;
    unitPrice: number;
    totalPrice?: number;
    unit: string;
    totalAmount: number;
  }