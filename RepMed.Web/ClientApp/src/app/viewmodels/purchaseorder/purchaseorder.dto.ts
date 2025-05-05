export interface BasePODto {

    CreatePODto:{
        pharmacyId: number;
        supplierId: number;
        poNumber: string;
        eddate?: Date;
        totalAmount?: number;
        remarks?: string;
        
    };
    items: BasePOItemDto[];
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