import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ViewChildren, OnInit, NgModule, ViewChild, QueryList, AfterViewInit } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator, MyErrorStateMatcher } from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
import { Title } from '@angular/platform-browser';
export class manualPack {
  maxWeight: string = "";
  plantCode: string = "";
  minWeight: string = "";




}
@Component({
  selector: 'app-material-tolerance',
  templateUrl: './material-tolerance.component.html',
  styleUrls: ['./material-tolerance.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class MaterialToleranceComponent implements OnInit {
  PackingOrder: any;
  MeterialCodeList: any;
  macAddresses: any;



  minWeight:  any;
  manualPackingdtls: any;
  plantCode: string = "";
  maxWeight: any;
  materialCode: any;
  packingOrder: string = "";
  ScanItem: string = "";
  lineOrderList: any;
  lineList: any;
  ipAddress: string;
  messageSplit: string[];
  message: string;

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent: ValidationService,
    private titleService: Title



  ) { }

  ngOnInit() {
    this.titleService.setTitle('Weight Tolerance');
    this.GetMetCode();



    this.addEditFormGroup.controls['materialCodeFormControl'].disable();


  }
  addEditFormGroup: FormGroup = this.formBuilder.group({
    plantCodeFormCControl: [null, [Validators.required, NoWhitespaceValidator]],
    maxWeightFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    materialCodeFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    minWeightFormControl: [null, [Validators.required, NoWhitespaceValidator]],

  });
  matcher = new MyErrorStateMatcher();
  GetMetCode() {
    this._apiservice.GetMaterialCode().subscribe((modeSelectList: SelectListDto[]) => {
      this.MeterialCodeList = modeSelectList["result"];
    });
  };



  markDirty() {
    this._appComponent.markGroupDirty(this.addEditFormGroup);
    return true;
  }
  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;

  }
  Save() {
    const maxWeightNum = parseFloat(this.maxWeight);
    const minWeightNum = parseFloat(this.minWeight);

    if (maxWeightNum > minWeightNum) {
      this._apiservice.MaterialToleranceSave(this.plantCode, this.minWeight, this.maxWeight).subscribe(result => {

        if (result["result"][0]['valid']) {

          abp.notify.success(result["result"][0]['valid']);
          this.Clear();

        }
        else {
          abp.notify.error(result["result"][0]['error']);

        }

      });
    }
    else {
      abp.notify.error("Max Weight should be greater than of Min Weight");

    }


  }
  onChangeMaterialCode() {
    if (this.plantCode !== '' && this.plantCode != undefined) {
      abp.ui.setBusy();
      this._apiservice.GetItemDesc(this.plantCode).subscribe(
        (response) => {

          this.lineOrderList = response["result"];
          this.materialCode = response["result"][0]['materialDescription'];
          abp.ui.clearBusy();


        },
        (error) => {
          // Handle the error here
          console.error('An error occurred:', error);
          // Optionally, display an error message to the user
          // You can use a notification service or display the error in the UI
          abp.notify.error('An error occurred while fetching data.');
          abp.ui.clearBusy();
        }
      );
    }
    else {
      this.materialCode = '';
    }
  }
  Clear() {

    this.addEditFormGroup.reset();
  }


}

