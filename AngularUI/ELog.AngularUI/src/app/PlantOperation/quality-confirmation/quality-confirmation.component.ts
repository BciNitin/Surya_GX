import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { ValidationService } from '@shared/ValidationService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';
import { error } from 'console';

interface grid {
  plantCode: string;
  materialCode: string;
  strLocCode: string;
  batchCode: string;
  packingOrderNo: string;
  packedQty: number;
  qcDate: Date;
  lineNo: string;
  okQty: number;
  ngQty: number;
  checkBoxValues : false;
}

@Component({
  selector: 'app-quality-confirmation',
  templateUrl: './quality-confirmation.component.html',
  styleUrls: ['./quality-confirmation.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class QualityConfirmationComponent implements OnInit, AfterViewInit {

  public pageSize = 10;
  public currentPage = 0;
  public totalSize = 0;
  public array: any;
  updateUIselectedOrderType: any;

  plantCode: any;
  packingOrder: any;

  plnaCodeList: any;
  lineList: any;
  packingOrderList: any;
  picklistItems: [];
  dataList:any[];

  // checkBoxValues: boolean[] = [];

  public dataSource: MatTableDataSource<any> = new MatTableDataSource<grid>();
  public dataSourceModel: MatTableDataSource<any> = new MatTableDataSource<grid>();
  public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<grid>();
  @ViewChild(MatSort, { static: false }) sort!: MatSort;
  @ViewChild('paginator', { static: true }) paginator: MatPaginator;

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent: ValidationService,
  ) { }

  ngOnInit() {
    abp.ui.setBusy(this.plantCode, this.GetPlantCode());
  }

  isAtLeastOneCheckboxSelected(): boolean {
    return this.dataSource.filteredData.some(obj => obj.checkBoxValue);
  }

  addEditFormGroup: FormGroup = this.formBuilder.group({
    plantCodeFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    packingOrderFormControl: [null, [Validators.required, NoWhitespaceValidator]],
  });

  matcher = new MyErrorStateMatcher();

  async GetPlantCode() {
    await this._apiservice.getPlantCode().subscribe((modeSelectList: SelectListDto[]) => {
      this.plnaCodeList = modeSelectList["result"];
      abp.ui.clearBusy();
    },
      (error => {
        abp.ui.clearBusy();
      }));

  };

  async getPackingorderNo(event) {
    abp.ui.setBusy();
    if (event != undefined && event != '' && event != '0') {
      await this._apiservice.GetConfirmationPackingOrderNo(event).subscribe(
        (result) => {
          this.packingOrderList = result['result'];
          abp.ui.clearBusy();
        },
        (error) => {
          abp.ui.clearBusy();
        }
      );
    }
    abp.ui.clearBusy();
  }

  async Save() {
    abp.ui.setBusy();
   
    for (let i = 0; i < this.dataSource.filteredData.length; i++) {
      if (this.dataSource.filteredData[i].checkBoxValue) {
         delete this.dataSource.filteredData[i].checkBoxValue;
         this.dataSourceModel.filteredData[i] = this.dataSource.filteredData[i];
      }
    }
        await this._apiservice.saveQualityConfirmation(this.dataSourceModel.filteredData).subscribe(
          (response) => {
            if (response.result[0].valid) {
              console.log(response)
              abp.notify.error(response.result[0].valid);
              this.dataSource.filteredData = null;
              abp.ui.clearBusy();
            } else {
              abp.notify.success(response.result[0].error);
              this.dataSource.filteredData = null;
              abp.ui.clearBusy();
            }
          },
          (error) => {
            abp.ui.clearBusy();
          }
        );
  }



  markDirty() {
    this._appComponent.markGroupDirty(this.addEditFormGroup);
    return true;
  }

  Clear() {
    this.addEditFormGroup.controls['plantCodeFormCControl'].setValue(null);
    this.addEditFormGroup.controls['packingOrderFormControl'].setValue(null);
    this.dataSource.filter = null;
  }

  async getDetails(event, plantCode) {
    abp.ui.setBusy();
    if (this.packingOrder != '0' && this.packingOrder != undefined) {
      await this._apiservice.GetQualityConfirmationDetails(plantCode, event)
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
        abp.ui.clearBusy();
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
    this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
    abp.ui.clearBusy();
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
}

