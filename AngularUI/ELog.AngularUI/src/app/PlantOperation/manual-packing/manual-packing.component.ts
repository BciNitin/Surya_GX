import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ViewChildren, OnInit, NgModule, ViewChild,QueryList, AfterViewInit } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator ,MyErrorStateMatcher} from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
 
 
  
export class manualPack
{
     plantcode: string="";
     linecode:string="";
     packingorder:string="";
     BinBarCode:string="";
     macAddresses:string="";

     
    
}
@Component({
  selector: 'app-manual-packing',
  templateUrl: './manual-packing.component.html',
  styleUrls: ['./manual-packing.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})

export class ManualPackingComponent implements OnInit {
  PackingOrder: any;
  plnatCodeList: any;
  macAddresses:any;
  materialCode:any;
  packSize:any;
  count:any;
  tCount:any;
  packingOrderList:any;
  picklistItems:any;
  BinBarCode:string="";
  manualPackingdtls: any;
  plantCode : string="";
  linecode:string="";
  packingOrder: string="";
  ScanItem: string="";
  lineList:any;
  ipAddress:string; 
   
  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent : ValidationService
    
    
  ) { }

  ngOnInit() {
    this.GetPlantCode();
    this.GetLineCode();
    this.getIP(); 
    //this.markControlDisabled(this.addEditFormGroup.controls['zoneCodeFormControl'])
    this.addEditFormGroup.controls['materialCodeFormControl'].disable();
    this.addEditFormGroup.controls['packSizeFormControl'].disable();
    this.addEditFormGroup.controls['CountFormControl'].disable();
    this.addEditFormGroup.controls['TotalCountFormControl'].disable();
  }
  addEditFormGroup: FormGroup = this.formBuilder.group({
    plantCodeFormCControl: [null, [Validators.required, NoWhitespaceValidator]],
    packingOrderFormControl: ['',[Validators.required,NoWhitespaceValidator]],
    scannedItemFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    LineCodeFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    materialCodeFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    packSizeFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    CountFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    TotalCountFormControl: [null, [Validators.required, NoWhitespaceValidator]]
});
matcher = new MyErrorStateMatcher();
GetPlantCode() {
  this._apiservice.getPlantCode().subscribe((modeSelectList: SelectListDto[]) => {
      this.plnatCodeList = modeSelectList["result"];
  });
};
onChangePlantCode(value)
{
 debugger;
 this._apiservice.getPackingOrderNo(value).subscribe((response) => {
  this.packingOrderList = response["result"];
  
});
}
GrtTableGrid()
{
  debugger;
  var _Storage =  new manualPack();
  _Storage.plantcode=this.plantCode;
  _Storage.packingorder=this.packingOrder;
  _Storage.linecode=this.linecode;
  this._apiservice.GetManualPackingDtls(this.plantCode,this.packingOrder,this.linecode).subscribe((response) => {
    debugger;
    this.manualPackingdtls = response["result"];
    this.materialCode=response["result"][0]['materialCode'];
    this.packSize=response["result"][0]['packSize'];
    this.count=response["result"][0]['count'];
    this.tCount=response["result"][0]['tCount'];
    // if(response["result"][0]['valid'])
    //        {
    //          abp.notify.success(response["result"][0]['valid']);
             
    //        }
    //        else
    //        {
    //         abp.notify.error(response["result"][0]['error']);
    //        }

})
}
GetLineCode() {
  this._apiservice.getLineWorkCenterNo().subscribe((response) => {
      this.lineList = response["result"];
  });
};
markDirty() {
  this._appComponent.markGroupDirty(this.addEditFormGroup);
   return true;
}

Save() {
  debugger;

  var _Storage =  new manualPack();
  _Storage.BinBarCode = this.BinBarCode;
  _Storage.macAddresses=this.macAddresses;
  _Storage.plantcode=this.plantCode;
  _Storage.packingorder=this.packingOrder;
  _Storage.linecode=this.linecode;
 this._apiservice.ValidateBarcode(this.BinBarCode,this.macAddresses,this.plantCode,this.packingOrder,this.linecode).subscribe(result => {
    debugger;
           if(result["result"][0]['valid'])
           {
             abp.notify.success(result["result"][0]['valid']);
             
           }
           else
           {
            abp.notify.error(result["result"][0]['error']);
           }
           
      });
  
}

Clear() {
        
  this.addEditFormGroup.reset();
}

getIP()  
  {  
    debugger;
    this._apiservice.getIPAddress().subscribe(result => {
      debugger;
             
             this.macAddresses=result["result"];
        });
    
  } 
  }  



