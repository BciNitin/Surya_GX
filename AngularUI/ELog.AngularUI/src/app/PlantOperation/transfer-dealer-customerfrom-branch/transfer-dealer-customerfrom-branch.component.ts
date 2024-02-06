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

export class customerDealer
{
    
     plantCode: string="";
     DeliveryChallanNo:string="";
     ShiperBarcode:string="";
     CartonBarcode:string="";
}
@Component({
  selector: 'app-transfer-dealer-customerfrom-branch',
  templateUrl: './transfer-dealer-customerfrom-branch.component.html',
  styleUrls: ['./transfer-dealer-customerfrom-branch.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class TransferDealerCustomerfromBranchComponent implements OnInit {
  challanNo:any;
  challanDtls:any;
  DeliveryChallanNo:string="";
     ShiperBarcode:string="";
     CartonBarcode:string="";
     materialList:any;
     MaterialCode:any;
     RecievedQty:any;
     SONo:any;
     PlantCode:any;
     CustomerCode:any;
     quantity:any;

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent : ValidationService,
    private titleService: Title

    ) { }

  ngOnInit() {
    this.titleService.setTitle('Transfer To Dealer/Customer From Branch');
    this.GetChallanNo();
    this.addEditFormGroup.controls['FormControlscanedQty'].disable();
    this.addEditFormGroup.controls['FormControlQuantity'].disable();
  }

  addEditFormGroup: FormGroup = this.formBuilder.group({
    
    ShiperBarcodeFormControl: ['',[Validators.required,NoWhitespaceValidator]],
    plantCodeFormCControl: ['',[Validators.required,NoWhitespaceValidator]],
    MaterialCodeFormCControl:[null],
    FormControlscanedQty:[null],
    FormControlQuantity:[null],
});

GetChallanNo() {
  this._apiservice.GetSOchallanNo().subscribe((modeSelectList: SelectListDto[]) => {
      this.challanNo = modeSelectList["result"];
  });
};

GrtTableGrid()
{
 if(this.DeliveryChallanNo != undefined && this.MaterialCode != undefined)
 {
  this._apiservice.GetDealerTransferSOChallanDetails(this.DeliveryChallanNo,this.MaterialCode).subscribe((response) => {
    this.challanDtls = response["result"];
    this.SONo = this.challanDtls[0].soNo;
    this.PlantCode = this.challanDtls[0].plantBranchCode;
    this.CustomerCode = this.challanDtls[0].customerCode;
  });
}
}

GetMaterialCode()
{
  if(this.DeliveryChallanNo != '')
  {
   this._apiservice.GetSOMaterialCode(this.DeliveryChallanNo).subscribe((response) => {
     this.materialList = response["result"];
   });
  }
}

ValidateCartonBarcode() {
  if(this.DeliveryChallanNo=="" || this.DeliveryChallanNo==null)
  {
    abp.notify.error("Please select delivery challan !");
    
  }
  else
  {
    
   this._apiservice.GetValidateSOScanCartonBarcode(this.DeliveryChallanNo,this.CartonBarcode,this.MaterialCode,this.SONo,this.PlantCode,this.CustomerCode).subscribe(result => {
    if(result["result"][0]['valid'])
    {
      abp.notify.success(result["result"][0]['valid']);
      this.quantity = result["result"][0].quantity;
      this.RecievedQty = result["result"][0].dispatchQty;
    }
    else
    {
     abp.notify.error(result["result"][0]['error']);
    }
 });
      
}

}
}
