import { Component, ElementRef, Injector, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { debounceTime, distinctUntilChanged, filter, finalize, map } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { EntityDto, PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';
import { PlantDto, SelectListServiceProxy, ChangePswdServiceProxy,ElogSuryaApiServiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute, Data, Router } from '@angular/router';
import { relative } from 'path';
import { FormControl } from '@angular/forms';
import * as moment from 'moment';
import { fromEvent } from 'rxjs';
import { AppConsts } from '@shared/AppConsts';
import { HttpClient } from '@angular/common/http';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import * as XLSX from 'xlsx';
import { Title } from '@angular/platform-browser';


interface CustomerMaster {
    code : string;
    type :string;
    description : string;
    address : string;
    
  
}

@Component({
    templateUrl: './customer.component.html',
    animations: [appModuleAnimation()],
    providers: [ElogSuryaApiServiceServiceProxy]
    //styleUrls: ['./plant.component.less']
})
export class CustomerComponent implements OnInit  {
    lines: any;
    searchText;
    searchTerm = '';
    p: Number = 1;
    public array: any;
    DownloadPdf:any;
    public pageSize = 10;
    public currentPage = 0;
    public totalSize = 0;


  inventoryExcel: CustomerMaster | any;
  exportExcel: any | 0;
  
    public dataSource: MatTableDataSource<any> = new MatTableDataSource<CustomerMaster>();
    public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<CustomerMaster>();
    @ViewChild(MatSort, { static: false }) sort!: MatSort;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  
    constructor(
      private _apiservice: ApiServiceService,
      private titleService: Title
    ) { }
  
    ngOnInit() {
      this.titleService.setTitle('Customer Master');
      this.getArray();
      this.paginator._intl.itemsPerPageLabel="Records per page";
    }
  
    ngAfterViewInit(): void {
      this.dataSource.sort = this.sort;
    }
  
    filterCountries(searchTerm: string) {
      this.dataSourcePagination.filter = searchTerm.trim().toLocaleLowerCase();
      const filterValue = searchTerm;
      this.dataSourcePagination.filter = filterValue.trim().toLowerCase();
      this.dataSource.filteredData = this.dataSourcePagination.filteredData;
      this.iterator();
    }
  
    onMatSortChange() {
      this.dataSource.sort = this.sort;
    }
  
    public handlePage(e: any) {
  
      this.currentPage = e.pageIndex;
      this.pageSize = e.pageSize;
      this.iterator();
    }
  
    private getArray() {
      this._apiservice.getCustomerMaster()
        .subscribe((response) => {
          this.dataSourcePagination = new MatTableDataSource<Element>(response['result']);
          this.dataSourcePagination.paginator = this.paginator;
          this.array = response['result'];
          this.totalSize = this.array.length;
          this.iterator();
        });
    }
  
    private iterator() {
      const end = (this.currentPage + 1) * this.pageSize;
      const start = this.currentPage * this.pageSize;
      this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
    }
    
    exportexcel(): void {
      this.exportExcel = 1
      
          this.inventoryExcel =this.array;
          let Heading = [['Customer Code', 'Name', 'Address', 'City', 'District']];
          const wb = XLSX.utils.book_new();
          const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(this.inventoryExcel);
          XLSX.utils.sheet_add_aoa(ws, Heading);
          XLSX.utils.sheet_add_json(ws, this.inventoryExcel, { origin: 'A2', skipHeader: true });
  
          XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
  
          XLSX.writeFile(wb, 'Customer.xlsx');
  
        }
  
    


printPDF(): void {
  let printContents, popupWin;
  printContents = document.getElementById('excel-table').innerHTML;
  popupWin = window.open('', '_blank', 'top=0,left=0,height=auto,width=auto');
  popupWin.document.open();
  popupWin.document.write(`
<html>
  <head>
    <title>CustomerMaster</title>
    <style>
    // @page title-page {
    //   margins: 75pt 30pt 50pt 30pt;
    //   background-color: red;
    // }
    </style>
  </head>
<body onload="window.print();window.close()">${printContents}</body>
</html>`
  );
  popupWin.document.close();
}

}  