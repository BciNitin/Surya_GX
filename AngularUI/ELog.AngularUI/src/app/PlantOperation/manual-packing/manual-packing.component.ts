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
import { debug } from 'console';
import { toInt } from '@rxweb/reactive-form-validators';

 
  
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
  lineOrderList:any;
  lineList:any;
  ipAddress:string; 
  messageSplit: string[];
  message: string;

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent : ValidationService,
    private titleService: Title

    
    
  ) { }

  ngOnInit() {
    this.titleService.setTitle('Manual Packing');
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
onChangeLineCode(value)
{
if(value != '' && value != undefined)
{
 this._apiservice.getManualPackingPackingOrderNo(this.plantCode,value).subscribe((response) => {
  this.packingOrderList = response["result"];
  
});
}
this.packingOrderList=null;
}
GrtTableGrid()
{
 
  var _Storage =  new manualPack();
  _Storage.plantcode=this.plantCode;
  _Storage.packingorder=this.packingOrder;
  _Storage.linecode=this.linecode;
  this._apiservice.GetManualPackingDtls(this.plantCode,this.packingOrder,this.linecode).subscribe((response) => {
    this.manualPackingdtls = response["result"];
    this.materialCode=response["result"][0]['materialCode'];
    this.packSize=response["result"][0]['packSize'];
    this.count=response["result"][0]['count'];
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
    abp.ui.setBusy();
    this._apiservice.ValidateBarcode(this.BinBarCode, this.macAddresses, this.plantCode, this.packingOrder, this.linecode, this.materialCode).subscribe(result => {

      if (result["result"][0]['valid']) {
        this.message = result["result"][0]['valid'];
        this.messageSplit = this.message.split('~');
        this.count = this.messageSplit[1];
        //this.tCount = this.messageSplit[2];
        if(this.tCount == null && this.tCount == undefined)
        {
          this.tCount = parseInt(this.messageSplit[2]);
        }
        else{
          this.tCount = parseInt(this.tCount) + 1;
        }
        //  abp.notify.success(this.messageSplit[0]+' '+ this.messageSplit[3]);
        abp.notify.success(this.messageSplit[0]);
        this.BinBarCode = null;
        abp.ui.clearBusy();
        if (this.packSize == this.count) {
          this.Clear();
        }
      }
      else {
        abp.notify.error(result["result"][0]['error']);
        this.BinBarCode = null;
        abp.ui.clearBusy();
      }

    });

  }
onChangePlantCode() {

  if (this.plantCode !== '') {
    abp.ui.setBusy();
    this._apiservice.getLineCodeasPerPlant(this.plantCode).subscribe(
      (response) => {
        
          this.lineOrderList = response["result"];
          
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
}
Clear() {
        
  this.addEditFormGroup.reset();
}

getIP()  
  {  
   
    this._apiservice.getIPAddress().subscribe(result => {
             
             this.macAddresses=result["result"];
        });
    
  } 
  }  



