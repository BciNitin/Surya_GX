import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ViewChildren, OnInit, NgModule, ViewChild, QueryList, AfterViewInit } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {  ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';

import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator, MyErrorStateMatcher } from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
export class DealerRevalidation {

  QRBarcode: string = "";
  id:any;
  
}
@Component({
  selector: 'app-approval-for-zonal-manager',
  templateUrl: './approval-for-zonal-manager.component.html',
  styleUrls: ['./approval-for-zonal-manager.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class ApprovalForZonalManagerComponent implements OnInit {
approvalDetails:any;
  constructor(
    private _apiservice: ApiServiceService,
    private _changePwdService: ChangePswdServiceProxy,
    private formBuilder: FormBuilder,
    private _router: Router,
    private _route: ActivatedRoute,
    public _appComponent: ValidationService
  ) { }
  ngOnInit() {
    this.GetApproveDetails();
  }
  public dataSource: MatTableDataSource<any> = new MatTableDataSource<DealerRevalidation>();
  GetApproveDetails() {
    debugger;
    this._apiservice.GetApproveDetails().subscribe((response) => {
      this.dataSource.filteredData=response["result"];
      })
 
}



GoToView(approvalData: any) {
debugger;
  this._changePwdService.encryptPassword(approvalData.id).subscribe(
      data => {

          this._router.navigate(['../add-edit-zonal', 'view', data], { relativeTo: this._route });

      }
  );

 
}
onSelect($event,approvalData: DealerRevalidation): void
{
   
      this.GoToView(approvalData);
   
   
}
}
