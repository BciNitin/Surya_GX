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

export class ItemCodedtl
{
    
     plantCode: string="";
     MaterialCode:string="";
     barcode:string="";
     
}
@Component({
  selector: 'app-revalidation-process-branch',
  templateUrl: './revalidation-process-branch.component.html',
  styleUrls: ['./revalidation-process-branch.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class RevalidationProcessBranchComponent implements OnInit {
  ItemCodes:any;
  itemDtls:any;
  MaterialCode:string="";
  barcode:string="";
  isSelected: boolean = false;
  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent : ValidationService,
    private titleService: Title

    ) { }

  ngOnInit() {
    this.titleService.setTitle('Revalidation Process Branch/Plant');
    this.GetItemCodes();
    
  }
  addEditFormGroup: FormGroup = this.formBuilder.group({
    
    
    ItemCodeFormCControl: ['',[Validators.required,NoWhitespaceValidator]]
});

GetItemCodes() {
  
  this._apiservice.GetItemCodes().subscribe((modeSelectList: SelectListDto[]) => {
      this.ItemCodes = modeSelectList["result"];
      
  });
};
GrtTableGrid()
{
  
  var _itemDtls =  new ItemCodedtl();
  
  _itemDtls.MaterialCode=this.MaterialCode;
  this._apiservice.GetExpiredItemCodeDetails(this.MaterialCode).subscribe((response) => {
    

  this.itemDtls = response["result"];
    
  this.isSelected=false;
  
  
  })
}
GetCheckBoxValue(event,barcode:any,materialCode:any) {
 
  this.isSelected = event.source._checked;
  this.barcode=barcode;
  this.MaterialCode=materialCode;
  }

  ValidateCartonBarcode() {
  
    if(this.isSelected)
    {
    var _itemDtls =  new ItemCodedtl();
    
    _itemDtls.barcode=this.barcode;
    _itemDtls.MaterialCode=this.MaterialCode;
    
      
  this._apiservice.GetValidateItem(this.barcode,this.MaterialCode).subscribe(result => {
      
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
      else
      {
        abp.notify.error("Please select the radio button.");
      }
    }
    markDirty() {
      this._appComponent.markGroupDirty(this.addEditFormGroup);
       return true;
    }
    Clear(){
        
      this.addEditFormGroup.reset();
    }
}
