import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder, FormsModule, ReactiveFormsModule,} from "@angular/forms";
import { AdminBaseComponent } from "../../admin.base.component";
import { CommonModule } from "@angular/common";

@Component({
  selector: 'app-pharmacy-dashboard',
  templateUrl: './pharmacy.dashboard.component.html',
  styleUrls: ['./pharmacy.dashboard.component.css'],
  imports: [CommonModule,ReactiveFormsModule, FormsModule],
})
export class PharmacyDashboardComponent extends AdminBaseComponent implements OnInit {
    constructor(
        public router: Router, public fb:FormBuilder
        
        ) {
        super(router,fb);
    }

  ngOnInit(): void {
    // This runs when the dashboard loads.
    console.log('Dashboard loaded!');
  }
}
