import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ViewChildren, OnInit, NgModule, ViewChild,QueryList, AfterViewInit } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator ,MyErrorStateMatcher} from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
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
  this._apiservice.GetSOchallanNo().subscribe((modeSelectList: SelectListDto[]) => {
      this.challanNo = modeSelectList["result"];
  });
};
GrtTableGrid()
{
  var _customerDealer =  new customerDealer();
  
  _customerDealer.DeliveryChallanNo=this.DeliveryChallanNo;
  this._apiservice.GetSOChallanDetails(this.DeliveryChallanNo).subscribe((response) => {
    this.challanDtls = response["result"];
    
  })
}
ValidateCartonBarcode() {
  debugger;

  var _customerDealer =  new customerDealer();
  
  _customerDealer.DeliveryChallanNo=this.DeliveryChallanNo;
  _customerDealer.CartonBarcode=this.CartonBarcode;
  if(this.DeliveryChallanNo=="" || this.DeliveryChallanNo==null)
  {
    abp.notify.error("Please select delivery challan !");
    
  }
  else
  {
    
this._apiservice.GetValidateSOScanCartonBarcode(this.DeliveryChallanNo,this.CartonBarcode).subscribe(result => {
    debugger;
    if(result["result"][0]['valid'])
    {
      abp.notify.success(result["result"][0]['valid']);
    }
    else
    {
     abp.notify.error(result["result"][0]['error']);
    }
    this.GrtTableGrid();  
      });
      
}

}
}
