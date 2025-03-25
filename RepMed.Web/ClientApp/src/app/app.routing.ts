import { Routes, RouterModule } from '@angular/router';
import { AdminLoginComponent } from './admin/views/accounts/admin.login.component';
import { AdminShiftsComponent } from './admin/shifts/admin.shifts.component';
import { LoginComponent } from './web/accounts/login.component';
import { FacilitesComponent } from './web/facilites/facilites.component';
import { AdminEmptyLayoutComponent } from './_layouts/admin/admin.emptylayout.component';
import { AdminLayoutComponent } from './_layouts/admin/admin.layout.component';
import { WebLayoutComponent } from './_layouts/web/web.layout.component';




const appRoutes: Routes = [
    
    //Admin routes goes here 
    { path: 'admin', 
      component: AdminEmptyLayoutComponent, 
      children: [
        { path: '', component: AdminLoginComponent },
        { path: 'login', component: AdminLoginComponent },
      ]
    },
    { 
        path: 'admin', 
        component: AdminLayoutComponent,
        children: [
          { path: 'shifts', component: AdminShiftsComponent },
        ]
    },
    

    //Web routes goes here
    { path: '', component: LoginComponent, pathMatch: 'full'},
    { 
      path: '', 
      component: WebLayoutComponent,
      children: [
        { path: 'facilites', component: FacilitesComponent }
      ]
  },
  { path: 'login', component: LoginComponent},
  
  
  // otherwise redirect to home
  { path: '**', redirectTo: '' }
];

export const routing = RouterModule.forRoot(appRoutes);


