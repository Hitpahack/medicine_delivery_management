import { Component, OnInit, ElementRef, HostListener, ViewChild } from '@angular/core';
import { purchaseInvoice } from "../../../viewmodels/pharmacy/purchaseInvoice";
import { FormBuilder, FormControl, FormsModule, FormGroup, ReactiveFormsModule, Validators, FormArray, FormControlName } from "@angular/forms";
import { AutoValidateDirective } from 'src/app/common/form.validator';
import { AdminBaseComponent } from 'src/app/admin/admin.base.component';
import { CommonModule } from "@angular/common";
import { Router } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";
import { PurchaseOrderService } from "../../../pharmacy/services/purchaseorder/purchaseorder.services";
import { debounceTime, distinctUntilChanged, Subject, switchMap } from 'rxjs';
import { GetSupppliersDto } from 'src/app/viewmodels/purchaseorder/supplier.dto';
import { ProductDto } from 'src/app/viewmodels/purchaseorder/productdetails.dto';
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
        private elementRef: ElementRef
    ) {
        super(router, fb);
    }

    PurchaseInvoice: FormGroup;
    pharmacyId: string | null = null;

    //this code for supplier search
    supplierSearchTerm$ = new Subject<string>();
    suppliers: GetSupppliersDto[] = [];
    showSupplierDropdown = false;
    selectedSupplier: any;

    //this code for Item search
    searchTerm$ = new Subject<string>();
    filteredProducts: ProductDto[] = [];
    selectedItem: any;
    showDropdown = false;

    @ViewChild('supplierBox') supplierBox!: ElementRef;
    @ViewChild('productBox') productBox!: ElementRef;

    ngOnInit(): void {
        this.pharmacyId = sessionStorage.getItem('pharmacyId');
        const id = Number(this.pharmacyId);
        this.PurchaseInvoice = this.initForm();

        // search Supplier base pharmacyId
        this.supplierSearchTerm$
            .pipe(
                debounceTime(300),
                distinctUntilChanged(),
                switchMap(term => this.PurchaseOrderService.getFilteredSupplier(id, term)) //API call
            )
            .subscribe((response: any) => {
                if (response.isSuccess && response.data?.length) {
                    this.suppliers = response.data.slice(0, 5);
                    this.showSupplierDropdown = true;
                } else {
                    this.suppliers = [];
                    this.showSupplierDropdown = false;
                }
            });

        // search Item
        this.searchTerm$
            .pipe(
                debounceTime(300),
                distinctUntilChanged(),
                switchMap(term => this.PurchaseOrderService.getFilteredProducts(term))
            )
            .subscribe((response: any) => {
                if (response.isSuccess && response.data?.length) {
                    this.filteredProducts = response.data.slice(0, 5);
                    this.showDropdown = true;
                } else {
                    this.filteredProducts = [];
                    this.showDropdown = false;
                }
            });
    }

    //#region this code for search Supplier
    onSupplierSearch(term: string) {
        if (!term) {
            this.suppliers = [];
            this.showSupplierDropdown = false;
            this.selectedSupplier = null;
            this.PurchaseInvoice.get('supplierId')?.setValue(null);
            return;
        }
        this.supplierSearchTerm$.next(term);
    }
    selectSupplier(supplier: any) {
        this.selectedSupplier = supplier;
        this.PurchaseInvoice.get('supplierId')?.setValue(supplier.id);
        this.showSupplierDropdown = false;
    }
    //#endregion

    //#region this code for search Item
    onSearchChange(term: string) {
        if (term) {
            this.searchTerm$.next(term);
        } else {
            this.filteredProducts = [];
            this.showDropdown = false;
        }
    }
    selectProduct(product: any) {
        this.selectedItem = product;
        this.PurchaseInvoice.patchValue({
            ItemId: product.name,
            MRP: product.price
        });
        this.PurchaseInvoice.get('ItemId')?.setValue(product.id);
        this.showDropdown = false;
    }
    //#endregion

    //#region Automatically close dropdowm (supplier/Item)
    @HostListener('document:click', ['$event'])
    handleClickOutside(event: MouseEvent) {
        const clickedInsideSupplier = this.supplierBox?.nativeElement.contains(event.target);
        const clickedInsideProduct = this.productBox?.nativeElement.contains(event.target);

        if (!clickedInsideSupplier) {
            this.showSupplierDropdown = false;
        }

        if (!clickedInsideProduct) {
            this.showDropdown = false;
        }
    }
    //#endregion

    initForm(): FormGroup {
        return this.fb.group({
            supplierId: new FormControl(null, [Validators.required]),
            BillNo: new FormControl(null, [Validators.required]),
            PONumber: new FormControl(null, [Validators.required]),
            Billdate: [null, [Validators.required]],
            ItemId: new FormControl(null, [Validators.required]),
            BatchNo: new FormControl(null, [Validators.required]),
            MRP: new FormControl(null, [Validators.required]),

        })
    }

    onSubmit() {
        if (this.PurchaseInvoice.invalid) {
            this.validator.markInvalidFieldsTouched(this.PurchaseInvoice);
            return;
        }
    }
}