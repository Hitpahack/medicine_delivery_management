import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";
import { AdminBaseComponent } from "../../admin.base.component";
import { Helper } from "../../../../app/common/helper.extenstions";
import { AdminUserService } from "../../services/users/admin.user.services";
import { AddPersonDto } from "../../../viewmodels/User/Person.add.dto";
import { Role } from "../../../viewmodels/User/role.model";
import { AutoValidateDirective } from "src/app/common/form.validator";

@Component({
  selector: 'admin-add-user',
  templateUrl: './admin.adduser.component.html',
  standalone: true,
  styleUrls: ['./admin.editprofile.component.css'],
  imports: [CommonModule, ReactiveFormsModule, FormsModule, AutoValidateDirective],
})

export class AddUserComponent extends AdminBaseComponent implements OnInit {
  addUserForm: FormGroup;
  addUserData: AddPersonDto;
  roles: Role[] = [];

  constructor(public validator: CustomValidator, public adminuserservice: AdminUserService) {
    super();
  }

  ngOnInit(): void {
    this.addUserForm = this.initForm();
    this.adminuserservice.getRoles().subscribe((res) => {
      if (res?.isSuccess) {
        this.roles = res.data;
      }
    });
  }

  initForm(): FormGroup {
    const form = this.fb.group({
      Role: new FormControl(null, [Validators.required]),
      email: new FormControl(null, [Validators.required, this.validator.ValidateEmail]),
      Password: new FormControl(null, [Validators.required, this.validator.validateStrongPassword]),
      ConfirmPassword: new FormControl(null, [Validators.required]),
    }, {
      validators: this.validator.passwordMatchValidator
    });

    // 👇 Re-evaluate the form group validator when password or confirm password changes
    form.get('Password')?.valueChanges.subscribe(() => {
      form.updateValueAndValidity({ onlySelf: false });
    });

    form.get('ConfirmPassword')?.valueChanges.subscribe(() => {
      form.updateValueAndValidity({ onlySelf: false });
    });

    return form;
  }

  onSubmit() {
    if (this.addUserForm.invalid) {
      this.validator.markInvalidFieldsTouched(this.addUserForm);
      return;
    }
    const dto: AddPersonDto = this.addUserForm.value;
    this.adminuserservice.add(dto).subscribe({
      next: res => this.router.navigate(['/admin/user/list']),
      error: err => Helper.ShowError(err)
    });

   
  }

  allowOnlyNumbers(event: KeyboardEvent) {
    const charCode = event.key.charCodeAt(0);
    if (charCode < 48 || charCode > 57) {
      event.preventDefault();
    }
  }
}