import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router, RouterModule } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";
import { AdminBaseComponent } from "../../admin.base.component";
import { Helper } from "../../../../app/common/helper.extenstions";
import { AdminUserService } from "../../services/users/admin.user.services";
import { ActivatedRoute } from '@angular/router';
declare const window: any;

@Component({
  selector: 'admin-Change-password',
  templateUrl: './admin.changepassword.component.html',
  standalone: true,
  styleUrls: ['./admin.editprofile.component.css'],
  imports: [CommonModule, ReactiveFormsModule, FormsModule, RouterModule],
})

export class ChangePassword extends AdminBaseComponent implements OnInit {
  ChangePasswordForm: FormGroup;

  constructor(public router: Router, public fb: FormBuilder, public validator: CustomValidator, public adminuserservice: AdminUserService, private route: ActivatedRoute) {
    super(router, fb);
  }

  ngAfterViewInit(): void {
    if (window.Helpers && typeof window.Helpers.initPasswordToggle === 'function') {
      window.Helpers.initPasswordToggle();
    }
  }

  ngOnInit(): void {
    this.ChangePasswordForm = this.initForm();
    this.route.queryParams.subscribe(params => {
      const token = params['token'];
      console.log("token", token);
      if (token) {
        localStorage.setItem('authToken', token); // Store it if needed
      }
    });
  }
  
  initForm(): FormGroup {
    const form = this.fb.group({
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
    if (this.ChangePasswordForm.invalid) {
      this.ChangePasswordForm.markAllAsTouched();
      return;
    }
    let isValid = this.validateForm(this.ChangePasswordForm)
    if (isValid) {
    }
    else
      Helper.ShowError('Please fill the required fields');
  }

}