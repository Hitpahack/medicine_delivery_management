import { Component, OnInit, Renderer2 } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { getBaseUrl, loadScript, loadScripts, loadStylesheets, setTitle } from '../../../main';
import { AfterViewInit } from '@angular/core';
declare const window: any;
@Component({
  selector: 'app-admin-layout',
  imports: [RouterModule],
  templateUrl: './admin.layout.component.html',
  styleUrls: ['./admin.layout.component.css']
})
export class AdminLayoutComponent implements OnInit, AfterViewInit {

  // Show/hide pharmacy submenu
  isPharmacySubmenuVisible: boolean = false;
  id: string | null = null;
  Userid: string | null = null;

  constructor(private renderer: Renderer2, private router: Router) { }
  scripts: Array<string> = [

  ];
  styles: Array<string> = [];

  ngAfterViewInit(): void {
    if (window.Helpers && typeof window.Helpers.initPasswordToggle === 'function') {
      window.Helpers.initPasswordToggle();
    }
    $('.menu-toggle').on('click', function () {
      $(this).next('.menu-sub').slideToggle();
      $(this).parent().toggleClass('open');
    });
  }

  ngOnInit() {
    // const script = this.renderer.createElement('script');
    //script.src = `https://cdnjs.cloudflare.com/ajax/libs/le_js/0.0.3/le.min.js`;
    //this.renderer.appendChild(document.head, script);
    this.id = sessionStorage.getItem('personid');
    this.Userid = sessionStorage.getItem('userId');
    setTitle(':: REPMED :: ');
    loadStylesheets(this.styles);
    loadScripts(this.scripts);
    if(this.Userid == null)
    {
      this.router.navigate(['/admin/login']);
    }
    else{
      this.router.navigate(['/admin/dashboard']);
    }
  }

  toggleSubmenu(id: string): void {
    const submenu = document.getElementById(id);
    if (submenu) {
      submenu.classList.toggle('active');
    }
  }

  logout() {
    // Token/session/local storage clear
    localStorage.removeItem('authToken'); // ya jo bhi token ka naam ho
    sessionStorage.clear(); // optional
    sessionStorage.removeItem('userId');
    sessionStorage.removeItem('personid');

    // Redirect to login page
    this.router.navigate(['/admin/login']);
  }

  

}