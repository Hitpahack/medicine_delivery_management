import { CommonModule } from "@angular/common";
import { ModuleWithProviders, NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { WebLayoutComponent } from "../_layouts/web/web.layout.component";
import { LoginComponent } from "./accounts/login.component";
import { FacilitesComponent } from "./facilites/facilites.component";

@NgModule({
    imports: [CommonModule, RouterModule],
    declarations: [   WebLayoutComponent, FacilitesComponent, LoginComponent ],
    exports: [        WebLayoutComponent   ]
  })
  export class WebModule { 
    static forRoot(): ModuleWithProviders {
      return {
        ngModule: WebModule,
          providers: []
      };
    }
  }