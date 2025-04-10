import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder, FormsModule, ReactiveFormsModule, FormControl, FormGroup, Validators } from "@angular/forms";
import { AdminBaseComponent } from "../../admin.base.component";
import { CommonModule } from "@angular/common";
import { CustomValidator } from "../../../common/custom.validators";
import { Helper } from "../../../common/helper.extenstions";
import { adminAccountsService } from "../../services/accounts/admin.accountsservice";

@Component({
    selector: 'app-admin-addpharmacy',
    templateUrl: './admin.addpharmacy.component.html',
    styleUrls: ['./admin.addpharmacy.component.css'],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, FormsModule],
})
export class AdminAddPharmacyComponent extends AdminBaseComponent implements OnInit {
    constructor(
        public router: Router, public fb: FormBuilder,
        public validator: CustomValidator,
        public accountservice: adminAccountsService
    ) {
        super(router, fb);
    }

    phForm: FormGroup;
    ngOnInit(): void {
        // This runs when the dashboard loads.
        //console.log('Add Pharmacy Page loaded!');
        this.phForm = this.initForm();
    }

    initForm(): FormGroup {
        return this.fb.group({
            ownerName: new FormControl(null, [Validators.required]),
            storeName: new FormControl(null, [Validators.required]),
            businessName: new FormControl(null, [Validators.required]),
            licenseNumber: new FormControl(null, [Validators.required]),
            licenseExpiry: new FormControl(null, [Validators.required]),
            gstNumber: new FormControl(null, [Validators.required]),
            registeredMobile: new FormControl(null, [Validators.required, Validators.pattern(/^\d{10}$/)]),
            officialEmail: new FormControl(null, [Validators.required, Validators.email]),
            storeMobile1: new FormControl(null, [Validators.required, Validators.pattern(/^\d{10}$/)]),
            storeEmail1: new FormControl(null, [Validators.required, Validators.email]),
            storeEmail2: new FormControl(null, [Validators.required, Validators.email]),
            
            address1: new FormControl(null, [Validators.required]),
            address2: new FormControl(null, [Validators.required]),
            countryId: new FormControl(null, [Validators.required]),
            stateId: new FormControl(null, [Validators.required]),
            cityId: new FormControl(null, [Validators.required]),
            
            firstName: new FormControl(null, [Validators.required]),
            lastName: new FormControl(null, [Validators.required]),
            personEmail: new FormControl(null, [Validators.required, Validators.email]),
            personMobile: new FormControl(null, [Validators.required, Validators.pattern(/^\d{10}$/)]),
            dateofBirth: new FormControl(null, [Validators.required]),
            gender: new FormControl(null, [Validators.required]),
            picture: new FormControl(null, [Validators.required]),
            
            bankName: new FormControl(null, [Validators.required]),
            accountholderName: new FormControl(null, [Validators.required]),
            accountNumber: new FormControl(null, [Validators.required]),
            ifscCode: new FormControl(null, [Validators.required]),
            branchName: new FormControl(null, [Validators.required]),
            upiId: new FormControl(null, [Validators.required]),
            
        });
    }

    onSubmit() {
        console.log("Hello");
        debugger;
        let isValid = this.validateForm(this.phForm);
        if (isValid) {
            console.log("All fields fill(Successfully)");
        }
        else
        console.log("not fill");
            Helper.ShowError('Please fill the required fields');
    }
}
