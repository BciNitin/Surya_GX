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

export class GrnConfirm
{
     plantCode: string="";
     DeliveryChallanNo:string="";
     ShiperBarcode:string="";
     CartonBarcode:string="";
}
@Component({
  selector: 'app-grn-confirmation',
  templateUrl: './grn-confirmation.component.html',
  styleUrls: ['./grn-confirmation.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class GrnConfirmationComponent implements OnInit {
  challanNo:any;
  challanDtls:any;
  DeliveryChallanNo:string=null;
  CartonBarcode:string="";
  quantity:any="";
  scanedQty:any="";
  CheckNGOK:boolean=false;
  fromPlantCode:any;
  toPlantCode:any;

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent : ValidationService,
    private titleService: Title

    ) { }

  ngOnInit() {
    this.titleService.setTitle('GRN Confirmation');
    this.GetChallanNo();
    this.addEditFormGroup.controls['FormControlQuantity'].disable();
    this.addEditFormGroup.controls['FormControlscanedQty'].disable();
  }
  addEditFormGroup: FormGroup = this.formBuilder.group({
    
    ShiperBarcodeFormControl: ['',[Validators.required,NoWhitespaceValidator]],
    challanFormCControl: ['',[Validators.required,NoWhitespaceValidator]],
    FormControlQuantity:[null],
    FormControlscanedQty:[null],
    FormControlOKNG:[null]
});

GetChallanNo() {
  abp.ui.setBusy();
  this._apiservice.GetChallanNoForGRNConfirm().subscribe((modeSelectList: SelectListDto[]) => {
      this.challanNo = modeSelectList["result"];
      abp.ui.clearBusy();
  });
};

GrtTableGrid()
{
  abp.ui.setBusy();
  if ( this.DeliveryChallanNo !== undefined) {
  this._apiservice.GetSOChallanDetails(this.DeliveryChallanNo).subscribe((response) => {
    this.challanDtls = response["result"];
    console.log('res',response["result"])
    this.fromPlantCode = response["result"][0]['fromPlantCode']
     this.toPlantCode = response["result"][0]['toPlantCode']
     this.CartonBarcode = '';
     this.scanedQty ='';
     this.quantity = '';
     abp.ui.clearBusy();
  })
}
else{
  abp.ui.clearBusy();
}
}

GRNConfirmation() {

  if(this.DeliveryChallanNo=="" || this.DeliveryChallanNo==null)
  {
    abp.notify.error("Please select delivery challan.");
    this.quantity = '';
    this.scanedQty = '';
    this.CartonBarcode = '';
    this.Clear();
  }
  else
  {
    
   this._apiservice.GetValidateGRNConfirmation(this.DeliveryChallanNo,this.CartonBarcode,this.CheckNGOK,this.toPlantCode,this.fromPlantCode).subscribe(result => {
    
    if(result["result"][0]['valid'])
    { 
      abp.notify.success(result["result"][0]['valid']);
      this.quantity = result["result"][0]['quantity']
      this.scanedQty = result["result"][0]['scanedQty']
      
    }
    else
    {
     abp.notify.error(result["result"][0]['error']);
    }
         
      });
}
}

Clear(){
        
  this.addEditFormGroup.reset();
}
markDirty() {
  this._appComponent.markGroupDirty(this.addEditFormGroup);
   return true;
}

GRNValidation()
{

  if(this.DeliveryChallanNo=="" || this.DeliveryChallanNo==null && this.CartonBarcode !=undefined)
  {
    abp.notify.error("Please select delivery challan.");
    this.quantity = '';
    this.scanedQty = '';
    this.CartonBarcode = '';
    this.Clear();
  }
  else
  {
    
   this._apiservice.ConfirmGRN(this.DeliveryChallanNo,this.CartonBarcode,this.CheckNGOK,this.toPlantCode,this.fromPlantCode).subscribe(result => {
    
    if(result["result"][0]['valid'])
    { 
      abp.notify.success(result["result"][0]['valid']);
      this.quantity = result["result"][0]['quantity']
      this.scanedQty = result["result"][0]['scanedQty']
      
    }
    else
    {
     abp.notify.error(result["result"][0]['error']);
    }
         
      });
}
}

}
