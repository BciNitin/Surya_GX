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

export class SgtorageLocation {
  plantcode: string = "";
  Barcode: string = "";
  LocationID: string = "";
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
  LocationID: string = "";
  MaterialCode: string = "";
  storageLocationQty: any;
  qty: any;
  ScanItem: string = "";
  materiallist:any;

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent: ValidationService,
    private titleService: Title


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
    scannedItemFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    scannedqtyFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    MaterialCodeFormControl:[null, [Validators.required, NoWhitespaceValidator]]
  });

  GetPlantCode() {
    this._apiservice.getPlantCode().subscribe((modeSelectList: SelectListDto[]) => {
      this.plnatCodeList = modeSelectList["result"];
      abp.ui.clearBusy();
    });
  };

  GetStorageCode(plantCode) {
    abp.ui.setBusy();
    if (plantCode !== '' && plantCode !== '') {
      this._apiservice.GetStorageLocation(plantCode).subscribe((response) => {
        this.storageLocation = response["result"];
        abp.ui.clearBusy();
      });
    }
  };

  GrtTableGrid() {
    debugger
    if (this.plantCode !== '' && this.LocationID !== '' && this.plantCode !== undefined && this.LocationID !== undefined) {
      abp.ui.setBusy();
      this._apiservice.GetStrLocationDtls(this.plantCode, this.LocationID,this.MaterialCode).subscribe((response) => {
        if (response['result'][0].error) {
          abp.notify.error(response['result'][0].error);
          abp.ui.clearBusy();
        } else {
          this.storageLocationdtls = response["result"];
          abp.ui.clearBusy();

        }
      })
    }
  }
  GetBarcodeScannedDetails() {
    if (this.ScanItem !== '' && this.plantCode !== '' && this.MaterialCode !== '') {
      abp.ui.setBusy();
      this._apiservice.GetBarcodeScannedDetails(this.ScanItem, this.plantCode, this.LocationID,this.MaterialCode).subscribe((response) => {
        if (response['result'][0].error) {
          abp.notify.error(response['result'][0].error);
          abp.ui.clearBusy();
        } else {
          this.storageLocationQty = response["result"];
          this.qty = response["result"][0]['qty'];
          abp.ui.clearBusy();
        }

      })
    }
  }


  Save() {
    abp.ui.setBusy();
    if (this.ScanItem !== '' && this.LocationID !== '') {
      this._apiservice.StorageLocationConfirmation(this.ScanItem, this.LocationID,this.MaterialCode).subscribe(result => {

        if (result["result"][0]['valid']) {
          abp.notify.success(result["result"][0]['valid']);
          this.storageLocationdtls = null;
          this.Clear();
          abp.ui.clearBusy();
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
    // this.addEditFormGroup.get('plantCodeFormCControl').setValue(null);
    // this.addEditFormGroup.get('toStoragelocationFormControl').setValue('');
    // this.addEditFormGroup.get('scannedItemFormControl').setValue(null);
    // this.addEditFormGroup.get('scannedqtyFormControl').setValue(null);
    this.addEditFormGroup.reset({
      plantCodeFormCControl: null,
      toStoragelocationFormControl: '',
      scannedItemFormControl: null,
      scannedqtyFormControl: null,
      MaterialCodeFormControl:null
    });
    this.storageLocationdtls = null;
  }

  GetMaterialCode(plantCode) {
    abp.ui.setBusy();
    if (this.plantCode !== '') {
      this._apiservice.GetSTRMaterialCode(plantCode).subscribe((response) => {
        if (response['result'][0].error) {
          abp.notify.error(response['result'][0].error);
          abp.ui.clearBusy();
        } else {
          this.materiallist = response["result"];
          abp.ui.clearBusy();
        }

      })
    }
  }
}
