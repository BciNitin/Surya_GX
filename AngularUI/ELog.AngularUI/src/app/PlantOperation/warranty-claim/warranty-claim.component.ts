import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ViewChildren, OnInit, NgModule, ViewChild, QueryList, AfterViewInit } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator, MyErrorStateMatcher } from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
export class warranty {

  Barcode: string = "";
  CustomerCode: string = "";
}
@Component({
  selector: 'app-warranty-claim',
  templateUrl: './warranty-claim.component.html',
  styleUrls: ['./warranty-claim.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class WarrantyClaimComponent implements OnInit {
  warrantyDtls: any;
  CustomerCode: string = "";
  Barcode: string = "";
  customerCodelist: any;


  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent: ValidationService
  ) { }

  ngOnInit() {
    this.GetCustomerCode();
  }

  public dataSource: MatTableDataSource<any> = new MatTableDataSource<warranty>();

  addEditFormGroup: FormGroup = this.formBuilder.group({

    ShiperBarcodeFormControl: ['', [Validators.required, NoWhitespaceValidator]],
    plantCodeFormCControl: ['', [Validators.required, NoWhitespaceValidator]]
  });

  GetCustomerCode() {
    this._apiservice.GetCustomerCode().subscribe((modeSelectList: SelectListDto[]) => {
      this.customerCodelist = modeSelectList["result"];
    });
  };

  GrtTableGrid() {
    if (this.CustomerCode == "" || this.CustomerCode == null) {
      abp.notify.error("Please select customer code !");
      this.Clear();
    }
    else {
      debugger;
      var _warranty = new warranty();
      _warranty.Barcode = this.Barcode;
      _warranty.CustomerCode = this.CustomerCode;
      this._apiservice.GetWarrantyDetails(this.Barcode, this.CustomerCode).subscribe((response) => {
        //this.warrantyDtls = response["result"];
        if (response["result"][0]['error']) {
          abp.notify.error(response["result"][0]['error']);
          this.Clear();

        }
        else {
          //this.warrantyDtls = response["result"];
          this.dataSource.filteredData=response["result"];
        }

      })
    }
  }


  Save() {

    var _warranty = new warranty();
    _warranty.Barcode = this.Barcode;
    _warranty.CustomerCode = this.CustomerCode;
    this._apiservice.GetValidateWarrranty(this.Barcode, this.CustomerCode).subscribe(result => {
      debugger;
      if (result["result"][0]['valid']) {
        abp.notify.success(result["result"][0]['valid']);
      }
      else {
        abp.notify.error(result["result"][0]['error']);
      }

    });
  }

  markDirty() {
    this._appComponent.markGroupDirty(this.addEditFormGroup);
    return true;
  }

  Clear() {

    this.addEditFormGroup.reset();
    this.dataSource.filteredData=null;
  }
}
