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
    styleUrl: './admin.addpharmacy.component.css',
    imports: [CommonModule, ReactiveFormsModule, FormsModule, RouterModule],
})

export class EditPharmacy extends AdminBaseComponent implements OnInit {

    constructor(public router: Router,
        public fb: FormBuilder,
        public validator: CustomValidator,
        public adminpharmacyservice: AdminPharmacyService,
        private route: ActivatedRoute) {
        super(router, fb);
    }
    editpharmacyForm: FormGroup
    addpharmacyData: PharmacyDto;
    minExpiryDate: string = '';
    maxDate = new Date().toISOString().split('T')[0];
    countries: any[] = [
        { id: 1, name: 'India' },
        { id: 2, name: 'USA' },
        { id: 3, name: 'Canada' },
        { id: 4, name: 'Australia' },
        { id: 5, name: 'Germany' },
        { id: 6, name: 'France' },
        { id: 7, name: 'Brazil' },
        { id: 8, name: 'Japan' },
        { id: 9, name: 'South Africa' },
        { id: 10, name: 'United Kingdom' }
    ]; // If you need to load them
    states: any[] = [];
    cities: any[] = [];

    ngOnInit(): void {
        const today = new Date();
        this.minExpiryDate = today.toISOString().split('T')[0];

        const pharmacyGroup = this.editpharmacyForm.get('pharmacy');
        pharmacyGroup?.get('countryId')?.valueChanges.subscribe(countryId => {
            this.states = this.getStatesByCountry(countryId);
            pharmacyGroup.get('stateId')?.reset();
            pharmacyGroup.get('cityId')?.reset();
            this.cities = [];
        });

        pharmacyGroup?.get('stateId')?.valueChanges.subscribe(stateId => {
            this.cities = this.getCitiesByState(stateId);
            pharmacyGroup.get('cityId')?.reset();
        });
        this.editpharmacyForm = this.initForm();
        const userId = this.route.snapshot.params['id'];
        if (userId) {
            this.adminpharmacyservice.getpharmacybyId(userId).subscribe((response) => {

                const pharmacydata = response;
                console.log("User data received:", pharmacydata.pharmacy.ownerName);

                this.editpharmacyForm.patchValue({
                    pharmacy: {
                        ownerName: pharmacydata.pharmacy.ownerName,
                        storeName: pharmacydata.pharmacy.storeName,
                        businessName: pharmacydata.pharmacy.businessName,
                        licenseNumber: pharmacydata.pharmacy.licenseNumber,
                        licenseExpiry: pharmacydata.pharmacy.licenseExpiry,
                        gstNumber: pharmacydata.pharmacy.gstnumber,
                        registeredMobile: pharmacydata.pharmacy.registeredMobile,
                        officialEmail: pharmacydata.pharmacy.officialEmail,
                        storeMobile1: pharmacydata.pharmacy.storeMobile1,
                        storeEmail1: pharmacydata.pharmacy.storeEmail1,
                        storeEmail2: pharmacydata.pharmacy.storeEmail2,
                        address1: pharmacydata.pharmacy.address1,
                        address2: pharmacydata.pharmacy.address2,
                        countryId: pharmacydata.pharmacy.countryId,
                        stateId: pharmacydata.pharmacy.stateId,
                        cityId: pharmacydata.pharmacy.cityId,
                    },
                    // user: {
                    //     firstName: pharmacydata.person.firstName,
                    //     lastName: pharmacydata.person.lastName,
                    //     email: pharmacydata.person.email,
                    //     mobile: pharmacydata.person.mobile,
                    //     dateofBirth: pharmacydata.person.dateOfBirth,
                    //     gender: pharmacydata.person.gender,
                    //     picture: pharmacydata.person.picture,
                    //     //Password: pharmacydata.person.password,
                    //     //ConfirmPassword: pharmacydata.person.confirmPassword
                    // },
                    pharmacyBankDetails: {
                        bankName: pharmacydata.pharmacyBankDetails.bankName,
                        accountholderName: pharmacydata.pharmacyBankDetails.accountHolderName,
                        accountNumber: pharmacydata.pharmacyBankDetails.accountNumber,
                        ifscCode: pharmacydata.pharmacyBankDetails.ifsccode,
                        branchName: pharmacydata.pharmacyBankDetails.branchName,
                        upiId: pharmacydata.pharmacyBankDetails.upiId,
                    }

                });
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
            // user: this.fb.group({
            //     firstName: new FormControl(null, [Validators.required]),
            //     lastName: new FormControl(null, [Validators.required]),
            //     email: new FormControl(null, [Validators.required, Validators.email]),
            //     mobile: new FormControl(null, [Validators.required, Validators.pattern(/^\d{10}$/)]),
            //     dateofBirth: new FormControl(null, [Validators.required, this.validator.pastDateOnly]),
            //     gender: new FormControl(null, [Validators.required]),
            //     picture: new FormControl(null, []),
            // }),
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

    getStatesByCountry(countryId: number) {
        const allStates = [
            { id: 1, name: 'Maharashtra', countryId: 1 },
            { id: 2, name: 'Gujarat', countryId: 1 },
            { id: 3, name: 'California', countryId: 2 },
            { id: 4, name: 'Texas', countryId: 2 },
            { id: 5, name: 'Ontario', countryId: 3 },
            { id: 6, name: 'Quebec', countryId: 3 },
            { id: 7, name: 'New South Wales', countryId: 4 },
            { id: 8, name: 'Victoria', countryId: 4 },
            { id: 9, name: 'Bavaria', countryId: 5 },
            { id: 10, name: 'Berlin', countryId: 5 },
            { id: 11, name: 'Île-de-France', countryId: 6 },
            { id: 12, name: 'Provence', countryId: 6 },
            { id: 13, name: 'São Paulo', countryId: 7 },
            { id: 14, name: 'Rio de Janeiro', countryId: 7 },
            { id: 15, name: 'Tokyo', countryId: 8 },
            { id: 16, name: 'Osaka', countryId: 8 },
            { id: 17, name: 'Gauteng', countryId: 9 },
            { id: 18, name: 'Western Cape', countryId: 9 },
            { id: 19, name: 'England', countryId: 10 },
            { id: 20, name: 'Scotland', countryId: 10 }
        ];
        return allStates.filter(s => s.countryId == countryId);
    }

    getCitiesByState(stateId: number) {
        const allCities = [
            { id: 1, name: 'Mumbai', stateId: 1 },
            { id: 2, name: 'Pune', stateId: 1 },
            { id: 3, name: 'Ahmedabad', stateId: 2 },
            { id: 4, name: 'Surat', stateId: 2 },
            { id: 5, name: 'Los Angeles', stateId: 3 },
            { id: 6, name: 'San Francisco', stateId: 3 },
            { id: 7, name: 'Houston', stateId: 4 },
            { id: 8, name: 'Dallas', stateId: 4 },
            { id: 9, name: 'Toronto', stateId: 5 },
            { id: 10, name: 'Ottawa', stateId: 5 },
            { id: 11, name: 'Montreal', stateId: 6 },
            { id: 12, name: 'Quebec City', stateId: 6 },
            { id: 13, name: 'Sydney', stateId: 7 },
            { id: 14, name: 'Newcastle', stateId: 7 },
            { id: 15, name: 'Melbourne', stateId: 8 },
            { id: 16, name: 'Geelong', stateId: 8 },
            { id: 17, name: 'Munich', stateId: 9 },
            { id: 18, name: 'Nuremberg', stateId: 9 },
            { id: 19, name: 'Berlin City', stateId: 10 },
            { id: 20, name: 'Charlottenburg', stateId: 10 },
            { id: 21, name: 'Paris', stateId: 11 },
            { id: 22, name: 'Versailles', stateId: 11 },
            { id: 23, name: 'Marseille', stateId: 12 },
            { id: 24, name: 'Nice', stateId: 12 },
            { id: 25, name: 'São Paulo City', stateId: 13 },
            { id: 26, name: 'Campinas', stateId: 13 },
            { id: 27, name: 'Rio City', stateId: 14 },
            { id: 28, name: 'Niterói', stateId: 14 },
            { id: 29, name: 'Tokyo City', stateId: 15 },
            { id: 30, name: 'Shibuya', stateId: 15 },
            { id: 31, name: 'Osaka City', stateId: 16 },
            { id: 32, name: 'Sakai', stateId: 16 },
            { id: 33, name: 'Johannesburg', stateId: 17 },
            { id: 34, name: 'Pretoria', stateId: 17 },
            { id: 35, name: 'Cape Town', stateId: 18 },
            { id: 36, name: 'Stellenbosch', stateId: 18 },
            { id: 37, name: 'London', stateId: 19 },
            { id: 38, name: 'Manchester', stateId: 19 },
            { id: 39, name: 'Edinburgh', stateId: 20 },
            { id: 40, name: 'Glasgow', stateId: 20 }
        ];
        return allCities.filter(c => c.stateId == stateId);
    }
}