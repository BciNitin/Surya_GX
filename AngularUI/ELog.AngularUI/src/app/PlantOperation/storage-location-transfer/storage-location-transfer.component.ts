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
import { Router } from '@angular/router';

export class SgtorageLocation {
  plantcode: string = "";
  Barcode: string = "";
  FromLocation: string = "";
}
@Component({
  selector: 'app-storage-location-transfer',
  templateUrl: './storage-location-transfer.component.html',
  styleUrls: ['./storage-location-transfer.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class StorageLocationTransferComponent implements OnInit {
  plnatCodeList: any;
  storageLocation: any;
  storageLocationdtls: any;
  plantCode: string = "";
  FromLocation: string = "";
  MaterialCode: string = "";
  ToLocation:string ="";
  storageLocationQty: any;
  qty: any;
  ScanItem: string = "";
  materiallist:any;
  FromstorageLocation:any;
  scanningQty:any;

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent: ValidationService,
    private titleService: Title,
    private _router: Router,
  ) { }

  ngOnInit() {
    abp.ui.setBusy();
    this.titleService.setTitle('Storage Location Transfer');
    this.GetPlantCode();
    //  this.GetStorageCode();

  }
  addEditFormGroup: FormGroup = this.formBuilder.group({
    plantCodeFormCControl: [null, [Validators.required, NoWhitespaceValidator]],
    toStoragelocationFormControl: ['', [Validators.required, NoWhitespaceValidator]],
    scannedItemFormControl: [null],
    scannedqtyFormControl: [null],
    MaterialCodeFormControl:[null, [Validators.required, NoWhitespaceValidator]],
    RemainingQtyFormControl:[null],
    FromStoragelocationFormControl:[null, [Validators.required, NoWhitespaceValidator]]
  });

  GetPlantCode() {
    this._apiservice.getPlantCode().subscribe((modeSelectList: SelectListDto[]) => {
      this.plnatCodeList = modeSelectList["result"];
      abp.ui.clearBusy();
    });
  };

  GetToStorageLocationCode(plantCode) {
    abp.ui.setBusy();
    if (this.plantCode !== '' && this.plantCode !== undefined) {
      this._apiservice.GetStorageLocation(this.plantCode).subscribe((response) => {
        this.storageLocation = response["result"];
        abp.ui.clearBusy();
      });
    }
  };

  GrtTableGrid() {

    if (this.FromLocation !== '' && this.FromLocation !== undefined && this.plantCode !== '' && this.plantCode !== undefined && this.MaterialCode !== undefined && this.MaterialCode !== '' && this.ToLocation !=='') {
      abp.ui.setBusy();
      this._apiservice.GetStrLocationDtls(this.plantCode, this.FromLocation,this.MaterialCode,this.ToLocation).subscribe((response) => {
        if (response['result'][0].error) {
          abp.notify.error(response['result'][0].error);
          abp.ui.clearBusy();
        } else {
          this.storageLocationdtls = response["result"];
          this.scanningQty = response['result'][0].scanningQty;
          abp.ui.clearBusy();

        }
      })
    }
  }
  GetBarcodeScannedDetails() {
    if (this.ScanItem !== '' && this.plantCode !== '' && this.MaterialCode !== '' && this.ToLocation !=='') {
      abp.ui.setBusy();
      this._apiservice.GetBarcodeScannedDetails(this.ScanItem, this.plantCode, this.FromLocation,this.ToLocation,this.MaterialCode).subscribe((response) => {
        if (response['result'][0].error) {
          abp.notify.error(response['result'][0].error);
          abp.ui.clearBusy();
        } else {
          this.storageLocationQty = response["result"];
          abp.notify.success(response['result'][0].valid);
          this.qty = response["result"][0]['qty'];
          this.GrtTableGrid();
          abp.ui.clearBusy();
        }

      })
    }
  }


  Confirm() {
    abp.ui.setBusy();
    if (this.FromLocation !== '' && this.ToLocation !=='') {
      this._apiservice.StorageLocationConfirmation(this.plantCode,this.ScanItem, this.FromLocation,this.MaterialCode,this.ToLocation).subscribe(result => {

        if (result["result"][0]['valid']) {
          abp.notify.success(result["result"][0]['valid']);
          this.storageLocationdtls = null;
          this.Clear();
          abp.ui.clearBusy();
          let currentUrl = this._router.url;
          this._router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
          this._router.navigate([currentUrl]);
         });
        }
        else {
          abp.notify.error(result["result"][0]['error']);
          abp.ui.clearBusy();
        }

      });
    }
  }
  markDirty() {
    this._appComponent.markGroupDirty(this.addEditFormGroup);
    return true;
  }
  Clear() {
    this.addEditFormGroup.reset({
      plantCodeFormCControl: null,
      toStoragelocationFormControl: '',
      scannedItemFormControl: null,
      scannedqtyFormControl: null,
      MaterialCodeFormControl:null,
      RemainingQtyFormControl:null,
      FromStoragelocationFormControl:null
    });
    this.storageLocationdtls = null;
  }

  GetMaterialCode(ToLocation) {
    abp.ui.setBusy();
    if (this.plantCode !== '' && this.FromLocation !== '' && ToLocation !== undefined) {
      this._apiservice.GetSTRMaterialCode(this.plantCode,this.FromLocation).subscribe((response) => {
        if (response['result'][0].error) {
          abp.notify.error(response['result'][0].error);
          abp.ui.clearBusy();
          this.materiallist=null;
        } else {
          this.materiallist = response["result"];
          abp.ui.clearBusy();
        }

      })
    }
  }

  GetFromStorageLocation(plantCode) {
    abp.ui.setBusy();
    
    if (this.plantCode !== '' && plantCode !== undefined) {
      this._apiservice.GetFromStorageLocation(this.plantCode).subscribe((response) => {
        if (response['result'][0].error) {
          abp.notify.error(response['result'][0].error);
          this.FromstorageLocation = null;
          this.GetToStorageLocationCode(this.plantCode)
          abp.ui.clearBusy();
        } else {
          this.FromstorageLocation = response["result"];
          abp.ui.clearBusy();
        }

      })
    }
    abp.ui.clearBusy();
  }
}
