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
import { AdminAddPharmacyComponent } from './admin/views/pharmacy/add.component';
import { PharmacyLayoutComponent } from './_layouts/admin/pharmacy.layout.component';
import { PharmacyDashboardComponent } from './admin/views/pharmacy/dashboard.component';
import { AdminPharmacyListsComponent } from './admin/views/pharmacy/list.component';

import { AddUserComponent } from '../app/admin/views/user/admin.adduser.component';
import { EditProfile } from '../app/admin/views/user/admin.editprofile.component';
import { SetPassword } from './admin/views/user/admin.setpassword.component';
import { ChangePassword } from './admin/views/user/admin.changepassword.component';
import { UserListComponent } from './admin/views/user/admin.userlist.component';

import { SubAdminLayoutComponent } from './_layouts/admin/subadmin.layout.component';
import { AdminPharmacyDetailsComponent } from './admin/views/pharmacy/details.component';
import { ProductListsComponent } from './admin/views/product/list.component';
import { NotFoundComponent } from './admin/views/pagenotfound/not-found.component';



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
      { path: 'pharmacy/add', component: AdminAddPharmacyComponent },
      { path: 'pharmacy', component: AdminPharmacyListsComponent },
      { path: 'pharmacy/view', component: AdminPharmacyDetailsComponent },
      
      { path: 'user/add', component: AddUserComponent },
      { path: 'user/edit/:id', component: AddUserComponent },
      { path: 'editprofile/:id', component: EditProfile },
      { path: 'changepassword', component:SetPassword},
      { path: 'setpassword', component:ChangePassword},
      {path: 'user', component:UserListComponent},

      {path:  'productlist', component:ProductListsComponent}
    ]
  },
  {
    path: 'admin',
    component: PharmacyLayoutComponent,
    children: [
      { path: 'pharmacy/dashboard', component: PharmacyDashboardComponent }
    ]
  },
  {
    path: 'admin',
    component: SubAdminLayoutComponent,
    children: [
      //{ path: 'subadmin', component: Sub_Dr_AdminDashboardComponent }
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
  { path: '**', component: NotFoundComponent }
];

//export const routing = RouterModule.forRoot(appRoutes);


