import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, EventEmitter, Input, OnDestroy, Output } from '@angular/core';
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

  public dtInstance: any;
  @Output() rowClicked = new EventEmitter<any>();

  @Input() selectedItemIds: number[] = [];
  @Output() selectedItemIdsChange = new EventEmitter<number[]>();

  get tableSelector(): string {
    return `#${this.options.tableId}`;
  }

  ngAfterViewInit(): void {

    this.dtInstance = $(this.tableSelector).DataTable({
      dom: 'Bfrtip',
      buttons: this.options.customButtons ?? [],
      processing: this.options.processing ?? true,
      serverSide: this.options.serverSide ?? true,
      searching: this.options.searching ?? true,
      drawCallback: (settings) => {
        this.initDrawCallback(settings)
      },
      ajax: this.options.ajax,
      columns: this.options.columns,

    });

    $(this.tableSelector + ' tbody').on('click', 'tr', (event) => {
      if ($(event.target).is('input[type="checkbox"]')) return;

      const rowData = this.dtInstance?.row(event.currentTarget)?.data();
      if (rowData?.id) {
        this.rowClicked.emit(rowData);
      }
    });

  }

  public reload(): void {
    if (this.dtInstance) {
      this.dtInstance.ajax.reload(null, false); // false = don't reset pagination
    }
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
          }, this.options.delaySearchTimeOut || 2000); // 3 seconds
        });
      }
    }


    const table = $(this.tableSelector);

    //Re-check selected items
    table.find('.item_checkbox').each((_, checkbox) => {
      const id = +$(checkbox).data('id');
      if (this.selectedItemIds.includes(id)) {
        $(checkbox).prop('checked', true);
      }
    });

    //Listen to checkbox change
    table.find('.item_checkbox').off('change').on('change', (e: any) => {
      const id = +$(e.target).data('id');
      if (e.target.checked) {
        if (!this.selectedItemIds.includes(id)) {
          this.selectedItemIds.push(id);
        }
      } else {
        this.selectedItemIds = this.selectedItemIds.filter(x => x !== id);
      }
      this.selectedItemIdsChange.emit(this.selectedItemIds);
    });

    //Handle select all
    table.find('#select_all_main_checkbox').off('change').on('change', (e: any) => {
      const checked = e.target.checked;
      this.selectedItemIds = [];

      table.find('.item_checkbox').each((_, checkbox) => {
        const id = +$(checkbox).data('id');
        $(checkbox).prop('checked', checked);
        if (checked) {
          this.selectedItemIds.push(id);
        }
      });

      this.selectedItemIdsChange.emit(this.selectedItemIds);
    });

  }
}
