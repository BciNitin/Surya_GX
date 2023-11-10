import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ViewChildren, OnInit, NgModule, ViewChild,QueryList, AfterViewInit } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator ,MyErrorStateMatcher} from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
import { Title } from '@angular/platform-browser';

export class SgtorageLocation
{
    
     
     plantcode: string="";
     Barcode:string="";
     LocationID:string="";
     
    
}
@Component({
  selector: 'app-storage-location-transfer',
  templateUrl: './storage-location-transfer.component.html',
  styleUrls: ['./storage-location-transfer.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class StorageLocationTransferComponent implements OnInit {
  plnatCodeList:any;
  storageLocation:any;
  storageLocationdtls: any;
  plantCode : string="";
  LocationID:string="";
     
  storageLocationQty:any;
  qty:any;
  ScanItem: string="";
  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent : ValidationService,
    private titleService: Title

    
  ) { }

  ngOnInit() {
    this.titleService.setTitle('Storage Location Transfer');
    this.GetPlantCode();
    this.GetStorageCode();
    
  }
  addEditFormGroup: FormGroup = this.formBuilder.group({
    plantCodeFormCControl: [null, [Validators.required, NoWhitespaceValidator]],
   
    toStoragelocationFormControl: ['',[Validators.required,NoWhitespaceValidator]],
    scannedItemFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    scannedqtyFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    
    
});

GetPlantCode() {
  this._apiservice.getPlantCode().subscribe((modeSelectList: SelectListDto[]) => {
      this.plnatCodeList = modeSelectList["result"];
  });
};

GetStorageCode() {
  this._apiservice.GetStorageLocation().subscribe((response) => {
      this.storageLocation = response["result"];
  });
};

GrtTableGrid()
{
  var _Storage =  new SgtorageLocation();
  _Storage.plantcode=this.plantCode;
  _Storage.LocationID=this.LocationID;
  this._apiservice.GetStrLocationDtls(this.plantCode,this.LocationID).subscribe((response) => {
    
    if (response['result'][0].error) {
      abp.notify.error(response['result'][0].error);
    } else {
      this.storageLocationdtls = response["result"];
      
      
    }
})
}
GetBarcodeScannedDetails()
{
  var _Storage =  new SgtorageLocation();

  _Storage.Barcode=this.ScanItem;
  _Storage.plantcode=this.plantCode;
  _Storage.LocationID=this.LocationID;
  this._apiservice.GetBarcodeScannedDetails(this.ScanItem,this.plantCode).subscribe((response) => {
    

    if (response['result'][0].error) {
      abp.notify.error(response['result'][0].error);
    } else {
      this.storageLocationQty = response["result"];
    this.qty=response["result"][0]['qty'];
      
      
    }
    
})
}


Save() {
  
  var _Storage =  new SgtorageLocation();
  _Storage.Barcode=this.ScanItem;
  _Storage.LocationID=this.LocationID;
  this._apiservice.StorageLocationConfirmation(this.ScanItem,this.LocationID).subscribe(result => {
    
           if(result["result"][0]['valid'])
           {
             abp.notify.success(result["result"][0]['valid']);
             this.Clear();

           }
           else
           {
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
}
}
