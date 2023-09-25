import { Component, OnInit, ElementRef, PipeTransform, Pipe, Injector, ViewChild, NgModule, InjectionToken, Inject, Optional } from '@angular/core';
import { debounceTime, distinctUntilChanged, filter, finalize, map } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpClientModule, HttpInterceptor, HttpBackend, HttpParams } from '@angular/common/http';
import * as XLSX from 'xlsx';
import 'datatables.net-colreorder';
import 'datatables.net-select';
import { DataTableDirective } from 'angular-datatables';
import { ReportSubModule } from '../../../shared/PmmsEnums';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');


import { exists } from 'fs';
import { fromEvent } from 'rxjs';
import {
    PagedListingComponentBase,
    PagedRequestDto
} from '@shared/paged-listing-component-base';
import { SelectListServiceProxy, SelectListDto, ModuleServiceProxy, ModuleDto, ModuleListDto, ClientFormsDtoPagedResultDto, SubModuleListDto, SubModuleListDtoPagedResultDto, ClientFormsServiceServiceProxy, ClientFormsDto, ModuleListDtoPagedResultDto, SubModuleDto, ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';
import { MatDialog } from '@angular/material';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { Options } from 'selenium-webdriver/ie';
import { AppConsts } from '../../../shared/AppConsts';

class PagedModuleRequestDto extends PagedRequestDto {
    keyword: string;
    isActive: boolean | null;
}
@Component({
    selector: 'log-data-approval-report',
    templateUrl: './log-data-approval.component.html',
    animations: [appModuleAnimation()]
})

// @NgModule({

//     imports: [

//         BrowserModule,
//         HttpClient
//     ],
//     providers: [],

// })
export class LogdataapprovalComponent extends PagedListingComponentBase<ModuleDto> implements OnInit {
    @ViewChild('reportTable', { static: true }) table: ElementRef;
    moduleMasters: any;
    modules = [];
    plants: ClientFormsDto[] = [];
    submodules: SubModuleListDto[] = [];
    myArrayList = [];
    formHindden: boolean;
    value: any;
    keys = [];
    status: number | null = 1;
    approvalStatus: number | null;
    name: number | null;
    displayName: string | null;
    description: string | null;
    sortBy: number | null;
    Drops: boolean;
    private baseUrl: string;

    roleNames: SelectListDto[] | null;
    moduleSortBy: SelectListDto[] | null;
    moduleStatuses: SelectListDto[] | null;
    filterBy: string;
    sortByOrder: number | null;
    statuses: SelectListDto[] | null;
    moduleId: any;
    submoduleID: any;
    apiVariable: any;
    viewReportButton: any;
    columnsHidden: string[] = [];
    keyword: any;
    clientFormJson: any;
    nodata: any;
    filters: string[] = [];
    selectedSubModule: any = "";
    selectedModule = "";
    client: any; FormName: any; DBName: any;
    filterData: any = {};
    formId: any;
    constructor(
        injector: Injector,
        private _changePwdService: ChangePswdServiceProxy, private _modulesService: ModuleServiceProxy,
        private _selectListService: SelectListServiceProxy,
        private _clientFormsService: ClientFormsServiceServiceProxy,
        private _dialog: MatDialog,
        private _router: Router,
        private _route: ActivatedRoute,
        private http: HttpClient,
        handler: HttpBackend,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {
        super(injector);
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    ngOnInit() {
        this.nodata = 1;
        this.moduleMasters = [];
        this.formHindden = false;
        this.setTitle('Data List');
        this.GetModuleSortBy();
        super.ngOnInit();
        this.keyword = '';
        this.Drops = true;
        this.viewReportButton = true;
        this.myArrayList = [10];
        debugger;
        this.formId = null;
        this._route.params.subscribe((routeData: Params) => {
            if (routeData['formId']) {
                this.formId = routeData['formId'];

            }
        });

        this._clientFormsService.get(this.formId).pipe(
            finalize(() => {
                abp.ui.clearBusy();
            })
        ).subscribe((successData: ClientFormsDto) => {
            this.client = successData;

            this.plants = this.plants.concat(successData)
            //   console.log(this.plants[0].formJson)
            //  this.clientFormJson = JSON.stringify(this.plants[0].formJson);
            this.clientFormJson = JSON.parse(this.plants[0].formJson);
            this.FormName = this.clientFormJson.form_name;
         
            this.DBName = this.clientFormJson.db_table;
            this.viewreport();
          
       



        });




        for (let key in this.value) {
            for (let keyval in this.value[key]) {
                if (this.keys.findIndex(elem => elem === keyval) !== -1) {
                    console.log('element exist');

                }
                else {
                    this.keys.push(keyval);
                }


            }


        }

    }

    dropClick() {

        this.Drops = false;
    }

    dropdeClick() {

        this.Drops = true;
    }

    addOrRemore(event) {
        // var  hiddenElements = $(':hidden');

        if (this.columnsHidden.indexOf(event) > -1) {

            let index: number = this.columnsHidden.indexOf(event);
            this.columnsHidden.splice(index, 1);
            console.log(this.columnsHidden);

        } else {
            this.columnsHidden.push(event);
            console.log(this.columnsHidden);
        }



        //    // var clsName = event.target.className;
        //     if ($("." + event).css('display') == 'none') {
        //         $("." + event).show();
        //   //      $(this).removeClass("notSelected");
        //         $(this).parent('button').removeClass('notSelected');
        //     }
        //     else {
        //         $("." + event).remove();
        //         $(this).parent('button').addClass('notSelected');
        //         $(this).addClass('yyyyyyyyyyy');

        //     }


    }

    private serializeObject(data: any): string {
        let serialized = "";
        if (data && Object.keys(data).length) {
            let keysArr = Object.keys(data);
            let index = 0;
            keysArr.forEach(key => {
                serialized = serialized + key + "=" + data[key];
                if (index < keysArr.length - 1) {
                    serialized += "&";
                }
                index += 1;

            });
        }
        return serialized;
    }
    viewreport() {
        // var params1 = $("#DynamicForm").serialize();
        var params1 = this.serializeObject(this.filterData);

        console.log(this.serializeObject(this.filterData));
        var numbersArray = params1.split('&');

        this.columnsHidden = [];

        var params = new HttpParams();
        var finalParam = "";
        for (var i = 0; i < numbersArray.length; i++) {
            var keyVal = numbersArray[i].split('=');;
            if (keyVal[1] != '' && keyVal[0] != '') {

                params = params.append(keyVal[0], keyVal[1]);
                if (finalParam != '') {
                    finalParam = finalParam + ' and ';
                }
                finalParam += 'es.' + keyVal[0] + '=\'' + keyVal[1] + '\' ';
            }

        }




        this.moduleId;
        this.submoduleID;
        this.apiVariable;
        // var apiPath = this.apiVariable;
        this.formHindden = true;
        //  var apiPath = { 'tablename': this.keyword,'param':null };
        this.keys = [];
        var paramsValue = new HttpParams();
   
        let result = '';
     

        result = encodeURIComponent(result);

        abp.ui.setBusy();
        this.http.get<any>(AppConsts.remoteServiceBaseUrl + '/api/services/app/Report/GetAuditTrail?tablename=' + this.DBName).subscribe({
            next: data => {
                this.nodata = 0;

                this.value = data.result;
                console.log(this.value);
                console.log(this.selectedModule);
                console.log(this.selectedSubModule);
                if (this.selectedSubModule.name === "FgPutAway") {
                    if (this.value && this.value.length) {
                        this.value.forEach(fgPutAwayObj => {

                            delete fgPutAwayObj.huCode;
                            delete fgPutAwayObj.isPicked;
                            delete fgPutAwayObj.productBatchNo;
                            delete fgPutAwayObj.locationId;
                            delete fgPutAwayObj.palletId;


                        });
                    }
                }
                this.viewReportButton = true;
                abp.ui.clearBusy();

                for (let key in this.value) {
                    for (let keyval in this.value[key]) {
                        if (this.keys.findIndex(elem => elem === keyval) !== -1) {
                            console.log('element exist');

                        }
                        else {
                            this.keys.push(keyval);
                        }


                    }


                }
            },
            error: error => {
                abp.ui.clearBusy();
                console.error('There was an error!', error);
            }
        });

    }

    clearFilters() {
        abp.ui.setBusy();
        var mapKeys = [{
            "MaterialStatusLabel": "history.EquipmentCleaningStatus", "Loading": "Loading", "FgPicking": "FgPicking", "FgPutAway": "FgPutAway", "Destruction(Sampling)": "history.SampleDestructions", "StageOut(Sampling)": "history.StageOutDetails",
            "Sampling": "history.SamplingTypeMaster"
        }];

        //  const headers = new HttpHeaders().append('header', 'value');
        var reportTable = mapKeys[0][this.apiVariable];
        if (reportTable == undefined) {
            reportTable = this.apiVariable;
        }
        this.http.get<any>(AppConsts.remoteServiceBaseUrl + '/api/services/app/Report/GetAuditTrail?tablename=' + reportTable + '&param=').subscribe({
            next: data => {
                this.nodata = 0;

                this.value = data.result;
                this.viewReportButton = true;
                abp.ui.clearBusy();

                for (let key in this.value) {
                    for (let keyval in this.value[key]) {
                        if (this.keys.findIndex(elem => elem === keyval) !== -1) {
                            console.log('element exist');

                        }
                        else {
                            this.keys.push(keyval);
                        }


                    }


                }
            },
            error: error => {
                abp.ui.clearBusy();
                console.error('There was an error!', error);
            }
        });
    }

    viewReportFilter() {

        this.formHindden = false;
    }


    protected delete(entity: ModuleDto): void {
        throw new Error("Delete not implemented.");
    }
    GetSortBy(sortBy: number, orderBy: number) {
        let order = '';
        if (orderBy) {
            if (orderBy == 1) {
                order = 'asc'
            } else if (orderBy == 2) {
                order = 'desc';
            }
        }

        if (sortBy == 1) {
            return `Name ${order}`;
        } else if (sortBy == 2) {
            return `DisplayName ${order}`;
        } else if (sortBy == 3) {
            return `Description ${order}`
        }
        else if (sortBy == 4) {
            return `isActive ${order}`
        }
        else {
            return '';
        }
    }
    GetModuleSortBy() {
        this._selectListService.getSortByModule().subscribe((moduleSelectList: SelectListDto[]) => {
            this.moduleSortBy = moduleSelectList;
        });
    }

    list(
        request: PagedModuleRequestDto,
        pageNumber: number,
        finishedCallback: Function
    ): void {

        //  this.modules = [{ "id": "elog3_Usage_Record_Poly_Bag", "displayName": "Usage Record Of Polythene Bags" }];

        this._clientFormsService
            .getAll( null ,null, this.FormName,null, 0,null,null,null,null,0,100)
            .pipe(
                finalize(() => {
                    finishedCallback();
                    abp.ui.clearBusy();
                })
            )
            .subscribe((result: ClientFormsDtoPagedResultDto) => {
                if (result.items.length > 0) {
                    for (var i = 0; i < result.items.length; i++) {
                        this.modules.push({ "id": "elog3_" + result.items[i].formName.replace(/ /g, "_"), "displayName": result.items[i].formName });
                    }
                }
            });

        /*    let moduleSortBy = this.GetSortBy(this.sortBy, this.sortByOrder);
            this._modulesService
                .getAllModule(null, this.status, moduleSortBy, request.skipCount, request.maxResultCount)
                .pipe(
                    finalize(() => {
                        finishedCallback();
                    })
                )
                .subscribe((result: ModuleListDtoPagedResultDto) => {
                    if (result.items.length > 0) {
                       
                       
                        this.modules = this.modules.concat(result.items);
                        console.log(this.modules);
                        for(let module of this.modules) {
                            if(module.name === "WIP") {
                                module.displayName = "MOTS";
                            }
                        }
                    
                        this.showPaging(result, pageNumber);
                        //   this.getActivity(8);
                    }
                });*/
    }

    getActivity(id) {
        this.viewReportButton = true;
        this.moduleId = id;
        this.submodules = [];
        this._modulesService
            .getAllSubModule(null, this.status, id, 0, null, 0, 10)
            .pipe(
                finalize(() => {
                    // finishedCallback();
                })
            )
            .subscribe((result: SubModuleListDtoPagedResultDto) => {


                if (result.items.length > 0) {
                    this.submodules = this.submodules.concat(result.items);

                    this.showPaging(result, 1);
                }
            });
    }

    getsubActivity(subId) {
        debugger;
        this.submoduleID = subId.value.id;
        this.apiVariable = subId.value;
        this.viewReportButton = false;

    }


    clearSelections(): void {
        this.value = [];
        this.keys = [];
        this.filterData = {};
        this.selectedModule = "";
        this.selectedSubModule = "";
    }

    ClearSearch() {

        //this.groupStatusId = null;
        //  this.refresh();

        this.filterData = {};
        // form.resetForm();
        this.clearFilters();

    }
    exportAsPDF(tableElem: any) {
        window.print();
    }


    exportexcel(): void {
        let element = document.getElementById('excel-table');
        //  $(element+":hidden").remove();
        const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);

        /* generate workbook and add the worksheet */
        const wb: XLSX.WorkBook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

        /* save to file */
        XLSX.writeFile(wb, 'ExcelSheet.xlsx');

    }
    GoToViewApprove(id: any) {
        this._router.navigate(['../../createforms/edit/'+this.formId+'/' + id], { relativeTo: this._route });
        
       
    }
    ApplySearch() {
        var form = $("#DynamicForm");
        var data = form.serialize();

    }

}
