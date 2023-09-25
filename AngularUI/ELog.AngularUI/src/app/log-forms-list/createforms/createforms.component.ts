import { Component, ElementRef, Injector, InjectionToken, ViewChild, Renderer2, Inject, Optional } from '@angular/core';
import { MatDialog, DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material';
import { debounceTime, distinctUntilChanged, filter, finalize, map, concat } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';
import { SelectListServiceProxy, SelectListDto, ClientFormsDtoPagedResultDto, ElogApiServiceServiceProxy, ClientFormsServiceServiceProxy, ClientFormsDto, ChangePasswordDto, ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { fromEvent } from 'rxjs';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { FormControl, FormGroup, FormArray, FormBuilder, ValidatorFn, Validators, AbstractControl } from '@angular/forms';
import { resolveAny } from 'dns';
import { AppComponentBase, MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base'
import { DatePipe } from '@angular/common';
import { HttpClient, HttpHeaders, HttpClientModule, HttpInterceptor, HttpBackend, HttpParams } from '@angular/common/http';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import * as _moment from 'moment';
//import { default as _rollupMoment } from 'moment';
export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');
const moment = _moment;
import { AppConsts } from '../../../shared/AppConsts';
import { match } from 'minimatch';
import * as XLSX from 'xlsx';
//import { setTimeout } from 'timers';

class PagedClientFormsRequestDto extends PagedRequestDto {
    keyword: string;
    isActive: boolean | null;
}


@Component({
    selector: 'app-createforms',
    templateUrl: './createforms.component.html',
    styleUrls: ['./createforms.component.css'],
    animations: [appModuleAnimation()],

})

export class CreateformsComponent extends PagedListingComponentBase<ClientFormsDto> {

    plants: ClientFormsDto[] = [];
    keyword = '';
    freez: boolean | null;
    timestamp: any;
    client: ClientFormsDto | null;
    isActive: boolean | null;
    id: number;
    filteredProducts: any;
    formId: number = 99;
    FormName: string;
    isProductCodeLoading: any;
    FormStartDate: moment.Moment;
    FormEndDate: moment.Moment;
    FormJson: string;
    IsActive: boolean;
    CreationDate: moment.Moment;
    ModifiedDate: moment.Moment;
    clientFormJson: any;
    form_fields: any;
    countryId: number | null;
    approvalStatusId: number | null;
    plantTypeId: number | null;
    status: number | null;
    sortBy: number | null;
    sortByOrder: number | null;
    validatorform: any = [];
    tableHeader = [];
    tableData = [];
    filterBy: string;
    activeStatuses: SelectListDto[];
    approvalStatuses: SelectListDto[];
    countries: SelectListDto[];
    plantSortBy: SelectListDto[];
    activeStatus: number | null;
    plantTypes: SelectListDto[];
    plantName: string | null;
    Formvalidation: FormGroup;
    validateform: FormGroup = new FormGroup({});
    FormControlName: any = {};
    mapforComponent = {};
    options: any;
    optionsStatic: any
    dataTableOption: any;
    columnsHidden: string[] = [];
    currentDate: any;
    DBName: any;
    newObject = {};
    dt: any;
    dataId: any;
    pusheditems: any;
    pushOptions: any;
    selectedKeyValue: any;
    nodata: any;
    valueKey: any;
    //keys: [];
    keys: any;
    Datepipe: any;
    fieldData: any;
    randomNumber: any;
    value: any;
    multiKeys: any;
    multiValues: any;
    statickeys: any;
    staticvalue: any;
    scanCountValues: any; 
    formNames: any;
    pagetitle: any;
    showmaster: any; 
    private arrays: Array<any> = [];
    @ViewChild('searchTextBox', { static: true }) searchTextBox: ElementRef;
    collapse: boolean;
    collpaseName: any;
    columnCount: number;
    scanneddata: any;
    showInfo: any;
    fieldhtml: any = [{
        "single_input_text": "<div class='form-group itemText width100' id><label for='InputText' class><p class='lbtx'>Text Field</p><span class='lbrq'></span><span class='lbtt'></span></label><input type='text' autocomplete='off' name='name' class='form-control' id='InputText' placeholder='Write here...(click me to make me active)'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "search_input": "<div class='form-group itemText ' id><label for='searchInput' class><p class='lbtx'>Search Field</p><span class='lbrq'></span><span class='lbtt'></span></label><input type='search' autocomplete='off' name='name' class='form-control' id='searchInput' placeholder='Write here...(click me to make me active)'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "text_area_field": "<div class='form-group' id='textArea '><label for='textArea' class=''><p class='lbtx'>TextArea Field</p><span class='lbrq'></span><span class='lbtt'></span></label><textarea class='form-control' id='textArea' name='textarea' placeholder='Write here...'></textarea><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "input_number": "<div class='form-group' id='number_field '><label for='numberField' class=''><p class='lbtx'>Number Field</p><span class='lbrq'></span><span class='lbtt'></span></label><input type='number' autocomplete='off' name='numberField' class='form-control' id='numberField' placeholder='Write here...(click me to make me active)'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "input_email": "<div class='form-group' id='input_email '><label for='InputEmail' class=''><p class='lbtx'>Email address</p><span class='lbrq'></span><span class='lbtt'></span></label><input type='email'  name='email' class='form-control' autocomplete='off' id='InputEmail' placeholder='Write here...(click me to make me active)'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "input_phone": "<div class='form-group' id='itemphone'><label for='Inputtel' class=''><p class='lbtx'>Phone</p><span class='lbrq'></span><span class='lbtt'></span></label><input type='tel' class='form-control' id='Inputtel' placeholder='Write here...'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "input_password": "<div class='form-group' id='item_password'><label for='Inputpassword' class='' name='password'><p class='lbtx'>Password</p><span class='lbrq'></span><span class='lbtt'></span></label><input type='password' class='form-control' id='Inputpassword' placeholder='Write here...' name='password'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "select_field": "<div class='form-group' id='select_field'><label for='selectField' class=''><p class='lbtx'>Select Field</p><span class='lbrq'></span><span class='lbtt'></span></label><select id='selectField' class='form-control' name='select' placeholder='Choose option'></select><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "input_monthyear": "<div class='form-group' id='input_monthyear'><label for='Monthyear' class=''><p class='lbtx'>Date_Time</p><span class='lbrq'></span><span class='lbtt'></span></label><input type='month' class='form-control' id='Monthyear' placeholder='Write here...' name='date'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "input_date2": "<div class='form-group' id='input_date2'><label for='dateMonthyear' class=''><p class='lbtx'>Date_Month_Year</p><span class='lbrq'></span><span class='lbtt'></span></label><input type='date' class='form-control' id='dateMonthyear' placeholder='Write here...' name='date'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "heading_field": "<div class='form-group' id='heading_field'><label for='headingField' class='' id='heading'><h1 class='lbtx'>Heading</h1></label><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "paragraph_field": "<div class='form-group' id='paragraph_field'><label for='paragraphField' class='' id='paragraph'><h1 class='lbtx'>Paragraph</h1></label><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "input_button": "<div class='form-group' id='input_button'><input type='button' class='btn btn-primary form-control' id='inputButton' value='Submit' name='button'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "accordion_field": "<div class='form-group' id='accordionField'><div class='panel-group' id='accordion'><div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a data-toggle='collapse' data-parent='#accordion' href='#collapse1'>Collapsible Group</a></h4></div><div id='collapse1' class='panel-collapse collapse in'><div class='panel-body sortable droppable'></div></div></div></div>    <div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "radio_list": "<div class='form-group' id='itemradio'><div class='fieldLabel'><p>Radio List</p></div><ul><li><label for='radio1' class=''>Radio</label><input type='radio' class='form-control' id='radio1' name='radio' checked></li></ul><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "multiple_input_text": "",
        "check_list": "<div class='form-group' id='itemchecklist'><div class='fieldLabel'><p>Check List</p></div><ul name='check'><li><label for='check1' class=''>checkbox</label><input type='checkbox' class='form-control' id='check' name='check' checked></li></ul><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "divder_field": "<div class='form-group' id='divder_field'><p class='divder'></p><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "line_break": "<div class='form-group' id='line_break'><label for='lineBrake' class=''><span class='lbrq'></span><span class='lbtt'></span></label><p class='linebrake'></p></div>",
        "table_field": "<div class='form-group draggable' id='table_field'><label for='table' id='table' class=''><p class='lbtx'>Table</p><span class='lbrq'></span><span class='lbtt'></span></label><table class='table tableCss'><tr><th class='shubham1'>Header</th><th  class='shubham1'>Header</th></tr><tr><td>Data</td><td>Data</td></tr></table><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "checkbox": "<div class='form-group' id='itemchecklist'><ul name='checkbox'><li><label for='checkbox1' class=''>checkbox</label><input type='checkbox' class='form-control' id='checkbox' name='box' checked></li></ul><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "approval_button": "<div class='form-group' id='input_button'><input type='button' class='btn btn-primary form-control' id='approvalButton' value='Approval Button' name='approvalbutton'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "approval_checklist": "<div class='form-group' id='itemchecklist'><ul name='approvalcheck'><li><label for='approvalChecklist' class=''>Approval Checkbox</label><input type='checkbox' class='form-control' id='approvalChecklist' name='check' checked></li></ul><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "upload_button": "<div class='form-group' id='uploadButton'><label class='form-label' for='customFile'>Upload File</label><input type='file' class='form-control' id='customFile' name='upload'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "input_range": "<div class='form-group' id='input_range'><label for='inputRange' class=''>Input Range</label><input type='range' class='form-control' id='inputRange' name='Range'min='1' max='100' value='50'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
        "input_time": "<div class='form-group' id='input_time'><label for='InputTime' class=''><p class='lbtx'>Time</p><span class='lbrq'></span><span class='lbtt'></span></label><input type='time' class='form-control' id='Monthyear' placeholder='Write here...' name='time'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
    }];

    table: boolean;
    matchingElement: any;
    cssValue: any;
    constructor(
        private fb: FormBuilder,
        injector: Injector,
        private _clientFormsService: ClientFormsServiceServiceProxy,
        private _elogservice: ElogApiServiceServiceProxy,
        private _selectListService: SelectListServiceProxy,
        private _dialog: MatDialog,
        private _router: Router,
        private _route: ActivatedRoute,
        //private _reportservice: ReportServiceProxy,
        private http: HttpClient,
        private _changePwdService: ChangePswdServiceProxy,
        public datepipe: DatePipe,
        private renderer: Renderer2,
        handler: HttpBackend,
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
    ) {

        super(injector);

    }


    ngOnInit(): void {

        this.mapforComponent["text"] = "single_input_text"; this.mapforComponent["search"] = "search_input"; this.mapforComponent["textarea"] = "text_area_field";
        this.mapforComponent["number"] = "input_number"; this.mapforComponent["email"] = "input_email"; this.mapforComponent["password"] = "input_password";
        this.mapforComponent["checkbox"] = "check_list"; this.mapforComponent["radio"] = "radio_list"; this.mapforComponent["select-one"] = "select_field";
        this.mapforComponent["month"] = "input_monthyear"; this.mapforComponent["date"] = "input_date"; this.mapforComponent["time"] = "input_time";
        this.mapforComponent["button"] = "input_button"; this.mapforComponent["table"] = "table_field"; this.mapforComponent["collapse"] = "accordion_field";
        this.mapforComponent["Approval Button"] = "approval_button"; this.mapforComponent["Approval checkbox"] = "approval_checklist"; this.mapforComponent["file"] = "upload_button";
        this.mapforComponent["range"] = "input_range"; this.mapforComponent["heading_field"] = "heading_field"; this.mapforComponent["paragraph_field"] = "paragraph_field";
        this.mapforComponent["divder_field"] = "divder_field"; this.mapforComponent["line_break"] = "line_break";
        this.scanCountValues = {};
        this.isProductCodeLoading = false;
        this.pusheditems = {};
        this.multiValues = {};
        this.multiKeys = {};
        this.keys = {};
        this.value = {};
        this.options = {};
        this.statickeys = {};
        this.staticvalue = {};
        this.pushOptions;
        this.selectedKeyValue = {};
        this.dt = new Date();
        this.freez = false;
        //this.keys = [];
        this.columnsHidden = [];
        this.randomNumber = Math.floor(Math.random() * 999);;
        this.form_fields = null;
        this.showmaster = false; 
        this.showInfo = false;
        this._route.params.subscribe((routeData: Params) => {
            if (routeData['formId']) {
                this.formId = routeData['formId'];

            }
            if (routeData['dataId']) {
                this.dataId = routeData['dataId'];

            }
        });
        this.filteredProducts = null;
        this.filterBy = null;
        this.GetActiveInactiveStatus();
        this.GetCountries();
        this.GetPlantsortBy();
        this.GetApprovalStatuses();
        this.GetPlant();

        super.ngOnInit();
    }


    onFileChange(ev, formContrlName: any, staticGridDetails: any) {
        let workBook = null;
        let jsonData = null;
        const reader = new FileReader();
        const file = ev.target.files[0];
        reader.onload = (event) => {
            const data = reader.result;
            workBook = XLSX.read(data, { type: 'binary' });
            jsonData = workBook.SheetNames.reduce((initial, name) => {
                const sheet = workBook.Sheets[name];
                initial[name] = XLSX.utils.sheet_to_json(sheet);
                return initial;
            }, {});
            


            this.displayGrid(jsonData, formContrlName, staticGridDetails)
            //   const dataString = JSON.stringify(jsonData);
            //  document.getElementById('output').innerHTML = dataString.slice(0, 300).concat("...");
            // this.setDownload(dataString);
        }
        reader.readAsBinaryString(file);
    }

    GetSortBy(sortBy: number, orderBy: number) {
        let order = '';
        if (orderBy && orderBy != -1) {
            if (orderBy == 1) {
                order = 'asc'
            } else if (orderBy == 2) {
                order = 'desc';
            }
        }
        if (sortBy == 1) {
            return `ClientId ${order}`;
        } else if (sortBy == 2) {
            return `IsActive ${order}`;
        }
        else {
            return '';
        }
    }

    RefreshListBySearch(event) {
        if (this.keyword.length > 2 && this.keyword.trim().length > 2) {
            this.refresh();
        }
        else if (this.keyword.trim().length == 0) {
            this.refresh();
        }
    }

    getrandomNumber(max) {
        return Math.floor(Math.random() * max);
    }

    CreateFilterString() {
        this.filterBy = '';
        let order = '';
        if (!this.sortByOrder) {
            order = '; Order : Asc'
        }
        if (this.sortByOrder == 2) {
            order = '; Order : Desc';
        }
        if (this.FormName && this.FormName.trim() != '') {
            this.filterBy = `${this.filterBy}; Plant Name : ${this.FormName}`;
        }

        if (this.sortBy && this.sortBy != -1) {
            this.filterBy = `${this.filterBy}; Sort By : ${this.plantSortBy.filter(x => x.id == this.sortBy)[0].value} ${order}`;
        }

        if (this.filterBy && this.filterBy.length > 0) {
            this.filterBy = this.filterBy.replace(';', '');
        }
    }

    OnBlur(event: any) {

        if (event.target.value) {
            this.validateform.controls[event.target.id].disable();
        }


    }

    ApplySearch() {
        this.plants = [];
        this.pageNumber = 1;
        this.isFilterOpen = !this.isFilterOpen;
        this.CreateFilterString();
        this.refresh();
    }

    ClearSearch() {
        this.plants = [];
        this.pageNumber = 1;
        this.formId = null;
        this.FormName = null;
        this.DBName = null;
        this.sortBy = null;
        this.filterBy = null;

        // this.activeStatus = null;
        this.sortByOrder = null;
        // this.countryId = null;
        // this.approvalStatusId = null;
        this.refresh();
        this.isFilterOpen = !this.isFilterOpen;
    }

    GetActiveInactiveStatus() {
        this._selectListService.getStatus().subscribe((activeSelectList: SelectListDto[]) => {
            this.activeStatuses = activeSelectList;
        });
    }

    GetCountries() {
        this._selectListService.getCountries().subscribe((countrySelectList: SelectListDto[]) => {
            this.countries = countrySelectList;
        });
    }

    GetPlantsortBy() {
        this._selectListService.getSortByPlant().subscribe((plantSortBySelectList: SelectListDto[]) => {
            this.plantSortBy = plantSortBySelectList;
        });
    }

    GoToViewPlant(plant: any) {
        this._changePwdService.encryptPassword(plant.id).subscribe(
            data => {

                this._router.navigate(['../plant', 'view', data], { relativeTo: this._route });

            }
        );

    }

    GetApprovalStatuses() {
        this._selectListService.getApprovalStatus().subscribe((approvalStatusSelectList: SelectListDto[]) => {
            this.approvalStatuses = approvalStatusSelectList;
        });
    }

    addPlant() {
        this._router.navigate([], { relativeTo: this._route }).then(result => { window.open('../../elog-panel/add-edit-forms', '_blank'); });
    }

    protected list(
        request: PagedClientFormsRequestDto,
        pageNumber: number,
        finishedCallback: Function
    ): void {
        abp.ui.setBusy();
        request.keyword = this.keyword;
        request.isActive = this.isActive;
        let userSortBy = this.GetSortBy(this.sortBy, this.sortByOrder);


    }


    GetPlant() {
        const selectArr = [];
        // 
        abp.ui.setBusy();
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

            this.FormName = this.clientFormJson.form_name.replaceAll('_', ' ').replace(/^./, function (str) { return str.toUpperCase(); });

            this.setTitle('Add/Edit ' + this.FormName);
            if (this.clientFormJson.inputfrequency) {
                this.freez = this.clientFormJson.inputfrequency;
            }

            this.DBName = this.clientFormJson.db_table;
            this.form_fields = this.clientFormJson.form_fields;
            var validatorString = "";
            for (let i = 0; i < this.form_fields.length; i++) {
                this.arrays = [];

                if (this.form_fields[i].field_properties[0].field_type == 'checkbox') {
                    if (this.form_fields[i].dependency_attributes.options.length == 0) {
                        this.form_fields[i].dependency_attributes.options.push('');
                    }
                }

                if (this.form_fields[i].field_properties[0].field_type == 'select' || this.form_fields[i].field_properties[0].field_type == 'select-one') {
                    selectArr.push(this.form_fields[i].label.name);
                }
                if ((this.form_fields[i].field_properties[0].field_type != 'submit' && this.form_fields[i].field_properties[0].field_type != 'reset' && this.form_fields[i].field_properties[0].field_type != 'button' && this.form_fields[i].field_properties[0].field_type != 'line_break' && this.form_fields[i].field_properties[0].field_type != 'back' && this.form_fields[i].field_properties[0].field_type != 'print') && this.form_fields[i].validation_properties != null) {

                    if (this.form_fields[i].generic_properties.required) {
                        const validate = Validators.required;
                        this.arrays.push(Validators.required);

                        // this.validatorform.push(validate);
                    }



                    if (this.form_fields[i].validation_properties.Validators_email) {
                        const validate = Validators.email;
                        this.arrays.push(validate);
                        // this.validatorform.push(validate);
                    }

                    if (this.form_fields[i].validation_properties.Validators_min) {
                        const min: number = this.form_fields[i].validation_properties.Validators_min;
                        const validate = Validators.min(min);
                        this.arrays.push(validate);
                        //  this.validatorform.push(validate);
                    }
                    if (this.form_fields[i].validation_properties.Validators_max) {
                        const max: number = this.form_fields[i].validation_properties.Validators_max;
                        const validate = Validators.max(max);
                        this.arrays.push(validate);
                        //  this.validatorform.push(validate);
                    }
                    if (this.form_fields[i].validation_properties.Validators_minLength) {
                        const minLength: number = this.form_fields[i].validation_properties.Validators_minLength;
                        const validate = Validators.minLength(minLength);
                        this.arrays.push(validate);
                        //   this.validatorform.push(validate);
                    }
                    if (this.form_fields[i].validation_properties.Validators_maxLength) {
                        const maxLength: number = this.form_fields[i].validation_properties.Validators_maxLength;
                        const validate = Validators.maxLength(maxLength);
                        this.arrays.push(validate);
                        //this.validatorform.push(validate);
                    }

                    if (this.form_fields[i].validation_properties.RegExp) {
                        if (this.form_fields[i].validation_properties.RegExp != 'custome') {
                            const pattern_Value: string = this.form_fields[i].validation_properties.RegExp;
                            const validate = Validators.pattern(pattern_Value);
                            this.arrays.push(validate);
                        }
                    }

                    if (this.form_fields[i].validation_properties.NoWhitespaceValidator) {

                        this.arrays.push(NoWhitespaceValidator);
                        //  this.validatorform.push(this.noWhitespaceValidator);
                    }

                    if (this.arrays.length > 0) {
                        this.FormControlName[this.form_fields[i].label.name] = ['', this.arrays];
                    } else {
                        this.FormControlName[this.form_fields[i].label.name] = [];
                    }


                    this.validateform = this.fb.group(this.FormControlName);


                }
                if (this.form_fields[i].field_properties[0].field_type == 'print') {
                    this.cssValue = Object.values(this.form_fields[i].label.css_attribute)
                    //this.cssValue =  Object.entries(this.form_fields[i].label.css_attribute)
                    // alert('sdsd')
                }




                // if(this.form_fields[i].field_properties[0].field_type == 'collapse'){

                //     this.collapse = true;

                //     this.collpaseName = this.form_fields[i].label.name;

                //     var formkey = this.form_fields[i].field_properties[0].inputs_fields;
                //     const docs = document.getElementById('accordionField');


                //     docs.children[0].children[0].children[0].children[0].children[0]["innerText"] = this.collpaseName;
                //     for (var key in formkey) {
                //         if(formkey[key] != undefined){
                //             const elementAppend: HTMLDivElement = this.renderer.createElement('div');
                //             for (let value of Object.values(this.fieldhtml)) {
                //                 fieldType = formkey[key].field_properties[0].field_type;
                //                 if(fieldType != 'collapse'){
                //                 elementAppend.innerHTML = value[this.mapforComponent[fieldType]];
                //                 docs.firstChild.firstChild.childNodes[1].firstChild.appendChild(elementAppend);
                //                 }
                //             }
                //         }
                //     }
                // }
                // if(this.form_fields[i].field_properties[0].field_type == 'table'){
                //     this.table=true;
                //     var header = this.form_fields[i].field_properties[0].header;
                //     var data = this.form_fields[i].field_properties[0].table_fields;
                //     const doc = document.getElementById("excel-table");
                //     var tr = document.createElement('tr');
                //     var tbody = document.createElement('tbody');
                //     var newTableHeading = document.createElement('tr');
                //     var flag = false;
                //     for(var key in header){
                //         var th = document.createElement('th');
                //         var text1 = document.createTextNode(header[key].header_text);
                //         th.appendChild(text1);
                //         tr.appendChild(th);
                //         th.style.textAlign = "center";
                //         if(header[key].header_child.length > 0){
                //             th.colSpan = header[key].header_child.length;
                //             if(header[key].header_child.length > 0){
                //                 flag = true;
                //                 for (var k = 0; k < header[key].header_child.length; k++) {
                //                     var trs = document.createElement('th');
                //                     trs.id = header[key].header_child.header_text;
                //                     trs.classList.add('shubham1');
                //                     trs.innerText = 'Heading';
                //                     newTableHeading.appendChild(trs);
                //                 }
                //             }
                //             else{
                //                 th.rowSpan = 1; 
                //             } 
                //         }
                //         else{
                //             th.rowSpan = 2;
                //         }
                //     }
                //     tbody.appendChild(tr);
                //     if(flag){
                //     tbody.appendChild(newTableHeading);
                //     }
                //     var rows = this.form_fields[i].field_properties[0].no_rows-1;
                //     var columns = this.form_fields[i].field_properties[0].no_column;
                //     var tr1; var l =0;var fieldType
                //     for(var k=0;k<rows;k++){
                //         tr1 = document.createElement('tr');
                //         for(var j=0;j<columns;j++){
                //             var td = document.createElement('td');
                //             td.id = this.form_fields[i].field_properties[0].table_fields[l].td_text;
                //             if(this.form_fields[i].field_properties[0].table_fields[l].td_data[td.id]!=undefined){
                //                 const elementAppend: HTMLDivElement = this.renderer.createElement('div');
                //                 for (let value of Object.values(this.fieldhtml)) {
                //                     fieldType = this.form_fields[i].field_properties[0].table_fields[l].td_data[td.id].field_properties[0].field_type;
                //                     elementAppend.innerHTML = value[this.mapforComponent[fieldType]];
                //                     td.appendChild(elementAppend);
                //                 }
                //             }
                //             else{
                //                 td.appendChild(document.createTextNode("Data"));
                //             }
                //             tr1.appendChild(td);
                //             l+=1;
                //         } 
                //         tbody.appendChild(tr1);
                //     }
                //     if(flag){
                //     var x=0;
                //     var tr2 = tbody.getElementsByTagName("tr");
                //     if(tr2[1].children.length > 0 && tr2[1].children[0].nodeName.indexOf('TH')!=-1){
                //         x = 2;
                //     }
                //     else{
                //         x = 1;
                //     }
                //     for (var p = x; p < tr2.length; p++) {
                //         for (var j = 0; j < 1; j++) {
                //             var newHeadng = document.createElement('td');
                //             newHeadng.id = 'Data';
                //             newHeadng.classList.add('shubham1');
                //             newHeadng.innerText = 'Data';
                //             var clones = newHeadng.cloneNode(true);
                //             tr2[p].appendChild(newHeadng);
                //             //tr2[i].insertBefore(newHeadng, tr2[i].children[insertAfters]);
                //         }
                //     }
                //     }
                //     doc.appendChild(tbody);
                //     // var header = this.form_fields[i].field_properties[0].header;
                //     // var data = this.form_fields[i].field_properties[0].table_fields;
                //     // 
                //     // for (var key in header) {
                //     //     this.tableHeader.push(header[key].header_text)
                //     // }
                //     // this.columnCount=1;
                //     // for (var key in data) {
                //     //     if (this.columnCount <= 4){
                //     //     //this.columnCount = this.tableHeader.length;
                //     //     this.tableData.push(data[key].td_text)
                //     //     this.columnCount+=1
                //     //     }
                //     // }
                // }

            }

            /*   for (let sl in selectArr) {
                   setTimeout(function () { $("#" + selectArr[sl]).click(); }, 200);
   
               }*/
            // for(var i=1;i<=12;i++){
            //     var f  =  this.form_fields[0].field_properties[0].table_fields.slice(i*this.form_fields[0].field_properties[0].header.length , (i+1)*this.form_fields[0].field_properties[0].header.length);
            //     console.log(f)
            // }

            if (this.dataId != null) {


                if (this.DBName == 'elog3_material_master') {

                    this.DBName = 'sGet_ItemMaster';
                }
                if (this.DBName == 'elog3_erp_location_master') {

                    this.DBName = 'sGet_LocationMaster';
                }
                this._elogservice.fetchTableWiseData(this.DBName, '*', 98, 'Id', this.dataId).subscribe({
                    next: data => {
                        //    this.isProductCodeLoading = true;
                        if (data == null) {
                            abp.ui.clearBusy();
                            return;
                        }
                        abp.ui.clearBusy();
                        //  this.pusheditems[formContrlName] = JSON.parse(data);
                        // print button hide start
                        // this._route.params.subscribe((routeData: Params) => {
                            
                        //     if (this.formId == 3280 || this.formId == 1175  || this.formId == 3270  || this.formId == 3270) {
                        //         this.showmaster = true;
                        //         console.log(this.formId)
                        //     }
                        // });
                        // print button hide end
                        this.fieldData = JSON.parse(data);
                        this.form_fields.forEach(obj => {
                            const name = obj.label.name;
                            if (this.fieldData[0].hasOwnProperty(name)) {
                                const value = this.fieldData[0][name];
                                obj.label.value = (value == "true" || value == "false") ? JSON.parse(value) : (value);
                               // obj.label.value = typeof value === 'boolean' ? value : String(value);
                                //   obj.label.value = this.fieldData[0][name];

                            }
                            
                        });
                        // print button hide on add click start
                                // for (let i = 0; i < this.form_fields.length; i++) {
                                //     if (this.form_fields[i].field_properties[0].field_type == 'print') {
                                //        // this.form_fields[i].generic_properties[0].showon_add = true;
                                //     }
                                //     }
                        // print button hide on add click start
                        
                        for (let key in this.fieldData[0]) {

                            //  setTimeout(function () { $("#" + key).click(); }, 200);
                            if (key != 'Id' && key != 'undefined' && key != 'IsDeleted' && key != 'line_break' && key != 'print') {
                                try {
                                    this.validateform.controls[key].setValue(this.fieldData[0][key]);
                                }
                                catch (e) { }
                                
                            }
                        }

                        for (let sl in selectArr) {
                            setTimeout(function () { $("#" + selectArr[sl]).click(); }, 1500);

                        }
                        // this.validateform.disable(); // isssue found
                        abp.ui.clearBusy();

                    },
                    error: error => {

                        abp.ui.clearBusy();
                        console.error('There was an error!', error);
                    }
                });
            }
        });


    }

    protected delete(plant: ClientFormsDto): void {
        abp.message.confirm(
            this.l('Are you sure you want to delete this plant?', plant.clientId),
            (result: boolean) => {
                if (result) {
                    // abp.ui.setBusy();
                    // this._plantService.delete(plant.id).pipe(
                    //     finalize(() => {
                    //         abp.ui.clearBusy();
                    //     })
                    // ).subscribe(() => {
                    //     abp.notify.success(this.l('Plant deleted successfully.'));
                    //     this.plants = [];
                    //     this.pageNumber=1;
                    //     this.refresh();
                    // });
                }
            }
        );
    }

    public onScroll() {
        this.pageNumber = this.pageNumber + 1;
        this.getDataPage(this.pageNumber);
    }

    onSelect($event, plant: ClientFormsDto): void {
        // if ($event.target.classList.contains('deletePlant')) {
        //     this.delete(plant);
        // }
        // else {
        //     this.GoToViewPlant(plant);
        // }
    }

    noWhitespaceValidator(control: FormControl) {
        const isWhitespace = (control.value || '').trim().length === 0;
        const isValid = !isWhitespace;
        return isValid ? null : { 'whitespace': true };
    }

    ValidatedForm(form: any) {

        if (this.validateform.valid) {

            abp.ui.setBusy();
            var colmnName = "";
            var columnValue = "";
            var FormValidators = form.value;


            if (Object.keys(FormValidators).length) {
                let colmnNames = Object.keys(FormValidators);
                let index = 0;
                colmnNames.forEach(key => {

                    if (colmnName == "") {
                        colmnName = key;
                    }
                    else {
                        colmnName = colmnName + "," + key;
                    }

                    if (columnValue == "") {
                        columnValue = "'" + FormValidators[key] + "'";
                        if (columnValue == "'null'") {
                            columnValue = "''";
                        }


                    }

                    else {
                        if (FormValidators[key] == null) {
                            columnValue = columnValue + "," + "''";
                        }
                        else {
                            columnValue = columnValue + "," + "'" + FormValidators[key] + "'";
                        }
                    }
                });
            }


            const tableName = this.DBName.toLowerCase();
            var actionType = "INSERT";
            if (this.dataId != null) {
                actionType = "UPDATE";
            }

            // for update grid
            var FormNamewithString = this.clientFormJson.form_name.toLowerCase()
            if (actionType == "UPDATE") {

                this._elogservice.updateLogWiseData(this.dataId, tableName, colmnName, columnValue, actionType).pipe(
                    finalize(() => {
                        abp.ui.clearBusy();
                    })
                ).subscribe(data => {
                    abp.notify.success(this.FormName + ' data updated successfully.');


                    setTimeout(() => {
                        this._router.navigate(['../../../../../logData/' + FormNamewithString], { relativeTo: this._route });
                        // this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
                        //     this.router.navigate(['app/logData/'+FormNamewithunder]);
                        // }); 
                    }, 200);

                });
            }
            // for insert grid

            else {

                this._elogservice.formDataPush(tableName, colmnName, columnValue, actionType).pipe(
                    finalize(() => {
                        abp.ui.clearBusy();
                    })
                ).subscribe(data => {
                    abp.notify.success(this.FormName + ' data submitted successfully.');
                    setTimeout(() => {
                        this._router.navigate(['../../logData/' + FormNamewithString], { relativeTo: this._route });
                    }, 200);

                });
            }

        }
        else {
            Object.keys(this.validateform.controls).forEach(field => {

                const controls = this.validateform.get(field);
                controls.markAsTouched({ onlySelf: true });
            })
        }
    }

    formatLabel(value: number): string {
        if (value >= 1000) {
            return Math.round(value / 1000) + 'k';
        }

        return `${value}`;
    }

    resetFeildValues() {
        //   this.options = null;
        //   this.dataTableOption = null;
    }

    resetFormOnSelect(selectField: any, resetRequired: any) {

        var allowRest = 0;
        Object.keys(this.validateform.controls).forEach(field => {

            if (allowRest == 1) {
                const controls = this.validateform.get(field);
                if (resetRequired == true) {
                    this.validateform.get(field).setValue(null);
                    this.validateform.get(field).updateValueAndValidity();
                    try {
                        this.pusheditems[field] = null;

                    }
                    catch (e) { console.log('nothing to pop'); }
                }


            }
            if (selectField == field) {
                allowRest = 1;
            }
        })
    }

    showErrowMsg(event: any, formContrlName: any, fieldProperties: any, validation_properties: any) {
        // Error msg start
        var eventValue = event.target.value.trim()
        if (fieldProperties[0].field_type == "text" || fieldProperties[0].field_type == "search" && eventValue != '') {
            if (validation_properties.RegExp == "^[0-9]*$") {
                if (isNaN(eventValue)) {
                    var classValue = formContrlName.replaceAll(' ', '_');
                    var dummy = document.getElementsByClassName(classValue);
                    dummy[0].setAttribute("style", "display:none;")

                }

            }
            if (validation_properties.RegExp == "[a-zA-Z][a-zA-Z ]+") {
                if (!(/[a-zA-Z][a-zA-Z ]+/.test(eventValue))) {
                    var classValue = formContrlName.replaceAll(' ', '_');
                    var dummy = document.getElementsByClassName(classValue);
                    dummy[0].setAttribute("style", "display:none;")

                }

            }
            if (validation_properties.RegExp == "^[a-zA-Z]+$") {
                if (!(/^[a-zA-Z]+$/.test(eventValue))) {
                    var classValue = formContrlName.replaceAll(' ', '_');
                    var dummy = document.getElementsByClassName(classValue);
                    dummy[0].setAttribute("style", "display:none;")

                }

            }
        }
        if (eventValue == '') {
            var classValue = formContrlName.replaceAll(' ', '_');
            var dummy = document.getElementsByClassName(classValue);
            dummy[0].setAttribute("style", "display:block;")
        }


        // Error msg end
    }

    SearchPartialVal(event: any, dependancyAttr: any, formContrlName: any, fieldProperties: any, validation_properties: any) {

        let table = null;
        let fetchColumns = null;
        let filterColumn = null;
        let filterColumnValue = null;
        let depedency = null;
        let displayColumn = null;
        let bindingColumn = null;
        var Params = event.target.value;
        /* if (Params.length < 2) {
             return false;
         }*/
        // Error msg start
        /*  if (event.target.value) {
              var eventValue = event.target.value.trim()
              if (fieldProperties[0].field_type == "text") {
                  if (validation_properties.RegExp != "" && eventValue != "") {
                      if (isNaN(eventValue)) {
                          var dummy = document.getElementsByClassName(formContrlName);
                          dummy[0].setAttribute("style", "display:none;")
                          var dummy2 = document.getElementsByClassName(formContrlName + 's');
                          dummy2[0].setAttribute("style", "display:block;")
                      }
                      else if (!(/^[a-zA-Z]+$/.test(eventValue))) {
                          var dummy = document.getElementsByClassName(formContrlName);
                          dummy[0].setAttribute("style", "display:none;")
                          var dummy2 = document.getElementsByClassName(formContrlName + 's');
                          dummy2[0].setAttribute("style", "display:block;")
                      }
                      else if (!(/[a-zA-Z][a-zA-Z ]/.test(eventValue))) {
                          var dummy = document.getElementsByClassName(formContrlName);
                          dummy[0].setAttribute("style", "display:none;")
                          var dummy2 = document.getElementsByClassName(formContrlName + 's');
                          dummy2[0].setAttribute("style", "display:block;")
                      }
                  }
                  else {
                      var dummy = document.getElementsByClassName(formContrlName);
                      //var dummy2 = document.getElementsByClassName(formContrlName+'s');
                      dummy[0].setAttribute("style", "display:block;")
                      //          dummy2[0].setAttribute("style", "display:none;")
                  }
              }
  
          }*/
        // Error msg end
        if (dependancyAttr.db_table) {
            table = dependancyAttr.db_table;
        }
        if (dependancyAttr.fetch_columns) {
            fetchColumns = dependancyAttr.fetch_columns.replace(/,+/g, ',');
        }

        if (dependancyAttr.filter_object) {
            if (dependancyAttr.filter_object[0].filterColumn != "") {
                filterColumn = dependancyAttr.filter_object[0].filterColumn;
            }

        }
        if (fieldProperties[0].field_type == "search") {
            filterColumn = dependancyAttr.display_column;
        }


        if (dependancyAttr.filter_object) {
            if (dependancyAttr.filter_object[0].dependencyOn != "") {
                filterColumnValue = dependancyAttr.filter_object[0].dependencyOn;
            }

        }


        if (dependancyAttr.display_column) {
            displayColumn = dependancyAttr.display_column;
        }
        if (dependancyAttr.dependent) {
            depedency = dependancyAttr.dependent;
        }
        if (dependancyAttr.binding_column) {
            bindingColumn = dependancyAttr.binding_column;
        }

        if (!depedency) {
            return;
        }


        let Datadependency = null;
        if (dependancyAttr.depedency_on) {
            Datadependency = dependancyAttr.depedency_on;
        }

        if (Datadependency == "static") {
            this.pusheditems[formContrlName] = dependancyAttr.options;

            this.options[formContrlName] = dependancyAttr.options;
            return;
        }

        // console.log(event.target.value);

        if (!Params) {
            Params = "";
        }

        if (filterColumn != "" && filterColumnValue != "" && filterColumn != null && filterColumnValue != null) {
            if (this.selectedKeyValue[filterColumnValue].value) {
                Params = this.selectedKeyValue[filterColumnValue].value;
            }
            else {
                Params = this.selectedKeyValue[filterColumnValue];
            }
        }

        ////  this.isProductCodeLoading = true;

        abp.ui.setBusy();

        if (this.pusheditems[formContrlName] && Params == "" && this.pusheditems[formContrlName] != null) {
            abp.ui.clearBusy();
            return;
        }
        var TableData = null;

        if (dependancyAttr.thirdParty) {

            this.performGetaction(dependancyAttr, formContrlName);
        }
        else {

            this._elogservice.fetchTableWiseData(table, fetchColumns, 97, filterColumn, Params).subscribe({
                next: data => {

                    if (data == null) {
                        TableData = null;;
                    }
                    else {
                        TableData = data;
                    }



                    //  this.isProductCodeLoading = true;
                    if (data == null) {
                        abp.ui.clearBusy();
                        return;
                    }
                    abp.ui.clearBusy();
                    this.pusheditems[formContrlName] = JSON.parse(TableData);
                    this.options[formContrlName] = null;

                    this.options[formContrlName] = JSON.parse(TableData);

                    //  setTimeout(function () { $("#" + formContrlName).click(); abp.ui.clearBusy(); }, 200);


                    this.validateValue(formContrlName, event.target.value, dependancyAttr, null);
                    abp.ui.clearBusy();

                },
                error: error => {

                    abp.ui.clearBusy();
                    console.error('There was an error!', error);
                }
            });

        }

    }

    reset() {

        this.validateform.reset();
        this.keys = {};
        this.value = {};
        this.multiKeys = {};
        this.multiValues = {};
        this.staticvalue = {};
        this.statickeys = {};
        ;
    }

    validateValue(formContrlName: any, val: any, dependanantArray: any, validationProp: any) {



        let dispalyColumn = null
        if (dependanantArray.display_column) {
            dispalyColumn = dependanantArray.display_column;
        }
        if (validationProp != null) {

            if (validationProp.others != "" && validationProp.others != null) {
                for (let h = 0; h < validationProp.others.length; h++) {
                    let validTerm = validationProp.others[h].validation_term;
                    if (validationProp.others[h].validationType == 'field') {

                        if (this.selectedKeyValue[validationProp.others[h].validatorField].value > val) {

                            alert(validationProp.others[h].alertMsg);
                            if (validationProp.others[h].action == "barred") {
                                this.validateform.get(formContrlName).setValidators([null, Validators.required]);
                                this.validateform.get(formContrlName).updateValueAndValidity();
                            }
                        }
                    }
                    if (validationProp.others[h].validationType == 'value') {
                        let validateVal = validationProp.others[h].validatingValue;
                        if (validateVal == val) {
                            alert(validationProp.others[h].alertMsg);
                            if (validationProp.others[h].action == "barred") {
                                this.validateform.get(formContrlName).setValidators([null, Validators.required]);
                                this.validateform.get(formContrlName).updateValueAndValidity();
                            }

                        }
                    }
                }

            }

            //  this.performaction(dependanantArray, formContrlName);
            return;
        }
        if (dispalyColumn != "" && val != "" && val != null) {
            if (this.options[formContrlName] == null) {
                this.validateform.get(formContrlName).setValidators([this.autocompleteStringValidators(dispalyColumn, formContrlName), Validators.required]);
                this.validateform.get(formContrlName).updateValueAndValidity()
            }
            else {
                this.validateform.get(formContrlName).setValidators([this.autocompleteStringValidators(dispalyColumn, formContrlName), Validators.required]);
                this.validateform.get(formContrlName).updateValueAndValidity()
            }
        }
    }

    performaction(dependanantArray: any, formName: any) {
        if (this.validateform.valid) {
            var paramsValue = new HttpParams();
            var gridPrm = [];
            try {
                if (dependanantArray.InfoBox) {
                    this.showInfo = true;
                }
            }
            catch (e) { }
            if (dependanantArray.routing) {
                var URLparams = {};
                var URLValue = "";
                for (var b = 0; b < dependanantArray.routing.param.length;b++) {
                    var vaiableParams = dependanantArray.routing.param[b].name;
                    var vaiablParamVal = dependanantArray.routing.param[b].value;
                    if (dependanantArray.routing.param[b].custom) {
                        URLValue = vaiablParamVal;
                    } else {
                        URLValue = this.validateform.get(vaiablParamVal).value;
                    }
                    URLparams[vaiableParams] = URLValue
                }
                this.OpenURL(dependanantArray.routing.url, URLparams);
            }
            if (dependanantArray.thirdParty_attributes.length > 0) {
                for (var y = 0; y < dependanantArray.thirdParty_attributes.length; y++) {
                    if (dependanantArray.thirdParty_attributes[y].gridparams) {
                        var vaiableFrmTable = dependanantArray.thirdParty_attributes[y].valueHolder;
                        var vaiablCntrl = dependanantArray.thirdParty_attributes[y].keyHolder;
                        var dataFrmTable = this[vaiablCntrl][vaiableFrmTable];
                        for (var q = 0; q < dataFrmTable.length; q++) {
                            var fValue = null;
                            var gridval = {};
                            for (let keyval in dependanantArray.thirdParty_attributes[y].gridparams) {
                                var chValue = dependanantArray.thirdParty_attributes[y].gridparams[keyval].value;
                                if (dependanantArray.thirdParty_attributes[y].gridparams[keyval].type == "grid") {
                                    fValue = this[vaiablCntrl][vaiableFrmTable][q][chValue];
                                }
                                else {
                                    fValue = this.validateform.get(chValue).value;

                                }
                                gridval[keyval] = (fValue);
                                // paramsValue = paramsValue.append(keyval, fValue.toString());   
                            }
                            gridPrm.push(gridval);

                        }

                    }

                    for (let keyval in dependanantArray.thirdParty_attributes[y].params) {

                        var chValue = dependanantArray.thirdParty_attributes[y].params[keyval].value;

                        if (chValue) {
                            //  var fValue = $("#" + chValue).val();

                            var fValue = null;
                            if (dependanantArray.thirdParty_attributes[y].params[keyval].custom) {
                                if (dependanantArray.thirdParty_attributes[y].params[keyval].fetchData) {
                                    var calcValFrmJson = "";
                                    for (var clc = 0; clc < this.staticvalue[dependanantArray.thirdParty_attributes[y].params[keyval].arrayPointer].length; clc++) {
                                        var endpointer = dependanantArray.thirdParty_attributes[y].params[keyval].valueIdentifier;
                                        if (calcValFrmJson == "") {
                                            calcValFrmJson += this.staticvalue[dependanantArray.thirdParty_attributes[y].params[keyval].arrayPointer][clc][endpointer];
                                        }
                                        else {
                                            calcValFrmJson += "," + this.staticvalue[dependanantArray.thirdParty_attributes[y].params[keyval].arrayPointer][clc][endpointer];
                                        }

                                    }
                                    fValue = calcValFrmJson;
                                }
                                else { fValue = chValue; }

                            }
                            else {
                                try {
                                    fValue = this.validateform.get(chValue).value;
                                }
                                catch (e) {
                                    fValue = "";
                                }
                            }
                            if (fValue == null) {
                                if (dependanantArray.thirdParty_attributes[y].params[keyval].valueType && dependanantArray.thirdParty_attributes[y].params[keyval].valueType == "boolean") {
                                    fValue = false;
                                }
                                else {
                                    fValue = "";
                                }

                            }
                            paramsValue = paramsValue.append(keyval, fValue.toString());
                        }
                        else {
                            //var fValue = $("#" + chValue).val();
                            fValue = this.validateform.get(chValue).value;
                            paramsValue = paramsValue.append(keyval, null);
                        }

                    }


                    if (dependanantArray.perform_action) {
                        debugger;
                        var preCheck = false;
                        try {
                            preCheck = dependanantArray.preCheck;
                        } catch (e) {
                            preCheck = false
                        }
                        var that = this;
                        var sendType = dependanantArray.thirdParty_attributes[y].action_type;
                        var redirect_url=dependanantArray.thirdParty_attributes[y].redirect_url;
                        var paramType=dependanantArray.thirdParty_attributes[y].paramType;
                        if (preCheck) {
                            for (var ii = 0; ii < dependanantArray.preCheckCondition.length; ii++) {

                                var formulastring = dependanantArray.preCheckCondition[ii].formula;
                                const formula = String(formulastring).split("|");
                                var actulaFormula = "";
                                let caluclatedVal = 0;
                                for (var m = 0; m < formula.length; m++) {
                                    if (formula[m].includes("^^")) {
                                        actulaFormula += formula[m].replace("^^", "");
                                    }
                                    else {
                                        var formActualValue = this.validateform.get(formula[m]).value;
                                        actulaFormula += formActualValue;
                                    }

                                }

                                caluclatedVal = this.calculationCustom(actulaFormula);

                                if (caluclatedVal) {

                                }
                                else {
                                    abp.message.confirm(
                                        dependanantArray.preCheckCondition[ii].msgFail,
                                        'Do you want to proceed?',
                                        function (isConfirmed) {
                                            if (isConfirmed) {
                                                if (dependanantArray.thirdParty_attributes.length > 0) {
                                                   
                                                    if (dependanantArray.thirdParty_attributes.length > 0) {
                                                        if (paramType == 'json') {
                                                            sendType = 'json';
                                                        }
                                                        that.thirPartyAPIservice(sendType, 1, AppConsts.remoteServiceBaseUrl + redirect_url, paramsValue, dependanantArray, formName, sendType, gridPrm);
                                                    }
                                                }

                                            }
                                          
                                        }
                                    );
                                }

                            }

                        }
                        else {
                            debugger;
                            if (dependanantArray.thirdParty_attributes.length > 0) {
                               
                                if (dependanantArray.thirdParty_attributes.length > 0) {
                                    if (paramType == 'json') {
                                        sendType = 'json';
                                    }
                                    this.thirPartyAPIservice(sendType, 1, AppConsts.remoteServiceBaseUrl + redirect_url, paramsValue, dependanantArray, formName, sendType, gridPrm);
                                }
                            }
                        }
                    }
                }
            }

          
        }
        else {
            Object.keys(this.validateform.controls).forEach(field => {

                const controls = this.validateform.get(field);
                controls.markAsTouched({ onlySelf: true });
            })
        }
    }

    performGetaction(dependanantArray: any, formContrlName: any) {
        var paramsValues = new HttpParams();
        /*     for (var k = 0; k <= dependanantArray.thirdParty_attributes[y].params.length; k++) {
                 paramsValue = paramsValue.append(dependanantArray.thirdParty_attributes[y].params, "ewde");
             }*/

        if (dependanantArray.thirdParty_attributes.length > 0) {
            for (var y = 0; y < dependanantArray.thirdParty_attributes.length; y++) {
                for (let keyval in dependanantArray.thirdParty_attributes[y].params) {

                    var chValue = dependanantArray.thirdParty_attributes[y].params[keyval].value;
                    //  var fValue = $("#" + chValue).val();



                    var fValue = "";
                    if (dependanantArray.thirdParty_attributes[y].params[keyval].custom) {
                        if (dependanantArray.thirdParty_attributes[y].params[keyval].fetchData) {
                            var calcValFrmJson = "";
                            for (var clc = 0; clc < this.staticvalue[dependanantArray.thirdParty_attributes[y].params[keyval].arrayPointer].length; clc++) {
                                var endpointer = dependanantArray.thirdParty_attributes[y].params[keyval].valueIdentifier;
                                calcValFrmJson += this.staticvalue[dependanantArray.thirdParty_attributes[y].params[keyval].arrayPointer][clc][endpointer];
                            }
                            fValue = calcValFrmJson;
                        }
                        else { fValue = dependanantArray.thirdParty_attributes[y].params[keyval].value; }

                    }
                    else {
                        fValue = this.validateform.get(chValue).value;
                    }




                    if (fValue == null) {
                        fValue = "";
                    }
                    paramsValues = paramsValues.append(keyval, fValue.toString());
                }
                if (dependanantArray.perform_action) {
                    var TableData = null;
                    /*  this.http.get<any>(AppConsts.remoteServiceBaseUrl + dependanantArray.thirdParty_attributes[y].redirect_url, {'params': paramsValues }).subscribe(data => {
      
      
                         //// this.isProductCodeLoading = true;
                          if (data == null) {
                              abp.ui.clearBusy();
                              return;
                          }
                          abp.ui.clearBusy();
                          this.pusheditems[formContrlName] = data.result;
      
                          this.options = data.result;
      
                        //  setTimeout(function () { $("#" + formContrlName).click(); abp.ui.clearBusy(); }, 200);
                          //////
      
                        //  this.validateValue(formContrlName, event.target.value, dependanantArray, null);
                          abp.ui.clearBusy();
                      })*/
                  

                        this.thirPartyAPIservice(dependanantArray.thirdParty_attributes[y].action_type, 2, AppConsts.remoteServiceBaseUrl + dependanantArray.thirdParty_attributes[y].redirect_url, paramsValues, dependanantArray, formContrlName, null, null);
                    
                    }
            }
        }
        //  paramsValue = paramsValue.append('Location', "ewde");
        // paramsValue = paramsValue.append('barCode', "ewde");


    }

    autocompleteStringValidators(dispalyColumn, formContrlName): ValidatorFn {
        if (this.options[formContrlName] != undefined) {
            ;
            return (control: AbstractControl): { [key: string]: any } | null => {
                if (control.value) {
                    for (var i = 0; i < this.options[formContrlName].length; i++) {
                        if (this.options[formContrlName][i][dispalyColumn] == control.value) {
                            return null  /* valid option selected */
                        }
                    }

                    return { 'invalidAutocompleteString': { value: control.value } }
                }
                return null;
            }
        }
    }

    changeOption(event: any, psuhField: any, displayVal: any, dependancyattr: any, formContrlName: any) {
        var resetrequired = true;

        try {
            resetrequired = dependancyattr.resetRequird;
            if (dependancyattr.resetRequird == undefined) {
                resetrequired = true;
            }
        }
        catch (e) { }

        try {
            if (dependancyattr.InfoBox) {
                this.showInfo = true;
            }
        }
        catch (e) { }
       
        this.resetFormOnSelect(formContrlName, resetrequired);

        if (psuhField != null) {
            try {
                this.selectedKeyValue[psuhField] = event;
                this.selectedKeyValue[psuhField]['displayVal'] = displayVal;
            }
            catch (e) {
                console.log('ok ');
            }

        }
        if (dependancyattr) {
            try {
                debugger;
                var checkGrid = dependancyattr.changeAction.checkUnique.checkObject;
                var checkGridVal = dependancyattr.changeAction.checkUnique.checkValue;
                var checkVal = this.validateform.get(formContrlName).value;
                if (this.value[formContrlName] != undefined) {
                    for (var duplicatecheck = 0; duplicatecheck < this.value[formContrlName].length; duplicatecheck++) {
                        if (this.value[checkGrid][duplicatecheck][checkGridVal] == checkVal) {
                            this.validateform.get(formContrlName).setValue('');
                            this.validateform.get(formContrlName).updateValueAndValidity();
                            abp.notify.error("Already scanned");
                            return;

                        }

                    }
                }
            }
            catch (e) { }
            try {
                if (dependancyattr.changeAction.customAction) {
                    for (let custAction in dependancyattr.changeAction.customActionList) {

                        var actionType = dependancyattr.changeAction.customActionList[custAction].actionType;
                        var actionOn = dependancyattr.changeAction.customActionList[custAction].actionOn;
                        var customCal = dependancyattr.changeAction.customActionList[custAction].customCal;
                        var formulastring = dependancyattr.changeAction.customActionList[custAction].formula;
                        const formula = String(formulastring).split("|");
                        var actulaFormula = "";
                        let caluclatedVal = 0;
                        for (var m = 0; m < formula.length; m++) {
                            if (formula[m].includes("^^")) {
                                actulaFormula += formula[m].replace("^^", "");
                            }
                            else {
                                var formActualValue = this.validateform.get(formula[m]).value;
                                actulaFormula += formActualValue;
                            }

                        }
                        caluclatedVal = this.calculationCustom(actulaFormula);
                        this.validateform.get(actionOn).setValue(caluclatedVal);
                        this.validateform.get(actionOn).updateValueAndValidity();
                    }
                }


            }
            catch (e) { console.log('customAction type not present') }

            try {
                if (dependancyattr.changeAction.thirdParty) {
                    var TableData = null;
                    for (var y = 0; y < dependancyattr.changeAction.thirdParty_attributes.length; y++) {
                        var paramsValuetable = new HttpParams();
                        for (let keyvals in dependancyattr.changeAction.thirdParty_attributes[y].params) {

                            var chValue = dependancyattr.changeAction.thirdParty_attributes[y].params[keyvals].value;
                            //   var fValue = $("#" + chValue).val();
                            var fValue = "";
                            if (dependancyattr.changeAction.thirdParty_attributes[y].params[keyvals].custom) {
                                if (dependancyattr.thirdParty_attributes[y].params[keyvals].fetchData) {
                                    var calcValFrmJson = "";
                                    for (var clc = 0; clc < this.staticvalue[dependancyattr.thirdParty_attributes[y].params[keyvals].arrayPointer].length; clc++) {
                                        var endpointer = dependancyattr.thirdParty_attributes[y].params[keyvals].valueIdentifier;
                                        calcValFrmJson += this.staticvalue[dependancyattr.thirdParty_attributes[y].params[keyvals].arrayPointer][clc][endpointer];
                                    }
                                    fValue = calcValFrmJson;
                                }
                                else { fValue = dependancyattr.changeAction.thirdParty_attributes[y].params[keyvals].value; }

                            }
                            else {
                                try {
                                    fValue = this.validateform.get(chValue).value;
                                }
                                catch (e) {
                                    fValue = "";
                                }
                            }

                            if (fValue == null) {
                                fValue = "";
                            }
                            paramsValuetable = paramsValuetable.append(keyvals, fValue.toString());
                        }
                        try {
                            for (var j = 0; j < dependancyattr.changeAction.thirdParty_attributes[y].changeFields.length; j++) {
                            
                                if (event.value.toLowerCase() == dependancyattr.changeAction.changeFields[j].OnValue.toLowerCase()) {
                                    
                                    for (var k = 0; k < dependancyattr.changeAction.changeFields[j].action.disableFields.length; k++) {
                                        this.validateform.controls[dependancyattr.changeAction.thirdParty_attributes[y].changeFields[j].action.disableFields[k]].disable();
                                        this.validateform.get(dependancyattr.changeAction.thirdParty_attributes[y].changeFields[j].action.disableFields[k]).setValidators(null);
                                        this.validateform.get(dependancyattr.changeAction.thirdParty_attributes[y].changeFields[j].action.disableFields[k]).updateValueAndValidity();
                                        //  this.validateform.get(dependancyattr.changeAction.thirdParty_attributes[y].changeFields[j].action.disableFields[k]).disable();

                                    }

                                    for (var k = 0; k < dependancyattr.changeAction.thirdParty_attributes[y].changeFields[j].action.enableFields.length; k++) {
                                        this.validateform.controls[dependancyattr.changeAction.thirdParty_attributes[y].changeFields[j].action.enableFields[k]].enable();

                                        this.validateform.get(dependancyattr.changeAction.thirdParty_attributes[y].changeFields[j].action.disableFields[k]).setValidators([null, Validators.required]);
                                        this.validateform.get(dependancyattr.changeAction.thirdParty_attributes[y].changeFields[j].action.disableFields[k]).updateValueAndValidity();
                                        //  this.validateform.get(dependancyattr.changeAction.thirdParty_attributes[y].changeFields[j].action.disableFields[k]).enable();
                                    }
                                }

                               
                            }
                        }
                        catch (e) { }


                        this.thirPartyAPIservice(dependancyattr.changeAction.thirdParty_attributes[y].action_type, 3, AppConsts.remoteServiceBaseUrl + dependancyattr.changeAction.thirdParty_attributes[y].redirect_url, paramsValuetable, dependancyattr.changeAction.thirdParty_attributes[y], formContrlName, null, null);
                    }
                }


                if (dependancyattr.db_table != null) {
                    this.GetDataTableValueChangeValue("", dependancyattr.changeAction.db_table, dependancyattr.changeAction.fetch_columns, dependancyattr.changeAction.changeFields);
                }

            }
            catch (e) {
                console.log('Thirds party type not present')
            }
        }

    }

    calculationCustom(fn) {
        return new Function('return ' + fn)();
    }

    GetDataTableValueChangeValue(Params: any, table: any, fetchColumns: any, dipAttr: any) {


        if (!table) {
            return;
        }

        if (fetchColumns) {
            this.keys = [];
            let tableName = "";



            this._elogservice.fetchTableWiseData(table, fetchColumns, 100, null, Params).subscribe({
                next: data => {
                    //  this.dataTableOption = JSON.parse(data);  
                    this.nodata = 0;
                    if (data != null) {
                        this.valueKey = JSON.parse(data);

                        abp.ui.clearBusy();

                        for (var f = 0; f <= dipAttr.length; f++) {
                            var chValue = dipAttr[f].changeValue;
                            this.validateform.get(dipAttr[f].actionField).setValue(this.valueKey[0][chValue]);
                            this.validateform.get(dipAttr[f].actionField).updateValueAndValidity();
                        }
                    }
                },
                error: error => {
                    abp.ui.clearBusy();
                    console.error('There was an error!', error);
                }
            });
        }
    }

    processResponse(resType: any, dependanantArray: any, formContrlName: any, data: any) {

        this.valueKey = data.result;
        const myArray = String(this.valueKey).split("~");
        try {

           
            

           
            var res = myArray[0];
            if (res == 'SUCCESS') {

                abp.notify.success(myArray[1]);
            }
            if (res == 'Y') {

                abp.notify.success(myArray[1]);
            }
            else if (res == 'ERROR') {
                try {
                    this.validateform.get(formContrlName).setValue(null);
                    this.validateform.get(formContrlName).updateValueAndValidity();
                }
                catch (e) {

                }

                abp.notify.error(myArray[1]);
                return;
            }
            try {
                
                if (data.result[0].column1) {
                    const myArrayString = String(data.result[0].column1).split("~");
                    var res = myArrayString[0];
                    if (res == 'ERROR') {
                        abp.notify.error(myArrayString[1]);
                        return;
                    }
                    if (res == 'SUCCESS') {

                        abp.notify.success(myArrayString[1]);
                    }
                    if (res == 'Y') {

                        abp.notify.success(myArrayString[1]);
                    }
                }

            } catch (e) { }


     

            try {
                debugger;
                if (dependanantArray.showInfo) {
                    var info = this.valueKey[0][dependanantArray.showInfoCatcher];

                    abp.message.info(info);
                    setTimeout(function () {

                        $(".swal-button").click();
                    }, 5000);
                    return;
                }

            } catch (e) { }

            try {


                if (dependanantArray.resetAction) {
                    this.reset();
                }
                if (dependanantArray.resetDisplay) {
                    for (var z = 0; z < dependanantArray.resetDisplay.length; z++) {
                        var cntrlName = dependanantArray.resetDisplay[z].controlName;
                        var display = dependanantArray.resetDisplay[z].Display;
                        var div = document.getElementById(cntrlName);
                        if (display) {
                            div.style.display = 'block';
                        }
                        else {
                            div.style.display = 'none';
                        }

                    }
                }

                if (dependanantArray.changeAction.type) {
                    if (dependanantArray.changeAction.type == 'WEIGHING') {
                        var weighID = data.result.resultObject.id;
                        //this.GetCapturedWeight(weighID, dependanantArray.changeAction.anchor);
                    }
                }

            }
            catch (e) {

            }





            if (this.valueKey.length > 0) {
                try {
                   
                    if (!this.scanCountValues[formContrlName]) {
                        this.scanCountValues[formContrlName] = 1;
                    }
                    else {
                        this.scanCountValues[formContrlName] = this.scanCountValues[formContrlName] + 1;
                    }
                    if (dependanantArray.ShowCount) {
                        this.validateform.get(dependanantArray.ShowCountAnchor).setValue(this.scanCountValues[formContrlName]);
                        this.validateform.get(dependanantArray.ShowCountAnchor).updateValueAndValidity();
                    }
                }
                catch (e) { }
            }

            //  if (resType == 1) {
            try {
                if (dependanantArray.changeFields) {
                   
                    for (var f = 0; f < dependanantArray.changeFields.length; f++) {

                     /*   try {
                            if (dependanantArray.changeFields[f].fieldType == "select") {
                                let fillDropdownVal = [];
                                var keyPoint = dependanantArray.bindArray[0];
                                
                                for (var g = 0; g < this.valueKey[keyPoint].length; g++) {

                                    var gridValues = this.valueKey[keyPoint][g][dependanantArray.changeFields[f].changeValue];
                                    fillDropdownVal.push(gridValues);
                                }
                                this.pusheditems[dependanantArray.changeFields[f].actionField] = fillDropdownVal;
                            }

                        } catch (e) {


                        }*/



                        var chValue = dependanantArray.changeFields[f].changeValue;
                        this.validateform.get(dependanantArray.changeFields[f].actionField).setValue(this.valueKey[0][chValue]);
                        this.validateform.get(dependanantArray.changeFields[f].actionField).updateValueAndValidity();

                     
                    }
                  
                }

                if (this.valueKey != null && this.valueKey.length > 2) {

                    this.changeOption(null, formContrlName, null, dependanantArray.thirdParty_attributes[0], formContrlName)

                    //  })
                }
            }
            catch (e) { }

            //   }
            //    else if (resType == 2) {
            //  if () {
            if (dependanantArray.perform_action) {
            this.pusheditems[formContrlName] = data.result;

            this.options = data.result;
            // }
            }
            //    }
            //       else if (resType == 3) {
            // this.value = data.result;
            if (dependanantArray.type == "DISPLAY") {
                var keyPointtable = formContrlName;
                this.value[keyPointtable] = data.result;
                this.keys[keyPointtable] = [];
                for (let key in this.value[keyPointtable]) {
                    for (let keyvalHead in this.value[keyPointtable][key]) {
                        if (this.keys[keyPointtable].findIndex(elem => elem === keyvalHead) !== -1) {
                            console.log('element exist');

                        }
                        else {
                            this.keys[keyPointtable].push(keyvalHead);
                        }
                    }
                }
            }
            //   else if (resType == 4) {
            
            if (dependanantArray.type == "HOLDDISPLAY") {
                var keyPointtable = formContrlName;
                if (this.value[keyPointtable]) {
                    this.value[keyPointtable].push(data.result[0]);
                  //  this.keys[keyPointtable] = [];
                }
                else {
                    this.value[keyPointtable] = data.result;
                    this.keys[keyPointtable] = [];
                }
              
                for (let key in this.value[keyPointtable]) {
                    for (let keyvalHead in this.value[keyPointtable][key]) {
                        if (this.keys[keyPointtable].findIndex(elem => elem === keyvalHead) !== -1) {
                            console.log('element exist');

                        }
                        else {
                            this.keys[keyPointtable].push(keyvalHead);
                        }
                    }
                }
            }

            abp.ui.clearBusy();
            if (dependanantArray.type == "CHANGEVALUES") {
                for (var f = 0; f < dependanantArray.changeFields.length; f++) {
                    var chValue = dependanantArray.changeFields[f].changeValue;
                    this.validateform.get(dependanantArray.changeFields[f].actionField).setValue(this.valueKey[0][chValue]);
                    this.validateform.get(dependanantArray.changeFields[f].actionField).updateValueAndValidity();
                }
            }
            //   }
            //   else if (resType == 5) {
            if (dependanantArray.type == 'BINDMULTIPLE') {

                for (var p = 0; dependanantArray.bindArray.length >= p; p++) {

                    var bindControlName = dependanantArray.bindArray[p].bindControlName;
                    var bindResponse = dependanantArray.bindArray[p].bindResponse;
                    var bindResponseKey = dependanantArray.bindArray[p].bindResponseKey;
                    if (this.valueKey[bindResponse] && this.valueKey[bindResponse].length > 0) {

                        let fillDropdownVal = [];
                        for (var g = 0; g < this.valueKey[bindResponse].length; g++) {

                            var gridValues = this.valueKey[bindResponse][g][bindResponseKey];
                            fillDropdownVal.push(gridValues);
                            try {
                             
                                this.validateform.get(bindControlName).setValue(gridValues);
                                this.validateform.get(bindControlName).updateValueAndValidity();

                            } catch (e) { }
                        }
                        this.pusheditems[bindControlName] = fillDropdownVal;
                     
                    }
                    /*   var keyPoint1 = dependanantArray.bindArray[p];
                       if (keyPoint1 != undefined) {
                           for (var g = 0; g < dependanantArray.changeFields.length; g++) {
                               var chValue = dependanantArray.changeFields[g].changeValue;
                               this.validateform.get(dependanantArray.changeFields[g].actionField).setValue(data.result[keyPoint1][0][chValue]);
                               this.validateform.get(dependanantArray.changeFields[g].actionField).updateValueAndValidity();
                           }
                       }*/
                }

            }
            else {
                for (var p = 0; dependanantArray.bindArray.length >= p; p++) {
                    var keyPoint1 = dependanantArray.bindArray[p];
                    if (keyPoint1 != undefined) {
                        for (var g = 0; g < dependanantArray.changeFields.length; g++) {
                            var chValue = dependanantArray.changeFields[g].changeValue;
                            this.validateform.get(dependanantArray.changeFields[g].actionField).setValue(data.result[keyPoint1][0][chValue]);
                            this.validateform.get(dependanantArray.changeFields[g].actionField).updateValueAndValidity();
                        }
                    }
                }

                for (var l = 0; dependanantArray.displayArray.length >= l; l++) {
                    var keyPoint = dependanantArray.displayArray[l];
                    this.multiValues[keyPoint] = data.result[keyPoint];
                    this.multiKeys[keyPoint] = [];
                    for (let key in this.multiValues[keyPoint]) {
                        for (let keyvalHeads in this.multiValues[keyPoint][key]) {
                            if (this.multiKeys[keyPoint].findIndex(elem => elem === keyvalHeads) !== -1) {
                                console.log('element exist');

                            }
                            else {
                                this.multiKeys[keyPoint].push(keyvalHeads);
                            }



                        }


                    }
                }
            }


            //  }
        }
        catch (e) { console.log('something wrong in response') }
    }

    thirPartyAPIservice(poastType: any, resType: any, Apiurl: any, paramsValues: any, dependanantArray: any, formContrlName: any, sendType: any, gridVal: any) {
        try {

            try {
                if (dependanantArray.InfoBox) {
                    this.showInfo = true;
                }
            }
            catch (e) { }

            if (poastType == "get") {

                this.http.get<any>(Apiurl, { 'params': paramsValues }).subscribe(data => {
                    if (data == null) {
                        abp.notify.error("No Data Recieved");
                        abp.ui.clearBusy();
                        return;
                    }
                    abp.ui.clearBusy();
                    this.processResponse(resType, dependanantArray, formContrlName, data);
                })

            }
            else if (poastType == "post") {
                try {
                    if (gridVal.length == 0) {
                        gridVal = null;
                    }

                }
                catch (e) {
                    gridVal = null;
                }
                if (sendType == "json" || gridVal != null) {
                    let reqHeaders = new HttpHeaders().set('Content-Type', 'application/json');

                    var dataset = dependanantArray.thirdParty_attributes[0].paramKey;
                    var paramJson = null;
                    if (gridVal == null) {
                        gridVal = JSON.stringify(this.staticvalue[dataset]);
                    }
                    if (paramsValues != null) {
                        Apiurl = Apiurl + '?' + paramsValues;
                    }
                    this.http.post<any>(Apiurl, gridVal, { headers: reqHeaders }).subscribe(data => {
                        if (data == null) {
                            abp.notify.error("No Data Recieved");
                            abp.ui.clearBusy();
                            return;
                        }
                        this.processResponse(resType, dependanantArray, formContrlName, data);
                    })
                }
                else {

                    this.http.post<any>(Apiurl, paramsValues).subscribe(data => {
                        if (data == null) {
                            abp.notify.error("No Data Recieved");
                            abp.ui.clearBusy();
                            return;
                        }
                        this.processResponse(resType, dependanantArray, formContrlName, data);
                    })
                }
            }

            else if (poastType == "put") {
                try {
                    if (gridVal.length == 0) {
                        gridVal = null;
                    }

                }
                catch (e) {
                    gridVal = null;
                }
                if (sendType == 'json' || gridVal != null) {
                    let reqHeaders = new HttpHeaders().set('Content-Type', 'application/json');

                    var dataset = dependanantArray.thirdParty_attributes[0].paramKey;
                    var paramJson = null;
                    if (gridVal == 0) {
                        gridVal = JSON.stringify(this.staticvalue[dataset]);
                    }
                    this.http.put<any>(Apiurl, gridVal, { headers: reqHeaders }).subscribe(data => {
                        if (data == null) {
                            abp.notify.error("No Data Recieved");
                            abp.ui.clearBusy();
                            return;
                        }
                        this.processResponse(resType, dependanantArray, formContrlName, data);
                    })
                }
                else {
                    this.http.put<any>(Apiurl, paramsValues).subscribe(data => {
                        if (data == null) {
                            abp.notify.error("No Data Recieved");
                            abp.ui.clearBusy();
                            return;
                        }
                        abp.ui.clearBusy();
                        this.processResponse(resType, dependanantArray, formContrlName, data)
                    })
                }

            }
            else {
                alert('api request type not defined');
            }
        }
        catch (e) {
            console.log('cdsc');
        }

    }

    uniqueCheck(ev: any, clmn: any) {

        abp.ui.setBusy();
        var checkVal = ev.target.value;
        this._elogservice.fetchTableWiseData(this.DBName.toLowerCase(), clmn, 99, clmn, checkVal).subscribe({
            next: data => {

                if (data.length > 3) {
                    this.validateform.get(clmn).setValue('');
                    this.validateform.get(clmn).updateValueAndValidity();
                    abp.notify.error('This value is already present. This is unique field. Please use another value.');
                }

                abp.ui.clearBusy();

            },
            error: error => {

                abp.ui.clearBusy();
                console.error('There was an error!', error);
            }
        });
    }

    GetDataTableValue(Params: any, table: any, fetchColumns: any, filterColumn: any) {

        
        if (!table) {
            return;
        }
        //this.isProductCodeLoading = true;
        if (table) {
            this.keys = [];
            let tableName = "";
            this._elogservice.fetchTableWiseData(table, fetchColumns, 100, filterColumn, Params).subscribe({
                next: data => {

                    this.nodata = 0;

                    console.log(data);

                    if (data != null) {
                        this.keys = JSON.parse(data);

                        abp.ui.clearBusy();

                    }
                },
                error: error => {
                    abp.ui.clearBusy();
                    console.error('There was an error!', error);
                }
            });
        }
    }

    getObjectKeys(obj: any): string[] {
        return Object.keys(obj);
    }

    addStaticFromFieldInputs(data: any, formContrlName: any, staticGridDetails: any) {
        var tableLines = {};

        for (var z = 0; z < staticGridDetails.staticFromFieldInputs.length; z++) {
            var keyOfStaticFrm = staticGridDetails.staticFromFieldInputs[z].checkObject;
            var valOfStaticFrm = this.validateform.get(staticGridDetails.staticFromFieldInputs[z].checkObject).value;
            if (valOfStaticFrm == null || valOfStaticFrm == "") {
                abp.notify.error('Invalid Values to add');
                return;
            }
            tableLines[keyOfStaticFrm] = valOfStaticFrm;
        }
        var insert = false;
        if (this.staticvalue[formContrlName] == undefined) {
            this.staticvalue[formContrlName] = [];
            this.statickeys[formContrlName] = [];
            insert = true;
        }

        var match = 0;
        for (var c = 0; c < this.staticvalue[formContrlName].length; c++) {
            if (match == 2) {
                abp.notify.error('Invalid Values to add');
                return;
            }
            match = 1;
            for (let keyx in tableLines) {
                if (tableLines[keyx] == null || tableLines[keyx] == "") {
                    abp.notify.error('Invalid Values to add');
                    return;
                }
                if (this.staticvalue[formContrlName][c][keyx] == tableLines[keyx]) {
                    console.log('exists');
                    match = 2;
                    insert = false;
                }
                else {
                    match = 1;
                    insert = true;
                    break;
                }
            }

        }
        if (insert) {
            this.staticvalue[formContrlName].push(tableLines);

            for (let statickey in this.staticvalue[formContrlName]) {
                for (let statickeyvalHead in this.staticvalue[formContrlName][statickey]) {
                    if (this.statickeys[formContrlName].findIndex(elem => elem === statickeyvalHead) !== -1) {
                        console.log('element exist');

                    }
                    else {
                        this.statickeys[formContrlName].push(statickeyvalHead);
                    }
                }





            }

            try {
                if (staticGridDetails.resetDisplay) {
                    for (var z = 0; z < staticGridDetails.resetDisplay.length; z++) {
                        var cntrlName = staticGridDetails.resetDisplay[z].controlName;
                        var display = staticGridDetails.resetDisplay[z].Display;
                        var div = document.getElementById(cntrlName);
                        if (display) {
                            div.style.display = 'block';
                        }
                        else {
                            div.style.display = 'none';
                        }

                    }
                }
            }
            catch (e) { }

        }
        else {
            abp.notify.error('The combination already exists');
        }
    }


    displayGrid(data: any, formContrlName: any, dependanantArray: any) {
        debugger;
        //  for (var l = 0; dependanantArray.displayArray.length >= l; l++) {
     
        this.value[formContrlName] = data['Sheet1'];
        this.keys[formContrlName] = [];
        for (let key in this.value[formContrlName]) {

            for (let keyvalHeads in this.value[formContrlName][key]) {
                if (dependanantArray.DisplayColumn) {
                    if (dependanantArray.DisplayColumn.length > 0 && dependanantArray.DisplayColumn.includes(keyvalHeads)) {
                        if (this.keys[formContrlName].findIndex(elem => elem === keyvalHeads) !== -1) {
                            console.log('element exist');

                        }
                        else {
                            this.keys[formContrlName].push(keyvalHeads);
                        }
                    }
                    else {
                        if (this.keys[formContrlName].findIndex(elem => elem === keyvalHeads) !== -1) {
                            console.log('element exist');

                        }
                        else {
                            this.keys[formContrlName].push(keyvalHeads);
                        }
                    }

                }
                else {
                    if (this.keys[formContrlName].findIndex(elem => elem === keyvalHeads) !== -1) {
                        console.log('element exist');

                    }
                    else {
                        this.keys[formContrlName].push(keyvalHeads);
                    }

                }

            }


            //   }
        }
    }


    addStaticItem(data: any, formContrlName: any, staticGridDetails: any) {

        if (staticGridDetails.staticFromFieldInputs) {
            this.addStaticFromFieldInputs(data, formContrlName, staticGridDetails);
            return;
        }

        var inputValue = data.target.value;


        var checkGrid = staticGridDetails.staticGridDetails.checkObject;
        var checkGridVal = staticGridDetails.staticGridDetails.checkValue;



        if (this.staticvalue[formContrlName] != undefined) {
            for (var duplicatecheck = 0; duplicatecheck < this.staticvalue[formContrlName].length; duplicatecheck++) {
                if (this.staticvalue[formContrlName][duplicatecheck][checkGridVal] == inputValue) {
                    this.validateform.get(formContrlName).setValue('');
                    this.validateform.get(formContrlName).updateValueAndValidity();
                    abp.notify.error("Already scanned");
                    return;

                }

            }
        }
        var matched = false;
        if (this.staticvalue[formContrlName] == undefined) {
            this.staticvalue[formContrlName] = [];
            this.statickeys[formContrlName] = [];
        }
        for (var check = 0; check < this.value[checkGrid].length; check++) {
            if (this.value[checkGrid][check][checkGridVal] == inputValue) {
                matched = true;
                this.staticvalue[formContrlName].push(this.value[checkGrid][check]);
                for (let statickey in this.staticvalue[formContrlName]) {
                    for (let statickeyvalHead in this.staticvalue[formContrlName][statickey]) {
                        if (this.statickeys[formContrlName].findIndex(elem => elem === statickeyvalHead) !== -1) {
                            console.log('element exist');

                        }
                        else {
                            this.statickeys[formContrlName].push(statickeyvalHead);
                        }
                    }
                }
                break;
            }


        }

        if (!matched) {

            abp.notify.error("Invalid scan");
        }
        this.validateform.get(formContrlName).setValue('');
        this.validateform.get(formContrlName).updateValueAndValidity();
        //  this.statickeys[formContrlName] = [];

    }


    GoToApproveLog() {
        this._router.navigate(['../../../../logDataList', this.formId], { relativeTo: this._route });
    }


    // GetCapturedWeight(weighingBalanceId: number, formContrlName: any) {
    //     abp.ui.setBusy();


    //     this._selectListService.getWeightByWeighingMachineId(weighingBalanceId, false).pipe(
    //         finalize(() => {
    //             abp.ui.clearBusy();
    //         })
    //     ).subscribe((result: number) => {
    //         // return result;
    //         this.validateform.get(formContrlName).setValue(result);
    //         this.validateform.get(formContrlName).updateValueAndValidity();
    //     });
    // }

    OpenURL(routName: any,queryParam:any) {
      //  const link = this.router.serializeUrl(this.router.createUrlTree(['/app/ChecklistReports/OQC/'], { queryParams: { 'Model': 'row', 'Ng_Qty': '1' } }));

        const link = this.router.serializeUrl(this.router.createUrlTree([routName], { queryParams: queryParam }));
        this.router.navigate([]).then(result => { window.open(link, '_blank'); });

    }



    toggleAllSelection(event: any) {
        var checkProp = event.target.checked;
        var checkpoints = document.querySelectorAll('.checkboxAll')
        if (checkProp) {
            for (let i = 0; i < checkpoints.length; i++) {
                const className = checkpoints[i];
                checkpoints[i]['checked'] = true
                // if (className.startsWith('width')) {

                //     divs.parentNode['classList'].remove(className);

                // }
            }
        }

        else {
            for (let i = 0; i < checkpoints.length; i++) {
                const className = checkpoints[i];
                checkpoints[i]['checked'] = false
                // if (className.startsWith('width')) {

                //     divs.parentNode['classList'].remove(className);

                // }
            }
        }
    }
    // check and uncheck 
    checkUncheck(event: any) {
        var checkProp = event.target.checked;
        if (checkProp == true) {
            event.target['checked'] = true
        }

        else {
            event.target['checked'] = false
        }
    }


    getAdditionalData(event: any, labelName: any, dependencyAttributes: any, index: any) {

        var paramsValuetable = new HttpParams();

        var indx = index;
       
        for (let keyvals in dependencyAttributes.grid_attributes[0].params) {

            var chValue = dependencyAttributes.grid_attributes[0].params[keyvals].value;
            //   var fValue = $("#" + chValue).val();
            var fValue = "";
            if (dependencyAttributes.grid_attributes[0].params[keyvals].custom) {
                fValue = dependencyAttributes.grid_attributes[0].params[keyvals].value;

            }
            else {
                try {
                    if (dependencyAttributes.grid_attributes[0].params[keyvals].type == "grid") {
                        var vaiableFrmTable = dependencyAttributes.grid_attributes[0].valueHolder;
                        var vaiablCntrl = dependencyAttributes.grid_attributes[0].keyHolder;

                        fValue = this[vaiablCntrl][vaiableFrmTable][indx][chValue];
                        // fValue = "1";
                    }
                    else {
                        fValue = this.validateform.get(chValue).value;

                    }
                    // fValue = this.validateform.get(chValue).value;
                }
                catch (e) {
                    fValue = "";
                }
            }

            if (fValue == null) {
                fValue = "";
            }
            paramsValuetable = paramsValuetable.append(keyvals, fValue.toString());
            //   this.thirPartyAPIservice(dependencyAttributes.changeAction.thirdParty_attributes[y].action_type, 3, AppConsts.remoteServiceBaseUrl + dependancyattr.changeAction.thirdParty_attributes[y].redirect_url, paramsValuetable, dependancyattr.changeAction.thirdParty_attributes[y], formContrlName, null, null);
            this.thirPartyAPIservice(dependencyAttributes.grid_attributes[0].action_type, 3, AppConsts.remoteServiceBaseUrl + dependencyAttributes.grid_attributes[0].redirect_url, paramsValuetable, dependencyAttributes.grid_attributes[0], labelName, null, null);

        }
    }

}