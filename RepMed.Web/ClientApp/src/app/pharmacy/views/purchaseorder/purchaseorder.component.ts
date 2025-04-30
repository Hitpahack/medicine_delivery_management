import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder, FormControl, FormsModule, FormGroup, ReactiveFormsModule, Validators, FormArray } from "@angular/forms";
import { AdminBaseComponent } from "../../../../app/admin/admin.base.component";
import { CommonModule } from "@angular/common";
import { GetSupppliersDto } from "../../../viewmodels/purchaseorder/supplier.dto";
import { PurchaseOrderService } from "../../../pharmacy/services/purchaseorder.services";
import { CustomValidator } from "../../../common/custom.validators";
import { Helper } from "../../../common/helper.extenstions";
import { PurchaseOrderDto } from "../../../viewmodels/purchaseorder/purchaseorder.dto";
import { AutoValidateDirective } from 'src/app/common/form.validator';
import { SupplierDto } from "../../../viewmodels/supplier/supplier.dto";

@Component({
    selector: 'app-pharmacy-dashboard',
    templateUrl: './purchaseorder.component.html',
    styleUrl: './purchaseorder.component.css',
    imports: [CommonModule, ReactiveFormsModule, FormsModule, AutoValidateDirective],
})
export class PurchaseOrderComponent extends AdminBaseComponent implements OnInit {
    constructor(
        public router: Router,
        public fb: FormBuilder,
        public PurchaseOrderService: PurchaseOrderService,
        public validator: CustomValidator

    ) {
        super(router, fb);
    }
    supplierForm!: FormGroup;
    showAddSupplierForm = false;
    poForm: FormGroup; // Define poForm here
    minDate: string;
    maxDate: string;
    errorMessage: string = '';
    PurchaseOrderDto: PurchaseOrderDto;

    suppliers: GetSupppliersDto[] = [];
    pharmacyId: string | null = null;
    poNumber: string | null = null;

    ngOnInit(): void {
        // Initialize the form using the correct method
        this.poForm = this.initForm();
        this.initPoForm();
        this.initSupplierForm();
        this.setDateLimits();

        this.pharmacyId = sessionStorage.getItem('pharmacyId');
        const id = Number(this.pharmacyId);
        this.PurchaseOrderService.getsupplier(id).subscribe((response) => {
            if (response?.isSuccess && response.data) {
                this.suppliers = response.data;
            } else {
                console.error("Failed to load suppliers data", response);
            }
        });
        this.loadPoNumber(id);
    }
    initPoForm() {
        this.poForm = this.fb.group({
            purchaseOrder: this.fb.group({
                supplierId: ['', Validators.required],
                poNumber: [''],
                eddate: [''],
                remarks: ['']
            })
        });
    }

    toggleAddSupplierForm() {
        this.showAddSupplierForm = !this.showAddSupplierForm;
        this.supplierForm.reset();
    }

    addSupplier() {
        if (this.supplierForm.invalid) {
            this.validator.markInvalidFieldsTouched(this.supplierForm);
            return;
        }
        //const dto: SupplierDto = this.supplierForm.value;
        const dto: SupplierDto = {
            ...this.supplierForm.value,
            pharmacyId: this.pharmacyId  // Add it manually here
          };
        this.PurchaseOrderService.addSupplier(dto).subscribe({
            next: (response) => {
                if (response.isSuccess) {
                    this.showAddSupplierForm = false;
                    this.supplierForm.reset();
                    Helper.ShowSuccess(response.message || 'Supplier add successfully.');
                    this.toggleAddSupplierForm();
                    this.router.navigate(['']);
                } else {
                    console.error('API returned isSuccess: false');
                    Helper.ShowError(this.errorMessage);  // Optional toast/popup
                }
            },
            error: (err) => {
                console.error('HTTP Error:', err);
                Helper.ShowError(this.errorMessage);  // Optional toast/popup
            }
        });
    }

    initSupplierForm() {
        this.supplierForm = this.fb.group({
            name: [null, [Validators.required, Validators.pattern(/^[A-Za-z\s]+$/)]],
            contact: [null, [Validators.required, Validators.pattern(/^\d{10}$/)]],
            address: [''],
            email: ['', [Validators.required, Validators.email]],
            gst: [null, [Validators.required, Validators.pattern(/^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}[Z]{1}[0-9A-Z]{1}$/)]]
        });
    }

    setDateLimits(): void {
        const today = new Date();
        const maxDate = new Date();
        maxDate.setMonth(today.getMonth() + 3);

        this.minDate = today.toISOString().split('T')[0];
        this.maxDate = maxDate.toISOString().split('T')[0];
    }
    disableTyping(event: KeyboardEvent | ClipboardEvent): void {
        event.preventDefault();
    }

    loadPoNumber(pharmacyId: number) {
        this.PurchaseOrderService.getPoNumber(pharmacyId).subscribe({
            next: (response) => {
                if (response?.isSuccess && response.data) {
                    console.log('this is po number', response.data)
                    // Patch the value into the form field
                    this.poForm.get('purchaseOrder.poNumber')?.setValue(response.data.nextPONumber);
                } else {
                    console.error('Failed to get PO number', response);
                }
            },
            error: (err) => {
                console.error('API error:', err);
            }
        });
    }

    initForm(): FormGroup {
        return this.fb.group({
            purchaseOrder: this.fb.group({
                pharmacyId: [null],
                supplierId: [null, Validators.required],
                poNumber: [{ value: null, disabled: true }, Validators.required], // read-only
                eddate: [null, [this.validator.dateWithinRangeValidator(this.minDate, this.maxDate)]],
                remarks: [''],
                taxAmount: [null, [Validators.required]],
                totalAmount: [null, [Validators.required]],
            })
        });
    }

    onSubmit(): void {
        if (this.poForm.invalid) {
            this.validator.markInvalidFieldsTouched(this.poForm);
            return;
        }
        const dto: PurchaseOrderDto = this.poForm.value;
        this.PurchaseOrderService.add(dto).subscribe({
            next: (response) => {
                if (response.isSuccess) {
                    Helper.ShowSuccess(response.message || 'Purchase Order Created successfully.');
                    this.router.navigate(['']);
                } else {
                    console.error('API returned isSuccess: false');
                    Helper.ShowError(this.errorMessage);  // Optional toast/popup
                }
            },
            error: (err) => {
                console.error('HTTP Error:', err);
                Helper.ShowError(this.errorMessage);  // Optional toast/popup
            }
        });
    }

    allowOnlyNumbers(event: KeyboardEvent) {
        const charCode = event.key.charCodeAt(0);
        if (charCode < 48 || charCode > 57) {
            event.preventDefault();
        }
    }

    disablePaste(event: ClipboardEvent): void {
        event.preventDefault();
    }
}
