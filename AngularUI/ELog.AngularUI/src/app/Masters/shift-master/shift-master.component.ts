import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { id } from 'date-fns/locale';
import { finalize } from 'rxjs/operators';
import { ActivatedRoute, Data, Router } from '@angular/router';
import { Title } from '@angular/platform-browser';

interface ShiftMaster {
  id:string,
  shift_Code: string,
  shift_Description: string,
  shift_StartTime: string,
  shift_EndTime: string,
}
@Component({
  selector: 'app-shift-master',
  templateUrl: './shift-master.component.html',
  styleUrls: ['./shift-master.component.css'],
  animations: [appModuleAnimation()],
})

export class ShiftMasterComponent implements OnInit {
  lines: any;
  searchText;
  searchTerm = '';
  p: Number = 1;
  public array: any;

  public pageSize = 10;
  public currentPage = 0;
  public totalSize = 0;
  ShiftCode:any|undefined;
  // displayedColumns: string[] = ['PlantCode', 'WorkCenterCode', 'WorkCenterDiscription','Active'];

  public dataSource: MatTableDataSource<any> = new MatTableDataSource<ShiftMaster>();
  public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<ShiftMaster>();
  @ViewChild(MatSort, { static: false }) sort!: MatSort;
  @ViewChild('paginator', { static: true }) paginator: MatPaginator;
  constructor( 
    private _apiservice: ApiServiceService,
    private _router: Router,
    private _route: ActivatedRoute,
    private titleService: Title

    ) { }

  ngOnInit() {
    this.titleService.setTitle('Shift Master');
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
    this._apiservice.getShiftMaster()
      .subscribe((response) => {
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
  addUser() {
    this._router.navigate(['../add-shift'], { relativeTo: this._route });
  };
    
  delete(id){  
      this._apiservice.DeleteSiftMasterbyid(id).pipe(
      finalize(()=>{abp.ui.clearBusy})
    )
    .subscribe((response) => {
      console.log("res", response['result'])
      if(response['result'][0].valid)
       {
         abp.notify.success(response['result'][0].valid);
         this.getArray();
       }
       else{
         abp.notify.error(response['result'][0].error);
       }  
     
    });
  
   
  }
  
  
}
