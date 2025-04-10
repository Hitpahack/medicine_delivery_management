import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";
import { AdminBaseComponent } from "../../admin.base.component";
import { Helper } from "../../../../app/common/helper.extenstions";

@Component({
selector: 'admin-add-user',
templateUrl: './admin.adduser.component.html',
standalone: true,
styles: [''],
imports: [CommonModule,ReactiveFormsModule, FormsModule],
})

export class AddUserComponent extends AdminBaseComponent implements OnInit{
addUserForm: FormGroup;
constructor(public router: Router, public fb: FormBuilder, public validator: CustomValidator)
{
    super(router, fb);
}

markInvalidFieldsTouched(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);
      if (control && control.invalid) {
        control.markAsTouched({ onlySelf: true });
      }
    });
  }
      

ngOnInit(): void {
    this.addUserForm = this.initForm();
  }    

  initForm(): FormGroup {
    return this.fb.group({
      firstname: new FormControl(null, [Validators.required]),
      lastname: new FormControl(null, [Validators.required]),
      email: new FormControl(null, [Validators.required, this.validator.ValidateEmail]),
      mobile: new FormControl(null, [Validators.required]),
    });
  }

  onSubmit() {

    let isValid = this.validateForm(this.addUserForm);
       if(isValid){
        console.log("form is valid")
       }
       else
       this.markInvalidFieldsTouched(this.addUserForm);
       Helper.ShowError('Please fill the required fields');
    // if(this.addUserForm.invalid){
    //     console.log("Form not Submitted");
    //     this.addUserForm.markAllAsTouched();
    // }else{
    //     console.log("Form Submitted:", this.addUserForm.value);
    // }
  }
}