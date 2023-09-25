import { Component, ElementRef, Injector, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material';
import { debounceTime, distinctUntilChanged, filter, finalize, map } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';
import { SelectListServiceProxy, SelectListDto, ClientFormsDtoPagedResultDto, ClientFormsServiceServiceProxy, ClientFormsDto, ChangePasswordDto ,ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute, Router } from '@angular/router';
import { fromEvent } from 'rxjs';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { date } from '@rxweb/reactive-form-validators';
import * as moment from 'moment';

class PagedClientFormsRequestDto extends PagedRequestDto {
    keyword: string;
    isActive: boolean | null;
}
@Component({
    selector:"app-approve-forms-list",
    templateUrl:"./approve-forms-list.component.html",
    animations:[appModuleAnimation()],
    styleUrls:['./approve-forms-list.component.css']
})
export class ApproveFormlistComponent extends PagedListingComponentBase<ClientFormsDto> {

        // debugger;
        plants: ClientFormsDto[] = [];
        keyword = '';
        isActive: boolean | null;
        id: number;
        ClientId: number;
        FormName: string;
        FormStartDate: moment.Moment;
        FormEndDate: moment.Moment;
        FormJson: string;
        IsActive: boolean;
        CreationDate: moment.Moment;
        ModifiedDate: moment.Moment;
    
        countryId: number | null;
        approvalStatusId: number | null;
        plantTypeId: number | null;
        status: number | null;
        sortBy: number | null;
        sortByOrder: number | null;
    
        filterBy: string;
        activeStatuses: SelectListDto[];
        approvalStatuses: SelectListDto[];
        countries: SelectListDto[];
        plantSortBy: SelectListDto[];
        activeStatus: number | null;
        plantTypes: SelectListDto[];
        plantName: string | null;
        currentdate:any;
        modules:any;
        @ViewChild('searchTextBox', { static: true }) searchTextBox: ElementRef;
    
        constructor(
            injector: Injector,
            private _clientFormsService: ClientFormsServiceServiceProxy,
            private _selectListService: SelectListServiceProxy,
            private _dialog: MatDialog,
            private _router: Router,
            private _route: ActivatedRoute,
      
            private _changePwdService:ChangePswdServiceProxy,
        ) {
            super(injector);
        }
    
        ngOnInit(): void {
           
            this.currentdate=new Date();
            this.setTitle('Client Forms');
            //console.log(this.appSession.getShownLoginName());
            // console.log(this.plants)
            fromEvent(this.searchTextBox.nativeElement, 'keyup').pipe(
    
                // get value
                map((event: any) => {
                    return event.target.value;
                })
                // Time in milliseconds between key events
                , debounceTime(1000)
    
                // If previous query is diffent from current
                , distinctUntilChanged()
    
                // subscription for response
            ).subscribe((text: string) => {
                this.plants = [];
                this.refresh();
            });
    
            this.filterBy = null;
            this.GetActiveInactiveStatus();
            this.GetCountries();
            this.GetPlantsortBy();
            this.GetApprovalStatuses();
            super.ngOnInit();
        }
        GoToApprovePlant(ID:any)
        {
            const status=2;
            this._clientFormsService.get(ID).subscribe((res:ClientFormsDto)=>{
                if(res)
                {
                    this.modules = {
                        "clientId": 0,
                        "formName": res.formName,
                        "formStartDate":res.formStartDate,
                        "formEndDate":res.formEndDate,
                        "formJson": res.formJson,
                        "isActive": res.isActive,
                        "creationDate":res.creationDate,
                        "modifiedDate":res.modifiedDate,
                        "formStatus":status,
                        "approvedBy":'Approved',
                        "checkedBy":'',
                        "createdBy":this.appSession.getShownLoginName(),
                        "approveDateTime":moment().format('YYYY-MM-DD hh:mm:ss a'),
                        "id": ID
                    }
                    this._clientFormsService.update(this.modules).subscribe((res:ClientFormsDto)=>{
                        if(res)
                        {
                            alert('Approved successfully!');
                            //window.location.reload();
                            this.displaydata();
                        }
                    })
                }
            })
        }
        GoToDisapprovePlant(ID:any)
        {
            const status=3;
            this._clientFormsService.get(ID).subscribe((res:ClientFormsDto)=>{
                if(res)
                {
                    this.modules={
                        "clientId": 0,
                        "formName": res.formName,
                        "formStartDate":res.formStartDate,
                        "formEndDate":res.formEndDate,
                        "formJson": res.formJson,
                        "isActive": res.isActive,
                        "creationDate":res.creationDate,
                        "modifiedDate":res.modifiedDate,
                        "formStatus":status,
                        "approvedBy":'disapproved',
                        "checkedBy":'',
                        "createdBy":this.appSession.getShownLoginName(),
                        "approveDateTime":moment().format('YYYY-MM-DD hh:mm:ss a'),
                        "id": ID
                    };
                    this._clientFormsService.update(this.modules).subscribe((res:ClientFormsDto)=>{
                        if(res)
                        {
                            alert('Disapproved successfully!');
                            //window.location.reload();
                            this.displaydata();
                        }
                    })
                }
            })
           
        }
        displaydata()
        {
            this.plants=[];
            this._clientFormsService.getAll( null , this.ClientId, this.FormName,null, 0, this.FormStartDate,'',null,null,0,10)
            .pipe(
                finalize(() => {
                    finishedCallback();
                    abp.ui.clearBusy();
                })
            )
            .subscribe((result: ClientFormsDtoPagedResultDto) => {
                if (result.items.length > 0) {
                   // console.log(result.items);
                    this.plants = this.plants.concat(result.items);  
                    //console.log(this.plants)                
                    this.showPaging(result, this.pageNumber);
                }
            });
        }
        ChangedIsActive(val:any)
        {
            return (val==true) ? 'Yes' :'No';
        }
        ChangedFormStatus(val:any)
        {
         
            switch (val) {
                case 0:
                return 'Pending'
                case 1:
                return 'Submitted'
                case 2:
                return 'Approved'
                case 3:
                return 'Rejected'
                default:
                break;
            }
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
            // if (this.plantTypeId) {
            //     this.filterBy = `${this.filterBy}; Plant Type : ${this.plantTypeId == 1 ? 'Master Plant' : 'Sub Plant'}`;
            // }
            // if (this.countryId) {
            //     this.filterBy = `${this.filterBy}; Country : ${this.countries.filter(x => x.id == this.countryId)[0].value}`;
            // }
            // if (this.activeStatus && this.activeStatus != -1) {
            //     this.filterBy = `${this.filterBy}; Status : ${this.activeStatuses.filter(x => x.id == this.activeStatus)[0].value}`;
            // }
            // if (this.approvalStatusId && this.approvalStatusId != -1) {
            //     this.filterBy = `${this.filterBy}; Approval Status : ${this.approvalStatuses.filter(x => x.id == this.approvalStatusId)[0].value}`;
    
            // }
            if (this.sortBy && this.sortBy != -1) {
                this.filterBy = `${this.filterBy}; Sort By : ${this.plantSortBy.filter(x => x.id == this.sortBy)[0].value} ${order}`;
            }
    
            if (this.filterBy && this.filterBy.length > 0) {
                this.filterBy = this.filterBy.replace(';', '');
            }
        }
        ApplySearch() {
            this.plants = [];
            this.pageNumber = 1;
            this.isFilterOpen = !this.isFilterOpen;
            this.CreateFilterString();
            this.refresh();
    }

    GoToViewCreateForm(ID: any) {
    const baseUrl = document.getElementsByTagName('base')[0].href;
    if (ID) {
        this._router.navigate(['../Creator', ID], { relativeTo: this._route });
    }
    else {

        this._router.navigate(['../Creator'], { relativeTo: this._route });
    }
}
        ClearSearch() {
            this.plants = [];
            this.pageNumber = 1;
            this.ClientId = null;
            this.FormName = null;
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
        //    this._router.navigate(['../../elog-panel/add-edit-forms'], { relativeTo: this._route });
        console.log(plant);
        this._router.navigate(['../createforms',plant.id], { relativeTo: this._route });
     
        // this._router.navigate([], { relativeTo: this._route }).then(result => {  window.open('../app/createform?Id='+plant.id, '_parent'); });
        
            // this._changePwdService.encryptPassword(plant.id).subscribe(
            //     data => {
            //         console.log(data);
            //         this._router.navigate(['../../elog-panel/add-edit-forms', 'view', data], { relativeTo: this._route });
    
            //     }
            // );
            
        }
        GetApprovalStatuses() {
            this._selectListService.getApprovalStatus().subscribe((approvalStatusSelectList: SelectListDto[]) => {
                this.approvalStatuses = approvalStatusSelectList;
            });
        } 
        addPlant() {
            this._router.navigate(['../add-edit-forms'], { relativeTo: this._route });
     
            // this._router.navigate([], { relativeTo: this._route }).then(result => {  window.open('../../elog-panel/add-edit-forms', '_blank'); });
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
    
            this._clientFormsService
            .getAll( null , this.ClientId, this.FormName,null, 0, this.FormStartDate,null,null,null,0,100)
            .pipe(
                finalize(() => {
                    finishedCallback();
                    abp.ui.clearBusy();
                })
            )
            .subscribe((result: ClientFormsDtoPagedResultDto) => {
                if (result.items.length > 0) {
                    this.plants = this.plants.concat(result.items);  
                    console.log(this.plants)                
                    this.showPaging(result, pageNumber);
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
        // getMasterPlantName(ClientId:number){
        //   return this.plants.filter(
        //     plant => plant.id === ClientId).map(x=>x.ClientId);
        // }
        onSelect($event, plant: ClientFormsDto): void {
            // if ($event.target.classList.contains('deletePlant')) {
            //     this.delete(plant);
            // }
            // else {
            //     this.GoToViewPlant(plant);
            // }
        }
    
}


function finishedCallback() {
    throw new Error('Function not implemented.');
}
