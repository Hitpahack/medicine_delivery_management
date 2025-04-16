import { Routes, RouterModule } from '@angular/router';
import { AdminLoginComponent } from './admin/views/accounts/admin.login.component';
import { AdminShiftsComponent } from './admin/shifts/admin.shifts.component';
//import { LoginComponent } from './web/accounts/login.component';
import { FacilitesComponent } from './web/facilites/facilites.component';
import { AdminEmptyLayoutComponent } from './_layouts/admin/admin.emptylayout.component';
import { AdminLayoutComponent } from './_layouts/admin/admin.layout.component';
import { WebLayoutComponent } from './_layouts/web/web.layout.component';
import { LoginComponent } from './pages/login/login.component';
import { AdminDashboardComponent } from './admin/views/admindashboard/admin.dashboard.component';
import { AdminAddPharmacyComponent } from './admin/views/pharmacy/admin.addpharmacy.component';
import { PharmacyLayoutComponent } from './_layouts/admin/pharmacy.layout.component';
import { PharmacyDashboardComponent } from './admin/views/pharmacydashboard/pharmacy.dashboard.component';
import { AdminPharmacyListsComponent } from './admin/views/pharmacy/admin.pharmacylists.component';
import { AdminPharmacyDetailsComponent} from './admin/views/pharmacy/admin.pharmacydetails.component';

import { AddUserComponent } from '../app/admin/views/user/admin.adduser.component';
import { EditProfile } from '../app/admin/views/user/admin.editprofile.component';
import { SetPassword } from './admin/views/user/admin.setpassword.component';
import { EditPharmacy } from './admin/views/pharmacy/admin.pharmacyedit.component';
import { ChangePassword } from './admin/views/user/admin.changepassword.component';



export const routes: Routes = [

  //Admin routes goes here 
  {
    path: 'admin',
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
      { path: 'dashboard', component: AdminDashboardComponent },
      { path: 'addpharmacy', component: AdminAddPharmacyComponent },
      { path: 'pharmacylist', component: AdminPharmacyListsComponent },
      { path: 'pharmacydetails/:id', component: AdminPharmacyDetailsComponent },
      { path: 'pharmacyedit/:id', component: EditPharmacy},
      
      { path: 'adduser', component: AddUserComponent },
      { path: 'editprofile/:id', component: EditProfile },

      { path: 'setpassword', component:SetPassword}
    ]
  },
  {
    path: 'admin',
    component: PharmacyLayoutComponent,
    children: [
      { path: 'pharmacy', component: PharmacyDashboardComponent }
    ]
  },

  //Web routes goes here
  //{ path: '', component: LoginComponent, pathMatch: 'full'},
  //{ 
  //  path: '', 
  //  component: WebLayoutComponent,
  //  children: [
  //    { path: 'facilites', component: FacilitesComponent }
  //  ]
  //},
  { path: 'login', component: LoginComponent },


  // otherwise redirect to home
  { path: '**', redirectTo: '' }
];

//export const routing = RouterModule.forRoot(appRoutes);


