import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder, FormsModule, ReactiveFormsModule, FormControl, FormGroup, Validators } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { AdminBaseComponent } from "../../admin.base.component";
import * as $ from 'jquery';
import 'datatables.net';

@Component({
    selector: 'app-pharmacy-list',
    templateUrl: './admin.pharmacylist.component.html',
    styleUrls: [''],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, FormsModule],
})
export class AdminPharmacyListComponent extends AdminBaseComponent implements OnInit {

    constructor(
        public router: Router, public fb: FormBuilder
    ) {
        super(router, fb);
    }

    pharmacyList = [
      { id : 1,
        ownerName: 'Dr. Rajesh Kumar',
        storeName: 'HealthPlus Pharmacy',
        businessName: 'HealthPlus Pharmaceuticals Pvt. Ltd.',
        licenseNumber: 'LIC123456789',
        licenseExpiry: new Date('2026-12-31'),
        gstNumber: '27ABCDE1234F1Z5'
      },
      {
        id : 2,
        ownerName: 'Ms. Anita Sharma',
        storeName: 'WellCare Pharmacy',
        businessName: 'WellCare Enterprises',
        licenseNumber: 'LIC987654321',
        licenseExpiry: new Date('2025-06-15'),
        gstNumber: '27WXYZB4321F1Z8'
      },
      {
        id : 3,
        ownerName: 'Mr. Harish Mehta',
        storeName: 'MediCare Plus',
        businessName: 'MediCare Solutions',
        licenseNumber: 'LIC456789321',
        licenseExpiry: new Date('2027-03-01'),
        gstNumber: '27LMNOP1234F1Z2'
      }
    ];
    
  
    ngOnInit(): void {
      // Wait a bit to ensure HTML table is rendered
      setTimeout(() => {
        $('#example').DataTable();
      }, 100);
    }

    viewPharmacy(id: number) {
      this.router.navigate(['admin/pharmacydetails', id]);
      // Show modal or redirect
    }
    
    editPharmacy(pharmacy: any) {
      console.log('Edit', pharmacy);
      // Navigate to edit page or open popup
    }
    
    toggleStatus(pharmacy: any) {
      pharmacy.isActive = !pharmacy.isActive;
      console.log('Toggled', pharmacy.isActive);
      // Optionally call API to update status
    }
    
    deletePharmacy(pharmacy: any) {
      if (confirm(`Are you sure to delete ${pharmacy.storeName}?`)) {
        this.pharmacyList = this.pharmacyList.filter(p => p !== pharmacy);
        console.log('Deleted:', pharmacy);
        // Optionally call API to delete
      }
    }
            
}
