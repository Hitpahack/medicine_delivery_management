import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router, RouterModule } from "@angular/router";
import { CustomValidator } from "../../../common/custom.validators";
import { AdminBaseComponent } from "../../admin.base.component";
import { Helper } from "../../../../app/common/helper.extenstions";
import { AdminPharmacyService } from "../../services/pharmacy/admin.pharmacy.services";
import { PharmacyDto } from "src/app/viewmodels/pharmacy/Pharmacy.add.dto";
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'admin-edit-pharmacy',
    templateUrl: 'admin.pharmacyedit.component.html',
    standalone: true,
    styles: [''],
    imports: [CommonModule, ReactiveFormsModule, FormsModule, RouterModule],
})

export class EditPharmacy extends AdminBaseComponent implements OnInit {
    editpharmacyForm: FormGroup
    addpharmacyData: PharmacyDto;
    constructor(public router: Router,
         public fb: FormBuilder, 
         public validator: CustomValidator, 
         public adminpharmacyservice: AdminPharmacyService, 
         private route: ActivatedRoute) {
        super(router, fb);
    }

    minExpiryDate: string = '';
    maxDate = new Date().toISOString().split('T')[0];

    ngOnInit(): void {
        const today = new Date();
        this.minExpiryDate = today.toISOString().split('T')[0]; // YYYY-MM-DD
        this.editpharmacyForm = this.initForm();
        const userId = this.route.snapshot.params['id'];
        console.log("userids", userId)
        if (userId) {
            this.adminpharmacyservice.getpharmacybyId(userId).subscribe((response) => {
                if (response?.isSuccess && response.data) {
                    const pharmacydto = response.data;
                    console.log("User data received:", pharmacydto.pharmacy.ownerName);

                    this.editpharmacyForm.patchValue({
                        ownerName: pharmacydto.pharmacy.ownerName,
                        storeName: pharmacydto.pharmacy.storeName,
                        businessName: pharmacydto.pharmacy.businessName,
                        licenseNumber: pharmacydto.pharmacy.licenseNumber,
                        licenseExpiry: pharmacydto.pharmacy.licenseExpiry,
                        gstNumber: pharmacydto.pharmacy.gstnumber,
                        registeredMobile: pharmacydto.pharmacy.registeredMobile,
                        officialEmail: pharmacydto.pharmacy.officialEmail,
                        storeMobile1: pharmacydto.pharmacy.storeMobile1,
                        storeEmail1: pharmacydto.pharmacy.storeEmail1,
                        storeEmail2: pharmacydto.pharmacy.storeEmail2,
                        address1: pharmacydto.pharmacy.address1,
                        address2: pharmacydto.pharmacy.address2,
                        countryId: pharmacydto.pharmacy.countryId,
                        stateId: pharmacydto.pharmacy.stateId,
                        cityId: pharmacydto.pharmacy.cityId,
                        firstName: pharmacydto.person.firstName,
                        lastName: pharmacydto.person.lastName,
                        email: pharmacydto.person.email,
                        mobile: pharmacydto.person.mobile,
                        dateofBirth: pharmacydto.person.dateOfBirth,
                        gender: pharmacydto.person.gender,
                        bankName: pharmacydto.pharmacyBankDetails.bankName,
                        accountholderName: pharmacydto.pharmacyBankDetails.accountHolderName,
                        accountNumber: pharmacydto.pharmacyBankDetails.accountNumber,
                        ifscCode: pharmacydto.pharmacyBankDetails.ifsccode,
                        branchName: pharmacydto.pharmacyBankDetails.branchName,
                        upiId: pharmacydto.pharmacyBankDetails.upiId,

                    });
                } else {
                    console.error("Failed to load pharmacy data", response);
                }
            });
        }
    }


    initForm(): FormGroup {
        return this.fb.group({
            pharmacy: this.fb.group({
                ownerName: new FormControl(null, [Validators.required]),
                storeName: new FormControl(null, [Validators.required]),
                businessName: new FormControl(null, [Validators.required]),
                licenseNumber: new FormControl(null, [Validators.required]),
                licenseExpiry: [null, [Validators.required, this.validator.futureDateValidator]],
                gstNumber: new FormControl(null, [Validators.required, Validators.pattern(/^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}[Z]{1}[0-9A-Z]{1}$/)]),
                registeredMobile: new FormControl(null, [Validators.required, Validators.pattern(/^\d{10}$/)]),
                officialEmail: new FormControl(null, [Validators.required, Validators.email]),
                storeMobile1: new FormControl(null, [Validators.required, Validators.pattern(/^\d{10}$/)]),
                storeEmail1: new FormControl(null, [Validators.email]),
                storeEmail2: new FormControl(null, [Validators.email]),
                address1: new FormControl(null, [Validators.required]),
                address2: new FormControl(null, [Validators.required]),
                countryId: new FormControl(null, [Validators.required]),
                stateId: new FormControl(null, [Validators.required]),
                cityId: new FormControl(null, [Validators.required]),
            }),
            user: this.fb.group({
                firstName: new FormControl(null, [Validators.required]),
                lastName: new FormControl(null, [Validators.required]),
                email: new FormControl(null, [Validators.required, Validators.email]),
                mobile: new FormControl(null, [Validators.required, Validators.pattern(/^\d{10}$/)]),
                dateofBirth: new FormControl(null, [Validators.required, this.validator.pastDateOnly]),
                gender: new FormControl(null, [Validators.required]),
                picture: new FormControl(null, []),
            }),
            pharmacyBankDetails: this.fb.group({ 
                bankName: new FormControl(null, [Validators.required]),
                accountholderName: new FormControl(null, [Validators.required]),
                accountNumber: new FormControl(null, [Validators.required, Validators.pattern(/^\d{16}$/)]),
                ifscCode: new FormControl(null, [Validators.required]),
                branchName: new FormControl(null, [Validators.required]),
                upiId: new FormControl(null, [Validators.required, Validators.pattern(/^[a-zA-Z0-9.\-_]{2,256}@[a-zA-Z]{2,64}$/)])
            })
        });
    }

    onSubmit() {
        console.log("form submited")
        let isValid = this.validateForm(this.editpharmacyForm);
        console.log("isValid")
        if (isValid) {
            const pharmacyId = this.route.snapshot.params['id'];

            const dto: PharmacyDto = this.editpharmacyForm.value;
            this.adminpharmacyservice.add(dto, pharmacyId).subscribe(response => {
                console.log("Pharmacy updated successfully!")
            })
        }
        else
            Helper.ShowError('Please fill the required fields');
    }


    allowOnlyNumbers(event: KeyboardEvent) {
        const charCode = event.key.charCodeAt(0);
        if (charCode < 48 || charCode > 57) {
            event.preventDefault();
        }
    }
}