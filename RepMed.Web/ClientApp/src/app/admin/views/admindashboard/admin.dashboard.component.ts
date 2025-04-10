import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder, FormsModule, ReactiveFormsModule,} from "@angular/forms";
import { AdminBaseComponent } from "../../admin.base.component";
import { CommonModule } from "@angular/common";

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin.dashboard.component.html',
  styleUrls: ['./admin.dashboard.component.css'],
  imports: [CommonModule,ReactiveFormsModule, FormsModule],
})
export class AdminDashboardComponent extends AdminBaseComponent implements OnInit {
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
