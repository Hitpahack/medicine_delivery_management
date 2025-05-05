import { Component, OnInit } from '@angular/core';
import { purchaseInvoice } from "../../../viewmodels/pharmacy/purchaseInvoice";
import { FormBuilder, FormControl, FormsModule, FormGroup, ReactiveFormsModule, Validators, FormArray, FormControlName } from "@angular/forms";
import { AutoValidateDirective } from 'src/app/common/form.validator';
import { AdminBaseComponent } from 'src/app/admin/admin.base.component';
import { CommonModule } from "@angular/common";
import { Router } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";
import { PurchaseOrderService } from "../../../pharmacy/services/purchaseorder/purchaseorder.services";
@Component({
    selector: 'aap-purchase-Invoice',
    templateUrl: './Invoice.component.html',
    styleUrl: './Invoice.component.css',
    imports: [CommonModule, ReactiveFormsModule, FormsModule, AutoValidateDirective],
})

export class PurchaseInvoice extends AdminBaseComponent implements OnInit {
    PurchaseInvoice: FormGroup;

    constructor(
        public router: Router,
        public fb: FormBuilder,
        public validator: CustomValidator,
        public PurchaseOrderService: PurchaseOrderService,
    ) {
        super(router, fb);
    }

    ngOnInit(): void {
        this.PurchaseInvoice = this.initForm();
    }

    noWhitespaceValidator(control: FormControl) {
        const isWhitespace = (control.value || '').trim().length === 0;
        return isWhitespace ? { whitespace: true } : null;
    }

    initForm(): FormGroup {
        return this.fb.group({
            invoiceDetails: this.fb.group({
                InvoiceNumber: new FormControl(null, [Validators.required, Validators.pattern(/^[A-Za-z0-9-]+$/)]),
                PONumber: new FormControl(null, [Validators.required]),
                PaymentMode: new FormControl(null, [Validators.required]),
                PaymentStatus1: new FormControl(null, [Validators.required]),
                TotalDiscount: new FormControl(null, [Validators.required]),
                TaxAmount: new FormControl(null, [Validators.required]),
                TotalAmount1: new FormControl(null, [Validators.required]),
                InvoiceDate: new FormControl(null, [Validators.required]),
                ReceivedDate: new FormControl(null, [Validators.required]),
                DueDate: new FormControl(null, [Validators.required]),
            }),
            productDetails: this.fb.group({
                Manufacturer: new FormControl(null, [Validators.required]),
                BatchNumber: new FormControl(null, [Validators.required]),
                QuantityPurchased: new FormControl(null, [Validators.required]),
                Unit: new FormControl(null, [Validators.required]),
                ProductPrice: new FormControl(null, [Validators.required]),
                MRP: new FormControl(null, [Validators.required]),
                SellingPrice: new FormControl(null, [Validators.required]),
                GSTIncluded: new FormControl(null, [Validators.required]),
                GSTPercentage: new FormControl(null, [Validators.required]),
                GSTAmount: new FormControl(null, [Validators.required]),
                Discount: new FormControl(null, [Validators.required]),
                PaymentStatus2: new FormControl(null, [Validators.required]),
                ExpiryDate: new FormControl(null, [Validators.required]),
                TotalAmount2: new FormControl(null, [Validators.required]),
            }),
        })
    }

    onSubmit() {
        if (this.PurchaseInvoice.invalid) {
            this.validator.markInvalidFieldsTouched(this.PurchaseInvoice);
            return;
        }
        // this.PurchaseOrderService.add(this.PurchaseInvoice.value).subscribe({

        // })
    }
}