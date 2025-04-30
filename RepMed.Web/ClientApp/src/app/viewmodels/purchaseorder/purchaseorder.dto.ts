export interface PurchaseOrderDto {

    BasePODto:{
        pharmacyId: number;
        supplierId: number;
        poNumber: string;
        orderDate?: Date;
        eddate?: Date;
        status?: string;
        totalAmount?: number;
        taxAmount: number;
        remarks?: string;
        createdAt?: Date;
        updatedAt?: Date;
    };
    items: BasePOItemDto[];
}
export interface BasePOItemDto {
    purchaseOrderId: number;
    productId: number;
    quantity: number;
    unitPrice: number;
    totalPrice?: number;
    unit: string;
    createdAt?: Date;
    updatedAt?: Date;
  }