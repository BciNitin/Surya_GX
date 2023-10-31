import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ViewChildren, OnInit, NgModule, ViewChild,QueryList, AfterViewInit } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator ,MyErrorStateMatcher} from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
export class itemplacement
{
     plantCode: string="";
     itemBarcode:string="";
     ShiperBarcode:string="";
}
@Component({
  selector: 'app-quality-tested-itemplacement',
  templateUrl: './quality-tested-itemplacement.component.html',
  styleUrls: ['./quality-tested-itemplacement.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class QualityTestedItemplacementComponent implements OnInit {
  itemBarcode:string="";
  qualityItemDtls:any;
  plnatCodeList:any;
  plantCode: string="";
  ShiperBarcode:string="";
  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent : ValidationService
    ) { }

  ngOnInit() {
    this.GetPlantCode();
  }
  addEditFormGroup: FormGroup = this.formBuilder.group({
    itemBarcodeFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    ShiperBarcodeFormControl: ['',[Validators.required,NoWhitespaceValidator]],
    plantCodeFormCControl: ['',[Validators.required,NoWhitespaceValidator]]
});

Validate() {
  

  var _iPlacement =  new itemplacement();
  _iPlacement.itemBarcode = this.itemBarcode;
  _iPlacement.plantCode=this.plantCode;
  if(this.plantCode=="" || this.plantCode==null)
  {
    abp.notify.error("Please select Plant code!");
    this.Clear();
  }
  else
  {
  this._apiservice.GetQualityItemTestedDtls(this.itemBarcode,this.plantCode).subscribe(result => {
    
           if(result["result"][0]['error'])
           {
            abp.notify.error(result["result"][0]['error']);
            this.Clear();
           }
           else
           {
            this.qualityItemDtls = result["result"];
           }
           
      });
    }
}
ValidateShiperBarcode() {
  

  var _iPlacement =  new itemplacement();
  _iPlacement.itemBarcode = this.itemBarcode;
  _iPlacement.plantCode=this.plantCode;
  _iPlacement.ShiperBarcode=this.ShiperBarcode;
  if(this.plantCode=="" || this.plantCode==null)
  {
    abp.notify.error("Please select Plant code!");
    this.Clear();
  }
  else
  {
    if(this.itemBarcode=="" || this.itemBarcode==null)
    {
      abp.notify.error("Please first validate  Item barcode!");
      this.Clear();
    }
    else
    {
this._apiservice.ValidateShiperBarcode(this.itemBarcode,this.plantCode,this.ShiperBarcode).subscribe(result => {
    
    if(result["result"][0]['valid'])
    {
      abp.notify.success(result["result"][0]['valid']);
    }
    else
    {
     abp.notify.error(result["result"][0]['error']);
    }
        this.Validate();   
      });
}
}
}

GetPlantCode() {
  this._apiservice.getPlantCode().subscribe((modeSelectList: SelectListDto[]) => {
      this.plnatCodeList = modeSelectList["result"];
  });
};

Clear(){
        
  this.addEditFormGroup.reset();
}
}
