// other imports..
import { Routes, RouterModule } from '@angular/router';

// common imports..
import { AdminLoginComponent } from './admin/views/accounts/admin.login.component';
import { AdminShiftsComponent } from './admin/shifts/admin.shifts.component';
import { AdminEmptyLayoutComponent } from './_layouts/admin/admin.emptylayout.component';
import { AdminLayoutComponent } from './_layouts/admin/admin.layout.component';
import { LoginComponent } from './pages/login/login.component';
import { EditProfile } from '../app/admin/views/user/admin.editprofile.component';
import { SetPassword } from './admin/views/user/admin.setpassword.component';
import { SubAdminLayoutComponent } from './_layouts/admin/subadmin.layout.component';
import { NotFoundComponent } from './admin/views/pagenotfound/not-found.component';
import { ForgotPassword } from "../app/admin/views/user/admin.forgetPassword.component";
import { ResetPassword } from "../app/admin/views/user/admin.ResetPassword.component";
import { AuthGuardService } from './security/auth.guard';
import { NotAuthorizedComponent } from '../app/commonpage/views/not-authorized.component'

// admin imports..
import { AdminDashboardComponent } from './admin/views/admindashboard/admin.dashboard.component';
import { AdminAddPharmacyComponent } from './admin/views/pharmacy/add.component';
import { AdminPharmacyListsComponent } from './admin/views/pharmacy/list.component';
import { AddUserComponent } from '../app/admin/views/user/admin.adduser.component';
import { UserListComponent } from './admin/views/user/admin.userlist.component';
import { ProductListsComponent } from './admin/views/product/list.component';
import { AddRoleComponent } from './admin/views/rolemanage/addrole.component';
import { RoleListComponent } from './admin/views/rolemanage/rolelist.component';
import { EditRoleComponent } from './admin/views/rolemanage/roleedit.component';

// pharmacy imports..
import { PharmacDashboardComponent } from "../app/pharmacy/views/pharmacydashboard/dashboard.component";
import { PurchaseOrderComponent } from "../app/pharmacy/views/purchaseorder/purchaseorder.component";
import { AddStaffComponent } from "../app/pharmacy/views/staffmanagement/addstaff.component";
import { PurchaseInvoice } from '../app/pharmacy/views/PurchaseInvoice/Invoice.component' ;

// doctor imports..
import { DoctorDashboardComponent } from "../app/doctor/views/doctordashboard/dashboard.component";
import { DoctorList } from "../app/admin/views/doctor/list.component";  




export const routes: Routes = [

  //Admin routes goes here 

  { path: '', redirectTo: '/login', pathMatch: 'full' },
  {
    path: '',
    component: AdminEmptyLayoutComponent,
    children: [
      { path: '', component: AdminLoginComponent },
      { path: 'login', component: AdminLoginComponent },
      { path: 'not-authorized', component: NotAuthorizedComponent}
    ]
  },

  // This routing for Admin panel...
  {
    path: 'admin',
    component: AdminLayoutComponent,
    children: [
      { path: 'shifts', component: AdminShiftsComponent },
      { path: 'dashboard', component: AdminDashboardComponent, canActivate: [AuthGuardService], data: { module: 'dashboard' } },
      { path: 'pharmacy/add', component: AdminAddPharmacyComponent, canActivate: [AuthGuardService], data: { module: 'pharmacy' } },
      { path: 'pharmacy/edit/:id', component: AdminAddPharmacyComponent, canActivate: [AuthGuardService], data: { module: 'pharmacy' } },
      { path: 'pharmacy', component: AdminPharmacyListsComponent, canActivate: [AuthGuardService], data: { module: 'pharmacy' } },

      { path: 'user/add', component: AddUserComponent, canActivate: [AuthGuardService] },
      { path: 'user/edit/:id', component: AddUserComponent, canActivate: [AuthGuardService] },
      { path: 'user', component: UserListComponent, canActivate: [AuthGuardService] },

      { path: 'role/add', component: AddRoleComponent, canActivate: [AuthGuardService] },
      { path: 'role/list', component: RoleListComponent, canActivate: [AuthGuardService] },
      { path: 'role/edit/:id', component: EditRoleComponent, canActivate: [AuthGuardService] },

      { path: 'productlist', component: ProductListsComponent, canActivate: [AuthGuardService] },

      { path: 'editprofile/:id', component: EditProfile, canActivate: [AuthGuardService] },
      { path: 'changepassword', component: SetPassword, canActivate: [AuthGuardService] },
    ]
  },

  // This routing for doctor panel...
  {
    path: 'doctor',
    component: AdminLayoutComponent,
    children: [
      { path: 'dashboard', component: DoctorDashboardComponent, canActivate: [AuthGuardService] },
      { path: 'list', component: DoctorList, canActivate: [AuthGuardService] },
    ]
  },

  // This routing for Pharmacy panel...
  {
    path: 'pharmacy',
    component: AdminLayoutComponent,
    children: [
      { path: 'dashboard', component: PharmacDashboardComponent, canActivate: [AuthGuardService] },
      { path: 'purchaseorder', component: PurchaseOrderComponent, canActivate: [AuthGuardService]},
      { path: 'purchaseinvoice', component: PurchaseInvoice, canActivate: [AuthGuardService]},
      { path: 'staff/add', component: AddStaffComponent, canActivate: [AuthGuardService]},
      { path: 'staff/edit/:id', component: AddStaffComponent, canActivate: [AuthGuardService]}
    ]
  },
  {
    path: 'admin',
    component: SubAdminLayoutComponent,
    children: [
      //{ path: 'subadmin', component: Sub_Dr_AdminDashboardComponent }
    ]
  },
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
  { path: 'set-password', component: SetPassword },
  // otherwise redirect to home
  { path: '**', component: NotFoundComponent }
];

//export const routing = RouterModule.forRoot(appRoutes);


