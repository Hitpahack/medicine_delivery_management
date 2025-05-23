import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";
import { AdminBaseComponent } from "../../../admin/admin.base.component";
import { Helper } from "../../../../app/common/helper.extenstions";
import { SupplierService } from "../../../admin/services/supplier/supplier.services";
import { AddPersonDto } from "../../../viewmodels/User/Person.add.dto";
import { Role } from "../../../viewmodels/User/role.model";
import { AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AdminCommonServices } from "../../../../app/admin/services/Common/admin.commonservices";
import { AutoValidateDirective } from "../../../common/form.validator";
import { SupplierDto } from "../../../viewmodels/supplier/supplier.dto";
declare const window: any;

@Component({
    selector: 'admin-add-supplier',
    templateUrl: './addsupplier.component.html',
    standalone: true,
    styleUrls: ['./addsupplier.component.css'],
    imports: [CommonModule, ReactiveFormsModule, FormsModule, AutoValidateDirective],
})

export class AddSupplierComponent extends AdminBaseComponent implements OnInit, AfterViewInit {
    addSupplierForm: FormGroup;
    editsupplierForm: FormGroup;
    addUserData: AddPersonDto;
    pharmacyId: string | null = null;
    roles: Role[] = [];
    errorMessage: string = '';

    constructor(
        public validator: CustomValidator,
        public SupplierService: SupplierService,
        private route: ActivatedRoute,
        public AdminCommonServices: AdminCommonServices,
        public fb: FormBuilder,
        public router: Router
    ) {
        super();
    }
    ngAfterViewInit(): void {

    }


    supplierID: number;
    ngOnInit(): void {
        this.pharmacyId = sessionStorage.getItem('pharmacyId');
        this.supplierID = this.route.snapshot.params['id'];
        if (this.supplierID && this.supplierID !== 0) {
            // For Edit
            this.editsupplierForm = this.initEditForm();
            this.initEditForm();
            this.loadSupplierData(this.supplierID);
        } else {
            // For Add
            this.addSupplierForm = this.initAddForm();
            this.initAddForm();
        }
    }

    // load Supplier data for update
    loadSupplierData(id: number) {
        this.SupplierService.getSupplierbyId(id).subscribe((response) => {
            if (response?.isSuccess && response.data) {
                const supplierdata = response.data;
                console.log('this is supplier data',supplierdata)
                this.editsupplierForm.patchValue({
                    name: supplierdata.name,
                    mobile: supplierdata.mobile,
                    email: supplierdata.email,
                    gstnumber: supplierdata.gstnumber,
                    address: supplierdata.address,
                });
            } else {
                console.error("Failed to load Supplier data", response);
            }
        });
    }

    // Add User Form
    initAddForm(): FormGroup {
        const form = this.fb.group({
            parmacyId: [null],
            name: [null, [Validators.required,]],
            mobile: [null, [Validators.required, Validators.pattern(/^\d{10}$/)]],
            address: [''],
            email: ['', [Validators.required, this.validator.ValidateEmail]],
            gstnumber: [null, [Validators.required, Validators.pattern(/^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}[Z]{1}[0-9A-Z]{1}$/)]]
        });
        return form;
    }

    // Edit User Form
    initEditForm(): FormGroup {
        return this.fb.group({
            pharmacyId: [null],
            name: [null, [Validators.required,]],
            mobile: [null, [Validators.required, Validators.pattern(/^\d{10}$/)]],
            address: [''],
            email: ['', [Validators.required, this.validator.ValidateEmail]],
            gstnumber: [null, [Validators.required, Validators.pattern(/^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}[Z]{1}[0-9A-Z]{1}$/)]]
        });
    }

    onSubmit() {
        if (this.supplierID && this.supplierID !== 0) {
            if (this.editsupplierForm.invalid) {
                this.validator.markInvalidFieldsTouched(this.editsupplierForm);
                return;
            }
            const dto: SupplierDto = {
                ...this.editsupplierForm.value,
                pharmacyId: this.pharmacyId
            };
            this.SupplierService.editSupplier(dto, this.supplierID).subscribe({
                next: (response) => {
                    if (response.isSuccess) {
                        Helper.ShowSuccess(response.message || 'Supplier Updated successfully.');
                        console.log('check pharmacy id is null', this.pharmacyId)
                        this.router.navigate(['/pharmacy/supplier/list']);


                    } else {
                        this.errorMessage = response.message || 'Failed to add supplier.';
                        Helper.ShowError(this.errorMessage);
                    }
                },
                error: (err) => {
                    this.errorMessage = err?.error?.message || 'Something went wrong. Please try again.';
                    Helper.ShowError(this.errorMessage);
                }
            })
        } else {
            if (this.addSupplierForm.invalid) {
                this.validator.markInvalidFieldsTouched(this.addSupplierForm);
                return;
            }
            const dto: SupplierDto = {
                ...this.addSupplierForm.value,
                pharmacyId: this.pharmacyId
            };
            this.SupplierService.addSupplier(dto).subscribe({
                next: (response) => {
                    if (response.isSuccess) {
                        Helper.ShowSuccess(response.message || 'Supplier add successfully.');
                        console.log('check pharmacy id is null', this.pharmacyId)
                        this.router.navigate(['/pharmacy/supplier/list']);
                    } else {
                        Helper.ShowError(this.errorMessage);
                    }
                },
                error: (err) => {
                    Helper.ShowError(this.errorMessage);
                }
            });
        }

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