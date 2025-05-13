import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";
import { AdminBaseComponent } from "../../admin.base.component";
import { Helper } from "../../../../app/common/helper.extenstions";
import { RoleService } from "../../services/role/role.services";
import { ComponentDto } from "../../../viewmodels/role/Component.dto";
import { AddRoleDto } from "../../../viewmodels/role/role.dto";
import { AutoValidateDirective } from "src/app/common/form.validator";

@Component({
    selector: 'app-edit-role',
    templateUrl: './roleedit.component.html',
    styleUrls: ['./addrole.component.css'],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, FormsModule, AutoValidateDirective],
})
export class EditRoleComponent extends AdminBaseComponent implements OnInit {
    editRoleForm: FormGroup;
    roleId: number;
    componentList: ComponentDto[] = [];
    errorMessage: string = '';

    readonlyRoles: string[] = ['admin', 'doctor', 'pharmacy'];
    isReadonlyRole: boolean = false;

    constructor(public validator: CustomValidator,
        public roleService: RoleService,
        public fb: FormBuilder,
        public route: ActivatedRoute,
        public router: Router,) {
        super();
    }

    ngOnInit(): void {
        this.roleId = Number(this.route.snapshot.paramMap.get('id'));
        this.editRoleForm = this.initForm();
        this.loadModules();
        this.loadRoleDetails();
    }

    initForm(): FormGroup {
        return this.fb.group({
            roleName: new FormControl(null, Validators.required),
            description: new FormControl(null),
            PermissionIds: [[], [Validators.required, this.validator.checkboxRequiredValidator]]  // Ensure it's an empty array initially
        });
    }

    loadModules() {
        this.roleService.getmodule().subscribe((response) => {
            if (response?.isSuccess && response.data) {
                console.log('data component', response.data)
                this.componentList = response.data;
            } else {
                console.error("Failed to load component data", response);
            }
        });
    }

    loadRoleDetails() {
        this.roleService.getmodulebyroleid(this.roleId).subscribe((response: any) => {
            const roleData = response.data.role;
            const permissionsData = response.data.permissions;

            // Debugging to make sure roleName is loaded
            console.log('Loaded Role Name:', roleData.roleName);

            const permissionIds = Array.isArray(permissionsData) ? permissionsData.map((permission: any) => permission.id) : [];

            this.editRoleForm.patchValue({
                roleName: roleData.roleName,
                description: roleData.description,
                PermissionIds: permissionIds
            });

            this.editRoleForm.get('PermissionIds')?.updateValueAndValidity();

            const currentRoleName = (roleData.roleName || '').toLowerCase();
            this.isReadonlyRole = this.readonlyRoles.includes(currentRoleName);

            if (this.isReadonlyRole) {
                this.editRoleForm.get('roleName')?.disable(); // Disable if readonly role
            }
        });
    }

    onCancel() {
        this.router.navigate(['/admin/role/list']);
    }

    onCheckboxChange(event: any) {
        const permissionIds = this.editRoleForm.get('PermissionIds').value as number[];
        if (event.target.checked) {
            permissionIds.push(+event.target.value);
        } else {
            const index = permissionIds.indexOf(+event.target.value);
            if (index !== -1) {
                permissionIds.splice(index, 1);
            }
        }
        this.editRoleForm.get('PermissionIds').setValue(permissionIds);
        this.editRoleForm.get('PermissionIds').markAsTouched();
    }

    onSubmit() {
        if (this.isReadonlyRole) {
            Helper.ShowError('You cannot update Admin, Doctor, or Pharmacy roles.');
            return;
        }
        this.validator.markInvalidFieldsTouched(this.editRoleForm);

        if (this.editRoleForm.invalid) {

            const componentIdControl = this.editRoleForm.get('PermissionIds');

            componentIdControl?.updateValueAndValidity();

            if (componentIdControl?.hasError('checkboxRequired')) {
                this.errorMessage = 'Please select at least one module.';
                Helper.ShowError(this.errorMessage);
            }
            return;
        }
        const dto: AddRoleDto = this.editRoleForm.value;
        const roleId = this.route.snapshot.params['id'];
        this.roleService.editrole(this.editRoleForm.value, roleId).subscribe({
            next: (response) => {
                if (response.isSuccess) {
                    console.log('Successfully updated');
                    Helper.ShowSuccess(response.message || 'Role updated successfully.');
                    this.router.navigate(['/admin/role/list']);
                } else {
                    console.log('Update not successful');
                    console.error('API returned isSuccess: false');
                    this.errorMessage = response.message || 'Failed to update role.';
                    Helper.ShowError(this.errorMessage);
                }
            },
            error: (err) => {
                console.log('API error during update');
                console.error('HTTP Error:', err);
                this.errorMessage = err?.error?.message || 'Something went wrong. Please try again.';
                Helper.ShowError(this.errorMessage);
            }
        });
    }
}
