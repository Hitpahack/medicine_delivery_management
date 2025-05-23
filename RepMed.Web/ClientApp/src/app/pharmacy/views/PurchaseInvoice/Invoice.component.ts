import { Component, OnInit } from '@angular/core';
import { purchaseInvoice } from "../../../viewmodels/pharmacy/purchaseInvoice";
import { FormBuilder, FormControl, FormsModule, FormGroup, ReactiveFormsModule, Validators, FormArray, FormControlName } from "@angular/forms";
import { AutoValidateDirective } from 'src/app/common/form.validator';
import { AdminBaseComponent } from 'src/app/admin/admin.base.component';
import { CommonModule } from "@angular/common";
import { Router } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";
import { PurchaseOrderService } from "../../../pharmacy/services/purchaseorder/purchaseorder.services";
import { InvoiceService } from "../../../pharmacy/services/invoice/invoice.service";
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
        public InvoiceService: InvoiceService,
    ) {
        super(router, fb);
    }

    PurchaseInvoice: FormGroup;
    pharmacyId: string | null = null;

    productList: any[] = [];
    editIndex: number | null = null;

    ngOnInit(): void {
        this.pharmacyId = sessionStorage.getItem('pharmacyId');
        const id = Number(this.pharmacyId);
        this.PurchaseInvoice = this.initForm();
    }


    fetchPOData() {
        const poNumber = this.PurchaseInvoice.get('PONumber')?.value;
        if (!poNumber) return;
        this.InvoiceService.getProductListByPONumber(poNumber).subscribe((response) => {
            if (response?.isSuccess && response.data) {
                this.productList = response || [];
            } else {
                console.error("Failed to load Item data", response);
            }
        });
    }

    enableEdit(index: number) {
        this.editIndex = index;
    }

    cancelEdit() {
        this.editIndex = null;
    }

    updateRow(index: number) {
        const updatedItem = this.productList[index];

        this.InvoiceService.updateProductDetails(updatedItem).subscribe(
            (res) => {
                alert('Record updated successfully!');
                this.fetchPOData(); // Refresh
                this.editIndex = null;
            },
            (err) => {
                console.error('Error updating row:', err);
            }
        );
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