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
import { AfterViewInit } from '@angular/core';
declare const window: any;

@Component({
  selector: 'admin-add-user',
  templateUrl: './admin.adduser.component.html',
  standalone: true,
  styleUrls: ['./admin.editprofile.component.css'],
  imports: [CommonModule, ReactiveFormsModule, FormsModule, AutoValidateDirective],
})

export class AddUserComponent extends AdminBaseComponent implements OnInit, AfterViewInit {
  addUserForm: FormGroup;
  addUserData: AddPersonDto;
  roles: Role[] = [];
  errorMessage: string = '';

  constructor(public validator: CustomValidator, public adminuserservice: AdminUserService) {
    super();
  }
  ngAfterViewInit(): void {
    if (window.Helpers && typeof window.Helpers.initPasswordToggle === 'function') {
      window.Helpers.initPasswordToggle();
    }
    $('.menu-toggle').on('click', function () {
      $(this).next('.menu-sub').slideToggle();
      $(this).parent().toggleClass('open');
    });
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
      next: (response) => {
        if (response.isSuccess) {
          console.log('Success:', response.data);
          this.router.navigate(['/admin/user']);
        } else {
          console.error('API returned isSuccess: false');
          this.errorMessage = response.message || 'Failed to add user.';
          Helper.ShowError(this.errorMessage);  // Optional toast/popup
        }
      },
      error: (err) => {
        console.error('HTTP Error:', err);
        this.errorMessage = err?.error?.message || 'Something went wrong. Please try again.';
        Helper.ShowError(this.errorMessage);  // Optional toast/popup
      }
    });
  }

  allowOnlyNumbers(event: KeyboardEvent) {
    const charCode = event.key.charCodeAt(0);
    if (charCode < 48 || charCode > 57) {
      event.preventDefault();
    }
  }
}