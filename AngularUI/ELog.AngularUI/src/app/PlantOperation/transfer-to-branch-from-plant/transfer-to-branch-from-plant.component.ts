import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ViewChildren, OnInit, NgModule, ViewChild,QueryList, AfterViewInit } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator ,MyErrorStateMatcher} from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
export class transferToBranch
{
     plantCode: string="";
     DeliveryChallanNo:string="";
     ShiperBarcode:string="";
     CartonBarcode:string="";
}
@Component({
  selector: 'app-transfer-to-branch-from-plant',
  templateUrl: './transfer-to-branch-from-plant.component.html',
  styleUrls: ['./transfer-to-branch-from-plant.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class TransferToBranchFromPlantComponent implements OnInit {
  challanNo:any;
  challanDtls:any;
  DeliveryChallanNo:string="";
  CartonBarcode:string="";
  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent : ValidationService
    ) { }

  ngOnInit() {
    this.GetChallanNo();
  }
  addEditFormGroup: FormGroup = this.formBuilder.group({
    
    ShiperBarcodeFormControl: ['',[Validators.required,NoWhitespaceValidator]],
    plantCodeFormCControl: ['',[Validators.required,NoWhitespaceValidator]]
});

GetChallanNo() {
  this._apiservice.GetchallanNo().subscribe((modeSelectList: SelectListDto[]) => {
      this.challanNo = modeSelectList["result"];
  });
};


GrtTableGrid()
{
  var _grnConfirm =  new transferToBranch();
  _grnConfirm.DeliveryChallanNo=this.DeliveryChallanNo;
  this._apiservice.GetChallanDetails(this.DeliveryChallanNo).subscribe((response) => {
    this.challanDtls = response["result"];
    
  })
}
ValidateCartonBarcode() {
  var _transferToBranch =  new transferToBranch();
  
  _transferToBranch.DeliveryChallanNo=this.DeliveryChallanNo;
  _transferToBranch.CartonBarcode=this.CartonBarcode;
  if(this.DeliveryChallanNo=="" || this.DeliveryChallanNo==null)
  {
    abp.notify.error("Please select delivery challan !");
    this.Clear();
  }
  else
  {
    
this._apiservice.GetValidateScanCartonBarcode(this.DeliveryChallanNo,this.CartonBarcode).subscribe(result => {
    
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
this.GrtTableGrid();
}

Clear(){
        
  this.addEditFormGroup.reset();
}
}
