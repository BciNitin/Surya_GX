import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { ValidationService } from '@shared/ValidationService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
import { Title } from '@angular/platform-browser';

import { error } from 'console';
import { Session } from 'inspector';

interface grid
{
   materialCode: string;
   batchCode: string | null;
   packingDate: string | null;
   qty: number;
   parantBarCode:string;
   DealerCode:string;
   itemBarCode:string;
}

@Component({
  selector: 'app-revalidation-dealer-location',
  templateUrl: './revalidation-dealer-location.component.html',
  styleUrls: ['./revalidation-dealer-location.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class RevalidationDealerLocationComponent implements OnInit, AfterViewInit {
  public pageSize = 10;
  public currentPage = 0;
  public totalSize = 0;
  public array: any;

  dealerCode: any;
  itemBarCode:any;
  CartonBarCode:any;
  dealerCodeList:any;
  isItemBarCodeValid:boolean= false;

  public dataSource: MatTableDataSource<any> = new MatTableDataSource<grid>();
  public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<grid>();
  @ViewChild(MatSort, { static: false }) sort!: MatSort;
  @ViewChild('paginator', { static: true }) paginator: MatPaginator;

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent: ValidationService,
    private titleService: Title

  ) { }

  ngOnInit() {
    
    
    this.titleService.setTitle('Revalidation Dealer Location');
    abp.ui.setBusy();
    this.GetDealerCode();
    this.toggleValidation(false);
    abp.ui.clearBusy();

  }

  addEditFormGroup: FormGroup = this.formBuilder.group({
    dealerCodeFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    ItemBarCodeFormControl: [],
    CartonBarCodeFormControl: []
  });
  matcher = new MyErrorStateMatcher();

  toggleValidation(event) {
    this.isItemBarCodeValid = event.checked;
    if (event.checked) {
     // this.addEditFormGroup.get('ItemBarCodeFormControl').setValidators([Validators.required, NoWhitespaceValidator]);
      //this.addEditFormGroup.get('CartonBarCodeFormControl').clearValidators();
      this.addEditFormGroup.controls['CartonBarCodeFormControl'].disable();
      this.addEditFormGroup.controls['ItemBarCodeFormControl'].enable();
      this.addEditFormGroup.get('CartonBarCodeFormControl').clearValidators();
      this.addEditFormGroup.get('ItemBarCodeFormControl').clearValidators();
      this.addEditFormGroup.updateValueAndValidity();
    } else {
      //this.addEditFormGroup.get('CartonBarCodeFormControl').setValidators([Validators.required, NoWhitespaceValidator]);
      this.addEditFormGroup.get('CartonBarCodeFormControl').clearValidators();
      this.addEditFormGroup.get('ItemBarCodeFormControl').clearValidators();
      this.addEditFormGroup.controls['ItemBarCodeFormControl'].disable();
      this.addEditFormGroup.controls['CartonBarCodeFormControl'].enable();
      this.addEditFormGroup.updateValueAndValidity();
    }
 
  }

  async GetDealerCode() {
    abp.ui.setBusy();
    await this._apiservice.GetDealerCode().subscribe((modeSelectList: SelectListDto[]) => {
      this.dealerCodeList = modeSelectList["result"];
      abp.ui.clearBusy();
    });
    
  };

  Approve() {
    abp.ui.setBusy();
    for (let i = 0; i < this.dataSource.filteredData.length; i++) {
      this.dataSource.filteredData[i].DealerCode = this.dealerCode;
    }
    this._apiservice.ApproveOnDealerLocation(this.dataSource.filteredData,this.isItemBarCodeValid).subscribe(
      (response) => {
        abp.ui.clearBusy();
        if (Array.isArray(response['result']) && response['result'].length > 0) {
          const firstResult = response['result'][0];
          if (typeof firstResult === 'object' && 'error' in firstResult) {
            abp.notify.error(response['result'][0].error);
          } else {
            this.dataSource.filter = '';
            this.addEditFormGroup.controls['CartonBarCodeFormControl'].setValue(null);
            this.addEditFormGroup.controls['ItemBarCodeFormControl'].setValue(null);
            this.dataSource.data = [];
            abp.notify.success(response['result'][0].valid);
          }
        } else {
          abp.notify.error(response['result'][0].error);
        }
      },
      (error) => {
        // Handle HTTP error
      }
    );
  }



  markDirty() {
    this._appComponent.markGroupDirty(this.addEditFormGroup);
    return true;
  }

  Clear() {

    this.addEditFormGroup.controls['dealerCodeFormControl'].setValue(null);
    this.addEditFormGroup.controls['CartonBarCodeFormControl'].setValue(null);
    this.addEditFormGroup.controls['ItemBarCodeFormControl'].setValue(null);
    this.dataSource.data = [];
    this.isItemBarCodeValid = false;
    this.toggleValidation(this.isItemBarCodeValid);
  
  }

  ValidateCartonBarCode(barcode) {
    
    const sCartonValid =  this.addEditFormGroup.controls['CartonBarCodeFormControl'].status;
    const sDealerCode =  this.addEditFormGroup.controls['dealerCodeFormControl'].status;
    abp.ui.setBusy();
    if (sCartonValid == 'VALID' && sDealerCode == 'VALID') {
      this._apiservice.GetRevalidationOnDealerCarton(this.dealerCode,barcode)
        .subscribe((response) => {
          if (Array.isArray(response['result']) && response['result'].length > 0) {
            const firstResult = response['result'][0];
            if (typeof firstResult === 'object' && 'error' in firstResult) {
              abp.notify.error(response['result'][0].error);
            } else {
              this.dataSourcePagination = new MatTableDataSource<Element>(response['result']);
              this.dataSourcePagination.paginator = this.paginator;
              this.array = response['result'];
              this.totalSize = this.array.length;
              this.iterator();
            }
          } else {
            abp.notify.error(response['result'][0].error);
          }

        },
        (error=>
          {
            abp.ui.clearBusy();
          })
        )
    }
    else {
      this.dataSource.filter = '';
    }
    abp.ui.clearBusy();
  }

  ValidateItemBarCode(barcode) {
    const sItemBarCodeValid =  this.addEditFormGroup.controls['ItemBarCodeFormControl'].status;
    const sDealerCode =  this.addEditFormGroup.controls['dealerCodeFormControl'].status;
    abp.ui.setBusy();
    if (sItemBarCodeValid == 'VALID' && sDealerCode == 'VALID') {
      this._apiservice.GetRevalidationDealerOnItem(this.dealerCode,barcode)
        .subscribe((response) => {
          console.log(response['result'])
          if (Array.isArray(response['result']) && response['result'].length > 0) {
            const firstResult = response['result'][0];
            if (typeof firstResult === 'object' && 'error' in firstResult) {
              abp.notify.error(response['result'][0].error);
            } else {
              this.addEditFormGroup.controls['ItemBarCodeFormControl'].setValue(null);
              this.dataSourcePagination = new MatTableDataSource<Element>(response['result']);
              this.dataSourcePagination.paginator = this.paginator;
              this.array = response['result'];
              this.totalSize = this.array.length;
              this.iterator();
            }
          } else {
            abp.notify.error(response['result'][0].error);
          }
        })
    }
    else {
      this.dataSource.filter = '';
    }
    abp.ui.clearBusy();
  }

  private iterator() {
    abp.ui.setBusy();
    const end = (this.currentPage + 1) * this.pageSize;
    const start = this.currentPage * this.pageSize;
    this.dataSource.filteredData.push(...this.dataSourcePagination.filteredData.slice(start, end));
    //this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
    abp.ui.clearBusy();
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
}

