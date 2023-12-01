import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { Router } from '@angular/router';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { ValidationService } from '@shared/ValidationService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base';
import { SelectListDto, SmartDateTime } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';
import { Title } from '@angular/platform-browser';
interface grid {
  MaterialCode: string;
  PlantCode: string;
  
}

@Component({
  selector: 'app-consolidated-non-barcoded-products',
  templateUrl: './consolidated-non-barcoded-products.component.html',
  styleUrls: ['./consolidated-non-barcoded-products.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class ConsolidatedNonBarcodedProductsComponent implements OnInit {
  public array: any;
  challanNo:any;
  public pageSize = 10;
  public currentPage = 0;
  public totalSize = 0;
  validationTypes: string[] | null;
  showProcurmentError: boolean | false;
  showInstallationError: boolean | false;
  showExpirationError: boolean | false;
 public dataSource: MatTableDataSource<any> = new MatTableDataSource<grid>();
 public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<grid>();
 @ViewChild(MatSort, { static: false }) sort!: MatSort;
 @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
 

constructor(
  private _apiservice: ApiServiceService,
  private formBuilder: FormBuilder,
  public _appComponent: ValidationService,
  private titleService: Title,
  
) { }
ngOnInit() {
  this.titleService.setTitle('Consolidated Report of Non Barcoded Products');
  
  this.paginator._intl.itemsPerPageLabel="Records per page";
  this.getArray();
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
debugger;
  this.currentPage = e.pageIndex;
  this.pageSize = e.pageSize;
  this.iterator();
}

private getArray() {
      this._apiservice.GetConsNonBarcodedProductDetails().subscribe(result => {
      this.dataSourcePagination = new MatTableDataSource<Element>(result['result']);
      this.dataSourcePagination.paginator = this.paginator;
      if(result["result"][0]['error'])
      {
        abp.notify.error(result["result"][0]['error']);
        
       
        this.iterator();
        this.dataSource.filteredData.length=0;
        this.totalSize = 0;
       
      }
      else
      {
        debugger;
        this.array = result['result'];
        this.totalSize = this.array.length;
        this.iterator();
        
      }
    });
}

private iterator() {
  const end = (this.currentPage + 1) * this.pageSize;
  const start = this.currentPage * this.pageSize;
  this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
}
addEditFormGroup: FormGroup = this.formBuilder.group({
  plantCodeFormCControl: [null, [Validators.required, NoWhitespaceValidator]],
  packingOrderFormControl: [''],
  ItemCodeFormCControl: [''],
  FromDateFormControl:[null],
  ToDateFormControl: [null]
  
});
matcher = new MyErrorStateMatcher();
}

