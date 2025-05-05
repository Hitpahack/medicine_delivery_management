import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder, FormControl, FormsModule, FormGroup, ReactiveFormsModule, Validators, FormArray } from "@angular/forms";
import { AdminBaseComponent } from "../../../../app/admin/admin.base.component";
import { CommonModule } from "@angular/common";
import { GetSupppliersDto } from "../../../viewmodels/purchaseorder/supplier.dto";
import { PurchaseOrderService } from "../../../pharmacy/services/purchaseorder/purchaseorder.services";
import { CustomValidator } from "../../../common/custom.validators";
import { Helper } from "../../../common/helper.extenstions";
import { CreatePODto } from "../../../viewmodels/purchaseorder/purchaseorder.dto";
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
    PurchaseOrderDto: CreatePODto;
    productsList: Product[] = [];

    suppliers: GetSupppliersDto[] = [];
    pharmacyId: string | null = null;
    poNumber: string | null = null;
    addedProducts: any[] = [];

    ngOnInit(): void {
        this.setDateLimits();
        this.poForm = this.initPoForm();
        this.initSupplierForm();
        this.initAddProductForm();
        this.pharmacyId = sessionStorage.getItem('pharmacyId');
        const id = Number(this.pharmacyId);
        this.getallsupplier(id);
        this.loadPoNumber(id);
        this.loadProducts();
    }

    TotalAmount: number = 0;
    PriceValue: number = 0;
    QuantityValue: number = 0;
    blankValue: number = 0;
    priceChange(event: Event) {
        this.PriceValue = Number((event.target as HTMLInputElement).value);
        this.calculateTotal();
    }

    quantityChange(event: Event) {
        this.QuantityValue = Number((event.target as HTMLInputElement).value);
        this.calculateTotal();
    }

    calculateTotal() {
        if (this.PriceValue > 0 && this.QuantityValue > 0) {
            this.TotalAmount = this.PriceValue * this.QuantityValue;
            this.productForm.patchValue({ totalPrice: this.TotalAmount });
        } else {
            this.productForm.patchValue({ totalPrice: this.blankValue });
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
                //taxAmount: [null, [Validators.required]],
                totalAmount: [null],
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
            quantity: ['', [Validators.required, Validators.min(1), Validators.max(9999999999)]],
            unitPrice: ['', [Validators.required, Validators.min(1), Validators.max(9999999999)]],
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
        this.productForm.reset();
        this.PriceValue = 0;
        this.QuantityValue = 0;
        this.TotalAmount = 0;
    }

    loadProducts() {
        this.PurchaseOrderService.getproductlist().subscribe((response) => {
            if (response?.isSuccess && response.data) {
                this.productsList = response.data;
            } else {
                console.error("Failed to load products data", response);
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
        //
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

    addProduct() {
        if (this.productForm.valid) {
            const product = this.productsList.find(p => p.value == this.productForm.value.productId);
            const addedProduct = {
                productId: this.productForm.value.productId,
                productName: product?.text,
                unitPrice: this.productForm.value.unitPrice,
                quantity: this.productForm.value.quantity,
                totalPrice: this.productForm.value.unitPrice * this.productForm.value.quantity,
                unit: this.productForm.value.unit
            };

            this.addedProducts.push(addedProduct);
            this.showAddProductForm = false;
            this.productForm.reset();
            this.PriceValue = 0;
            this.QuantityValue = 0;
            this.TotalAmount = 0;
        }
    }

    removeProduct(index: number) {
        this.addedProducts.splice(index, 1);
    }

    get totalAmount(): number {
        return this.addedProducts.reduce((sum, p) => sum + p.totalPrice, 0);
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
        if (this.addedProducts.length === 0) {
            Helper.ShowError("Please select(Add) at least one product.");
            return;
        }
        if (this.poForm.invalid) {
            this.validator.markInvalidFieldsTouched(this.poForm);
            return;
        }
        const dto: CreatePODto = {
            po: {
                pharmacyId: Number(this.pharmacyId),
                supplierId: this.poForm.value.purchaseOrder.supplierId,
                poNumber: this.poForm.get('purchaseOrder.poNumber')?.value,
                eddate: this.poForm.value.purchaseOrder.eddate,
                totalAmount: this.totalAmount,
                remarks: this.poForm.value.purchaseOrder.remarks
            },
            items: this.addedProducts.map(p => ({
                productId: p.productId,
                quantity: p.quantity,
                unitPrice: p.unitPrice,
                totalPrice: p.totalPrice,
                unit: p.unit,
                totalAmount: p.totalPrice  // <-- fix the casing here
            }))
        };
        this.PurchaseOrderService.addpurchaseorder(dto).subscribe({
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
    allowOnlyAlphabets(event: KeyboardEvent) {
        const char = event.key;
        const regex = /^[A-Za-z ]$/;
        if (!regex.test(char)) {
            event.preventDefault();
        }
    }
}
