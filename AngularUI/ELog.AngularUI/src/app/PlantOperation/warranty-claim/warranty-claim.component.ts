import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ViewChildren, OnInit, NgModule, ViewChild, QueryList, AfterViewInit } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator, MyErrorStateMatcher } from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
import { Title } from '@angular/platform-browser';

export class warranty {

  Barcode: string = "";
  CustomerCode: string = "";
  Qty:string="";
  ApprovedQty:string="";
  MaterialCode:string="";
  BarCodeApprovedQty:string="";
}
@Component({
  selector: 'app-warranty-claim',
  templateUrl: './warranty-claim.component.html',
  styleUrls: ['./warranty-claim.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class WarrantyClaimComponent implements OnInit {
  warrantyDtls: any;
  CustomerCode: string = "";
  Barcode: string = "";
  customerCodelist: any;
  isBarCoded:boolean = false;
  isNoneBarcoded:boolean = false;
  ItemCodes:any;
  Qty:string="";
  IsBarcodes:any;
  ApprovedQty:string="";
  MaterialCode:string="";
  BarCodeApprovedQty:string="";

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent: ValidationService,
    private titleService: Title

  ) { }

  ngOnInit() {
    this.titleService.setTitle('Warranty Claim');
    this.GetCustomerCode();
    this.GetItemCodes();
    this.isNoneBarcoded = true;  
    
  }

  public dataSource: MatTableDataSource<any> = new MatTableDataSource<warranty>();
  isBlockedFormControl: FormControl = new FormControl('barcoded');

  addEditFormGroup: FormGroup = this.formBuilder.group({

    ScannedCartonBarcodeFormControl: ['', [Validators.required, NoWhitespaceValidator]],
    plantCodeFormCControl: ['', [Validators.required, NoWhitespaceValidator]],
    CartonBarcodeFormControl: ['', [Validators.required, NoWhitespaceValidator]],
    CustomerFormCControl: ['', [Validators.required, NoWhitespaceValidator]],
    isBlOCKFormControl:['', [Validators.required, NoWhitespaceValidator]],
    ItemCodeFormCControl:['', [Validators.required, NoWhitespaceValidator]],
    QtyFormControl:['', [Validators.required, NoWhitespaceValidator]],
    ApprovedQtyFormControl:['', [Validators.required, NoWhitespaceValidator]],
    RemarkFormControl:['', [Validators.required, NoWhitespaceValidator]],
    BarApprovedQtyFormControl:['', [Validators.required, NoWhitespaceValidator]]

  });

  GetCustomerCode() {
    this._apiservice.GetCustomerCode().subscribe((modeSelectList: SelectListDto[]) => {
      this.customerCodelist = modeSelectList["result"];
    });
  };

  GrtTableGrid() {
    if (this.CustomerCode == "" || this.CustomerCode == null) {
      abp.notify.error("Please select customer code !");
      this.Clear();
    }
    else {
      
      var _warranty = new warranty();
      _warranty.Barcode = this.Barcode;
      _warranty.CustomerCode = this.CustomerCode;
      this._apiservice.GetWarrantyDetails(this.Barcode, this.CustomerCode).subscribe((response) => {
        //this.warrantyDtls = response["result"];
        if (response["result"][0]['error']) {
          abp.notify.error(response["result"][0]['error']);
          this.Clear();

        }
        else {
          //this.warrantyDtls = response["result"];
          this.dataSource.filteredData=response["result"];
        }

      })
    }
  }

  GrtNonbarcodedGrid() {
    if (this.CustomerCode == "" || this.CustomerCode == null) {
      abp.notify.error("Please select customer code !");
      this.Clear();
    }
    else {
      
      var _warranty = new warranty();
      _warranty.Qty = this.Qty;
      _warranty.CustomerCode = this.CustomerCode;
      _warranty.ApprovedQty=this.ApprovedQty;
      _warranty.MaterialCode = this.MaterialCode;
      this._apiservice.GetNonBarcodedWarrantyDetails(this.Qty, this.CustomerCode,this.ApprovedQty,this.MaterialCode).subscribe((response) => {
        
        if (response["result"][0]['error']) {
          abp.notify.error(response["result"][0]['error']);
          this.Clear();

        }
        else {
         
          this.dataSource.filteredData=response["result"];
        }

      })
    }
  }
  GetItemCodes() {
  
    this._apiservice.GetItemCodes().subscribe((modeSelectList: SelectListDto[]) => {
        this.ItemCodes = modeSelectList["result"];
        
    });
  };

  Save() {
    debugger;
if(this.isNoneBarcoded==true)
{
  var _warranty = new warranty();
  _warranty.Barcode = this.Barcode;
  _warranty.CustomerCode = this.CustomerCode;
  _warranty.BarCodeApprovedQty=this.BarCodeApprovedQty;
  this._apiservice.GetValidateWarrranty(this.Barcode, this.CustomerCode,this.BarCodeApprovedQty).subscribe(result => {
  
    if (result["result"][0]['valid']) {
      abp.notify.success(result["result"][0]['valid']);
      this.GrtTableGrid();
    }
    else {
      abp.notify.error(result["result"][0]['error']);
    }

  });

  
}
else 
{
  
  var _warranty = new warranty();
  _warranty.MaterialCode = this.MaterialCode;
  _warranty.CustomerCode = this.CustomerCode;
  _warranty.Qty = this.Qty;
  _warranty.ApprovedQty=this.ApprovedQty;
  
  this._apiservice.GetValidateNonBarcodedWarrranty(this.MaterialCode, this.CustomerCode,this.Qty,this.ApprovedQty).subscribe(result => {
  
    if (result["result"][0]['valid']) {
      abp.notify.success(result["result"][0]['valid']);
      this.GrtNonbarcodedGrid()
    }
    else {
      abp.notify.error(result["result"][0]['error']);
    }

  });
  
}
  
  }

  markDirty() {
    this._appComponent.markGroupDirty(this.addEditFormGroup);
    return true;
  }

  Clear() {

    this.addEditFormGroup.reset();
    this.dataSource.filteredData=null;
  }

  BarcodedChange(check)
  {
    this.isNoneBarcoded = check.source.checked;
    
    console.log(check.source.checked);
    this.Clear();
  }
  
  NoneBarcodedChange(check)
  {
    
    this.isNoneBarcoded = false; 
    this.Clear();
    }
}
