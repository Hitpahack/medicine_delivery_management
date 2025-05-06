import { Component, OnInit, Renderer2 } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { getBaseUrl, loadScript, loadScripts, loadStylesheets, setTitle } from '../../../main';
import { AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SideNav } from '../../../app/security/sideNav';
import { AuthService } from '../../../app/security/auth.service';
import { adminAccountsService } from '../../admin/services/accounts/admin.accountsservice';

declare const window: any;
@Component({
  selector: 'app-pharmacy-layout',
  imports: [RouterModule, CommonModule],
  templateUrl: './pharmacy.layout.component.html',
  styleUrls: ['./admin.layout.component.css']
})
export class PharmacyLayoutComponent implements OnInit {

  // Show/hide pharmacy submenu 
  isPharmacySubmenuVisible: boolean = false;
  editpharmacyId: string | null = null;
  id: string | null = null;
  Userid: string | null = null;
  roleaccess: string | null = null;

  constructor(private renderer: Renderer2,
    private router: Router,
    private authService: AuthService,
    public accountservice: adminAccountsService,) { }
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

  userRole: string = '';
  filteredLinks: SideNav[] = [];

  private filterLinksByAccess(links: SideNav[], allowedModules: string[]): SideNav[] {
    return links
      .map(link => {
        if (link.children) {
          const filteredChildren = link.children.filter(child => allowedModules.includes(child.module!));
          if (filteredChildren.length > 0 && allowedModules.includes(link.module || '')) {
            return { ...link, children: filteredChildren };
          }
          return null;
        } else {
          return allowedModules.includes(link.module!) ? link : null;
        }
      })
      .filter((link): link is SideNav => link !== null);
  }

  ngOnInit() {
    this.editpharmacyId = sessionStorage.getItem('pharmacyId');
    this.id = sessionStorage.getItem('personid');
    this.Userid = sessionStorage.getItem('userId');
    const allowedModules = this.authService.getUserModules();
    this.filteredLinks = this.filterLinksByAccess(this.authService.getSideBarLinks(), allowedModules);
    this.userRole = (sessionStorage.getItem('rolename') || '').toLowerCase();
    console.log('userRole', this.userRole);
    setTitle(':: REPMED :: ');
    loadStylesheets(this.styles);
    loadScripts(this.scripts);

    if (this.authService.isLoggedIn()) {
      console.log('User is logged in');
    } else {
      console.log('User is NOT logged in');
    }
  }
  toggleSubmenu(id: string): void {
    const submenu = document.getElementById(id);
    if (submenu) {
      submenu.classList.toggle('active');
    }
  }

  logout(): void {
    this.accountservice.logout().subscribe(
      (response) => {
        if (response.isSuccess) {
          // Token/session/local storage clear
          localStorage.removeItem('token');
          localStorage.clear();  // optional
          sessionStorage.removeItem('userId');
          sessionStorage.removeItem('personid');
          sessionStorage.clear(); // optional
          this.router.navigate(['/login']); // navigate to login page
        } else {
          console.error('Logout failed:', response.message);
        }
      },
      (err) => {
        console.error('Logout error:', err);
        this.router.navigate(['/login']);
      }
    );
  }

}