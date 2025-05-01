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

// doctor imports..
import { DoctorDashboardComponent } from "../app/doctor/views/doctordashboard/dashboard.component";
import { PurchaseInvoice } from './pharmacy/views/PurchaseInvoice/Invoice.component';


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
      { path: 'pharmacy/edit/:id', component: AdminAddPharmacyComponent },
      { path: 'pharmacy', component: AdminPharmacyListsComponent },

      { path: 'user/add', component: AddUserComponent },
      { path: 'user/edit/:id', component: AddUserComponent },
      { path: 'editprofile/:id', component: EditProfile },
      { path: 'changepassword', component: SetPassword },

      { path: 'user', component: UserListComponent },

      { path: 'productlist', component: ProductListsComponent },

      { path: 'role/add', component: AddRoleComponent },
      { path: 'role/list', component: RoleListComponent },
      { path: 'role/edit/:id', component: EditRoleComponent },
      
      { path: 'doctor', component: DoctorDashboardComponent },

      { path: 'pharmacy/user', component: PharmacDashboardComponent },
      { path: 'purchaseorder', component: PurchaseOrderComponent},
      {path: 'purchaseinvoice', component: PurchaseInvoice}

    ]
  },
  {
    path: 'doctor',
    component: AdminLayoutComponent,
    children: [
      { path: 'dashboard', component: DoctorDashboardComponent }
    ]
  },
  {
    path: 'pharmacy',
    component: AdminLayoutComponent,
    children: [
      { path: 'dashboard', component: PharmacDashboardComponent },
      { path: 'purchaseorder', component: PurchaseOrderComponent}
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


