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
import { ReactiveFormsModule } from "@angular/forms";
import { FormControlErrorsDirective } from "../common/app.directives";

@NgModule({
    imports: [CommonModule, RouterModule,ReactiveFormsModule],
    declarations: [   
      AdminLayoutComponent,
      AdminEmptyLayoutComponent, 
      AdminShiftsComponent, 
      AdminLoginComponent,
      AdminFooterComponent,
      AdminHeaderComponent,
      AdminNavComponent ,
      FormControlErrorsDirective
    ],
    exports: [ AdminLayoutComponent   ]
  })
  export class AdminModule { 
    static forRoot(): ModuleWithProviders {
      return {
        ngModule: AdminModule,
        providers: []
      };
    }
  }