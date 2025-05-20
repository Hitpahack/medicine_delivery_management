import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { RouterModule } from '@angular/router';
import { FormBuilder } from "@angular/forms";
import { CustomValidator } from "../../../../app/common/custom.validators";
import { PurchaseOrderService } from "../../services/purchaseorder/purchaseorder.services";
import { Helper } from "../../../../app/common/helper.extenstions";
import { AdminBaseComponent } from '../../../admin/admin.base.component';
import { DatatableComponent } from '../../../admin/shared/datatables/datatable.component';
import { Title } from '@angular/platform-browser';
declare var $: any;

@Component({
    selector: 'app-POItem-list',
    templateUrl: 'POItemList.component.html',
    imports: [DatatableComponent, RouterModule]
})

export class POIDItemListComponent extends AdminBaseComponent implements OnInit, AfterViewInit {
    constructor(
        public validator: CustomValidator,
        public PurchaseOrderService: PurchaseOrderService,
        public route: ActivatedRoute
    ) {
        super();
    }
    pharmacyId: string | null = null;
    POID: Number | null = null;

    ngAfterViewInit(): void {
        $(document).off('click', '.editItem');
        $(document).off('click', '.deleteItem');

        $(document).on('click', '.editItem', (event) => {
            const id = $(event.currentTarget).data('id');
            this.router.navigate(['/pharmacy/role/edit', id]);
        });

        $(document).on('click', '.deleteItem', (event) => {
            const id = $(event.currentTarget).data('id');
            this.onDelete(id);
        });
    }

    tableOptions = {
        tableId: 'post_POItemListlist_datatable',
        ajax: {
            url: this.admin_apiconfig.endpoints.purchaseorder.getItemBaseOnPONumber,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: (d) => {
                d.pharmacyId = this.pharmacyId;
                d.POId = this.POID;
                return JSON.stringify(d);
            }

        },
        searching: true,
        columns: [
            {
                title: 'Item Name',
                data: 'itemName',
                render: function (data: any, type: any, row: any) {
                    if (data && data.length > 30) {
                        return data.substring(0, 30) + '...';
                    }
                    return data;
                }
            },
            { title: 'current Stock', data: 'currentStock' },
            { title: 'stock Ava.', data: 'stockAvailability' },
            { title: 'status', data: 'status' },
            { title: 'mrp', data: 'mrp' },
            { title: 'ptr', data: 'ptr' },
            { title: 'req. Qty', data: 'requiredQty' },
            { title: 'amount', data: 'amount' },
            {
                title: 'Action',
                data: null,
                orderable: false,
                serverSide: true,
                processing: true,
                searching: false,
                render: (data, type, row) => {
                    return `
                        <i class="fa fa-edit editItem" data-id="${row.poItemId}" style="cursor:pointer; color:blue; font-size:18px; margin-right:10px;" title="Edit"></i>
                        <i class="fa fa-trash deleteItem" data-id="${row.poItemId}" style="cursor:pointer; color:red; font-size:18px;" title="Delete"></i>
                    `;
                }
            }
        ],
    };

    onDelete(id: number): void {
        if (confirm('Are you sure you want to delete this item?')) {
            this.PurchaseOrderService.deleteitembyid(id).subscribe({
                next: (response) => {
                    if (response?.isSuccess) {
                        Helper.ShowSuccess(response.message || 'Item deleted successfully');
                        $('#post_POItemListlist_datatable').DataTable().ajax.reload();
                    } else {
                        Helper.ShowError(response.message || 'Failed to delete the item');
                    }
                },
                error: (error) => {
                    console.error('Delete error:', error);
                    const errorMessage = error?.error?.message || 'An error occurred while deleting the item';
                    Helper.ShowError(errorMessage);
                }
            });
        }
    }

    ngOnInit(): void {
        this.POID = Number(this.route.snapshot.paramMap.get('id'));
        this.pharmacyId = sessionStorage.getItem('pharmacyId');

    }

}