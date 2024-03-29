import { Component, ElementRef, Injector, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {ElogSuryaApiServiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { Title } from '@angular/platform-browser';


interface PlantMaster{
    code :string,
    type : string,
    description : string,
    address : string,
}

@Component({
    selector: 'app-plant',
    templateUrl: './plant.component.html',
    animations: [appModuleAnimation()],
    providers: [ElogSuryaApiServiceServiceProxy]
    //styleUrls: ['./plant.component.less']
})
export class PlantComponent implements OnInit  {
  
    lines: any;
    searchText;
    searchTerm = '';
    p: Number = 1;
    public array: any;
  
    public pageSize = 10;
    public currentPage = 0;
    public totalSize = 0;
  
    public dataSource: MatTableDataSource<any> = new MatTableDataSource<PlantMaster>();
    public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<PlantMaster>();
    @ViewChild(MatSort, { static: false }) sort!: MatSort;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  
    constructor(
      private _apiservice: ApiServiceService,
      private titleService: Title
    ) { }
  
    ngOnInit() {
      this.titleService.setTitle('Plant Master');
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
      this._apiservice.getPlantMaster()
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
      // this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
      this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
    }
  
}  