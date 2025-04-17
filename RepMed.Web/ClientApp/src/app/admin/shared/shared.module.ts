import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminFooterComponent } from './footer/admin.footer.component';
import { AdminHeaderComponent } from './header/admin.header.component';
import { AdminNavComponent } from './nav/admin.nav.component';


@NgModule({
  declarations: [],
  imports: [CommonModule,AdminFooterComponent,AdminHeaderComponent,AdminNavComponent],
  exports: [AdminFooterComponent,AdminHeaderComponent,AdminNavComponent]
})
export class SharedModule {}