export interface purchaseInvoice {
    Id: number;
    PharmacyId: number;
    SupplierId: number;
    InvoiceNumber: string;
    TotalAmount: string;
    PONumber: string;
    TotalDiscount: string;
    PaymentMode: string;
    PaymentStatus: string;
    InvoiceDate?: Date;
    TaxAmount?: string;
    ReceivedDate?: Date;
    DueDate?: Date;
}


export interface Product {
    Id: number;
    PurchaseInvoiceId: number;
    ProductId: number;
    Manufacturer: string;
    BatchNumber: string;
    QuantityPurchased: string;
    Unit: string;
    ProductPrice: string;
    PaymentStatus: string;
    MRP?: string;
    SellingPrice?: string;
    GSTIncluded?: string;
    GSTPercentage?: string;
    GSTAmount?: string;
    TotalAmount: string;
    Discount?: string;
    ExpiryDate?: Date;
}