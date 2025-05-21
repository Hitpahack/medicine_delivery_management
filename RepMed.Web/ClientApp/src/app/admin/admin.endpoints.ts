import { get } from 'jquery';
import { environment } from '../../../src/environments/environment';
import { ConfigService } from '../../config.service';
import { Injectable } from '@angular/core';


@Injectable({ providedIn: 'root' })
export class AdminApiConfigService {
  constructor(private config: ConfigService) {}

  get requestSettings() {
    const token = localStorage.getItem('token');
    return {
      header: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Accept-Language': 'en',
        'Authorization': `Bearer ${token}`
      },
      headerFormData: {
        //'Content-Disposition': 'multipart/form-data',
        'Accept': 'application/json',
        'Accept-Language': 'en',
        'Authorization': `Bearer ${token}`
      }
    };
  }

  get endpoints() {
    const v1 = this.config.admin_apiv1;
    const api = this.config.admin_api;

    return {
      accounts: {
        login: `${v1}/accounts/login`,
        forgetpassword:`${v1}/accounts/forgotpassword`,
        resetpassword:`${v1}/accounts/resetpassword`,
        setpassword: `${v1}/accounts/set-password`,
        logout: `${v1}/accounts/logout`,
      },
      pharmacy: {
        add: `${v1}/pharmacy/addpharmacy/`,
        update: (id: number) => `${v1}/pharmacy/editpharmacy/${id}`,
        getPaged: `${v1}/pharmacy/getpharmacies`,
        get: (id: number) => `${v1}/pharmacy/getpharmacy/${id}`,
        getcountries: `${api}/masters/getcountries/`,
        getstates: (id: number) => `${api}/masters/getstates/${id}`,
        getcities: (id: number) => `${api}/masters/getcities/${id}`,
        updateStatus: `${v1}/pharmacy/changestatus`,
      },
      doctor:{
        list:`${v1}/doctor/getdoctors`
      },
      user: {
        add: `${v1}/users/adduser/`,
        get: (id: number) => `${v1}/users/get/${id}`,
        update: (id: number) => `${v1}/users/edituser/${id}`,
        userroles: `${api}/masters/getroles`,
        list: `${v1}/users/getusers`,
        updateStatus: `${v1}/users/lockstatus`,
      },
      staff: {
        add: `${v1}/users/adduser/`,
        get: (id: number) => `${v1}/users/get/${id}`,
        update: (id: number) => `${v1}/users/edituser/${id}`,
        userroles: (id: number) => `${api}/masters/getpharmacyroles/${id}`,
        list: `${v1}/users/getpharmacystaff`,
      },
      product: {
        get: `${v1}/products/getproducts`,
        getcount: `${v1}/products/getcount`,
      }, 
      role: {
        add: `${v1}/roles/addrole`,
        getmodule: `${v1}/roles/getpermissions/`,
        list: `${v1}/roles/getroles/`,
        getmodulebyid: (id: number) => `${v1}/roles/getrole/${id}`,
        editrole: (id: number) => `${v1}/roles/editrole/${id}`,
        updateStatus: `${v1}/roles/changestatus`,
        deleterole: (id: number) => `${v1}/roles/deleterole/${id}`,
      },
      pharmacyrole: {
        add: `${v1}/roles/addpharmacyrole`,
        getmodule: `${v1}/roles/getpharmactpermissions/`,
        list: `${v1}/roles/getpharmacyroles/`,
        editrole: (id: number) => `${v1}/roles/editpharmacyrole/${id}`,

        getmodulebyid: (id: number) => `${v1}/roles/getrole/${id}`,
        updateStatus: `${v1}/roles/changestatus`,
        deleterole: (id: number) => `${v1}/roles/deleterole/${id}`,
      },
      purchaseorder: {
        getsuppliers: (id: number) => `${v1}/po/getsuppliers/${id}`,
        getPoNumber: (id: number) => `${v1}/po/getponumber/${id}`,
        addSupplier: `${v1}/po/addsupplier`,
        getproduct: `${api}/masters/getproducts`,
        add: `${v1}/po/createpo/`,
        searchProducts: `${v1}/po/search_products`,
        searchsupplier: `${v1}/po/getsuppliers`,
        addProduct: `${v1}/po/add_item/`,
        shortbookitemlist: `${v1}/po/get_items`,
        updateItem: `${v1}/po/edit_item`,
        deleteitem: (id: number) => `${v1}/po/delete_item/${id}`,
        generatepo: `${v1}/po/createpo/`,

        getItemOrderWise: `${v1}/po/get_po_orderwise`,
        getItemWise: `${v1}/po/get_po_itemwise`,
        getItemDistributorWise: `${v1}/po/get_po_distwise`,
        deletePO: (id: number) => `${v1}/po/delete_po/${id}`,
        SendMailToDistributor: (id: number) => `${v1}/po/  /${id}`,

        getItemBaseOnPONumber: `${v1}/po/get_po_owitems`,
        getPOListBaseOnItemID: `${v1}/po/get_po_iwitems`,
        deletePOItem: (id: number) => `${v1}/po/delete_po_item/${id}`,
        updateitem: (id: number) => `${v1}/po/edit_po_item/${id}`
      },
      CMS: {
        add: `${v1}/cms/addstaticpage`,
        getbyid: (id: number) => `${v1}/cms/getstaticpage/${id}`,
        edit: (id: number) => `${v1}/cms/editstaticpage/${id}`,
        list: `${v1}/cms/getstaticpages/`,
        updateStatus: `${v1}/cms/changestatus`,
        delete: (id: number) => `${v1}/cms/deletestaticpage/${id}`,
      },
      FAQ: {
        add: `${v1}/faq/addfaq`,
        getbyid: (id: number) => `${v1}/faq/getfaq/${id}`,
        edit: (id: number) => `${v1}/faq/editfaq/${id}`,
        list: `${v1}/faq/getfaqs/`,
        updateStatus: `${v1}/faq/changestatus`,
        delete: (id: number) => `${v1}/faq/deletefaq/${id}`,
      },
      
    };
  }
}


