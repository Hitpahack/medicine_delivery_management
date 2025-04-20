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
   
  ];

  styles: Array<string> = [
    
  ];

  ngOnInit() {
    setTitle(':: REPMED :: ');
    loadStylesheets(this.styles);
    loadScripts(this.scripts);

  }


}