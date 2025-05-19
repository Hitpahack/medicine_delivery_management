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
import { CmsComponent } from './admin/views/CMSmanagement/cms.component';
import { PageListComponent } from './admin/views/CMSmanagement/cmslist.component';
import { FAQComponent } from './admin/views/CMSmanagement/faq.component';
import { FAQListComponent } from './admin/views/CMSmanagement/faqlist.component';

// pharmacy imports..
import { PharmacDashboardComponent } from "../app/pharmacy/views/pharmacydashboard/dashboard.component";
import { PoGenerateComponent } from "../app/pharmacy/views/purchaseorder/pogenerate.component";
import { AddStaffComponent } from "../app/pharmacy/views/staffmanagement/addstaff.component";
import { PurchaseInvoice } from '../app/pharmacy/views/PurchaseInvoice/Invoice.component';
import { EditPharmacyComponent } from "../app/pharmacy/views/Details/editpharmacydetails";
import { PharmacyAddRoleComponent } from './pharmacy/views/rolemanage/addrole.component';
import { PharmacyEditRoleComponent } from './pharmacy/views/rolemanage/roleedit.component';
import { PharmacyRoleListComponent } from './pharmacy/views/rolemanage/rolelist.component';
import { StaffListComponent } from './pharmacy/views/staffmanagement/stafflist.component';

// doctor imports..
import { DoctorDashboardComponent } from "../app/doctor/views/doctordashboard/dashboard.component";
import { DoctorList } from "../app/admin/views/doctor/list.component";
import { PharmacyLayoutComponent } from './_layouts/admin/pharmacy.layout.component';


// Other imports...
import { DashboardComponent } from "../app/othermodule/dashboard/dashboard.component";
import { AddDoctorComponent } from './admin/views/doctor/adddoctor.component';




