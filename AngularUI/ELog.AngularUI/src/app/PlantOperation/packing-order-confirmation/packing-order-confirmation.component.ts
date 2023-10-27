import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { Router } from '@angular/router';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { ValidationService } from '@shared/ValidationService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';

interface PackingOrderConfirmation {
  packingOrderNo: string,
  plantCode: string,
  materialCode: string,
  packedQty: any,
  strLocCode: string,
  packing_Date: Date
  lineCode:string
}

@Component({
  selector: 'app-packing-order-confirmation',
  templateUrl: './packing-order-confirmation.component.html',
  styleUrls: ['./packing-order-confirmation.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class PackingOrderConfirmationComponent implements OnInit, AfterViewInit {
  lines: any;
  searchText;
  searchTerm = '';
  p: Number = 1;
  public array: any;

  public pageSize = 10;
  public currentPage = 0;
  public totalSize = 0;

  plnaCodeList: any;
  packingOrderList: any;
  packingOrderNo: any;
  plantCode: any;

  public dataSource: MatTableDataSource<any> = new MatTableDataSource<PackingOrderConfirmation>();
  public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<PackingOrderConfirmation>();
  @ViewChild(MatSort, { static: false }) sort!: MatSort;
  @ViewChild('paginator', { static: true }) paginator: MatPaginator;

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent: ValidationService,
  ) { }

  ngOnInit() {
    this.GetPlantCode();
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
    abp.ui.setBusy();
    if(this.packingOrderNo != 'undefined' && this.packingOrderNo != undefined)
    {
      this._apiservice.GetPackingOrderConfirmingDetails(this.packingOrderNo, this.plantCode)
      .subscribe((response) => {
        this.dataSourcePagination = new MatTableDataSource<Element>(response['result']);
        this.dataSourcePagination.paginator = this.paginator;
        this.array = response['result'];
        this.totalSize = this.array.length;
        this.iterator();
        abp.ui.clearBusy();
      },
      (error) => {
        abp.ui.clearBusy();
      }
      );
    }
    else
    {
      abp.ui.clearBusy();
    }
  }

  private iterator() {
    const end = (this.currentPage + 1) * this.pageSize;
    const start = this.currentPage * this.pageSize;
    this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
  }

  addEditFormGroup: FormGroup = this.formBuilder.group({
    plantCodeFormCControl: [null, [Validators.required, NoWhitespaceValidator]],
    packingOrderFormControl: ['', [Validators.required, NoWhitespaceValidator]]

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


  onChangePlantCode(value: any) {
    if(value != undefined &&  value != '')
    {
    abp.ui.setBusy();
    this._apiservice.GetConfirming_PO_No_(value).subscribe(
      (response) => {
        this.packingOrderList = response['result'];
        abp.ui.clearBusy();
      },
      (error) => {
        abp.ui.clearBusy();
      }
    );
    }
  }


  Save() {
    abp.ui.setBusy();
    this._apiservice.PackingOrderConfirmation(this.dataSourcePagination.filteredData).subscribe(result => {
      if (result["result"][0]['valid']) {
        abp.notify.success(result["result"][0]['valid']);
        this.plnaCodeList = null;
        this.GetPlantCode();
        abp.ui.clearBusy();
        this.dataSource.filteredData = null;
        this.Clear();
      }
      else {
        abp.notify.error(result["result"][0]['error']);
        abp.ui.clearBusy();
      }

    },
    (error) => {
      abp.ui.clearBusy();
    });
  }

  markDirty() {
    this._appComponent.markGroupDirty(this.addEditFormGroup);
    return true;
  }

  Clear() {
    this.addEditFormGroup.controls['plantCodeFormCControl'].setValue(null);
    this.addEditFormGroup.controls['packingOrderFormControl'].setValue(null);
  }

}
