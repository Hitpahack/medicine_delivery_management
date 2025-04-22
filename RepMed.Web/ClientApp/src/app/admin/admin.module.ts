import { CommonModule } from "@angular/common";
import { ModuleWithProviders, NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { AdminEmptyLayoutComponent } from "../_layouts/admin/admin.emptylayout.component";
import { AdminLayoutComponent } from "../_layouts/admin/admin.layout.component";
import { AdminLoginComponent } from "./views/accounts/admin.login.component";
import { AdminFooterComponent } from "./shared/footer/admin.footer.component";
import { AdminHeaderComponent } from "./shared/header/admin.header.component";
import { AdminNavComponent } from "./shared/nav/admin.nav.component";
import { AdminShiftsComponent } from "./shifts/admin.shifts.component";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormControlErrorsDirective } from "../common/app.directives";
import { AdminDashboardComponent } from "./views/admindashboard/admin.dashboard.component";
import { AdminAddPharmacyComponent } from "./views/pharmacy/add.component";
import { PharmacyDashboardComponent } from "./views/pharmacy/dashboard.component";
import { PharmacyLayoutComponent } from "../_layouts/admin/pharmacy.layout.component";
import { AdminPharmacyListsComponent } from "./views/pharmacy/list.component";
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from "../app.component";
import { AutoValidateDirective } from "../common/form.validator";
import { DatatableComponent } from "./shared/datatables/datatable.component";
import { SharedModule } from "./shared/shared.module";
import { ProductListsComponent } from "./views/product/list.component";


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    AdminLayoutComponent,
    AdminEmptyLayoutComponent,
    AdminShiftsComponent,
    //AdminLoginComponent,
    //AdminFooterComponent,
    //AdminHeaderComponent,
    //AdminNavComponent,
    FormControlErrorsDirective,
    AdminDashboardComponent,
    AdminAddPharmacyComponent,
    PharmacyDashboardComponent,
    PharmacyLayoutComponent,
    AdminPharmacyListsComponent,
    BrowserModule,
    AppComponent,
    AppComponent,
    AutoValidateDirective,
    SharedModule,
    ProductListsComponent
  ],
  exports: []

})
export class AdminModule {
  static forRoot(): ModuleWithProviders<AdminModule> {
    return {
      ngModule: AdminModule,
      providers: []
    };
  }
}