export const routes: Routes = [

  //Admin routes goes here 

  { path: '', redirectTo: '/login', pathMatch: 'full' },
  {
    path: '',
    component: AdminEmptyLayoutComponent,
    children: [
      { path: '', component: AdminLoginComponent },
      { path: 'login', component: AdminLoginComponent },
      { path: 'not-authorized', component: NotAuthorizedComponent },
      { path: 'changepassword', component: SetPassword, canActivate: [AuthGuardService] },
    ]
  },

  // This routing for Admin panel...
  {
    path: 'admin',
    component: AdminLayoutComponent,
    children: [
      { path: 'shifts', component: AdminShiftsComponent },
      { path: 'admindashboard', component: AdminDashboardComponent, canActivate: [AuthGuardService], data: { module: 'admindashboard' } },
      { path: 'pharmacy/add', component: AdminAddPharmacyComponent, canActivate: [AuthGuardService], data: { module: 'pharmacies' } },
      { path: 'pharmacy/edit/:id', component: AdminAddPharmacyComponent, canActivate: [AuthGuardService], data: { module: 'pharmacies' } },
      { path: 'pharmacy', component: AdminPharmacyListsComponent, canActivate: [AuthGuardService], data: { module: 'pharmacies' } },

      { path: 'user/add', component: AddUserComponent, canActivate: [AuthGuardService], data: { module: 'user' } },
      { path: 'user/edit/:id', component: AddUserComponent, canActivate: [AuthGuardService], data: { module: 'user' } },
      { path: 'user', component: UserListComponent, canActivate: [AuthGuardService], data: { module: 'user' } },

      { path: 'role/add', component: AddRoleComponent, canActivate: [AuthGuardService], data: { module: 'rolemanagement' } },
      { path: 'role/list', component: RoleListComponent, canActivate: [AuthGuardService], data: { module: 'rolemanagement' } },
      { path: 'role/edit/:id', component: EditRoleComponent, canActivate: [AuthGuardService], data: { module: 'rolemanagement' } },

      { path: 'productlist', component: ProductListsComponent, canActivate: [AuthGuardService], data: { module: 'productlist' } },

      { path: 'editprofile/:id', component: EditProfile, canActivate: [AuthGuardService], data: { module: 'admindashboard' } },

      { path: 'doctor/add', component: AddDoctorComponent, canActivate: [AuthGuardService], data: { module: 'doctormanagement' } },
      { path: 'doctor/list', component: DoctorList, canActivate: [AuthGuardService], data: { module: 'doctormanagement' } },
      { path: 'doctor/edit/:id', component: AddDoctorComponent, canActivate: [AuthGuardService], data: { module: 'doctormanagement' } },

      { path: 'cms/add', component: CmsComponent, canActivate: [AuthGuardService], data: { module: 'cmsManagement' } },
      { path: 'cms/edit/:id', component: CmsComponent, canActivate: [AuthGuardService], data: { module: 'cmsManagement' } },
      { path: 'cms/list', component: PageListComponent, canActivate: [AuthGuardService], data: { module: 'cmsManagement' } },

      { path: 'faq/add', component: FAQComponent, canActivate: [AuthGuardService], data: { module: 'cmsManagement' } },
      { path: 'faq/edit/:id', component: FAQComponent, canActivate: [AuthGuardService], data: { module: 'cmsManagement' } },
      { path: 'faq/list', component: FAQListComponent, canActivate: [AuthGuardService], data: { module: 'cmsManagement' }}
    ]
  },

  // This routing for doctor panel...
  {
    path: 'doctor',
    component: AdminLayoutComponent,
    children: [
      { path: 'doctordashboard', component: DoctorDashboardComponent, canActivate: [AuthGuardService], data: { module: 'doctordashboard' } },
      { path: 'list', component: DoctorList, canActivate: [AuthGuardService], data: { module: 'doctormanagement' } },
      { path: 'doctor/add', component: AddDoctorComponent, canActivate: [AuthGuardService], data: { module: 'doctormanagement' } },
      { path: 'editprofile/:id', component: EditProfile, canActivate: [AuthGuardService], data: { module: 'doctordashboard' } },
    ]
  },

  // This routing for Pharmacy panel...
  {
    path: 'pharmacy',
    component: PharmacyLayoutComponent,
    children: [
      { path: 'pharmacydashboard', component: PharmacDashboardComponent, canActivate: [AuthGuardService], data: { module: 'pharmacydashboard' } },
      { path: 'purchaseorder', component: PoGenerateComponent, canActivate: [AuthGuardService], data: { module: 'purchasemanagement' } },
      { path: 'purchaseinvoice', component: PurchaseInvoice, canActivate: [AuthGuardService], data: { module: 'purchasemanagement' } },

      { path: 'staff/add', component: AddStaffComponent, canActivate: [AuthGuardService], data: { module: 'pharmacystaff' } },
      { path: 'staff/edit/:id', component: AddStaffComponent, canActivate: [AuthGuardService], data: { module: 'pharmacystaff' } },
      { path: 'staff/list', component: StaffListComponent, canActivate: [AuthGuardService], data: { module: 'pharmacystaff' } },

      { path: 'editprofile/:id', component: EditProfile, canActivate: [AuthGuardService], data: { module: 'pharmacydashboard' } },
      { path: 'productlist', component: ProductListsComponent, canActivate: [AuthGuardService], data: { module: 'productlist' } },

      { path: 'role/add', component: PharmacyAddRoleComponent, canActivate: [AuthGuardService], data: { module: 'pharmacyrole' } },
      { path: 'role/list', component: PharmacyRoleListComponent, canActivate: [AuthGuardService], data: { module: 'pharmacyrole' } },
      { path: 'role/edit/:id', component: PharmacyEditRoleComponent, canActivate: [AuthGuardService], data: { module: 'pharmacyrole' } },
      { path: 'editpharmacy/:id', component: EditPharmacyComponent, canActivate: [AuthGuardService], data: { module: 'pharmacydashboard' } },
    ]
  },
  {
    path: 'common',
    component: AdminLayoutComponent,
    children: [
      { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuardService], data: { module: 'dashboard' } },
      { path: 'pharmacy/add', component: AdminAddPharmacyComponent, canActivate: [AuthGuardService], data: { module: 'pharmacies' } },
      { path: 'pharmacy/edit/:id', component: AdminAddPharmacyComponent, canActivate: [AuthGuardService], data: { module: 'pharmacies' } },
      { path: 'pharmacy', component: AdminPharmacyListsComponent, canActivate: [AuthGuardService], data: { module: 'pharmacies' } },

      { path: 'user/add', component: AddUserComponent, canActivate: [AuthGuardService], data: { module: 'user' } },
      { path: 'user/edit/:id', component: AddUserComponent, canActivate: [AuthGuardService], data: { module: 'dashboard' } },
      { path: 'user', component: UserListComponent, canActivate: [AuthGuardService], data: { module: 'user' } },

      { path: 'role/add', component: AddRoleComponent, canActivate: [AuthGuardService], data: { module: 'rolemanagement' } },
      { path: 'role/list', component: RoleListComponent, canActivate: [AuthGuardService], data: { module: 'rolemanagement' } },
      { path: 'role/edit/:id', component: EditRoleComponent, canActivate: [AuthGuardService], data: { module: 'rolemanagement' } },

      { path: 'productlists', component: ProductListsComponent, canActivate: [AuthGuardService], data: { module: 'productlists' } },

      { path: 'editprofile/:id', component: EditProfile, canActivate: [AuthGuardService], data: { module: 'dashboard' } },

      { path: 'doctor/list', component: DoctorList, canActivate: [AuthGuardService], data: { module: 'doctormanagement' } },
      { path: 'doctor/add', component: AddDoctorComponent, canActivate: [AuthGuardService], data: { module: 'doctormanagement' } },

      { path: 'purchaseorder', component: PoGenerateComponent, canActivate: [AuthGuardService], data: { module: 'purchasemanagement' } },
      { path: 'purchaseinvoice', component: PurchaseInvoice, canActivate: [AuthGuardService], data: { module: 'purchasemanagement' } },

      { path: 'staff/add', component: AddStaffComponent, canActivate: [AuthGuardService], data: { module: 'pharmacystaff' } },
      { path: 'staff/edit/:id', component: AddStaffComponent, canActivate: [AuthGuardService], data: { module: 'pharmacystaff' } },
      { path: 'editprofile/:id', component: EditProfile, canActivate: [AuthGuardService], data: { module: 'pharmacydashboard' } },

    ]
  },
  // {
  //   path: 'admin',
  //   component: SubAdminLayoutComponent,
  //   children: [
  //     //{ path: 'subadmin', component: Sub_Dr_AdminDashboardComponent }
  //   ]
  // },
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


