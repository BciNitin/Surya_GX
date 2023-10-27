import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { ValidationService } from '@shared/ValidationService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';

interface grid {
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
  parantBarCode:any;
  dealerCodeList:any;

  public dataSource: MatTableDataSource<any> = new MatTableDataSource<grid>();
  public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<grid>();
  @ViewChild(MatSort, { static: false }) sort!: MatSort;
  @ViewChild('paginator', { static: true }) paginator: MatPaginator;

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent: ValidationService,
  ) { }

  ngOnInit() {

    abp.ui.setBusy();
    this.GetDealerCode();
    abp.ui.clearBusy();

  }

  addEditFormGroup: FormGroup = this.formBuilder.group({
    dealerCodeFormControl: [null, [Validators.required, NoWhitespaceValidator]],
  });
  matcher = new MyErrorStateMatcher();

  async GetDealerCode() {
    abp.ui.setBusy();
    await this._apiservice.GetDealerCode().subscribe((modeSelectList: SelectListDto[]) => {
      this.dealerCodeList = modeSelectList["result"];
      abp.ui.clearBusy();
    });
    
  };

  Approve() {
    abp.ui.setBusy();
    this._apiservice.saveQualityChecking(this.dataSource.filteredData).subscribe(
      (result) => {
        abp.ui.clearBusy();
        if (result['result'][0].error) {
          abp.notify.error(result['result'][0].error);
        } else {
          abp.notify.error(result['result'][0].valid);
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
    this.dataSource.filter = null;
    this.dataSource.data.push('');
  }

  getDetails() {
    abp.ui.setBusy();
    if (this.itemBarCode != '0' && this.itemBarCode != undefined) {
      this._apiservice.GetQualityChecking('','','')
        .subscribe((response) => {
          if (!response['result'][0].error) {
            this.dataSourcePagination = new MatTableDataSource<Element>(response['result']);
            this.dataSourcePagination.paginator = this.paginator;
            this.array = response['result'];
            this.totalSize = this.array.length;
            this.iterator();
          }
          else {
            this.dataSource.filter = null;
            abp.notify.error(response['result'][0].error);
          }
        })
    }
    else {
      this.dataSource.filter = null;
    }
    abp.ui.clearBusy();
  }

  private iterator() {
    abp.ui.setBusy();
    const end = (this.currentPage + 1) * this.pageSize;
    const start = this.currentPage * this.pageSize;
    this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
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

