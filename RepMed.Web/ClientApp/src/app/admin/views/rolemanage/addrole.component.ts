import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";
import { AdminBaseComponent } from "../../admin.base.component";
import { Helper } from "../../../../app/common/helper.extenstions";
import { RoleService } from "../../services/role/role.services";
import { ComponentDto } from "../../../viewmodels/role/Component.dto";
import { AddRoleDto } from "../../../viewmodels/role/role.dto";
import { AutoValidateDirective } from "src/app/common/form.validator";
declare const window: any;

@Component({
    selector: 'admin-add-role',
    templateUrl: './addrole.component.html',
    styleUrls: ['./addrole.component.css'],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, FormsModule, AutoValidateDirective],
})

export class AddRoleComponent extends AdminBaseComponent implements OnInit {
    addroleForm: FormGroup;
    ComponentDto: ComponentDto;
    errorMessage: string = '';
    selectedComponentIds: number[] = [];
    componentList: ComponentDto[] = [];
    

    constructor(public validator: CustomValidator, public RoleService: RoleService) {
        super();
    }

    ngOnInit(): void {
        this.addroleForm = this.initForm();

        this.RoleService.getcomponent().subscribe((response) => {
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
            componentid: new FormControl([], [Validators.required, this.validator.checkboxRequiredValidator])  // Ensure it's an empty array initially
        });
    }

    onSubmit() {
        // Manually mark all fields as touched to trigger validation messages
        this.validator.markInvalidFieldsTouched(this.addroleForm);
        // Check if the form is invalid
        if (this.addroleForm.invalid) {
            const componentIdControl = this.addroleForm.get('componentid');
            if (componentIdControl?.hasError('checkboxRequired')) {
                // Error handling when checkbox is not selected
                this.errorMessage = 'Please select at least one component.';
                Helper.ShowError(this.errorMessage);  // Show the error message
            }
            return;
        }
    
        // Proceed with form submission if valid
        const dto: AddRoleDto = this.addroleForm.value;
        this.RoleService.add(dto).subscribe({
            next: (response) => {
                if (response.isSuccess) {
                    console.log('successfully add')
                    Helper.ShowSuccess(response.message || 'Role added successfully.');
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
        const componentIds = this.addroleForm.get('componentid').value;
        if (event.target.checked) {
            if (!componentIds.includes(id)) {
                componentIds.push(id);
            }
        } else {
            const index = componentIds.indexOf(id);
            if (index > -1) {
                componentIds.splice(index, 1);
            }
        }
        this.addroleForm.get('componentid').setValue(componentIds);
    }
}