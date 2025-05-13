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
import { ProductDto } from "../../../viewmodels/purchaseorder/productdetails.dto";
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { Subject } from 'rxjs';

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
    poForm: FormGroup;
    errorMessage: string = '';
    PurchaseOrderDto: CreatePODto;

    productsList: ProductDto[] = [];
    searchTerm$ = new Subject<string>();

    suppliers: GetSupppliersDto[] = [];
    pharmacyId: string | null = null;

    productSearch: string = '';
    filteredProducts: ProductDto[] = [];
    showDropdown = false;

    selectedProduct: ProductDto | null = null;
    selectedProductEdit = {
        supplierId: null,
        quantity: 1,
        priority: '',
        status: ''
    };

    editProduct(product: ProductDto) {
        this.selectedProduct = product;
        this.selectedProductEdit = {
            supplierId: product.SupplierId,
            quantity: product.Quantity,
            priority: product.Priority,
            status: product.Status
        };
    }

    updateProduct() {
        if (!this.selectedProduct) return;

        const payload = {
            productId: this.selectedProduct.id,
            pharmacyId: Number(this.pharmacyId),
            supplierId: this.selectedProductEdit.supplierId,
            quantity: this.selectedProductEdit.quantity,
            priority: this.selectedProductEdit.priority,
            status: this.selectedProductEdit.status
        };

        this.PurchaseOrderService.updateProductInOrder(payload).subscribe({
            next: (response) => {
                if (response.isSuccess) {
                    Helper.ShowSuccess('Product updated successfully.');
                    this.loadOrderProducts();
                    this.selectedProduct = null;  // Clear edit form
                } else {
                    Helper.ShowError(response.message || 'Failed to update.');
                }
            },
            error: (err) => {
                Helper.ShowError(err?.error?.message || 'Something went wrong.');
            }
        });
    }

    ngOnInit(): void {
        this.poForm = this.initPoForm();
        this.pharmacyId = sessionStorage.getItem('pharmacyId');
        const id = Number(this.pharmacyId);
        this.getallsupplier(id);
        this.loadOrderProducts();

        this.searchTerm$
            .pipe(
                debounceTime(300),
                distinctUntilChanged(),
                switchMap(term => this.PurchaseOrderService.getFilteredProducts(term))
            )
            .subscribe((response: any) => {
                if (response.isSuccess && response.data?.length) {
                    this.filteredProducts = response.data.slice(0, 5); // ✅ Use `response.data`
                    this.showDropdown = true;
                } else {
                    this.filteredProducts = [];
                    this.showDropdown = false;
                }
            });
    }

    TotalAmount: number = 0;
    PriceValue: number = 0;
    QuantityValue: number = 0;
    blankValue: number = 0;

    filterProducts() {
        const search = this.productSearch.toLowerCase();
        this.filteredProducts = this.productsList.filter(p =>
            p.name.toLowerCase().includes(search)
        );
    }

    onSearchChange(term: string) {
        if (term) {
            this.searchTerm$.next(term);
        } else {
            this.filteredProducts = []; // Clear the dropdown if search term is empty
            this.showDropdown = false;  // Hide dropdown
        }
    }

    selectProduct(product: ProductDto) {

        const payload = {
            pharmacyId: Number(this.pharmacyId),
            productId: product.id,  // ✅ Corrected
            supplierId: null,
            quantity: 1,
            priority: null,
            status: 'Pending',
            addedDate: null
        };

        this.PurchaseOrderService.addProductToOrder(payload).subscribe({
            next: (response) => {
                if (response.isSuccess) {
                    Helper.ShowSuccess(response.message || 'Product added in Cart.');
                    this.loadOrderProducts();  // Reload table
                } else {
                    this.errorMessage = response.message || 'Failed to add user.';
                    Helper.ShowError(this.errorMessage);
                    console.error('Add failed:', response.message);
                }
            },
            error: (err) => {
                this.errorMessage = err?.error?.message || 'Something went wrong. Please try again.';
                Helper.ShowError(this.errorMessage);
                console.error('API error:', err);
            }
        });

        this.productSearch = '';
        this.showDropdown = false;
    }

    loadOrderProducts() {

        if (!this.pharmacyId) return;

        this.PurchaseOrderService.getOrderProducts(Number(this.pharmacyId)).subscribe({
            next: (response) => {
                if (response.isSuccess && response.data) {
                    this.productsList = response.data;
                } else {
                    this.productsList = [];
                }
            },
            error: (err) => {
                console.error('Failed to load products:', err);
            }
        });
    }
    //#region get supplier
    getallsupplier(pharmacyId: number) {
        this.PurchaseOrderService.getsupplier(pharmacyId).subscribe((response) => {
            if (response?.isSuccess && response.data) {
                this.suppliers = response.data;
            } else {
                console.error("Failed to load suppliers data", response);
            }
        });
    }

    //#endregion
    initPoForm() {
        return this.fb.group({
            purchaseOrder: this.fb.group({
                supplierId: [null, Validators.required],
                poNumber: [''],
                expectedDelivery: [''],
                remarks: ['']
            }),
            products: this.fb.array([])  // dynamic products list
        });
    }


    onSubmit(): void {
        if (this.poForm.invalid) {
            this.validator.markInvalidFieldsTouched(this.poForm);
            return;
        }

    }
}
