import { Component, OnInit} from '@angular/core';
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

    constructor(
        public router: Router,
        public fb: FormBuilder,
        public validator: CustomValidator,
        public PurchaseOrderService: PurchaseOrderService,
    ) {
        super(router, fb);
    }

    PurchaseInvoice: FormGroup;
    pharmacyId: string | null = null;

    ngOnInit(): void {
        this.pharmacyId = sessionStorage.getItem('pharmacyId');
        const id = Number(this.pharmacyId);
        this.PurchaseInvoice = this.initForm();
    }

    initForm(): FormGroup {
        return this.fb.group({
            supplierId: new FormControl(null, [Validators.required]),
            BillNo: new FormControl(null, [Validators.required]),
            PONumber: new FormControl(null, [Validators.required]),
            Billdate: [null, [Validators.required]],
            ItemId: new FormControl(null, [Validators.required]),
            BatchNo: new FormControl(null, [Validators.required]),
            MRP: new FormControl(null, [Validators.required]),
            PTR: new FormControl(null, [Validators.required]),
            quantity: new FormControl(null, [Validators.required]),
            freequantity: new FormControl(null),
            LumpsumDiscount: new FormControl(null),
            Discount: new FormControl(null),
            Baseprice: new FormControl(null, [Validators.required]),
            GST: new FormControl(null, [Validators.required]),
            Amount: new FormControl(null, [Validators.required]),
        })
    }

    onSubmit() {
        if (this.PurchaseInvoice.invalid) {
            this.validator.markInvalidFieldsTouched(this.PurchaseInvoice);
            return;
        }
    }
}