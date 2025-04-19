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
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

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
    countries: any[] = [];
    states: any[] = [];
    cities: any[] = [];


    ngOnInit(): void {
        this.editpharmacyForm = this.initForm();
        this.loadCountries();
        const today = new Date();
        this.minExpiryDate = today.toISOString().split('T')[0];

        const pharmacyId = this.route.snapshot.params['id'];
        if (pharmacyId) {
            this.adminpharmacyservice.getpharmacybyId(pharmacyId).subscribe((response) => {

                const pharmacydata = response;
                const countryId = pharmacydata.pharmacy.countryId;
                const stateId = pharmacydata.pharmacy.stateId;
                const cityId = pharmacydata.pharmacy.cityId;
                this.editpharmacyForm.patchValue({
                    pharmacy: {
                        ownerName: pharmacydata.pharmacy.ownerName,
                        storeName: pharmacydata.pharmacy.storeName,
                        businessName: pharmacydata.pharmacy.businessName,
                        licenseNumber: pharmacydata.pharmacy.licenseNumber,
                        gstNumber: pharmacydata.pharmacy.gstnumber,
                        registeredMobile: pharmacydata.pharmacy.registeredMobile,
                        officialEmail: pharmacydata.pharmacy.officialEmail,
                        storeMobile1: pharmacydata.pharmacy.storeMobile1,
                        storeEmail1: pharmacydata.pharmacy.storeEmail1,
                        storeEmail2: pharmacydata.pharmacy.storeEmail2,
                        address1: pharmacydata.pharmacy.address1,
                        address2: pharmacydata.pharmacy.address2,
                        countryId: countryId,
                        //stateId: pharmacydata.pharmacy.stateId,
                        //cityId: pharmacydata.pharmacy.cityId,
                    },
                    user: {
                        firstName: pharmacydata.user.firstName,
                        lastName: pharmacydata.user.lastName,
                        email: pharmacydata.user.email,
                        mobile: pharmacydata.user.mobile,
                    },
                    pharmacyBankDetails: {
                        bankName: pharmacydata.pharmacyBankDetails.bankName,
                        accountholderName: pharmacydata.pharmacyBankDetails.accountHolderName,
                        accountNumber: pharmacydata.pharmacyBankDetails.accountNumber,
                        ifscCode: pharmacydata.pharmacyBankDetails.ifsccode,
                        branchName: pharmacydata.pharmacyBankDetails.branchName,
                        upiId: pharmacydata.pharmacyBankDetails.upiId,
                    }
                });
                // Now load states, then set stateId
                this.loadStates(countryId).subscribe(() => {
                    this.editpharmacyForm.get('pharmacy.stateId')?.setValue(stateId);

                    // Now load cities, then set cityId
                    this.loadCities(stateId).subscribe(() => {
                        setTimeout(() => {
                            this.editpharmacyForm.get('pharmacy.cityId')?.setValue(cityId);
                        });
                    });
                    
                });
            });
        }
        this.editpharmacyForm.get('pharmacy.countryId')?.valueChanges.subscribe(countryId => {
            this.editpharmacyForm.get('pharmacy.stateId')?.setValue(null);
            this.editpharmacyForm.get('pharmacy.cityId')?.setValue(null);
            this.states = [];
            this.cities = [];
            if (countryId) {
                this.loadStates(countryId).subscribe();
            }
        });

        this.editpharmacyForm.get('pharmacy.stateId')?.valueChanges.subscribe(stateId => {
            this.editpharmacyForm.get('pharmacy.cityId')?.setValue(null);
            this.cities = [];
            if (stateId) {
                this.loadCities(stateId).subscribe();
            }
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
                Role: new FormControl("pharmacy"),
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
                accountNumber: new FormControl(null, [Validators.required, Validators.maxLength(16)]),
                ifscCode: new FormControl(null, [Validators.required]),
                branchName: new FormControl(null, [Validators.required]),
                upiId: new FormControl(null, [Validators.pattern(/^[a-zA-Z0-9.\-_]{2,256}@[a-zA-Z]{2,64}$/)])
            })
        });
    }

    onSubmit() {
        if (this.editpharmacyForm.invalid) {
            this.validator.markInvalidFieldsTouched(this.editpharmacyForm);
            return;
        }
        const pharmacyId = this.route.snapshot.params['id'];
        const dto: PharmacyDto = this.editpharmacyForm.value;
        console.log("dto", dto)
        this.adminpharmacyservice.editpharmacy(dto, pharmacyId).subscribe({
            next: res => console.log("Success", res),
            error: err => console.error("Error", err)
        })
        // Submit the form
        console.log('Form submitted:', this.editpharmacyForm.value);
    }

    loadCountries() {
        this.adminpharmacyservice.getcountry().subscribe((res: any) => {
            if (res?.isSuccess) {
                this.countries = res.data;
            }

        });
    }
    loadStates(countryId: number): Observable<any> {
        return this.adminpharmacyservice.getstatebyId(countryId).pipe(
            tap((res: any) => {
                if (res?.isSuccess) {
                    this.states = res.data;
                }
            })
        );
    }

    loadCities(stateId: number): Observable<any> {
        return this.adminpharmacyservice.getcitiesbyId(stateId).pipe(
            tap((res: any) => {
                if (res?.isSuccess) {
                    this.cities = res.data;
                }
            })
        );
    }


    allowOnlyNumbers(event: KeyboardEvent) {
        const charCode = event.key.charCodeAt(0);
        if (charCode < 48 || charCode > 57) {
            event.preventDefault();
        }
    }
}