import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { Router } from '@angular/router';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { ValidationService } from '@shared/ValidationService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base';
import { SelectListDto, SmartDateTime } from '@shared/service-proxies/service-proxies';
import * as XLSX from 'xlsx';
import { Title } from '@angular/platform-browser';
interface grid {
  MaterialCode: string;
  PlantCode: string;
  
}
@Component({
  selector: 'app-revalidation-report',
  templateUrl: './revalidation-report.component.html',
  styleUrls: ['./revalidation-report.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class RevalidationReportComponent implements OnInit {
  MaterialCode: string;
  IExcel: grid | any;
  exportExcel: any | 0;
  PlantCode: string;
  packingReports:any;
  plnaCodeList:any;
  ItemCodes:any;
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
  this.titleService.setTitle('Revalidation Report');
  this.GetPlantCode();
  this.GetItemCodes();
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
debugger;
  this.currentPage = e.pageIndex;
  this.pageSize = e.pageSize;
  this.iterator();
}

private getArray() {
  const data = {
    MaterialCode: this.MaterialCode,
    PlantCode: this.PlantCode
  };


 this._apiservice.GetRevalidationReport(data).subscribe(result => {
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

GetPlantCode() {
  abp.ui.setBusy();
  this._apiservice.getPlantCode().subscribe((response) => {
    this.plnaCodeList = response["result"];
    abp.ui.clearBusy();
  },
  (error) => {
    abp.ui.clearBusy();
  }
  );
};

GetItemCodes() {

  this._apiservice.GetItemCodes().subscribe((modeSelectList: SelectListDto[]) => {
      this.ItemCodes = modeSelectList["result"];
      
  });
};



markDirty() {
  this._appComponent.markGroupDirty(this.addEditFormGroup);
  this.dataSource.filteredData.length=0;
    this.totalSize = 0;
  return true;
}

exportexcel(): void {
  this.exportExcel = 1
  
      this.IExcel =this.array;
      let Heading = [['Plant Code', 'Item Code', 'Item Description', 'Shipper Barcode','Part Barcode','Actually MFG Date','New MFG Date']];
      const wb = XLSX.utils.book_new();
      const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(this.IExcel);
      XLSX.utils.sheet_add_aoa(ws, Heading);
      XLSX.utils.sheet_add_json(ws, this.IExcel, { origin: 'A2', skipHeader: true });

      XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

      XLSX.writeFile(wb, 'RevalidationReport.xlsx');

    }
}
