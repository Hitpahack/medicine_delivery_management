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
import { AdminAddPharmacyComponent } from "./views/pharmacy/admin.addpharmacy.component";
import { PharmacyDashboardComponent } from "./views/pharmacydashboard/pharmacy.dashboard.component";
import { PharmacyLayoutComponent } from "../_layouts/admin/pharmacy.layout.component";
import { AdminPharmacyListsComponent } from "./views/pharmacy/admin.pharmacylists.component";
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from "../app.component";
import { EditPharmacy } from "./views/pharmacy/admin.pharmacyedit.component";
import { DtTableComponent } from "./shared/datatables/dt-table.component";


@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    AdminLayoutComponent,
    AdminEmptyLayoutComponent,
    AdminShiftsComponent,
    AdminLoginComponent,
    AdminFooterComponent,
    AdminHeaderComponent,
    AdminNavComponent,
    FormControlErrorsDirective,
    AdminDashboardComponent,
    AdminAddPharmacyComponent,
    PharmacyDashboardComponent,
    PharmacyLayoutComponent,
    AdminPharmacyListsComponent,
    BrowserModule,
    AppComponent,
    DtTableComponent
    DataTablesModule,
    AppComponent,
    EditPharmacy
  ],
  exports: [AdminLayoutComponent,DtTableComponent]

})
export class AdminModule {
  static forRoot(): ModuleWithProviders<AdminModule> {
    return {
      ngModule: AdminModule,
      providers: []
    };
  }
}