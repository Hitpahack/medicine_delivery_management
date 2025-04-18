import { get } from 'jquery';
import { environment } from '../../../src/environments/environment';

export const admin_apiconfig =
{
  requestSettings: {
    header: {
      'Content-Type': 'application/json',
      //'Access-Control-Allow-Origin': '*',
      //'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, PUT, PATCH, DELETE',
      'Accept-Language':'en'
    },
      
      headerFormData: {
          'Content-Disposition': 'multipart/form-data',
          'Accept-Language':'en'
          //'Access-Control-Allow-Origin': '*',
          //'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, PUT, PATCH, DELETE',
        },

  },
  endpoints:{
    accounts: {
      login: environment.admin_apiv1 + '/accounts/login',

      },

    pharmacy: {
      add: environment.admin_apiv1 + '/pharmacy/addpharmacy/',
      update: (id: number) => environment.admin_apiv1 + '/pharmacy/editpharmacy/' + id,
      getPaged: environment.admin_apiv1 + '/pharmacy/getpharmacies',
      get: (id: number) => environment.admin_apiv1 + '/pharmacy/getpharmacy/' + id,

      getcountries: environment.admin_api + '/masters/getcountries/',
      getstates: (id: number) => environment.admin_api + '/masters/getstates/' + id,
      getcities: (id: number) => environment.admin_api + '/masters/getcities/' + id

    },
    user: {
      add: environment.admin_apiv1 + '/users/adduser/',
      get: (id: number) => environment.admin_apiv1 + '/users/get/' + id,
      update: (id: number) => environment.admin_apiv1 + '/users/edituser/' + id,
      setpassword: environment.admin_apiv1 + '/users/change-password',
      userroles: environment.admin_api + '/masters/getroles',
      changepassword: environment.admin_apiv1 +'/users/SetPassword',
      list: environment.admin_apiv1 + '/users/getusers'
    }
  }
}
