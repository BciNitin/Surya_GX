import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ViewChildren, OnInit, NgModule, ViewChild, QueryList, AfterViewInit } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';

import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator, MyErrorStateMatcher } from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';

interface grid {
  id: number;
  dealerCode: string;
  materialCode: string;
  materialName: string;
  qty: number;
  parentBarcode: string;
  childBarcode: string;
  packingDate: any;
  expiryDate: any;
}

@Component({
  selector: 'app-approval-for-zonal-manager',
  templateUrl: './approval-for-zonal-manager.component.html',
  styleUrls: ['./approval-for-zonal-manager.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class ApprovalForZonalManagerComponent implements OnInit, AfterViewInit {

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
 dealerCode : any;
 itemCode : any;
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
    private _changePwdService: ChangePswdServiceProxy,
    private _router: Router,
    private _route: ActivatedRoute,
  ) { }

  ngOnInit() {
    abp.ui.setBusy();
    this.getDetails()
  }

  isAtLeastOneCheckboxSelected(): boolean {
    return this.dataSource.filteredData.some(obj => obj.checkBoxValue);
  }

  addEditFormGroup: FormGroup = this.formBuilder.group({
    plantCodeFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    packingOrderFormControl: [null, [Validators.required, NoWhitespaceValidator]],
  });

  matcher = new MyErrorStateMatcher();

  async Save() {
    abp.ui.setBusy();
   
    for (let i = 0; i < this.dataSource.filteredData.length; i++) {
      if (this.dataSource.filteredData[i].checkBoxValue) {
         delete this.dataSource.filteredData[i].checkBoxValue;
         this.dataSourceModel.filteredData[i] = this.dataSource.filteredData[i];
      }
    }
       const filteredData = this.dataSourceModel.filteredData.filter(item => item !== null && item !== undefined);
       await this._apiservice.saveQualityConfirmation(filteredData).subscribe(
          (response) => {
            if (response.result[0].valid) {
              console.log(response)
              abp.notify.success(response.result[0].valid);
              this.getDetails();
              abp.ui.clearBusy();
            } else {
              abp.notify.error(response.result[0].error);
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

  async getDetails() {
    abp.ui.setBusy();
      await this._apiservice.GetDelarLocationApproveDetails()
        .subscribe((response) => {
        if(response['result'].length !== 0){
          console.log(response['result'])
        if (Array.isArray(response['result']) && response['result'].length > 0) {
          const firstResult = response['result'][0];
         
          if (typeof firstResult === 'object' && 'error' in firstResult) {
            abp.notify.error(response['result'][0].error);
            abp.ui.clearBusy();
          } else {
               this.dataSourcePagination = new MatTableDataSource<Element>(response['result']);
                this.dataSourcePagination.paginator = this.paginator;
                this.array = response['result'];
                this.totalSize = this.array.length;
                this.iterator();
                abp.ui.clearBusy();
          }
        } else {
          this.dataSource.data = [];
          abp.notify.error(response['result'][0].error);
          abp.ui.clearBusy();
        }
      }
      else
      {
        abp.ui.clearBusy();
      }
    },
      (error) => {
        // Handle HTTP error
        abp.ui.clearBusy();
      }
    
  )
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

 async GoToView(dealerCode,itemcode) {

   await this._changePwdService.encryptPassword(dealerCode).subscribe(
    response => { 
      if(response !== '' && response !== 'undefined' && response !== undefined)
      {
        this.dealerCode = response;
      }
      },
      error => {
        console.error('Error:', error);
        // Handle the error, if any
      }
    );

    await this._changePwdService.encryptPassword(itemcode).subscribe(
      response => { 
        if(response !== '' && response !== 'undefined' && response !== undefined)
        {
          this.itemCode = response;
        }
        },
        error => {
          console.error('Error:', error);
          // Handle the error, if any
        }
      );
      if(this.dealerCode !== '' && this.dealerCode !== 'undefined' && this.dealerCode !== undefined)
      {
       this._router.navigate(['../add-edit-zonal', 'view',  this.dealerCode+'/'+this.itemCode], { relativeTo: this._route });
      }
  }

  onSelect($event, approvalData: ''): void {
    //this.GoToView(approvalData);
  }
}


