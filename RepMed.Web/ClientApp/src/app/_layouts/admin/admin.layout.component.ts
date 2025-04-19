import { Component, OnInit, Renderer2 } from '@angular/core';
import { RouterModule } from '@angular/router';
import { getBaseUrl, loadScript, loadScripts, loadStylesheets, setTitle } from '../../../main';
@Component({
    selector: 'app-admin-layout',
  imports: [RouterModule],
  templateUrl: './admin.layout.component.html',
  styleUrls: ['./admin.layout.component.css']
})
export class AdminLayoutComponent implements OnInit {

  // Show/hide pharmacy submenu
  isPharmacySubmenuVisible: boolean = false;
  
  constructor(private renderer: Renderer2) { }
  scripts: Array<string> = [ ];
  styles: Array<string> = [];

  ngOnInit() {
   // const script = this.renderer.createElement('script');
    //script.src = `https://cdnjs.cloudflare.com/ajax/libs/le_js/0.0.3/le.min.js`;
    //this.renderer.appendChild(document.head, script);
    setTitle(':: REPMED :: ');
    loadStylesheets(this.styles);
    loadScripts(this.scripts); 
  }

  toggleSubmenu(id: string): void {
    const submenu = document.getElementById(id);
    if (submenu) {
      submenu.classList.toggle('active');
    }
  }  

}