// export const admin_apiconfig =
// {
//   requestSettings: {
//     header: {
//       'Content-Type': 'application/json',
//       //'Access-Control-Allow-Origin': '*',
//       //'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, PUT, PATCH, DELETE',
//       'Accept-Language':'en'
//     },
      
//       headerFormData: {
//           'Content-Disposition': 'multipart/form-data',
//           'Accept-Language':'en'
//           //'Access-Control-Allow-Origin': '*',
//           //'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, PUT, PATCH, DELETE',
//         },

//   },
//   endpoints:{
//     accounts: {
//       login: environment.admin_apiv1 + '/accounts/login',

//       },

//     pharmacy: {
//       add: environment.admin_apiv1 + '/pharmacy/addpharmacy/',
//       update: (id: number) => environment.admin_apiv1 + '/pharmacy/editpharmacy/' + id,
//       getPaged: environment.admin_apiv1 + '/pharmacy/getpharmacies',
//       get: (id: number) => environment.admin_apiv1 + '/pharmacy/getpharmacy/' + id,

//       getcountries: environment.admin_api + '/masters/getcountries/',
//       getstates: (id: number) => environment.admin_api + '/masters/getstates/' + id,
//       getcities: (id: number) => environment.admin_api + '/masters/getcities/' + id

//     },
//     user: {
//       add: environment.admin_apiv1 + '/users/adduser/',
//       get: (id: number) => environment.admin_apiv1 + '/users/get/' + id,
//       update: (id: number) => environment.admin_apiv1 + '/users/edituser/' + id,
//       setpassword: environment.admin_apiv1 + '/users/change-password',
//       userroles: environment.admin_api + '/masters/getroles',
//       changepassword: environment.admin_apiv1 +'/users/SetPassword',
//       list: environment.admin_apiv1 + '/users/getusers'
//     }
//   }
// }
