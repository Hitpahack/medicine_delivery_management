import { Component, OnInit, Renderer2 } from '@angular/core';

@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin.layout.component.html',
  styles: ['']
})
export class AdminLayoutComponent implements OnInit {

  constructor(private renderer: Renderer2) { }

  ngOnInit() {
    const script = this.renderer.createElement('script');
    script.src = `https://cdnjs.cloudflare.com/ajax/libs/le_js/0.0.3/le.min.js`;
    this.renderer.appendChild(document.head, script);
  }

}