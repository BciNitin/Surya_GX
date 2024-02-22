import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { ValidationService } from '@shared/ValidationService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';
import { Title } from '@angular/platform-browser';

interface grid {
  parentBarcode: any,
  childBarcode: any,
  status: 'OK',
  plantCode: any,
  lineCode: any,
  packingOrderNo: any,
  qcStatus: any
}


@Component({
  selector: 'app-quality-checking',
  templateUrl: './quality-checking.component.html',
  styleUrls: ['./quality-checking.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class QualityCheckingComponent implements OnInit, AfterViewInit {

  public pageSize = 10;
  public currentPage = 0;
  public totalSize = 0;
  public array: any;
  updateUIselectedOrderType: any;

  plantCode: any;
  lineCode: any;
  packingOrder: any;
  cartonBarcode: any;

  plnaCodeList: any;
  lineList: any;
  packingOrderList: any;
  picklistItems: [];
  isSelected: boolean;
  Qty: number = 0;
  checked: boolean;
  Pass: string;
  Status = [];


  status: any[];
  parentBarcode: string[] = [];
  itemBarcode: string[] = [];

  options = ['OK', 'NG'];
  selectedOption: string;

  public dataSource: MatTableDataSource<any> = new MatTableDataSource<grid>();
  public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<grid>();
  @ViewChild(MatSort, { static: false }) sort!: MatSort;
  @ViewChild('paginator', { static: true }) paginator: MatPaginator;

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent: ValidationService,
    private titleService: Title

  ) { }

  ngOnInit() {
    this.titleService.setTitle('Quality Checking');
    abp.ui.setBusy();
    this.GetPlantCode();
    abp.ui.clearBusy();
  }

  addEditFormGroup: FormGroup = this.formBuilder.group({
    LineCodeFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    plantCodeFormCControl: [null, Validators.required, NoWhitespaceValidator],
    packingOrderFormControl: [null, Validators.required, NoWhitespaceValidator],
    passFormControl: [null, Validators.required, NoWhitespaceValidator]
  });
  matcher = new MyErrorStateMatcher();

  async GetPlantCode() {
    abp.ui.setBusy();
    await this._apiservice.getPlantCode().subscribe((modeSelectList: SelectListDto[]) => {
      this.plnaCodeList = modeSelectList["result"];
      abp.ui.clearBusy();
    });
    
  };

  GetLineCode() {
    if (this.plantCode !== undefined) {
      abp.ui.setBusy();
      this._apiservice.getqualitySamplingLineWorkCenterNo(this.plantCode).subscribe((response) => {
        this.lineList = response["result"];
        abp.ui.clearBusy();
      });

    };
  }

  onChangeLineCode() {
    abp.ui.setBusy();
    this._apiservice.QualityCheckingPackingOrderNo(this.plantCode, this.lineCode).subscribe((response) => {
      this.packingOrderList = response["result"];
      this.updateUIselectedOrderType = this.packingOrder;
      abp.ui.clearBusy();
      
    });
  }

  Save() {
    abp.ui.setBusy();
    this._apiservice.saveQualityChecking(this.dataSource.filteredData).subscribe(
      (result) => {
        abp.ui.clearBusy();
        if (result['result'][0].error) {
          abp.notify.error(result['result'][0].error);
        } else {
          abp.notify.success(result['result'][0].valid);
          this.iterator();
          this.Clear();
        }
      },
      (error) => {
        // Handle HTTP error
      }
    );
  }



  markDirty() {
    this._appComponent.markGroupDirty(this.addEditFormGroup);
    return true;
  }

  Clear() {
//LineCodeFormControl: [null, Validators.required, NoWhitespaceValidator],
this.addEditFormGroup.controls['LineCodeFormControl'].setValue(null);
    // this.addEditFormGroup.controls['plantCodeFormCControl'].setValue(null);
    // this.addEditFormGroup.controls['packingOrderFormControl'].setValue(null);
    // this.addEditFormGroup.controls['CartonBarCodeFormControl'].setValue(null);
    // this.addEditFormGroup.controls['ItemBarCodeFormControl'].setValue(null);
    // this.addEditFormGroup.controls['passFormControl'].setValue(null);
    this.dataSource.data = [];
    
  }

  getDetails() {
    abp.ui.setBusy();
    if (this.packingOrder != '0' && this.packingOrder != undefined) {
      this._apiservice.GetQualityChecking(this.plantCode, this.lineCode, this.packingOrder)
        .subscribe((response) => {
          if (!response['result'][0].error) {
            this.dataSourcePagination = new MatTableDataSource<Element>(response['result']);
            this.dataSourcePagination.paginator = this.paginator;
            this.array = response['result'];
            this.totalSize = this.array.length;
            this.iterator();
          }
          else {
            this.dataSource.filter = null;
            abp.notify.error(response['result'][0].error);
          }
        })
    }
    else {
      this.dataSource.filter = null;
    }
    abp.ui.clearBusy();
  }

  private iterator() {
    abp.ui.setBusy();
    const end = (this.currentPage + 1) * this.pageSize;
    const start = this.currentPage * this.pageSize;
    // this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
    this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
    //for dropdown default valu of status

    for (let i = 0; i < this.dataSource.filteredData.length; i++) {
      this.dataSource.filteredData[i].status = 'Yes';
      this.dataSource.filteredData[i].qcStatus = this.Pass;
      this.dataSource.filteredData[i].packingOrderNo = this.packingOrder;
      this.dataSource.filteredData[i].plantCode = this.plantCode;
      this.dataSource.filteredData[i].lineCode = this.lineCode;
    }
    abp.ui.clearBusy();
  }
  private iterators() {
    abp.ui.setBusy();
    const end = (this.currentPage + 1) * this.pageSize;
    const start = this.currentPage * this.pageSize;
    // this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
    this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
    //for dropdown default valu of status

    for (let i = 0; i < this.dataSource.filteredData.length; i++) {
      // this.dataSource.filteredData[i].status = 'Yes';
      this.dataSource.filteredData[i].qcStatus = this.Pass;
      this.dataSource.filteredData[i].packingOrderNo = this.packingOrder;
      this.dataSource.filteredData[i].plantCode = this.plantCode;
      this.dataSource.filteredData[i].lineCode = this.lineCode;
    }
    abp.ui.clearBusy();
  }
  getDetails1(v) {
    this.iterators();
    // this.iterator();
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    // this.dataSource = new MatTableDataSource(this.dataSource.filteredData);
    // this.dataSource.paginator = this.paginator;
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
}

