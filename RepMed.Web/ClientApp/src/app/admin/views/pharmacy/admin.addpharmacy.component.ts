import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder, FormsModule, ReactiveFormsModule, FormControl, FormGroup, Validators, AbstractControl } from "@angular/forms";
import { AdminBaseComponent } from "../../admin.base.component";
import { CommonModule } from "@angular/common";
import { CustomValidator } from "../../../common/custom.validators";
import { Helper } from "../../../common/helper.extenstions";
import { adminAccountsService } from "../../services/accounts/admin.accountsservice";
import { AdminPharmacyService } from "../../services/pharmacy/admin.pharmacy.services";
import { PharmacyDto } from 'src/app/viewmodels/pharmacy/Pharmacy.add.dto';

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
        public accountservice: adminAccountsService,
        public PharmacyService: AdminPharmacyService
    ) {
        super(router, fb);
    }
    minExpiryDate: string = '';
    phForm: FormGroup;
    PharmacyDto: PharmacyDto;
    maxDate = new Date().toISOString().split('T')[0];

    ngOnInit(): void {
        // This runs when the dashboard loads.
        //console.log('Add Pharmacy Page loaded!');
        this.phForm = this.initForm();
        const today = new Date();
        this.minExpiryDate = today.toISOString().split('T')[0]; // YYYY-MM-DD
    }

    initForm(): FormGroup {
        return this.fb.group({
            //email: new FormControl(null, [this.validator.ValidateEmail,Validators.required]),
            ownerName: new FormControl(null, [Validators.required]),
            storeName: new FormControl(null, [Validators.required]),
            businessName: new FormControl(null, [Validators.required]),
            licenseNumber: new FormControl(null, [Validators.required]),
            //licenseExpiry: new FormControl(null, [Validators.required]),
            licenseExpiry: [null, [Validators.required, this.validator.futureDateValidator]],
            gstNumber: new FormControl(null, [Validators.required, Validators.pattern(/^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}[Z]{1}[0-9A-Z]{1}$/)]),
            registeredMobile: new FormControl(null, [Validators.required, Validators.pattern(/^\d{10}$/)]),
            //registeredMobile: ['', [Validators.required,Validators.pattern(/^[0-9]{10}$/)]],  // only 10 digit numbers allowed
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
            dateofBirth: new FormControl(null, [Validators.required, this.validator.pastDateOnly]),
            gender: new FormControl(null, [Validators.required]),
            picture: new FormControl(null, [Validators.required]),

            bankName: new FormControl(null, [Validators.required]),
            accountholderName: new FormControl(null, [Validators.required]),
            accountNumber: new FormControl(null, [Validators.required]),
            ifscCode: new FormControl(null, [Validators.required]),
            branchName: new FormControl(null, [Validators.required]),
            upiId: new FormControl(null, [Validators.required, Validators.pattern(/^[a-zA-Z0-9.\-_]{2,256}@[a-zA-Z]{2,64}$/)])
        });
    }

    allowOnlyNumbers(event: KeyboardEvent) {
        const charCode = event.key.charCodeAt(0);
        // Allow only digits (0–9)
        if (charCode < 48 || charCode > 57) {
            event.preventDefault();
        }
    }
    onSubmit() {
        if (this.phForm.invalid) {
            this.validator.markInvalidFieldsTouched(this.phForm);
            return;
        }
        const dto: PharmacyDto = this.phForm.value;
        this.PharmacyService.add(dto,0).subscribe({
            next: res => console.log("Success", res),
            error: err => console.error("Error", err)
        })
        // Submit the form
        console.log('Form submitted:', this.phForm.value);
    }
}
