import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ViewChildren, OnInit, NgModule, ViewChild,QueryList, AfterViewInit } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { NgxPaginationModule } from 'ngx-pagination';
import { AppComponent } from '@app/app.component';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator, MyErrorStateMatcher } from '@shared/app-component-base';
interface SerialNo {
  plantCode: string,
  packingOrder: string,
  }
export class PackingOrder
{
  PackingOrderNo : string="";
     plantCode      : string="";
 }
@Component({
  selector: 'app-packing-order-confirmation',
  templateUrl: './packing-order-confirmation.component.html',
  styleUrls: ['./packing-order-confirmation.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class PackingOrderConfirmationComponent implements OnInit {
  searchText;
  quantity:any;
  packingOrder: string="";
  plantCode: string="";
  searchTerm = '';
  p: Number = 1;
  public array: any;

  public pageSize = 10;
  public currentPage = 0;
  public totalSize = 0;
  line: string;
  plnaCodeList: any;
  packingOrderList: any;
  picklistItems:[];
  //public dataSource: MatTableDataSource<any> = new MatTableDataSource<SerialNo>();
  public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<SerialNo>();
  @ViewChild(MatSort, { static: false }) sort!: MatSort;
  @ViewChild('paginator', { static: true }) paginator: MatPaginator;

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent : ValidationService,
  ) { }

  ngOnInit() {

  
    this.GetPlantCode();
   
    
  }

  addEditFormGroup: FormGroup = this.formBuilder.group({
    //registration: [null, [Validators.required, NoWhitespaceValidator]],
    
    plantCodeFormCControl: [null, [Validators.required, NoWhitespaceValidator]],
    packingOrderFormControl: ['',[Validators.required,NoWhitespaceValidator]]
    
});
matcher = new MyErrorStateMatcher();
GetPlantCode() {
  this._apiservice.getPlantCode().subscribe((modeSelectList: SelectListDto[]) => {
      this.plnaCodeList = modeSelectList["result"];
  });
};



onChangePlantCode(value)
{
 
 this._apiservice.getPackingOrderNo(value).subscribe((response) => {
  this.packingOrderList = response["result"];
});
}

GrtTableGrid(value)
{
  debugger;
  this._apiservice.GetPackingOrderConfirmation(value).subscribe((response) => {
    this.picklistItems = response["result"];
    this.quantity=this.quantity;
})
}


Save() {
  debugger;
  var _packing =  new PackingOrder();
  _packing.PackingOrderNo=this.packingOrder;
  _packing.plantCode=this.plantCode;
  this._apiservice.PackingOrderConfirmation(this.packingOrder,this.plantCode).subscribe(result => {
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

markDirty() {
  this._appComponent.markGroupDirty(this.addEditFormGroup);
   return true;
}

Clear() {
  this.addEditFormGroup.controls['plantCodeFormCControl'].setValue(null);
  this.addEditFormGroup.controls['packingOrderFormControl'].setValue(null);
}

}
