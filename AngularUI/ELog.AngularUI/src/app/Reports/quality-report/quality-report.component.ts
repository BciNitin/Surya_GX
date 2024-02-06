import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { Router } from '@angular/router';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { ValidationService } from '@shared/ValidationService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base';
import { Title } from '@angular/platform-browser';
import * as XLSX from 'xlsx';
interface grid {
  FromDate: Date;
  LineCode: string;
  MaterialCode: string;
  PackingOrder: string;
  PlantCode: string;
  ToDate: Date;
  ShiperBarcode:string;
  QCStatus:string;
}
@Component({
  selector: 'app-quality-report',
  templateUrl: './quality-report.component.html',
  styleUrls: ['./quality-report.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class QualityReportComponent implements OnInit {
  IExcel: grid | any;
  exportExcel: any | 0;
  PackingOrder: string;
  PlantCode: string;
  QCStatus:string;
  packingReports:any;
  plnaCodeList:any;
  public array: any;
  packingOrderlist:any;
  public pageSize = 10;
  public currentPage = 0;
  public totalSize = 0;
  
 public dataSource: MatTableDataSource<any> = new MatTableDataSource<grid>();
 public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<grid>();
 @ViewChild(MatSort, { static: false }) sort!: MatSort;
 @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
 constructor(
  private _apiservice: ApiServiceService,
  private formBuilder: FormBuilder,
  public _appComponent: ValidationService,
  private titleService: Title,
 

) { }
  ngOnInit() {
    this.GetPlantCode();
    this.paginator._intl.itemsPerPageLabel="Records per page";
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
    const data = {
      
      PackingOrder: this.PackingOrder,
      PlantCode: this.PlantCode,
      QCStatus:this.QCStatus
    };

       this._apiservice.GetQualityReport(data).subscribe(result => {
        this.dataSourcePagination = new MatTableDataSource<Element>(result['result']);
        this.dataSourcePagination.paginator = this.paginator;
        if(result["result"][0]['error'])
        {
          abp.notify.error(result["result"][0]['error']);
          this.iterator();
          this.dataSource.filteredData.length=0;
          this.totalSize = 0;
        }
        else
        {
          this.array = result['result'];
          this.totalSize = this.array.length;
          this.iterator();
        }
        
      });
  }

  private iterator() {
    const end = (this.currentPage + 1) * this.pageSize;
    const start = this.currentPage * this.pageSize;
    this.dataSource.filteredData = this.dataSourcePagination.filteredData.slice(start, end);
  }
  addEditFormGroup: FormGroup = this.formBuilder.group({
    plantCodeFormCControl: [null, [Validators.required, NoWhitespaceValidator]],
    packingOrderFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    QCStatusFormControl:[null, [Validators.required, NoWhitespaceValidator]]
    });
  matcher = new MyErrorStateMatcher();

  GetPlantCode() {
    abp.ui.setBusy();
    this._apiservice.getPlantCode().subscribe((response) => {
      this.plnaCodeList = response["result"];
      abp.ui.clearBusy();
    },
    (error) => {
      abp.ui.clearBusy();
    }
    );
  };
  
  GetPackingReportOrderNo(plantcode) {
    this._apiservice.GetPackingReportOrderNo(this.PlantCode).subscribe((response) => {
        this.packingOrderlist = response["result"];
    });
  };
  markDirty() {
    this._appComponent.markGroupDirty(this.addEditFormGroup);
    this.dataSource.filteredData.length=0;
    this.totalSize = 0;
    return true;
  }


  exportexcel(): void {
    this.exportExcel = 1
    this.IExcel =this.array;
        let Heading = [['Plant Code','Line Code','Packing Order No.', 'Item', 'Shipper Barcode', 'Item Barcode']];
        const wb = XLSX.utils.book_new();
        const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(this.IExcel);
        XLSX.utils.sheet_add_aoa(ws, Heading);
        XLSX.utils.sheet_add_json(ws, this.IExcel, { origin: 'A2', skipHeader: true });
  
        XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
  
        XLSX.writeFile(wb, 'QualityReport.xlsx');
  
      }
      
  
  }

