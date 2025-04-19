import { Component, OnInit, Renderer2 } from '@angular/core';
import { getBaseUrl, loadScript, loadScripts, loadStylesheets, setTitle } from '../../../main';
import { RouterModule } from '@angular/router';


@Component({
  selector: 'app-admin-emptylayout',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './admin.emptylayout.component.html',
  styles: ['']
})
export class AdminEmptyLayoutComponent implements OnInit {

  constructor(private renderer: Renderer2) 
  { 
    
  }

  scripts: Array<string> = [
    'assets/vendor/libs/jquery/jquery.min.js',
    'assets/vendor/libs/datatables.net/js/jquery.dataTables.min.js',
    'assets/vendor/libs/datatables.net-bs4/js/dataTables.bootstrap4.min.js',
    //'assets/vendor/libs/jquery/jquery.min.js', 
    //'assets/bundles/libscripts.bundle.js',
    //'assets/bundles/vendorscripts.bundle.js',
    'assets/vendor/libs/bootstrap/bootstrap.bundle.min.js',
    'assets/vendor/libs/bootstrap-notify/bootstrap-notify.js',
    'assets/vendor/js/helpers.js',
    'assets/js/config.js',
    'assets/vendor/libs/popper/popper.js',
    'assets/vendor/js/bootstrap.js',
    'assets/vendor/libs/perfect-scrollbar/perfect-scrollbar.js',
    'assets/vendor/js/menu.js',
    'assets/js/appmain.js',
  ];

  styles: Array<string> = [
    "assets/vendor/fonts/boxicons.css",
    "assets/vendor/css/core.css",
    "assets/vendor/css/theme-default.css",
    "assets/css/demo.css",
    "assets/vendor/libs/perfect-scrollbar/perfect-scrollbar.css",
    "assets/vendor/css/pages/page-auth.css",
    "assets/vendor/libs/datatables.net-bs4/css/dataTables.bootstrap4.min.css",
    "assets/vendor/libs/bootstrap-notify/bootstrap-notify.min.js",
    "assets/vendor/libs/perfect-scrollbar/perfect-scrollbar.css",
    "assets/vendor/libs/perfect-scrollbar/perfect-scrollbar.css",
    //'assets/plugins/bootstrap/css/bootstrap.min.css',
    //'assets/css/style.min.css'
  ];

  ngOnInit() {
    setTitle(':: REPMED :: ');
    loadStylesheets(this.styles);
    loadScripts(this.scripts);

  }


}