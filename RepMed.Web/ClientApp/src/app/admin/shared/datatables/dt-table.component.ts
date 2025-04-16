import {
    Component,
    Input,
    AfterViewInit,
    ViewChild,
    ElementRef,
    OnDestroy
  } from '@angular/core';
  import { HttpClient } from '@angular/common/http';
import { AdminBaseComponent } from '../../admin.base.component';
import { Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
  
  declare var $: any;
  
  @Component({
    selector: 'app-dt-table',
    templateUrl: './dt-table.component.html',
    styleUrls: ['./dt-table.component.css']
  })
  export class DtTableComponent extends AdminBaseComponent implements AfterViewInit, OnDestroy {
    @ViewChild('tableRef', { static: false }) tableRef!: ElementRef;
  
    @Input() options: any;
    @Input() columns: any[] = [];
    @Input() ajaxUrl!: string;
  
    dataTable: any;
    constructor(private http: HttpClient, public router: Router, fb:FormBuilder) {
        super(router,fb);
    }

    
  
    ngAfterViewInit(): void {
      this.dataTable = $(this.tableRef.nativeElement).DataTable({
        ajax: this.ajaxUrl,
        columns: this.columns,
        ...this.options
      });
    }
  
    ngOnDestroy(): void {
      if (this.dataTable) {
        this.dataTable.destroy(true);
      }
    }
  }
  