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
  DeliveryChallanNo:string="";
  CartonBarcode:string="";
  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent : ValidationService,
    private titleService: Title

    ) { }

  ngOnInit() {
    this.titleService.setTitle('GRN Confirmation');
    this.GetChallanNo();
  }
  addEditFormGroup: FormGroup = this.formBuilder.group({
    
    ShiperBarcodeFormControl: ['',[Validators.required,NoWhitespaceValidator]],
    challanFormCControl: ['',[Validators.required,NoWhitespaceValidator]]
});

GetChallanNo() {
  this._apiservice.GetchallanNo().subscribe((modeSelectList: SelectListDto[]) => {
      this.challanNo = modeSelectList["result"];
  });
};

GrtTableGrid()
{
  var _grnConfirm =  new GrnConfirm();
  
  _grnConfirm.DeliveryChallanNo=this.DeliveryChallanNo;
  this._apiservice.GetChallanDetails(this.DeliveryChallanNo).subscribe((response) => {
    this.challanDtls = response["result"];
    
  })
}

ValidateGRNConfirmation() {
  
var _grnConfirm =  new GrnConfirm();
  
  _grnConfirm.DeliveryChallanNo=this.DeliveryChallanNo;
  _grnConfirm.CartonBarcode=this.CartonBarcode;
  if(this.DeliveryChallanNo=="" || this.DeliveryChallanNo==null)
  {
    abp.notify.error("Please select delivery challan !");
    this.Clear();
  }
  else
  {
    
this._apiservice.GetValidateGRNConfirmation(this.DeliveryChallanNo,this.CartonBarcode).subscribe(result => {
    
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
