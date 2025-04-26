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
import { AddRoleComponent } from './admin/views/rolemanage/addrole.component';
import { RoleListComponent } from './admin/views/rolemanage/rolelist.component';
import { EditRoleComponent } from './admin/views/rolemanage/roleedit.component';
import { DoctorComponent } from './admin/views/doctor/doctor.component';
import { PharmacyUserComponent } from './admin/views/pharmacyuser/pharmacyuser.component';
import { AuthGuard } from "../app/security/authenticate.guard";
import { ForgotPassword } from "../app/admin/views/user/admin.forgetPassword.component";
import { ResetPassword } from "../app/admin/views/user/admin.ResetPassword.component";


export const routes: Routes = [

  //Admin routes goes here 

  { path: '', redirectTo: '/admin/login', pathMatch: 'full' },
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
      { path: 'changepassword', component: SetPassword },

      { path: 'user', component: UserListComponent },

      { path: 'productlist', component: ProductListsComponent },

      { path: 'role/add', component: AddRoleComponent },
      { path: 'role/list', component: RoleListComponent },
      { path: 'role/edit/:id', component: EditRoleComponent },
      
      { path: 'doctor', component: DoctorComponent },

      { path: 'pharmacy/user', component: PharmacyUserComponent },
    ]
  },
  {
    path: 'doctor',
    component: AdminLayoutComponent,
    children: [
      { path: 'dashboard', component: DoctorComponent }
    ]
  },
  {
    path: 'pharmacy',
    component: AdminLayoutComponent,
    children: [
      { path: 'dashboard', component: PharmacyUserComponent }
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
  { path: 'setpassword', component: ChangePassword },
  { path: 'reset-password', component: ResetPassword },
  { path: 'forgotpassword', component: ForgotPassword },
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


