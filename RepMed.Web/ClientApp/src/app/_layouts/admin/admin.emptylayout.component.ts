import { Component, OnInit, Renderer2 } from '@angular/core';
import { getBaseUrl, loadScript, loadScripts, loadStylesheets, setTitle } from 'src/main';

@Component({
  selector: 'app-admin-emptylayout',
  templateUrl: './admin.emptylayout.component.html',
  styles: ['']
})
export class AdminEmptyLayoutComponent implements OnInit {

  constructor(private renderer: Renderer2) { }
  
  scripts:Array<string> = [
    'assets/bundles/libscripts.bundle.js',
    'assets/bundles/vendorscripts.bundle.js',
    'assets/plugins/bootstrap-notify/bootstrap-notify.js'
  ];

  styles:Array<string> = [
    'assets/plugins/bootstrap/css/bootstrap.min.css',
    'assets/css/style.min.css'
  ];

  ngOnInit() {
  
    setTitle(':: BestShifts Admin :: Sign In');
    loadStylesheets(this.styles);
    loadScripts(this.scripts);
   
  }
   

}