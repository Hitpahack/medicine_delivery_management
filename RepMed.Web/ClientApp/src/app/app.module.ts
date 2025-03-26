import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import './global.extenstions';
import { AppComponent } from './app.component';
import { routing } from './app.routing';
import { AdminLayoutComponent } from './_layouts/admin/admin.layout.component';
import { WebModule } from './web/web.module';
import { AdminModule } from './admin/admin.module';

@NgModule({
     imports: [
    RouterModule,
    BrowserModule,
    HttpClientModule,
    AppComponent,
    FormsModule,
    WebModule.forRoot(),
    AdminModule.forRoot(),
    routing
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
