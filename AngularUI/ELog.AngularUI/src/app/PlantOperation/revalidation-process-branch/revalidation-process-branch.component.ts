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

interface ExpiryDetails {

  plantCode: string,
  MaterialCode: string,
  barcode: string,
  itemBarcode:string
}

interface CheckValue {
  barcode: string;
  materialCode: any;
  plantCode: any;
  checkBoxValues : boolean;
  itemBarcode:any;
}

@Component({
  selector: 'app-revalidation-process-branch',
  templateUrl: './revalidation-process-branch.component.html',
  styleUrls: ['./revalidation-process-branch.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class RevalidationProcessBranchComponent implements OnInit {
  ItemCodes: any;
  itemDtls: [];
  MaterialCode: string = "";
  barcode: string = "";
  isSelected: boolean = false;
  PlantCode: any;
  PlantCodeList: [];
  AddedCheckValue: CheckValue[] = [];
  ScanBarcode:any;
  itemBarcode:any;

  public pageLenght: any;
  public pageSize = 10;
  public currentPage = 0;
  public totalSize = 0;


  public dataSource: MatTableDataSource<any> = new MatTableDataSource<ExpiryDetails>();
  public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<ExpiryDetails>();
  @ViewChild(MatSort, { static: false }) sort!: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent: ValidationService,
    private titleService: Title

  ) { }

  ngOnInit() {
    this.titleService.setTitle('Revalidation Process Branch/Plant');
    this.GetPlantCodes();

  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
  }

  addEditFormGroup: FormGroup = this.formBuilder.group({
    PlantCodeFormControl: ['', [Validators.required, NoWhitespaceValidator]],
    ItemCodeFormCControl: ['', [Validators.required, NoWhitespaceValidator]],
    BarcodeFormControl: [null]
  });

  GetPlantCodes() {
    abp.ui.setBusy();
    this._apiservice.GetPlantToBranchPlantCode().subscribe((response) => {
      this.PlantCodeList = response["result"];
      abp.ui.clearBusy();
    });
  };

  GetItemCodes() {
    abp.ui.setBusy();
    this._apiservice.GetItemCodes().subscribe((modeSelectList: SelectListDto[]) => {
      this.ItemCodes = modeSelectList["result"];
      abp.ui.clearBusy();
    });
  };

  GetCheckBoxValue(event,barcode: any, materialCode: any, itemBarcode: any, isChecked: boolean) {
    if (event.checked) {
      this.addCheckBoxValue(barcode, materialCode, itemBarcode);
    } else {
      this.removeCheckBoxValue(barcode, materialCode, itemBarcode);
    }
  }
  
  // Function to add checkbox value
  addCheckBoxValue(barcode: any, materialCode: any, itemBarcode: any) {
    this.AddedCheckValue.push({
      barcode: barcode,
      materialCode: materialCode,
      plantCode: this.PlantCode,
      checkBoxValues: true,
      itemBarcode: itemBarcode
    });
  }
  
  // Function to remove checkbox value
  removeCheckBoxValue(barcode: any, materialCode: any, itemBarcode: any) {
    const index = this.AddedCheckValue.findIndex(item => 
      item.barcode === barcode && 
      item.materialCode === materialCode && 
      item.itemBarcode === itemBarcode
    );
  
    if (index !== -1) {
      this.AddedCheckValue.splice(index, 1);
    }
  }

  ValidateCartonBarcode() {
    
    if (this.AddedCheckValue.length > 0) {
      this._apiservice.ValidateItem(this.AddedCheckValue).subscribe(result => {

        if (result["result"][0]['valid']) {
          abp.notify.success(result["result"][0]['valid']);
        }
        else {
          abp.notify.error(result["result"][0]['error']);
        }

        this.getArray();
      });
    }
    else {
      abp.notify.error("Please select the radio button.");
    }
  }

  ValidateByBarcode() {
    
      this._apiservice.ValidateItemByBarCode(this.ScanBarcode).subscribe(result => {

        if (result["result"][0]['valid']) {
          abp.notify.success(result["result"][0]['valid']);
        }
        else {
          abp.notify.error(result["result"][0]['error']);
        }

        this.getArray();
      });
  }
  

  markDirty() {
    this._appComponent.markGroupDirty(this.addEditFormGroup);
    return true;
  }
  Clear() {

    this.addEditFormGroup.reset();
  }

  Searching(searchTerm: string) {
    this.dataSourcePagination.filter = searchTerm.trim().toLocaleLowerCase();
    const filterValue = searchTerm;
    this.dataSourcePagination.filter = filterValue.trim().toLowerCase();
    this.dataSource.filteredData = this.dataSourcePagination.filteredData;
    this.iterator();
  }


  public handlePage(e: any) {
    this.currentPage = e.pageIndex;
    this.pageSize = e.pageSize;
    this.iterator();
  }

  private getArray() {
    abp.ui.setBusy();
    if (this.MaterialCode != undefined && this.PlantCode !== undefined) {
      this._apiservice.GetExpiredItemCodeDetails(this.MaterialCode, this.PlantCode).subscribe((response) => {
        this.isSelected = false;
        this.dataSourcePagination = new MatTableDataSource<Element>(response['result']);
        this.dataSourcePagination.paginator = this.paginator;
        this.pageLenght = response['result'];
        this.totalSize = this.pageLenght.length;
        this.AddedCheckValue.splice(0, this.AddedCheckValue.length);
        this.iterator();
        abp.ui.clearBusy();
      });
    }
    else {
      abp.ui.clearBusy();
      this.dataSource.filter = null;
    }
  }

  private iterator() {
    const end = (this.currentPage + 1) * this.pageSize;
    const start = this.currentPage * this.pageSize;
    // this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
    this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
  }

}
