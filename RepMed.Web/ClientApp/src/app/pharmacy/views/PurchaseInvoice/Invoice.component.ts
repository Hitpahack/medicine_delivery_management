import { Component, OnInit } from '@angular/core';
import { purchase } from "../../../viewmodels/pharmacy/purchaseInvoice";
import { FormBuilder, FormControl, FormsModule, FormGroup, ReactiveFormsModule, Validators, FormArray } from "@angular/forms";
import { AutoValidateDirective } from 'src/app/common/form.validator';
import { AdminBaseComponent } from 'src/app/admin/admin.base.component';
import { CommonModule } from "@angular/common";
import { Router } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";

@Component({
    selector: 'aap-purchase-Invoice',
    templateUrl: './Invoice.component.html',
    styleUrl:'./Invoice.component.css',
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
        return this.fb.group({})
    }

    onSubmit() {

    }
}