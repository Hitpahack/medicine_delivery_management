import { AfterViewInit, Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
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
    ItemEditForm: FormGroup;

    errorMessage: string = '';
    PurchaseOrderDto: CreatePODto;
    productsList: ProductDto[] = [];

    searchTerm$ = new Subject<string>();
    supplierSearchTerm$ = new Subject<string>();

    suppliers: GetSupppliersDto[] = [];
    filteredProducts: ProductDto[] = [];
    Updateproduct: ProductDto[] = [];
    pharmacyId: string | null = null;

    productSearch: string = '';
    selectedSupplier: any;
    showDropdown = false;
    showSupplierDropdown = false;
    showEditForm: boolean = false;

    selectedProduct: ProductDto | null = null;

    @ViewChild('dataTable') datatable!: DatatableComponent;
    @Output() rowClicked = new EventEmitter<any>();
    @Output() dtInitialized = new EventEmitter<any>();

    ngAfterViewInit(): void {
        $(document).off('click', '.delete-role');
        $(document).on('click', '.delete-role', (event) => {
            const id = $(event.currentTarget).data('id');
            this.onDelete(id);
        });

        // Select all checkbox
        $(document).on('change', '#select_all_main_checkbox', function () {
            const checked = $(this).is(':checked');
            $('.item_checkbox').prop('checked', checked);
            $('#generatePoBtn').toggle($('.item_checkbox:checked').length > 0);
        });

        // Single checkbox click
        $(document).on('change', '.item_checkbox', function () {
            const total = $('.item_checkbox').length;
            const checkedCount = $('.item_checkbox:checked').length;
            $('#select_all_main_checkbox').prop('checked', total === checkedCount);
            $('#generatePoBtn').toggle(checkedCount > 0);
        });
    }
    get tableSelector(): string {
        return `#${this.tableOptions.tableId}`;
    }
    onRowClick(rowData: any): void {
        console.log('Row clicked:', rowData);
        //alert('Clicked row ID in purchase module: ' + rowData.id);

        //Update code here
        // Show the form if it’s hidden
        if (!this.ItemEditForm) {
            this.initItemEditForm(); // or make sure form is initialized
        }

        // Patch form values
        this.ItemEditForm.patchValue({
            productId: rowData.productId,
            itemId: rowData.id,
            date: rowData.addedDate,
            itemName: rowData.productName,
            supplierId: rowData.supplierName,
            manufacturer: rowData.manufacturer,
            Priority: rowData.priority || 'Low',
            min: rowData.min,
            stock: rowData.stock,
            quantity: rowData.quantity,
            status: rowData.status,
            source: rowData.source
        });

        // Also set selected supplier name for dropdown input
        if (rowData.supplierId && rowData.supplierName) {
            this.selectedSupplier = {
                id: rowData.supplierId,
                name: rowData.supplierName
            };
        }

        // Show dropdown as closed
        this.showSupplierDropdown = false;
        this.showEditForm = true;
    }
    ngOnInit(): void {
        this.poForm = this.initPoForm();
        this.initItemEditForm();
        this.pharmacyId = sessionStorage.getItem('pharmacyId');
        const id = Number(this.pharmacyId);
        this.callShortbookList();

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
                    return json.data?.data || [];
                }
            },
            searching: true,
            columns: [
                {
                    data: 'id',
                    title: `<input type="checkbox" id="select_all_main_checkbox" />`,
                    render: (data: any) => `<input class="item_checkbox" type="checkbox" value="${data}" data-id="${data}" />`
                },
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
                        hours = hours ? hours : 12;
                        const strHours = String(hours).padStart(2, '0');
                        return `${month}-${day}, ${strHours}:${minutes} ${ampm}`;
                    }
                },
                {
                    data: 'productName',
                    title: 'Item',
                    render: (data, type, row) => {
                        const truncated = data && data.length > 40 ? data.substring(0, 30) + '...' : data;
                        return `<span title="${Helper.encodeHtml(data)}">${Helper.encodeHtml(truncated)}</span>`;
                    }
                },

                { title: 'Distributor', data: 'supplierName' },
                //{ title: 'Manuf.', data: '' },
                //{ title: 'Min', data: '' },
                //{ title: 'Stock', data: '' },
                { title: 'Priority', data: 'priority' },
                { title: 'QTY.', data: 'quantity' },
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

    onSupplierSearch(term: string) {
        if (!term) {
            this.suppliers = [];
            this.showSupplierDropdown = false;
            this.selectedSupplier = null;
            this.ItemEditForm.get('supplierId')?.setValue(null);
            return;
        }
        this.supplierSearchTerm$.next(term);
    }
    selectSupplier(supplier: any) {
        this.selectedSupplier = supplier;
        this.ItemEditForm.get('supplierId')?.setValue(supplier.id);
        this.showSupplierDropdown = false;
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

    initItemEditForm() {
        this.ItemEditForm = this.fb.group({
            productId: [null],
            itemId: [null],
            date: [null],
            itemName: [''],
            supplierId: [null, Validators.required],
            manufacturer: [''],
            Priority: ['Low'],
            min: [0],
            stock: [0],
            quantity: [1, [Validators.required, Validators.min(1)]],
            status: [''],
            source: ['']
        });
    }

    //#region quantity key paste disable
    preventInvalidKeys(event: KeyboardEvent): void {
        const invalidKeys = ['-', 'e', 'E', '+', '.'];

        if (invalidKeys.includes(event.key)) {
            event.preventDefault();
        }
    }
    preventPaste(event: ClipboardEvent): void {
        const pastedInput: string = event.clipboardData?.getData('text') ?? '';
        const number = parseInt(pastedInput, 10);
        if (isNaN(number) || number < 1 || number > 1000) {
            event.preventDefault();
        }
    }
    //#endregion

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
    onUpdate() {
        // if (this.ItemEditForm.invalid) {
        //     this.ItemEditForm.markAllAsTouched();
        //     return;
        // }
        if (this.ItemEditForm.invalid) {
            this.validator.markInvalidFieldsTouched(this.ItemEditForm);
            return;
        }

        const formValue = this.ItemEditForm.value;
        const itemId = formValue.itemId;
        const updateDto: any = {
            id: this.selectedProduct?.id,
            productName: formValue.itemName,
            supplierId: this.selectedSupplier?.id,
            priority: formValue.Priority,
            quantity: formValue.quantity,
            pharmacyId: this.pharmacyId,
            productId: formValue.productId
        };
        this.PurchaseOrderService.updateProductInOrder(updateDto, itemId).subscribe({
            next: (response: any) => {
                if (response.isSuccess) {
                    Helper.ShowSuccess(response.message || 'Item Updated successfully.');
                    this.showEditForm = false;
                    this.datatable.reload();
                } else {
                    console.error('API returned isSuccess: false');
                    this.errorMessage = response.message || 'Failed to Update Item.';
                    Helper.ShowError(this.errorMessage);
                }
            },
            error: (err) => {
                this.errorMessage = err?.error?.message || 'Something went wrong. Please try again.';
                Helper.ShowError(this.errorMessage);
            }
        });

    }

    onSubmit(): void {
        if (this.poForm.invalid) {
            this.validator.markInvalidFieldsTouched(this.poForm);
            return;
        }

    }

    onGeneratePo() {
        const selectedIds: string[] = [];

        $('.item_checkbox:checked').each(function () {
            const id = $(this).data('id');
            selectedIds.push(id);
        });

        if (selectedIds.length === 0) {
            Helper.ShowError("Please select at least one item.");
            return;
        }

        // ✅ Access dtInstance via ViewChild
        const table = this.datatable.dtInstance;
        const selectedItems = [];

        table.rows().every(function () {
            const data = this.data();
            if (selectedIds.includes(data.id)) {
                selectedItems.push(data);
            }
        });

        // ✅ Supplier validation
        const missingSupplier = selectedItems.some(item => !item.supplierId);
        if (missingSupplier) {
            Helper.ShowError("Please select supplier for all selected items.");
            return;
        }

        const finalIds = selectedItems.map(x => x.id);

        const payload = {
            pharmacyId: this.pharmacyId,
            ShortbookId: finalIds
        };

        // ✅ Call your service to generate PO
        // this.purchaseOrderService.generatePO(payload).subscribe(...)
        console.log('hello call po generate api');
        this.PurchaseOrderService.pogenerate(payload).subscribe({
            next: (response) => {
                if (response.isSuccess) {
                    Helper.ShowSuccess(response.message || "PO generated successfully.");
                    this.datatable.reload();
                    // optionally hide/generate buttons or reset UI as needed
                } else {
                    this.errorMessage = response.message || "Failed to generate PO.";
                    Helper.ShowError(this.errorMessage);
                }
            },
            error: (err) => {
                this.errorMessage = err?.error?.message || "Something went wrong while generating PO.";
                Helper.ShowError(this.errorMessage);
            }
        });
    }


}
