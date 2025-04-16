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
        // This runs when the dashboard loads.
        //console.log('Add Pharmacy Page loaded!');
        this.phForm = this.initForm();
        const today = new Date();
        this.minExpiryDate = today.toISOString().split('T')[0];

        // this code for country, state, city
        const pharmacyGroup = this.phForm.get('pharmacy');
        pharmacyGroup?.get('countryId')?.valueChanges.subscribe(countryId => {
            this.states = this.getStatesByCountry(countryId); // Replace this with API if needed
            pharmacyGroup.get('stateId')?.reset();
            pharmacyGroup.get('cityId')?.reset();
            this.cities = [];
        });

        pharmacyGroup?.get('stateId')?.valueChanges.subscribe(stateId => {
            this.cities = this.getCitiesByState(stateId); // Replace this with API if needed
            pharmacyGroup.get('cityId')?.reset();
        });
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

    onSubmit() {
        if (this.phForm.invalid) {
            this.validator.markInvalidFieldsTouched(this.phForm);
            return;
        }
        const dto: PharmacyDto = this.phForm.value;
        this.PharmacyService.add(dto, 0).subscribe({
            next: res => console.log("Success", res),
            error: err => console.error("Error", err)
        })
        // Submit the form
        console.log('Form submitted:', this.phForm.value);
    }


}
