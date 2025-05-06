import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DatatableComponent } from './datatables/datatable.component';
import { AdminLayoutComponent } from 'src/app/_layouts/admin/admin.layout.component';
import { AdminFooterComponent } from './footer/admin.footer.component';
import { AdminHeaderComponent } from './header/admin.header.component';
import { AdminNavComponent } from './nav/admin.nav.component';
import { GoogleMapsModule } from '@angular/google-maps';

@NgModule({
  declarations: [],
  imports: [CommonModule,DatatableComponent,AdminLayoutComponent, AdminFooterComponent, AdminHeaderComponent,AdminNavComponent, GoogleMapsModule],
  exports: [DatatableComponent,AdminLayoutComponent, AdminFooterComponent, AdminHeaderComponent,AdminNavComponent, GoogleMapsModule]
})
export class SharedModule { }
