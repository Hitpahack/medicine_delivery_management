import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router, RouterModule } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";
import { AdminBaseComponent } from "../../admin.base.component";
import { Helper } from "../../../../app/common/helper.extenstions";
import { AdminUserService } from "../../services/users/admin.user.services";
import { ActivatedRoute } from '@angular/router';
import { AutoValidateDirective } from "src/app/common/form.validator";
declare const window: any;

@Component({
  selector: 'admin-set-password',
  templateUrl: './admin.setpassword.component.html',
  standalone: true,
  styleUrls: ['./admin.editprofile.component.css'],
  imports: [CommonModule, ReactiveFormsModule, FormsModule, RouterModule, AutoValidateDirective],
})


export class SetPassword extends AdminBaseComponent implements OnInit {
  SetPasswordForm: FormGroup;
  errorMessage: string = '';
  constructor(public router: Router, public fb: FormBuilder, public validator: CustomValidator, public adminuserservice: AdminUserService, private route: ActivatedRoute) {
    super(router, fb);
  }

  ngAfterViewInit(): void {
    if (window.Helpers && typeof window.Helpers.initPasswordToggle === 'function') {
      window.Helpers.initPasswordToggle();
    }
  }

  token: string;
  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.token = params['token'];
    });
    this.SetPasswordForm = this.initForm();
  }

  initForm(): FormGroup {
    const form = this.fb.group({
      token: new FormControl(this.token),
      Password: new FormControl(null, [Validators.required, this.validator.validateStrongPassword]),
      ConfirmPassword: new FormControl(null, [Validators.required]),
    },
      {
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
    if (this.SetPasswordForm.invalid) {
      this.validator.markInvalidFieldsTouched(this.SetPasswordForm);
      return;
    }
    let isValid = this.validateForm(this.SetPasswordForm)
    if (isValid) {
      this.adminuserservice.setpassword(this.SetPasswordForm.value).subscribe({
        next: (response) => {
          if (response.isSuccess) {
            Helper.ShowSuccess(response.message || '');
          } else {
            this.errorMessage = response.message || '';
            Helper.ShowError(this.errorMessage);
          }
        },
        error: (err) => {
          console.error('HTTP Error:', err);
          this.errorMessage = err?.error?.message || 'Something went wrong. Please try again.';
          Helper.ShowError(this.errorMessage);  // Optional toast/popup
        }
      });
    }
    else
      Helper.ShowError('Please fill the required fields');
  }
}
