import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder, FormsModule, ReactiveFormsModule,} from "@angular/forms";
import { AdminBaseComponent } from "../../admin.base.component";
import { CommonModule } from "@angular/common";

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './doctor.component.html',
  imports: [CommonModule,ReactiveFormsModule, FormsModule],
})
export class DoctorComponent extends AdminBaseComponent implements OnInit {
    constructor(
        public router: Router, public fb:FormBuilder
        
        ) {
        super(router,fb);
    }
    pharmacyCount: number = 0;
    productCount: number = 0;

  ngOnInit(): void {
    // This runs when the dashboard loads.
    console.log('Doctor Dashboard loaded!');
  }
}
