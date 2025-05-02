import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder, FormControl, FormsModule, FormGroup, ReactiveFormsModule, Validators, FormArray } from "@angular/forms";
import { AdminBaseComponent } from "../../../../app/admin/admin.base.component";
import { CommonModule } from "@angular/common";
import { GetSupppliersDto } from "../../../viewmodels/purchaseorder/supplier.dto";
import { PurchaseOrderService } from "../../../pharmacy/services/purchaseorder/purchaseorder.services";
import { CustomValidator } from "../../../common/custom.validators";
import { Helper } from "../../../common/helper.extenstions";
import { PurchaseOrderDto } from "../../../viewmodels/purchaseorder/purchaseorder.dto";
import { AutoValidateDirective } from 'src/app/common/form.validator';
import { SupplierDto } from "../../../viewmodels/supplier/supplier.dto";
import { Product } from "../../../viewmodels/purchaseorder/product.dto";

@Component({
    selector: 'app-pharmacy-dashboard',
    templateUrl: './purchaseorder.component.html',
    styleUrl: './purchaseorder.component.css',
    standalone: true,
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
    productForm !: FormGroup;
    showAddSupplierForm = false; // add supplier
    showAddProductForm = false; //  Add Product modal
    poForm: FormGroup; // Define poForm here
    minDate: string;
    maxDate: string;
    errorMessage: string = '';
    PurchaseOrderDto: PurchaseOrderDto;
    productsList: Product[] = [];

    suppliers: GetSupppliersDto[] = [];
    pharmacyId: string | null = null;
    poNumber: string | null = null;

    ngOnInit(): void {
        this.setDateLimits();
        this.poForm = this.initPoForm();
        this.initPoForm();
        this.initSupplierForm();
        this.initAddProductForm();
        this.pharmacyId = sessionStorage.getItem('pharmacyId');
        const id = Number(this.pharmacyId);
        this.getallsupplier(id);
        this.loadPoNumber(id);
    }

    TotalAmount: number;
    PriceValue: number;
    blankValue = 0;
    priceChange(event: Event) {
        this.PriceValue = Number((event.target as HTMLInputElement).value);
        if(this.PriceValue != null){
            this.productForm.patchValue({ totalPrice: this.blankValue });
            console.log("bb1", this.blankValue);
        }
    }

    QuantityValue: number;
    quantityChange(event: Event) {
        this.QuantityValue = Number((event.target as HTMLInputElement).value);
        if (this.PriceValue && this.QuantityValue) {
            console.log("runifblock!");
            this.TotalAmount = this.PriceValue * this.QuantityValue;
        } else {
            console.log("run else block!");
            this.productForm.patchValue({ totalPrice: this.blankValue });
            console.log("bb", this.blankValue);
            
        }
        if (this.TotalAmount != null) {
            this.productForm.patchValue({ totalPrice: this.TotalAmount });
        }
    }


    //#region get supplier and PONumber
    getallsupplier(pharmacyId: number) {
        this.PurchaseOrderService.getsupplier(pharmacyId).subscribe((response) => {
            if (response?.isSuccess && response.data) {
                this.suppliers = response.data;
            } else {
                console.error("Failed to load suppliers data", response);
            }
        });
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
    //#endregion
    initPoForm() {
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

    initSupplierForm() {
        this.supplierForm = this.fb.group({
            Name: [null, [Validators.required, Validators.pattern(/^[A-Za-z\s]+$/)]],
            Mobile: [null, [Validators.required, Validators.pattern(/^\d{10}$/)]],
            Address: [''],
            Email: ['', [Validators.required, Validators.email]],
            Gstnumber: [null, [Validators.required, Validators.pattern(/^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}[Z]{1}[0-9A-Z]{1}$/)]]
        });
    }

    initAddProductForm() {
        this.productForm = this.fb.group({
            productId: ['', Validators.required],
            quantity: ['', [Validators.required, Validators.min(1)]],
            unitPrice: ['', [Validators.required, Validators.min(0.01)]],
            totalPrice: [{ value: '', disabled: true }],
            unit: ['', Validators.required],
        });
    }

    toggleAddSupplierForm() {
        this.showAddSupplierForm = !this.showAddSupplierForm;
        this.supplierForm.reset();
    }

    toggleAddProductForm() {
        this.showAddProductForm = !this.showAddProductForm;
        this.supplierForm.reset();
        this.loadProducts();
    }

    loadProducts() {
        this.PurchaseOrderService.getproductlist().subscribe((response) => {
            if (response?.isSuccess && response.data) {
                this.productsList = response.data;
            } else {
                console.error("Failed to load suppliers data", response);
            }
        });
    }

    addSupplier() {
        console.log('call add supplier method');
        if (this.supplierForm.invalid) {
            this.validator.markInvalidFieldsTouched(this.supplierForm);
            return;
        }
        //const dto: SupplierDto = this.supplierForm.value;
        const dto: SupplierDto = {
            ...this.supplierForm.value,
            PharmacyId: this.pharmacyId  // Add it manually here
        };
        console.log('call add supplier method');
        this.PurchaseOrderService.addSupplier(dto).subscribe({
            next: (response) => {
                if (response.isSuccess) {
                    this.showAddSupplierForm = false;
                    this.supplierForm.reset();
                    Helper.ShowSuccess(response.message || 'Supplier add successfully.');

                    const id = Number(this.pharmacyId);
                    this.getallsupplier(id);

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
    preventInvalidKeys(event: KeyboardEvent, controlName: string) {
        const invalidChars = ['e', 'E', '+', '-'];
        if (invalidChars.includes(event.key)) {
            event.preventDefault();
        }

        // Get control value
        const control = this.productForm.get(controlName);
        if (control) {
            const newValue = control.value?.toString() + event.key;
            const numberValue = Number(newValue);

            // If the new value will be less than 1, prevent input
            if (!isNaN(numberValue) && numberValue < 1) {
                event.preventDefault();
            }
        }
    }
}
