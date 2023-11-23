import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ViewChildren, OnInit, NgModule, ViewChild, QueryList, AfterViewInit } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator, MyErrorStateMatcher } from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
import { forkJoin } from 'rxjs';

interface grid {
  dealarCode: string,
  materialCode: string,
  materialName: string,
  qty: number,
  parentBarcode: string,
  childBarcode: string,
  packingDate: any,
  expiryDate: any,
  checkBoxValue: false;
}

@Component({
  selector: 'app-add-edit-zonal',
  templateUrl: './add-edit-zonal.component.html',
  styleUrls: ['./add-edit-zonal.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class AddEditZonalComponent implements OnInit {
  approvalId: any;
  routeEncrypt: any;
  ApprovalDtls: any;

  searchText;
  searchTerm = '';
  p: Number = 1;
  public array: any;

  public pageSize = 10;
  public currentPage = 0;
  public totalSize = 0;

  itemcode: any;
  dealerCode; any;
  splitparts: any;

  public dataSource: MatTableDataSource<any> = new MatTableDataSource<grid>();
  public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<grid>();
  public dataSourceModel: MatTableDataSource<any> = new MatTableDataSource<grid>();
  @ViewChild(MatSort, { static: false }) sort!: MatSort;
  @ViewChild('paginator', { static: true }) paginator: MatPaginator;

  constructor(
    private _apiservice: ApiServiceService,
    private _changePwdService: ChangePswdServiceProxy,
    private formBuilder: FormBuilder,
    private _router: Router,
    private _route: ActivatedRoute,
    public _appComponent: ValidationService
  ) { }

  ngOnInit() {
    this.urlEncription();
  }

  isAtLeastOneCheckboxSelected(): boolean {
    return this.dataSource.filteredData.some(obj => obj.checkBoxValue);
  }

  urlEncription() {
    this.routeEncrypt = null;
    let that = this;
    this._route.params.subscribe((routeData: Params) => {
      const approvalId = routeData['approvalId'];
      this.splitparts = approvalId.split('/');

      if (this.splitparts) {
        const paramId = this._changePwdService.decryptPassword(this.splitparts[0]);
        const paramId2 = this._changePwdService.decryptPassword(this.splitparts[1]);

        forkJoin([paramId, paramId2]).subscribe(responses => {
          const response1 = responses[0];
          const response2 = responses[1];

          if (response1 !== '' && response1 !== 'undefined' && response1 !== undefined) {
            that.dealerCode = response1;
          }

          if (response2 !== '' && response2 !== 'undefined' && response2 !== undefined) {
            that.itemcode = response2;
          }
          if (that.dealerCode !== '' && that.dealerCode !== 'undefined' && that.dealerCode !== undefined) {
            if (that.itemcode !== '' && that.itemcode !== 'undefined' && that.itemcode !== undefined) {
              this.getArray(that.dealerCode, that.itemcode);
            }
          }
        });
      }
    });
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

  private getArray(dealercode, itemcode) {
    this._apiservice.GetApprovalDtlsById(dealercode, itemcode)
      .subscribe((response) => {
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

  Confirm(itembarCode) {

    const childBarcodesWithTrueCheckBox = this.dataSource.filteredData
      .filter(item => item.checkBoxValue === true)
      .map(item => item.childBarcode);

    if (childBarcodesWithTrueCheckBox) {
      this._apiservice.ConfirmRevalidation(childBarcodesWithTrueCheckBox)
        .subscribe((response) => {
          if (Array.isArray(response['result']) && response['result'].length > 0) {
            const firstResult = response['result'][0];
            if (typeof firstResult === 'object' && 'error' in firstResult) {
              abp.notify.error(response['result'][0].error);
            } else {
              abp.notify.success(response['result'][0].valid);
            }
          } else {
            abp.notify.error(response['result'][0].error);
          }

        },
          (error => {
            abp.ui.clearBusy();
          })
        )
    }
  }

  goBack(): void {
    
    this._router.navigate(['../../../approval-for-zonal-manager'], { relativeTo: this._route });
  }
}
