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
import { MatRadioChange } from '@angular/material';
import { NoWhitespaceValidator, MyErrorStateMatcher } from '@shared/app-component-base';
import { Title } from '@angular/platform-browser';

import { error } from 'console';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
interface SerialNo {
  plantCode: string,
  line: string,
  packingOrder: string,
  supplierCode: string
  driverCode: string
}
export class GenerateSerialNumber
{
    
     lineCode       : string="";
     PackingOrderNo : string="";
     plantCode      : string="";
     supplierCode   : string="";
     driverCode     : string="";
     packing_Date    : string="";
     ItemCode       : string="";
     printingQty     : Number;
     pendingQtyToPrint: Number;
     quantity:Number;
     work_Center:string="";
    
}
@Component({
  selector: 'app-serialbarcodegeneration',
  templateUrl: './serialbarcodegeneration.component.html',
  styleUrls: ['./serialbarcodegeneration.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})

export class SerialbarcodegenerationComponent implements OnInit {
  
  lines: any;
  SrBarcode: any;
  searchText;
  work_Center:string="";
  quantity:Number;
  lineCode: string="";
  packingOrder: string="";
  plantCode: string="";
  SupplierCode: string="";
  driverCode: string="";
  packing_Date: string="";
  materialCode: string="";
  ItemCode :string="";
  printingQty: any;
  pendingQtyToPrint: Number;
  searchTerm = '';
  p: Number = 1;
  public array: any;
  updateUIselectedOrderType:any;
  public pageSize = 10;
  public currentPage = 0;
  public totalSize = 0;
   line: string;
  
  supplierCode: string;
  
  plnaCodeList: any;
  plnaCodeListSelect: any;
  lineOrderList:any;
  lineList: any;
  packingOrderList: any;
  picklistItems:[];
  isSelected: boolean = false;

  // displayedColumns: string[] = ['PlantCode', 'WorkCenterCode', 'WorkCenterDiscription','Active'];

  public dataSource: MatTableDataSource<any> = new MatTableDataSource<SerialNo>();
  public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<SerialNo>();
  @ViewChild(MatSort, { static: false }) sort!: MatSort;
  @ViewChild('paginator', { static: true }) paginator: MatPaginator;

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent : ValidationService,
    private titleService: Title,
    private http: HttpClient

  ) { }

  ngOnInit() {
    this.titleService.setTitle('Serial Barcode Generation');
    this.getArray();
    this.GetPlantCode();
    this.GetLineCode();

    
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
  }

  filterCountries(searchTerm: string) {
    this.dataSourcePagination.filter = searchTerm.trim().toLocaleLowerCase();
    const filterValue = searchTerm;
    this.dataSourcePagination.filter = filterValue.trim().toLowerCase();
    this.dataSource.filteredData = this.dataSourcePagination.filteredData;
    this.iterator();
  }

  onMatSortChange() {
    this.dataSource.sort = this.sort;
  }

  public handlePage(e: any) {

    this.currentPage = e.pageIndex;
    this.pageSize = e.pageSize;
    this.iterator();
  }

  private getArray() {
    this._apiservice.getLineMaster()
      .subscribe((response) => {
        // this.dataSourcePagination  = new MatTableDataSource<Element>(response['result']);
        this.dataSourcePagination = new MatTableDataSource<Element>(response['result']);
        this.dataSourcePagination.paginator = this.paginator;
        this.array = response['result'];
        this.totalSize = this.array.length;
        this.iterator();
      });
  }

  private iterator() {
    const end = (this.currentPage + 1) * this.pageSize;
    const start = this.currentPage * this.pageSize;
    // this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
    this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
  }

  addEditFormGroup: FormGroup = this.formBuilder.group({
    //registration: [null, [Validators.required, NoWhitespaceValidator]],
    LineCodeFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    plantCodeFormCControl: [null, [Validators.required, NoWhitespaceValidator]],
    packingOrderFormControl: [null,[Validators.required,NoWhitespaceValidator]],
    supplierCodeFormControl:[null,[Validators.required,NoWhitespaceValidator]],
    driverCodeFormControl:[null,[Validators.required,NoWhitespaceValidator]],
   printingQty:[null,[Validators.required,NoWhitespaceValidator]]
});
matcher = new MyErrorStateMatcher();

GetPlantCode() {
  abp.ui.setBusy();
  this._apiservice.getPlantCode().subscribe((modeSelectList: SelectListDto[]) => {
      this.plnaCodeList = modeSelectList["result"];
      this.plnaCodeListSelect = [...this.plnaCodeList];
      abp.ui.clearBusy();
  });
};

  GetLineCode() {
    abp.ui.setBusy();
    this._apiservice.getLineWorkCenterNo().subscribe((response) => {
      this.lineList = response["result"];
      abp.ui.clearBusy();
    });
  };

validateForm(event: any) {
  if ((event.target.selectionStart === 0 && event.code === "Space") || (event.keyCode >= 48 && event.keyCode <= 64) || (event.keyCode >= 91 && event.keyCode <= 255)) {
      if (!parseInt(event.key) && event.key != '.' && event.key != '-') {
          event.preventDefault();
      }
  }
}
  onChangeLineCode() {
    
    if (typeof this.plantCode !== 'undefined' && typeof this.lineCode !== 'undefined') {
      abp.ui.setBusy();
      this._apiservice.getPackingOrderNoForSerialNumber(this.plantCode, this.lineCode).subscribe(
        (response) => {
          
          
            this.packingOrderList = response["result"];
            this.updateUIselectedOrderType = this.packingOrder;
            abp.ui.clearBusy();
         
          
        },
        (error) => {
          // Handle the error here
          console.error('An error occurred:', error);
          // Optionally, display an error message to the user
          // You can use a notification service or display the error in the UI
          abp.notify.error('An error occurred while fetching data.');
          abp.ui.clearBusy();
        }
      );
    }
    this.packingOrderList=null;
    this.picklistItems=null;
  }
  onChangePlantCode() {
    
    if (this.plantCode !== '') {
      abp.ui.setBusy();
      this._apiservice.getLineCodeasPerPlant(this.plantCode).subscribe(
        (response) => {
          
          
            this.lineOrderList = response["result"];
            this.updateUIselectedOrderType = this.packingOrder;
            abp.ui.clearBusy();
         
          
        },
        (error) => {
          // Handle the error here
          console.error('An error occurred:', error);
          // Optionally, display an error message to the user
          // You can use a notification service or display the error in the UI
          abp.notify.error('An error occurred while fetching data.');
          abp.ui.clearBusy();
        }
      );
    }
  }
  GrtTableGrid(value) {
    if (value != '') {
      abp.ui.setBusy();
      this._apiservice.GetSerialNumberDetails(value).subscribe((response) => {
        if (response["result"][0]['error'])
        {
          abp.notify.error(response["result"][0]['error']);
          abp.ui.clearBusy();
        }
        else{
          this.picklistItems = response["result"];
          abp.ui.clearBusy();
        }
        
      })
    }
  }


GetCheckBoxValue(event,plantCode:any,materialCode:any,quantity:any,pendingQtyToPrint:any,printedQty:any,packing_Date:any,work_Center:any,packingOrderNo:any) {
     this.isSelected = event.source._checked;
     this.plantCode=plantCode;
     this.materialCode=materialCode;
     this.quantity=quantity;
     this.pendingQtyToPrint=pendingQtyToPrint;
     this.packing_Date=packing_Date;
     this.work_Center=work_Center;
     
}

onKey(searchTerm: string) { 
  //this.plnaCodeList = this.search(value);
  const filterValue = searchTerm.trim().toLowerCase();
  this.plnaCodeListSelect = this.plnaCodeList.filter(item =>
   
     item.id.toLowerCase().includes(filterValue)
    
  );
   console.log('item',this.plnaCodeListSelect)
  }
  
  search(searchTerm: string) {
    this.plnaCodeList = searchTerm.trim().toLocaleLowerCase();
    const filterValue = searchTerm;
    this.plnaCodeList.filter = filterValue.trim().toLowerCase();
    this.dataSource.filteredData = this.dataSourcePagination.filteredData;
    this.iterator();
  }

  // search(value: string) { 
  //   let filter = value.toLowerCase();
  //   return this.plnaCodeList.filter(option => option.toLowerCase().startsWith(filter));
  // }

Save() {

  abp.ui.setBusy();
  if (this.isSelected) {
    var _GenSerial = new GenerateSerialNumber();
    _GenSerial.plantCode = this.plantCode;
    _GenSerial.ItemCode = this.materialCode;
    _GenSerial.printingQty = this.printingQty;
    _GenSerial.quantity = this.quantity;
    _GenSerial.pendingQtyToPrint = this.pendingQtyToPrint;
    _GenSerial.packing_Date = this.packing_Date;
    _GenSerial.lineCode = this.lineCode;
    _GenSerial.supplierCode = this.supplierCode;
    _GenSerial.driverCode = this.driverCode;
    _GenSerial.work_Center = this.work_Center;
    _GenSerial.PackingOrderNo = this.packingOrder;

    if (this.pendingQtyToPrint < this.printingQty || this.printingQty < 0) {
      abp.notify.error("Printing Quantity should be equal to or less than from Pending Quantity!");
      abp.ui.clearBusy();
      this.isSelected = false;
    }
    else {
      try {
        this._apiservice.SaveSerialBarcodeGen(_GenSerial).subscribe(result => {
          this.SrBarcode = result["result"];
          if (result["result"][0].valid) {
            abp.notify.success(result["result"][0].valid);
            this.GrtTableGrid(this.packingOrder);
            this.printingQty=0;
            abp.ui.clearBusy();
            this.isSelected = false;
            console.log("result",result["result"][0].fileName)
            const fileName = result["result"][0].fileName            ;
            this._apiservice.downloadSerialNumberCSV(fileName).subscribe((response) => {
              const blob = new Blob([response.body], { type: response.headers.get('content-type') });
              const link = document.createElement('a');
              link.href = window.URL.createObjectURL(blob);
              link.download = fileName;
              link.click();
            });
          }
          else {
            abp.notify.error(result["result"][0].error);
            abp.ui.clearBusy();
            this.isSelected = false;
          }

        },
          (error) => {
            abp.ui.clearBusy();
            this.isSelected = false;
            // Handle HTTP error
          });
      } catch (error) {
        abp.ui.clearBusy();
        abp.notify.error("There is error");
        this.isSelected = false;
      }
    }
  }
  else {
    abp.notify.error("Please select the checkbox.");
    abp.ui.clearBusy();
    this.isSelected = false;
  }
  this.isSelected = false;

}

// downloadFileService(): Observable<HttpResponse<Blob>> {
//   const url = `${"http://localhost:21021/"}/downloadFile`;
//   const headers = new HttpHeaders({
//     'Content-Type': 'application/json',
//     // Add any additional headers if needed
//   });

//   return this.http.get(url, {
//     headers: headers,
//     observe: 'response',
//     responseType: 'blob',
//   });
// }

markDirty() {
  this._appComponent.markGroupDirty(this.addEditFormGroup);
   return true;
}

Clear() {

  this.addEditFormGroup.controls['LineCodeFormControl'].setValue(null);
  this.addEditFormGroup.controls['plantCodeFormCControl'].setValue(null);
  this.addEditFormGroup.controls['packingOrderFormControl'].setValue(null);
  this.addEditFormGroup.controls['supplierCodeFormControl'].setValue(null);
  this.addEditFormGroup.controls['driverCodeFormControl'].setValue(null);

}

}
