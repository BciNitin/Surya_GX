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
  plantcode: any;
  fromDate: Date;
  toDate: Date;
   materialCode:any;
   linecode:any;
   packingOrder:any;
}

@Component({
  selector: 'app-packing-reports',
  templateUrl: './packing-reports.component.html',
  styleUrls: ['./packing-reports.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class PackingReportsComponent implements OnInit {
  
  fromDate: Date;
  toDate: Date;
  plantcode: any;
  plnaCodeList:any;
  ItemCodes:any;
  lineList:any;
  PackingOrder:any;
  MaterialCode:any;
   linecode:any;
   packingOrder:any;

   

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent: ValidationService,
    private titleService: Title,
    private datePipe: DatePipe

  ) { }

  public dataSource: MatTableDataSource<any> = new MatTableDataSource<grid>();

  ngOnInit() {
    this.titleService.setTitle('Packing Report');
    // this.fromDate = moment().subtract(7, 'day');
    
    // this.toDate = moment().add(0, 'day');
    this.GetPlantCode();
    this.GetItemCodes();
    this.GetLineCode();
    this.GetPackingReportOrderNo();
  }
  addEditFormGroup: FormGroup = this.formBuilder.group({
    plantCodeFormCControl: [null, [Validators.required, NoWhitespaceValidator]],
    packingOrderFormControl: ['', [Validators.required, NoWhitespaceValidator]],
    ItemCodeFormCControl: ['', [Validators.required, NoWhitespaceValidator]],
    packingOrderNoFormCControl:['', [Validators.required, NoWhitespaceValidator]],
    LineCodeFormControl:['', [Validators.required, NoWhitespaceValidator]],
    supportExpiresOnFormControl:['', [Validators.required, NoWhitespaceValidator]],
    dateOfInstallationFormControl:['', [Validators.required, NoWhitespaceValidator]]

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
        this.PackingOrder = response["result"];
    });
  };

  GetPackingReport() {
debugger
   const FromdDate = this.datePipe.transform(this.fromDate, 'dd MMM yyyy');
   const ToDate = this.datePipe.transform(this.toDate, 'dd MMM yyyy');
      this.dataSource.filteredData['plantcode'] = this.plantcode;
      this.dataSource.filteredData['fromDate'] = FromdDate;
      this.dataSource.filteredData['toDate'] = ToDate;
      this.dataSource.filteredData['MaterialCode'] = this.MaterialCode;
      this.dataSource.filteredData['linecode'] = this.linecode;
      this.dataSource.filteredData['packingOrder'] = this.packingOrder;

     console.log(this.dataSource.filteredData);
     this._apiservice.GetPackingReport(this.dataSource.filteredData).subscribe(result => {
      
      if(result["result"][0]['valid'])
      {
        abp.notify.success(result["result"][0]['valid']);
      }
      else
      {
       abp.notify.error(result["result"][0]['error']);
      }
             
        },
        (error=>
          {
            console.log(error)
          }));
    
    }
}
