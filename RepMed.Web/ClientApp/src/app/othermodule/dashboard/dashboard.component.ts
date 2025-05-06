import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder, FormsModule, ReactiveFormsModule, } from "@angular/forms";
import { AdminBaseComponent } from "../../../app/admin/admin.base.component";
import { CommonModule } from "@angular/common";

@Component({
    selector: 'app-common-dashboard',
    templateUrl: './dashboard.component.html',
    imports: [CommonModule, ReactiveFormsModule, FormsModule],
})
export class DashboardComponent extends AdminBaseComponent implements OnInit {
    constructor(
        public router: Router, public fb: FormBuilder

    ) {
        super(router, fb);
    }
    pharmacyCount: number = 0;
    productCount: number = 0;

    ngOnInit(): void {
        // This runs when the dashboard loads.
        console.log('Common Dashboard loaded!');
    }
    summaryCards = [
        { title: 'Users', value: 100 },
        { title: 'Orders', value: 250 },
        { title: 'Revenue', value: '$5,000' },
        { title: 'Tasks', value: 12 }
    ];

    recentActivities = [
        'User Alice signed up',
        'Order #001 completed',
        'Invoice #123 paid',
        'Task "Send report" finished'
    ];
}
