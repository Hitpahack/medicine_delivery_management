import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { CustomValidator } from "../../../../app/common/custom.validators";
import { Helper } from "../../../../app/common/helper.extenstions";
import { AdminBaseComponent } from "../../admin.base.component";
import { adminAccountsService } from "../../services/accounts/admin.accountsservice";


@Component({
    selector: 'admin-login',
    templateUrl: './admin.login.component.html',
    styles: [''],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, FormsModule],
})

export class AdminLoginComponent extends AdminBaseComponent implements OnInit {

    constructor(
        public router: Router, public fb: FormBuilder,
        public validator: CustomValidator,
        public accountservice: adminAccountsService

    ) {
        super(router, fb);
    }

    loginForm: FormGroup;
    ngOnInit(): void {
        this.loginForm = this.initForm();
    }


    initForm(): FormGroup {
        return this.fb.group({
            email: new FormControl(null, [this.validator.ValidateEmail, Validators.required]),
            password: new FormControl(null, [Validators.required]),
            remamber: new FormControl(false, [Validators.required]),
        });
    }

    onSubmit() {
       let isValid = this.validateForm(this.loginForm);
       if(isValid){
        console.log("form is valid")
           this.accountservice.login(this.loginForm.value).subscribe(
               (response)=>{debugger;
                if(!response.isSuccess){
                    
                }
                else{
                    this.router.navigate(['/admin/dashboard']); // example: /dashboard
                }
               },
               (err)=>{
                   Helper.ShowExecptions(err);
               },
               () => { }
           )
       }
       else
       Helper.ShowError('Please fill the required fields');
   }
}