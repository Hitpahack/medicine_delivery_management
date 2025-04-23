import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormBuilder, FormsModule, ReactiveFormsModule,} from "@angular/forms";
import { AdminBaseComponent } from "../../admin.base.component";
import { CommonModule } from "@angular/common";
import { ProductService } from 'src/app/admin/services/product/product.services';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin.dashboard.component.html',
  styleUrls: ['./admin.dashboard.component.css'],
  imports: [CommonModule,ReactiveFormsModule, FormsModule],
})
export class AdminDashboardComponent extends AdminBaseComponent implements OnInit {
    constructor(
        public router: Router, public fb:FormBuilder, public ProductService: ProductService
        
        ) {
        super(router,fb);
    }
    pharmacyCount: number = 0;
    productCount: number = 0;

  ngOnInit(): void {
    // This runs when the dashboard loads.
    console.log('Dashboard loaded!');
    this.ProductService.getcountrecord({}, 0).subscribe({
      next: (response) => {
        if (response?.isSuccess && response?.data) {
          this.pharmacyCount = response.data.pharmacyCount;
          this.productCount = response.data.productCount;
          console.log('Counts:', this.pharmacyCount, this.productCount);
        } else {
          console.error('API returned isSuccess: false', response);
        }
      },
      error: (err) => {
        console.error('HTTP Error:', err);
      }
    });
  }
}
