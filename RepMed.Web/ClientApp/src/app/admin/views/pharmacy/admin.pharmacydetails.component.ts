import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AdminBaseComponent } from '../../admin.base.component';
import { FormBuilder } from '@angular/forms';
declare var $: any;

@Component({
  selector: 'app-pharmacy-details',
  templateUrl: './admin.pharmacydetails.component.html'
})
export class AdminPharmacyDetailsComponent extends AdminBaseComponent implements OnInit, AfterViewInit {
    pharmacyId: number = 0;
    pharmacyDetails: any;

    allPharmacies = [
    {
        id: 1,
        ownerName: 'John Doe',
        storeName: 'MediStore',
        businessName: 'MediStore Pvt Ltd',
        licenseNumber: 'LIC12345',
        licenseExpiry: '2025-12-31',
        gstNumber: 'GSTIN123456',
        registeredMobile: '9876543210',
        officialEmail: 'official@medistore.com',
        storeMobile1: '9123456789',
        storeEmail1: 'store1@medistore.com',
        storeEmail2: 'store2@medistore.com',
        address1: '123 Main St',
        address2: 'Suite 5A',
        countryId: '1',
        stateId: '5',
        cityId: '10',
        firstName: 'John',
        lastName: 'Doe',
        personEmail: 'johndoe@gmail.com',
        personMobile: '9876543210',
        dateofBirth: '1990-01-01',
        gender: 'Male',
        picture: 'https://via.placeholder.com/100',
        bankName: 'HDFC',
        accountholderName: 'John Doe',
        accountNumber: '1234567890',
        ifscCode: 'HDFC0001234',
        branchName: 'MG Road',
        upiId: 'john@upi'
      },
      {
        id: 2,
        ownerName: 'Jane Smith',
        storeName: 'HealthPlus Pharmacy',
        businessName: 'HealthPlus Healthcare Ltd',
        licenseNumber: 'LIC67890',
        licenseExpiry: '2026-06-30',
        gstNumber: 'GSTIN654321',
        registeredMobile: '8765432109',
        officialEmail: 'official@healthplus.com',
        storeMobile1: '9234567890',
        storeEmail1: 'store1@healthplus.com',
        storeEmail2: 'store2@healthplus.com',
        address1: '456 Health Ave',
        address2: 'Floor 2, Unit B',
        countryId: '1',
        stateId: '3',
        cityId: '8',
        firstName: 'Jane',
        lastName: 'Smith',
        personEmail: 'janesmith@gmail.com',
        personMobile: '8765432109',
        dateofBirth: '1985-05-15',
        gender: 'Female',
        picture: 'https://via.placeholder.com/100',
        bankName: 'ICICI',
        accountholderName: 'Jane Smith',
        accountNumber: '0987654321',
        ifscCode: 'ICIC0005678',
        branchName: 'Park Street',
        upiId: 'jane@upi'
      }
  ]

  constructor(
    public router: Router, private actroute: ActivatedRoute, public fb: FormBuilder
) {
    super(router, fb);
}
  //constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.pharmacyId = Number(this.actroute.snapshot.paramMap.get('id'));
    this.pharmacyDetails = this.allPharmacies.find(x => x.id === this.pharmacyId);
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      $('#pharmacyTable').DataTable({
        paging: false,
        searching: false,
        info: false
      });
    }, 0);
  }
}
