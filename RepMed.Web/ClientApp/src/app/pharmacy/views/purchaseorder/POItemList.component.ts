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
    styleUrls: ['./POItemList.component.css'],
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
    POID: Number;
    errorMessage: string = '';

    ngAfterViewInit(): void {
        $(document).off('click', '.editItem');
        $(document).off('click', '.deleteItem');
        $(document).off('click', '.updateQtyBtn');

        $(document).on('click', '.editItem', (event) => {
            const id = $(event.currentTarget).data('id');
            const currentQty = $(event.currentTarget).data('qty');

            // Set Qty in modal input
            $('#qtyInput').val(currentQty);
            $('#qtyInput').data('id', id);
            $('#qtyEditModal').show();
        });

        $(document).on('click', '#updateQtyBtn', () => {
            const updatedQty = parseInt($('#qtyInput').val(), 10);
            const id = $('#qtyInput').data('id');

            // Remove any previous errors
            $('#qtyInput').removeClass('input-error');
            $('.error-message').remove();

            // Validation Check
            if (isNaN(updatedQty) || updatedQty < 1 || updatedQty > 1000) {
                $('#qtyInput')
                    .addClass('input-error')
                    .after('<div class="error-message" style="color: #dc3545; font-size: 13px; margin-top: 5px;">Quantity must be between 1 and 1000</div>');
                return;
            }
            this.PurchaseOrderService.updateItemQty(id, updatedQty).subscribe({
                next: (response) => {
                    if (response.isSuccess) {
                        Helper.ShowSuccess(response.message || 'Qty updated successfully.');
                        $('#post_POItemListlist_datatable').DataTable().ajax.reload();
                    } else {
                        console.error('API returned isSuccess: false');
                        this.errorMessage = response.message || 'Failed to update role.';
                        Helper.ShowError(this.errorMessage);
                    }
                },
                error: (err) => {
                    console.error('HTTP Error:', err);
                    this.errorMessage = err?.error?.message || 'Something went wrong. Please try again.';
                    Helper.ShowError(this.errorMessage);
                }
            });
            $('#qtyEditModal').hide();
        });

        // Remove error on typing valid value
        $(document).on('input', '#qtyInput', () => {
            const qty = parseInt($('#qtyInput').val(), 10);

            if (!isNaN(qty) && qty >= 1 && qty <= 1000) {
                $('#qtyInput').removeClass('input-error');
                $('.error-message').remove();
            }
        });


        $(document).on('click', '#closeModalBtn', () => {
            $('#qtyEditModal').hide();
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
                        <button class="deleteItem ${row.status !== 'Pending' ? 'blur-icon' : ''}"
                        data-id="${row.poItemId}"
                        ${row.status !== 'Pending' ? 'disabled' : ''}
                        style="background: none; border: none; padding: 0; margin: 0; color: red; font-size: 18px;"
                        title="Delete">
                        <i class="fa fa-trash"></i>
                        </button>
                    `;
                }
            }
        ],
    };

    onDelete(id: number): void {
        if (confirm('Are you sure you want to delete this item?')) {
            this.PurchaseOrderService.deletePOItembyid(id).subscribe({
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

    SendMailToDistributor(POID): void {
        this.PurchaseOrderService.sendMailToDistributor(POID).subscribe({
            next: (response) => {
                if (response?.isSuccess) {
                    Helper.ShowSuccess(response.message || 'Mail Send To Distributor successfully');
                    $('#orderwiseTable').DataTable().ajax.reload();
                } else {
                    Helper.ShowError(response.message || 'Failed to Send Mail To Distributor');
                }
            },
            error: (error) => {
                console.error('error:', error);
                const errorMessage = error?.error?.message || 'An error occurred while Send Mail To Distributor';
                Helper.ShowError(errorMessage);
            }
        });
    }

    ngOnInit(): void {
        this.POID = Number(this.route.snapshot.paramMap.get('id'));
        this.pharmacyId = sessionStorage.getItem('pharmacyId');

    }

}