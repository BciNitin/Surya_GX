import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { ValidationService } from '@shared/ValidationService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base';
import { SelectListDto, SmartDateTime } from '@shared/service-proxies/service-proxies';
import { Title } from '@angular/platform-browser';
import { DatePipe } from '@angular/common';
import * as XLSX from 'xlsx';
interface grid {
  FromDate: Date;
  LineCode: string;
  MaterialCode: string;
  challanNos: string;
  PlantCode: string;
  ToDate: Date;
}
@Component({
  selector: 'app-grn-at-branch',
  templateUrl: './grn-at-branch.component.html',
  styleUrls: ['./grn-at-branch.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class GrnAtBranchComponent implements OnInit {
  MaterialCode: string;
  
  IExcel: grid | any;
  exportExcel: any | 0;
  FromDate: Date;
  LineCode: string;
  challanNos: string;
  PlantCode: string;
  ToDate: Date;
  packingReports:any;
plnaCodeList:any;
ItemCodes:any;
lineList:any;
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
  private datePipe: DatePipe

) { }
ngOnInit() {
  this.titleService.setTitle('GRN at Branch');
  this.GetPlantCode();
  this.GetItemCodes();
  this.GetLineCode();
  this.GetChallanNo();
  this.paginator._intl.itemsPerPageLabel="Records per page";
  //this.getArray();
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
  const data = {
    MaterialCode: this.MaterialCode,
    FromDate: this.FromDate,
    ToDate: this.ToDate,
    LineCode: this.LineCode,
    challanNos: this.challanNos,
    PlantCode: this.PlantCode
  };


 this._apiservice.GetGRN_AtBranchReport(data).subscribe(result => {
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
  FromDateFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    ToDateFormControl: [null, [Validators.required, NoWhitespaceValidator]],
  //ToDateFormControl: [null, [this.dateSelectedValidator, Validators.required, NoWhitespaceValidator]]
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

GetLineCode() {
  this._apiservice.getLineWorkCenterNo().subscribe((response) => {
      this.lineList = response["result"];
  });
};
GetChallanNo() {
  this._apiservice.GetchallanNo().subscribe((modeSelectList: SelectListDto[]) => {
      this.challanNo = modeSelectList["result"];
  });
};

markDirty() {
  this._appComponent.markGroupDirty(this.addEditFormGroup);
  this.dataSource.filteredData.length=0;
    this.totalSize = 0;
  return true;
}
onDateChangeEvent() {
  this.validationTypes = [];
  this.showExpirationError = false;
  this.showInstallationError = false;
  this.showProcurmentError = false;
  var fromdate = this.addEditFormGroup.get("FromDateFormControl").value;
  var todate = this.addEditFormGroup.get("ToDateFormControl").value;
  if (fromdate > todate) {
      this.showExpirationError = true;
      this.validationTypes.push("frommustbeless");
  }
  this.FromDate = fromdate;
  this.ToDate = todate;
  return true;
}
exportexcel(): void {
  this.exportExcel = 1
  
      this.IExcel =this.array;
      let Heading = [['Plant Code', 'Received from Warehouse', 'GRN No.', 'Item','Total Qty.','Received Qty.']];
      const wb = XLSX.utils.book_new();
      const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(this.IExcel);
      XLSX.utils.sheet_add_aoa(ws, Heading);
      XLSX.utils.sheet_add_json(ws, this.IExcel, { origin: 'A2', skipHeader: true });

      XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

      XLSX.writeFile(wb, 'GrnAtBranchReport.xlsx');

    }
}
