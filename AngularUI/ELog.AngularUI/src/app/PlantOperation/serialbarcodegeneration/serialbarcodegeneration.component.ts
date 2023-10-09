import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit, NgModule, ViewChild, AfterViewInit } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { NgxPaginationModule } from 'ngx-pagination';
import { AppComponent } from '@app/app.component';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator } from '@shared/app-component-base';

interface SerialNo {
  plantCode: string,
  line: string,
  packingOrder: string,
  supplierCode: string
  driverCode: string
}

@Component({
  selector: 'app-serialbarcodegeneration',
  templateUrl: './serialbarcodegeneration.component.html',
  styleUrls: ['./serialbarcodegeneration.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})

export class SerialbarcodegenerationComponent implements OnInit {

  lines: any;
  searchText;
  searchTerm = '';
  p: Number = 1;
  public array: any;

  public pageSize = 10;
  public currentPage = 0;
  public totalSize = 0;

  plantCode: string;
  line: string;
  packingOrder: string;
  supplierCode: string;
  driverCode: string;
  plnaCodeList: any;
  lineList: any;
  packingOrderList: any;
  picklistItems:[];
  isSelected: boolean;

  // displayedColumns: string[] = ['PlantCode', 'WorkCenterCode', 'WorkCenterDiscription','Active'];

  public dataSource: MatTableDataSource<any> = new MatTableDataSource<SerialNo>();
  public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<SerialNo>();
  @ViewChild(MatSort, { static: false }) sort!: MatSort;
  @ViewChild('paginator', { static: true }) paginator: MatPaginator;

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent : ValidationService,
  ) { }

  ngOnInit() {
    this.getArray();
    this.GetPlantCode();
    this.GetLineCode();
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
    this._apiservice.getLineMaster()
      .subscribe((response) => {
        console.log("res", response['result'])
        // this.dataSourcePagination  = new MatTableDataSource<Element>(response['result']);
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

  addEditFormGroup: FormGroup = this.formBuilder.group({
    //registration: [null, [Validators.required, NoWhitespaceValidator]],
    LineCodeFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    plantCodeFormCControl: [null, [Validators.required, NoWhitespaceValidator]],
    packingOrderFormControl: ['',[Validators.required,NoWhitespaceValidator]],
    supplierCodeFormControl:[null,[Validators.required,NoWhitespaceValidator]],
    driverCodeFormControl:[null,[Validators.required,NoWhitespaceValidator]]
});

GetPlantCode() {
  this._apiservice.getPlantCode().subscribe((modeSelectList: SelectListDto[]) => {
      this.plnaCodeList = modeSelectList["result"];
  });
};

GetLineCode() {
  console.log("line",this.line);
  this._apiservice.getLineWorkCenterNo().subscribe((response) => {
      this.lineList = response["result"];
      console.log("line2",this.lineList)
  });
};


onChangePlantCode(value)
{
 
 this._apiservice.getPackingOrderNo(value).subscribe((response) => {
  this.packingOrderList = response["result"];
  console.log("value",response["result"])
});
}

GrtTableGrid(value)
{
  this._apiservice.GetSerialNumberDetails(value).subscribe((response) => {
    this.picklistItems = response["result"];
})
}

getCheckbox(value)
{
  console.log(value);
}

selectAll(value)
{
  console.log(value);
  
}

markDirty() {
  this._appComponent.markGroupDirty(this.addEditFormGroup);
   return true;
}

Clear() {

  this.addEditFormGroup.controls['LineCodeFormControl'].setValue(null);
  this.addEditFormGroup.controls['plantCodeFormCControl'].setValue(null);
  this.addEditFormGroup.controls['packingOrderFormControl'].setValue(null);
  this.addEditFormGroup.controls['supplierCodeFormControl'].setValue(null);
  this.addEditFormGroup.controls['driverCodeFormControl'].setValue(null);

}

}
