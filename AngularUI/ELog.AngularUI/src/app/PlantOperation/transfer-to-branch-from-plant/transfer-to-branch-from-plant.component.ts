import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ViewChildren, OnInit, NgModule, ViewChild, QueryList, AfterViewInit } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator, MyErrorStateMatcher } from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
import { Title } from '@angular/platform-browser';
import { toInt } from '@rxweb/reactive-form-validators';

export class transferToBranch {
  plantCode: string = "";
  DeliveryChallanNo: string = "";
  ShiperBarcode: string = "";
  CartonBarcode: string = "";
}

interface ScanCartonData {
  itemCode: string,
  quantity: 0,
  scanedQty: 0,
  barCode:string
}

@Component({
  selector: 'app-transfer-to-branch-from-plant',
  templateUrl: './transfer-to-branch-from-plant.component.html',
  styleUrls: ['./transfer-to-branch-from-plant.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class TransferToBranchFromPlantComponent implements OnInit {
  challanNo: any;
  challanDtls: any;
  DeliveryChallanNo: string = "";
  CartonBarcode: string = "";
  ScanCount:any;
  quantity:any;
  MaterialCode:any;
  materialList:any;
  FromPlantCode:any; 
  ToPlantCode:any;
   

  public array: any;
  public pageSize = 10;
  public currentPage = 0;
  public totalSize = 0;

  public dataSource: MatTableDataSource<any> = new MatTableDataSource<ScanCartonData>();
  public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<ScanCartonData>();
  @ViewChild(MatSort, { static: false }) sort!: MatSort;
  @ViewChild('paginator', { static: true }) paginator: MatPaginator;

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent: ValidationService,
    private titleService: Title

  ) { }

  ngOnInit() {
    this.titleService.setTitle('Transfer To Branch From Plant');
    this.GetChallanNo();
    this.addEditFormGroup.controls['FormControlscanedQty'].disable();
    this.addEditFormGroup.controls['FormControlQuantity'].disable();
  }
  addEditFormGroup: FormGroup = this.formBuilder.group({

    ShiperBarcodeFormControl: ['', [Validators.required, NoWhitespaceValidator]],
    plantCodeFormCControl: ['', [Validators.required, NoWhitespaceValidator]],
    FormControlscanedQty:[null],
    FormControlQuantity:[null],
    MaterialCodeFormCControl:['', [Validators.required, NoWhitespaceValidator]]
  });

  GetChallanNo() {
    abp.ui.setBusy();
    this._apiservice.GetchallanNo().subscribe((modeSelectList: SelectListDto[]) => {
      this.challanNo = modeSelectList["result"];
      this.CartonBarcode = '';
      abp.ui.clearBusy();
    });
  };

GetMaterialCode()
{
  if(this.DeliveryChallanNo != '')
  {
   this._apiservice.GetSTOMaterialCode(this.DeliveryChallanNo).subscribe((response) => {
     this.materialList = response["result"];
   });
  }
  else{
    abp.ui.clearBusy();
  }
}

  TableGrid() {
    abp.ui.setBusy();
    if ( this.DeliveryChallanNo !== undefined && this.MaterialCode !== undefined) {
      this._apiservice.GetSTOChallanDetails(this.DeliveryChallanNo,this.MaterialCode).subscribe((response) => {
        this.challanDtls = response["result"];
        this.FromPlantCode = response["result"][0].sendingPlantCode
        this.ToPlantCode = response["result"][0].receivingPlantCode 
        abp.ui.clearBusy();
      })
    }
    else
    {
      abp.ui.clearBusy();
    }
  }
  ValidateCartonBarcode() {
    if (this.DeliveryChallanNo == "" || this.DeliveryChallanNo == null) {
      abp.notify.error("Please select delivery challan !");
      this.Clear();
    }
    else {
      this._apiservice.GetValidateScanCartonBarcode(this.DeliveryChallanNo,this.CartonBarcode,this.MaterialCode, this.FromPlantCode, this.ToPlantCode).subscribe(result => {
        if (result["result"][0]['valid']) {
          this.quantity = result["result"][0].quantity;
          this.ScanCount = result["result"][0].dispatchQty;
          // this.dataSourcePagination = new MatTableDataSource<Element>(result['result']);
          // this.dataSourcePagination.paginator = this.paginator;
          // this.array = result['result'];
          // this.totalSize = this.array.length;
          // this.iterator();
          if(this.quantity == this.ScanCount)
          {
            this.Clear();
          }
          abp.notify.success(result["result"][0].valid)
        }
        else {
          abp.notify.error(result["result"][0]['error']);
        }

      });
    }
    this.TableGrid();
  }

  Clear() {

    this.addEditFormGroup.reset();
  }

  private iterator() {
    const end = (this.currentPage + 1) * this.pageSize;
    const start = this.currentPage * this.pageSize;
    // this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
    this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
    
  }

}
