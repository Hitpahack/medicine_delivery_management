import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
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
import { DatatableComponent } from "../../../../app/admin/shared/datatables/datatable.component";
import { Subject } from 'rxjs';

@Component({
    selector: 'app-pharmacy-dashboard',
    templateUrl: './purchaseorder.component.html',
    styleUrls: ['./purchaseorder.component.css'],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, FormsModule, AutoValidateDirective, DatatableComponent],
})
export class PurchaseOrderComponent extends AdminBaseComponent implements OnInit, AfterViewInit {
    constructor(
        public router: Router,
        public fb: FormBuilder,
        public PurchaseOrderService: PurchaseOrderService,
        public validator: CustomValidator

    ) {
        super(router, fb);
    }
    tableOptions: any;
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
    @ViewChild('dataTable') datatable!: DatatableComponent;
    //@ViewChild(DatatableComponent) datatable!: DatatableComponent;
    //selector: 'data-table'

    ngAfterViewInit(): void {
        $(document).off('click', '.delete-role');
        $(document).on('click', '.delete-role', (event) => {
            const id = $(event.currentTarget).data('id');
            this.onDelete(id);
        });
    }
    ngOnInit(): void {
        this.poForm = this.initPoForm();
        this.pharmacyId = sessionStorage.getItem('pharmacyId');
        const id = Number(this.pharmacyId);
        this.getallsupplier(id);
        this.callShortbookList();


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

    callShortbookList() {
        const self = this;
        this.tableOptions = {
            tableId: 'post_productlist_datatable',
            ajax: {
                url: this.admin_apiconfig.endpoints.purchaseorder.orderProductslist,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: (d: any) => {
                    d.pharmacyId = self.pharmacyId;
                    return JSON.stringify(d);
                },
                dataSrc: function (json) {
                    console.log('list API response:', json);
                    return json.data?.data || [];
                }
            },
            searching: true,
            columns: [
                { data: 'id', title: '#', render: (data: any) => `<input class="item_checkbox" id="${data}" type="checkbox" value="${data}" />` },
                //{ title: 'Date', data: 'addedDate' },
                {
                    title: 'Date',
                    data: 'addedDate',
                    render: function (data: any) {
                        if (!data) return '';
                        const date = new Date(data);
                        const month = String(date.getMonth() + 1).padStart(2, '0');
                        const day = String(date.getDate()).padStart(2, '0');
                        let hours = date.getHours();
                        const minutes = String(date.getMinutes()).padStart(2, '0');
                        const ampm = hours >= 12 ? 'PM' : 'AM';
                        hours = hours % 12;
                        hours = hours ? hours : 12; // 0 ko 12 banana
                        const strHours = String(hours).padStart(2, '0');
                        return `${month}-${day}, ${strHours}:${minutes} ${ampm}`;
                    }
                },
                { title: 'Item', data: 'productName' },
                { title: 'Distributor', data: 'supplierId' },
                //{ title: 'Manuf.', data: '' },
                //{ title: 'Min', data: '' },
                //{ title: 'Stock', data: '' },
                { title: 'quantity', data: 'quantity' },
                { title: 'status', data: 'status' },
                {
                    data: 'SortBook',
                    title: 'Source',
                    render: () => {
                        return 'Shortbook';
                    }
                },
                {
                    title: '',
                    data: null,
                    orderable: false,
                    render: (data: any, type: any, row: any) => {
                        return `
                    <button class="btn btn-sm btn-danger delete-role" data-id="${row.id}" title="Remove" 
                        style="display: inline-flex; align-items: center; justify-content: center; gap: 5px; padding: 5px 10px; border-radius: 12px;">
                        <i class="bi bi-trash" style="font-size: 16px;"></i>
                    </button>
                    `;
                    }
                },
            ],
        };
    }

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
            this.filteredProducts = [];
            this.showDropdown = false;
        }
    }

    // When search Item in searchbox and click item than item add in shortbook.
    selectProduct(product: ProductDto) {

        const payload = {
            pharmacyId: Number(this.pharmacyId),
            productId: product.id,
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
                    this.datatable.reload();
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
                remarks: [''],
                productSearch: ['']
            }),
            products: this.fb.array([])
        });
    }

    //remove product in shortbook
    onDelete(id: number): void {
        if (confirm('Are you sure you want to delete this item?')) {
            this.PurchaseOrderService.deleteitembyid(id).subscribe({
                next: (response) => {
                    if (response?.isSuccess) {
                        Helper.ShowSuccess(response.message || 'Item deleted successfully');
                        this.datatable.reload();
                    } else {
                        Helper.ShowError(response.message || 'Failed to delete the item');
                    }
                },
                error: (error) => {
                    console.error('Delete error:', error);
                    // Show error message if there is an API or HTTP error
                    const errorMessage = error?.error?.message || 'An error occurred while deleting the item';
                    Helper.ShowError(errorMessage);
                }
            });
        }
    }

    onSubmit(): void {
        if (this.poForm.invalid) {
            this.validator.markInvalidFieldsTouched(this.poForm);
            return;
        }

    }
}
