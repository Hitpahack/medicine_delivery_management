import { Component, OnInit, Renderer2 } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
    selector: 'app-subadmin-layout',
  imports: [RouterModule],
  templateUrl: './subadmin.layout.component.html',
  styleUrls: ['./admin.layout.component.css']
})
export class SubAdminLayoutComponent implements OnInit {
  
  constructor(private renderer: Renderer2) { }

  ngOnInit() {
    const script = this.renderer.createElement('script');
    script.src = `https://cdnjs.cloudflare.com/ajax/libs/le_js/0.0.3/le.min.js`;
    this.renderer.appendChild(document.head, script);
  }
}