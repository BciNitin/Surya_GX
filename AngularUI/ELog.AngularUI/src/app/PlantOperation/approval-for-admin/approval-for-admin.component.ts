import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ViewChildren, OnInit, NgModule, ViewChild, QueryList, AfterViewInit } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {  ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';

import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator, MyErrorStateMatcher } from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
interface grid
{
  id:number,
  dealarCode: string,
  materialCode: string,
  qty: number,
  
}
export class AdminRevalidation {

 
  id:any;
  
}
@Component({
  selector: 'app-approval-for-admin',
  templateUrl: './approval-for-admin.component.html',
  styleUrls: ['./approval-for-admin.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class ApprovalForAdminComponent implements OnInit {
  searchText;
  searchTerm = '';
  p: Number = 1;
  public array: any;

  public pageSize = 10;
  public currentPage = 0;
  public totalSize = 0;
  public dataSource: MatTableDataSource<any> = new MatTableDataSource<grid>();
  public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<grid>();
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
    this.getArray();
    //this.GetApproveDetails();
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

  private getArray() {
    this._apiservice.GetDelarLocationApproveDetails()
      .subscribe((response) => {
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



GoToView(approvalData: any) {
debugger;
  this._changePwdService.encryptPassword(approvalData.id).subscribe(
      data => {

          this._router.navigate(['../add-edit-admin', 'view', data], { relativeTo: this._route });

      }
  );

 
}
onSelect($event,approvalData: AdminRevalidation): void
{
   
      this.GoToView(approvalData);
   
   
}
}