import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnInit, NgModule, ViewChild, AfterViewInit } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { NgxPaginationModule } from 'ngx-pagination';
import { AppComponent } from '@app/app.component';
import { Title } from '@angular/platform-browser';

interface MaterialMaster {
    materialCode: string,
    description: string,
    packSize: string,
    uom: string,
    unitWeight: string,
    selfLife: string,
    active: boolean
}

@NgModule({
    declarations: [
        AppComponent,
    ],
    imports: [
        NgxPaginationModule,
    ],
})

@Component({
    selector: 'app-material',
    templateUrl: './material.component.html',
    styleUrls: ['./material.component.css'],
    animations: [appModuleAnimation()],
})
export class MaterialComponent implements OnInit, AfterViewInit {
    lines: any;
    searchText;
    searchTerm = '';
    p: Number = 1;
    public array: any;

    public pageSize = 10;
    public currentPage = 0;
    public totalSize = 0;

    // displayedColumns: string[] = ['PlantCode', 'WorkCenterCode', 'WorkCenterDiscription','Active'];

    public dataSource: MatTableDataSource<any> = new MatTableDataSource<MaterialMaster>();
    public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<MaterialMaster>();
    @ViewChild(MatSort, { static: false }) sort!: MatSort;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

    constructor(
        private _apiservice: ApiServiceService,
        private titleService: Title

    ) { }

    ngOnInit() {
        
        this.titleService.setTitle('Material Master');
        this.getArray();
        this.paginator._intl.itemsPerPageLabel="Records per page";
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
        this._apiservice.getMaterialMaster()
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


}
