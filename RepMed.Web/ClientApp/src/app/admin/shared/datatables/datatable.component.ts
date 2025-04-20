import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, Input, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';


declare const $: any;
@Component({
  selector: 'app-datatable',
  standalone: true,
  templateUrl: './datatable.component.html',
  imports: [CommonModule]
})
export class DatatableComponent implements AfterViewInit, OnDestroy {
  @Input() options!: {
    tableId: string;
    ajax: any;
    serverSide?: boolean;
    processing?: boolean;
    searching?: boolean;
    columns: any[];
    searchInputId?: string;
    delaySearchTimeOut?: number;
    customButtons?: any[];
  };

  private dtInstance: any;

  get tableSelector(): string {
    return `#${this.options.tableId}`;
  }

  ngAfterViewInit(): void {
   
    this.dtInstance = $(this.tableSelector).DataTable({
      dom: 'Bfrtip',
      buttons: this.options.customButtons??[],
      processing: this.options.processing??true,
      serverSide: this.options.serverSide??true,
      searching: this.options.searching??true,
      drawCallback: (settings) => {
        this.initDrawCallback(settings)
      },
      ajax: this.options.ajax,
      columns: this.options.columns,
     
    });

  }

  ngOnDestroy(): void {
    if (this.dtInstance) {
      this.dtInstance.destroy();
    }
  }
  initDrawCallback(settings): void {
    if (this.options.searchInputId) {
      const searchBox = document.getElementById(this.options.searchInputId) as HTMLInputElement;
      if (searchBox) {
        let debounceTimer: any;

        searchBox.addEventListener('keyup', () => {
          clearTimeout(debounceTimer);
          debounceTimer = setTimeout(() => {
            const value = searchBox.value;
            if (this.dtInstance) {
              this.dtInstance.search(value).draw();
            }
          }, this.options.delaySearchTimeOut||2000); // 3 seconds
        });
      }
    }

  }
}
