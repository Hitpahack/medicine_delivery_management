import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DtTableComponent } from './datatables/dt-table.component';
import { AdminFooterComponent } from './footer/admin.footer.component';
import { AdminHeaderComponent } from './header/admin.header.component';
import { AdminNavComponent } from './nav/admin.nav.component';


@NgModule({
  declarations: [DtTableComponent],
  imports: [CommonModule,AdminFooterComponent,AdminHeaderComponent,AdminNavComponent],
  exports: [DtTableComponent, AdminFooterComponent,AdminHeaderComponent,AdminNavComponent] // 👈 important
})
export class SharedModule {}