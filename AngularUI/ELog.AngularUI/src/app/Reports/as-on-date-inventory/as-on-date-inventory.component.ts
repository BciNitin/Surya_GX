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
  selector: 'app-as-on-date-inventory',
  templateUrl: './as-on-date-inventory.component.html',
  styleUrls: ['./as-on-date-inventory.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class AsOnDateInventoryComponent implements OnInit {
  MaterialCode: string;
  PlantCode: string;
  plnaCodeList:any;
  ItemCodes:any;
  public array: any;
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
 @ViewChild('paginator', { static: true }) paginator: MatPaginator;
 

constructor(
  private _apiservice: ApiServiceService,
  private formBuilder: FormBuilder,
  public _appComponent: ValidationService,
  private titleService: Title,
 

) { }
ngOnInit() {
  this.titleService.setTitle('As On Date Inventory');
  this.GetPlantCode();
  this.GetItemCodes();
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
    PlantCode: this.PlantCode
  };
this._apiservice.GetAsOnDateInventoryReport(data).subscribe(result => {
      this.dataSourcePagination = new MatTableDataSource<Element>(result['result']);
      this.dataSourcePagination.paginator = this.paginator;
      debugger;
      if(result["result"][0]['error'])
      {
        abp.notify.error(result["result"][0]['error']);

        this.iterator();
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
  LineCodeFormControl:[''],
  FromDateFormControl:[null],
  ToDateFormControl: [null]
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


markDirty() {
  this._appComponent.markGroupDirty(this.addEditFormGroup);
  return true;
}


}

