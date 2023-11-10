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
import * as moment from 'moment';
import { Moment } from 'moment';
import { Title } from '@angular/platform-browser';
import { DatePipe } from '@angular/common';
interface grid {
  FromDate: Date;
  LineCode: string;
  MaterialCode: string;
  PackingOrder: string;
  PlantCode: string;
  ToDate: Date;
  ShiperBarcode:string;
}
@Component({
  selector: 'app-packing-order-barcode-dtls',
  templateUrl: './packing-order-barcode-dtls.component.html',
  styleUrls: ['./packing-order-barcode-dtls.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class PackingOrderBarcodeDtlsComponent implements OnInit {
  MaterialCode: string;
  FromDate: Date;
  LineCode: string;
  PackingOrder: string;
  ShiperBarcode:string;
  PlantCode: string;
  ToDate: Date;
  packingReports:any;
  plnaCodeList:any;
  ItemCodes:any;
  lineList:any;
  public array: any;
  packingOrderlist:any;
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
  private datePipe: DatePipe

) { }

  ngOnInit() {
    this.titleService.setTitle('Packing Order Barcode Details');
    this.GetPlantCode();
    this.GetItemCodes();
    this.GetLineCode();
    this.GetPackingReportOrderNo();
    //this.getArray();
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
    debugger;
    const data = {
      MaterialCode: this.MaterialCode,
      FromDate: this.FromDate,
      ToDate: this.ToDate,
      LineCode: this.LineCode,
      PackingOrder: this.PackingOrder,
      PlantCode: this.PlantCode,
      ShiperBarcode: this.ShiperBarcode
    };
  this._apiservice.GetPackingOrderbarCodeDtlsReport(data).subscribe(result => {
        this.dataSourcePagination = new MatTableDataSource<Element>(result['result']);
        this.dataSourcePagination.paginator = this.paginator;
        this.array = result['result'];
        this.totalSize = this.array.length;
        this.iterator();
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
    ToDateFormControl: [null],
    ShiperBarcodeFormControl:['']

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
  GetPackingReportOrderNo() {
    this._apiservice.GetPackingReportOrderNo().subscribe((response) => {
        this.packingOrderlist = response["result"];
    });
  };

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
}
