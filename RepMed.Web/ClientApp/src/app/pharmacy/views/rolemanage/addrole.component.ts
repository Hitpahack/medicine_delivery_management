import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";
import { Helper } from "../../../../app/common/helper.extenstions";
import { ComponentDto } from "../../../viewmodels/role/Component.dto";
import { AddRoleDto } from "../../../viewmodels/role/role.dto";
import { AutoValidateDirective } from "../../../common/form.validator";
import { AdminBaseComponent } from "../../../admin/admin.base.component";
import { RoleService } from "../../../../app/pharmacy/services/role/role.services";
declare const window: any;

@Component({
    selector: 'admin-add-role',
    templateUrl: './addrole.component.html',
    styleUrls: ['./addrole.component.css'],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, FormsModule, AutoValidateDirective],
})

export class PharmacyAddRoleComponent extends AdminBaseComponent implements OnInit {
    addroleForm: FormGroup;
    ComponentDto: ComponentDto;
    errorMessage: string = '';
    selectedComponentIds: number[] = [];
    componentList: ComponentDto[] = [];
    pharmacyId: string | null = null;

    constructor(public validator: CustomValidator, public RoleService: RoleService, public fb: FormBuilder) {
        super();
    }

    ngOnInit(): void {
        this.addroleForm = this.initForm();
        this.pharmacyId = sessionStorage.getItem('pharmacyId');
        this.RoleService.getmodule().subscribe((response) => {
            if (response?.isSuccess && response.data) {
                console.log('data component', response.data)
                this.componentList = response.data;
            } else {
                console.error("Failed to load component data", response);
            }
        });
    }

    initForm(): FormGroup {
        return this.fb.group({
            roleName: new FormControl(null, [Validators.required]),
            description: new FormControl(null),
            PermissionIds: [[], [this.validator.checkboxRequiredValidator]]  // Ensure it's an empty array initially
        });
    }

    onSubmit() {
        this.validator.markInvalidFieldsTouched(this.addroleForm);

        if (this.addroleForm.invalid && this.validator.checkboxRequiredValidator) {

            const componentIdControl = this.addroleForm.get('PermissionIds');

            componentIdControl?.updateValueAndValidity();

            if (componentIdControl?.hasError('checkboxRequired')) {
                this.errorMessage = 'Please select at least one module.';
                Helper.ShowError(this.errorMessage);
            }
            return;
        }

        // Proceed with form submission if valid
        //const dto: AddRoleDto = this.addroleForm.value;
        const dto: AddRoleDto = {
            ...this.addroleForm.value,  // Preserve the other values
            pharmacyId: this.pharmacyId   // Add the pharmacyId
        };
        console.log("role", this.addroleForm)
        this.RoleService.add(dto).subscribe({
            next: (response) => {
                if (response.isSuccess) {
                    console.log('successfully add')
                    Helper.ShowSuccess(response.message || 'Role added successfully.');
                    this.router.navigate(['/pharmacy/role/list']);
                } else {
                    console.log('not successfully add')
                    console.error('API returned isSuccess: false');
                    this.errorMessage = response.message || 'Failed to add user.';
                    Helper.ShowError(this.errorMessage);
                }
            },
            error: (err) => {
                console.log('api error add')
                console.error('HTTP Error:', err);
                this.errorMessage = err?.error?.message || 'Something went wrong. Please try again.';
                Helper.ShowError(this.errorMessage);
            }
        });
    }

    onCheckboxChange(event: any) {
        const id = +event.target.value;
        let componentIds = [...this.addroleForm.get('PermissionIds').value]; // clone

        if (event.target.checked) {
            if (!componentIds.includes(id)) {
                componentIds.push(id);
            }
        } else {
            componentIds = componentIds.filter(x => x !== id);
        }

        const control = this.addroleForm.get('PermissionIds');
        control.setValue(componentIds);
        control.markAsTouched(); // important
        control.updateValueAndValidity(); // trigger validator
    }
}