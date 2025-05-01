import { Component, OnInit } from '@angular/core';
import { purchaseInvoice } from "../../../viewmodels/pharmacy/purchaseInvoice";
import { FormBuilder, FormControl, FormsModule, FormGroup, ReactiveFormsModule, Validators, FormArray, FormControlName } from "@angular/forms";
import { AutoValidateDirective } from 'src/app/common/form.validator';
import { AdminBaseComponent } from 'src/app/admin/admin.base.component';
import { CommonModule } from "@angular/common";
import { Router } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";

@Component({
    selector: 'aap-purchase-Invoice',
    templateUrl: './Invoice.component.html',
    styleUrl: './Invoice.component.css',
    imports: [CommonModule, ReactiveFormsModule, FormsModule],
})

export class PurchaseInvoice extends AdminBaseComponent implements OnInit {
    PurchaseInvoice: FormGroup;

    constructor(
        public router: Router,
        public fb: FormBuilder,
        public validator: CustomValidator
    ) {
        super(router, fb);
    }

    ngOnInit(): void {
        this.PurchaseInvoice = this.initForm();
    }

    initForm(): FormGroup {
        return this.fb.group({
            InvoiceNumber: new FormControl(null, [Validators.required]),
            PONumber: new FormControl(null),
            InvoiceDate: new FormControl(null),
            ReceivedDate: new FormControl(null),
            DueDate: new FormControl(null),
            PaymentMode: new FormControl(null),
            PaymentStatus: new FormControl(null),
            TaxAmount: new FormControl(null),
            TotalDiscount: new FormControl(null),
            TotalAmount: new FormControl(null),
            Manufacturer: new FormControl(null),
            BatchNumber: new FormControl(null),
            QuantityPurchased: new FormControl(null),
            Unit: new FormControl(null),
            ProductPrice: new FormControl(null),
            MRP: new FormControl(null),
            SellingPrice: new FormControl(null),
            GSTIncluded: new FormControl(null),
            GSTPercentage: new FormControl(null),
            GSTAmount: new FormControl(null),
            Discount: new FormControl(null),
            ExpiryDate: new FormControl(null),
        })
    }

    onSubmit() {

    }
}