import { get } from 'jquery';
import { environment } from '../../../src/environments/environment';
import { ConfigService } from '../../config.service';
import { Injectable } from '@angular/core';


@Injectable({ providedIn: 'root' })
export class AdminApiConfigService {
  constructor(private config: ConfigService) {}

  get requestSettings() {
    return {
      header: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Accept-Language': 'en'
      },
      headerFormData: {
        'Content-Disposition': 'multipart/form-data',
        'Accept-Language': 'en'
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
      },
      pharmacy: {
        add: `${v1}/pharmacy/addpharmacy/`,
        update: (id: number) => `${v1}/pharmacy/editpharmacy/${id}`,
        getPaged: `${v1}/pharmacy/getpharmacies`,
        get: (id: number) => `${v1}/pharmacy/getpharmacy/${id}`,
        getcountries: `${api}/masters/getcountries/`,
        getstates: (id: number) => `${api}/masters/getstates/${id}`,
        getcities: (id: number) => `${api}/masters/getcities/${id}`
      },
      user: {
        add: `${v1}/users/adduser/`,
        get: (id: number) => `${v1}/users/get/${id}`,
        update: (id: number) => `${v1}/users/edituser/${id}`,
        userroles: `${api}/masters/getroles`,
        list: `${v1}/users/getusers`,
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
      }
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
