import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Title } from '@angular/platform-browser';


interface LineMaster {
  packingOrderNo: string,
  plantCode: string,
  materialCode: string,
  quantity: string,
  printedQty: string,
  pendingQtyToPrint: string,
  packedQty: string,
  pendingQtyToPack: string,
  strLocCode: string,
  packing_Date: string,
  bom: string,
  work_Center: string,
}

@Component({
  selector: 'app-packing-order',
  templateUrl: './packing-order.component.html',
  styleUrls: ['./packing-order.component.css'],
  animations: [appModuleAnimation()],
})



export class PackingOrderComponent implements OnInit {
  lines: any;
  searchText;
  searchTerm = '';
  p: Number = 1;
  public array: any;

  public pageSize = 10;
  public currentPage = 0;
  public totalSize = 0;

  // displayedColumns: string[] = ['PlantCode', 'WorkCenterCode', 'WorkCenterDiscription','Active'];

  public dataSource: MatTableDataSource<any> = new MatTableDataSource<LineMaster>();
  public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<LineMaster>();
  @ViewChild(MatSort, { static: false }) sort!: MatSort;
  @ViewChild('paginator', { static: true }) paginator: MatPaginator;

  constructor(
    private _apiservice: ApiServiceService,
    private titleService: Title

  ) { }

  ngOnInit() {
    this.titleService.setTitle('Packing Order Master');
    this.getArray();
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    // this.dataSource = new MatTableDataSource(this.dataSource.filteredData);
    // this.dataSource.paginator = this.paginator;
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
    this._apiservice.getPackingMasters()
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


}
