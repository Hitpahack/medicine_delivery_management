import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder, FormsModule, ReactiveFormsModule, FormControl, FormGroup, Validators } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { AdminBaseComponent } from "../../admin.base.component";

@Component({
    selector: 'app-pharmacy-list',
    templateUrl: './admin.pharmacylist.component.html',
    styleUrls: ['./admin.pharmacylist.component.css'],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, FormsModule],
})
export class AdminPharmacyListComponent extends AdminBaseComponent implements OnInit {

    constructor(
        public router: Router, public fb: FormBuilder
    ) {
        super(router, fb);
    }
    searchTerm: string = '';

    pharmacies = [
        { id: 1, name: 'Apollo Pharmacy', city: 'Mumbai', contact: '9876543210', active: true },
        { id: 2, name: 'MedPlus', city: 'Delhi', contact: '8765432109', active: true },
        { id: 3, name: 'NetMeds', city: 'Bangalore', contact: '7654321098', active: true },
        { id: 4, name: '1mg Pharmacy', city: 'Chennai', contact: '6543210987', active: true },
        { id: 5, name: 'PharmEasy', city: 'Hyderabad', contact: '9123456780', active: true },
        { id: 6, name: 'Wellness Forever', city: 'Pune', contact: '9988776655', active: true },
        { id: 7, name: 'Guardian Pharmacy', city: 'Ahmedabad', contact: '8899776655', active: true },
        { id: 8, name: 'Zeno Health', city: 'Kolkata', contact: '8877665544', active: true }
      ];

      get filteredPharmacies() {
        if (!this.searchTerm) return this.pharmacies;
        return this.pharmacies.filter(pharmacy =>
            pharmacy.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
            pharmacy.city.toLowerCase().includes(this.searchTerm.toLowerCase())
        );
    }

    viewDetails(pharmacy: any) {
        console.log('Details:', pharmacy);
        // Navigate or open modal as needed
      }
      
      editPharmacy(pharmacy: any) {
        console.log('Edit:', pharmacy);
        // Navigate to edit form or open modal
      }
      
      deletePharmacy(pharmacy: any) {
        if (confirm(`Are you sure you want to delete ${pharmacy.name}?`)) {
          this.pharmacies = this.pharmacies.filter(p => p.id !== pharmacy.id);
        }
      }
      
      toggleStatus(pharmacy: any) {
        pharmacy.active = !pharmacy.active;
      }
            
}
