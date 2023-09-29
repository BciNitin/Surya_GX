import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit,NgModule, ViewChild, AfterViewInit } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiServiceService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {MatPaginatorModule} from '@angular/material/paginator';
import { NgxPaginationModule } from 'ngx-pagination';
import { AppComponent } from '@app/app.component';

interface LineMaster {
    plantCode: string,
    workCenterCode: string,
    WorkCenterDiscription: string,
    active: boolean
}
   
@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [  
    NgxPaginationModule,
   
  ],})

@Component({
  selector: 'app-line-master',
  templateUrl: './line-master.component.html',
  styleUrls: ['./line-master.component.css'],
  // animations: [
  //   trigger('detailExpand', [
  //     state('collapsed', style({height: '0px', minHeight: '0'})),
  //     state('expanded', style({height: '*'})),
  //     transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
  //   ]),
  // ],
})
export class LineMasterComponent implements OnInit,AfterViewInit  {
lines :any;
searchText;
searchTerm = '';
p: Number = 1;
count: Number = 2;

displayedColumns: string[] = ['PlantCode', 'WorkCenterCode', 'WorkCenterDiscription','Active'];

public dataSource: MatTableDataSource<any> = new MatTableDataSource<LineMaster>()
@ViewChild(MatSort, {static: false}) sort!: MatSort;

  constructor(
    private _apiservice:ApiServiceService
  ) { }

  ngOnInit() {
    

     this._apiservice.getLineMaster().subscribe((data: any) => {
      console.log("data['result']",data['result'])
      this.dataSource = new MatTableDataSource<LineMaster>(data['result'])
    });
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
  }

  filterCountries(searchTerm: string) {
    this.dataSource.filter = searchTerm.trim().toLocaleLowerCase();
    const filterValue = searchTerm;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  onMatSortChange() {
    this.dataSource.sort = this.sort;
  }


}
