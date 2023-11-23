import { Component, ElementRef, Injector, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material';
import { debounceTime, distinctUntilChanged, filter, finalize, map } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';
import { UserServiceProxy, UserDto, UsersListDtoPagedResultDto, UsersListDto, SelectListServiceProxy, SelectListDto, ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';
import { UsersFilterDialog } from './usersfilter-dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { relative } from 'path';
import { FormControl } from '@angular/forms';
import * as moment from 'moment';
import { fromEvent } from 'rxjs';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

class PagedUsersRequestDto extends PagedRequestDto {
    keyword: string;
    isActive: boolean | null;
}

@Component({
    templateUrl: './users.component.html',
    animations: [appModuleAnimation()],
    styleUrls: ['./users.component.less']
})
export class UsersComponent extends PagedListingComponentBase<UserDto> {
    users: UsersListDto[] = [];
    keyword = '';
    isActive: boolean | null;
    name: '';
    rolename: '';
    plants: SelectListDto[] | null;
    plantId: number | null;
    creationFromDate: moment.Moment | null;
    creationToDate: moment.Moment | null;
    maxDate: Date;
    mode: number | null;
    designation: number | null;
    sortBy: number | null;
    sortByOrder: number | null;

    userModes: SelectListDto[] | null;
    userSortBy: SelectListDto[] | null;
    userDesignations: SelectListDto[] | null;
    filterBy: string;
    activeStatuses: SelectListDto[];
    activeStatus: number | null;

    approvalStatusId:number|null;
    approvalStatuses: SelectListDto[];

    @ViewChild('searchTextBox', { static: true }) searchTextBox: ElementRef;

    constructor(
        injector: Injector,
        private _changePwdService: ChangePswdServiceProxy,private _userService: UserServiceProxy,
        private _selectListService: SelectListServiceProxy,
        private _dialog: MatDialog,
        private _router: Router,
        private _route: ActivatedRoute
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.setTitle('User Management');
        this.maxDate = moment(new Date()).toDate();
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
            this.users = [];
            this.refresh();
        });

        // this.GetModes();
        // this.GetUserDesignations();
        // this.GetUserSortBy();
        // this.GetApprovalStatuses();
        // this.GetPlants();
        this.filterBy = null;
        // this.GetActiveInactiveStatus();
        super.ngOnInit();
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
            return `UserListDto.UserName ${order}`;
        } else if (sortBy == 2) {
            return `UserListDto.CreationTime ${order}`;
        } else if (sortBy == 3) {
            return `UserListDto.ApprovalStatusId ${order}`
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
        if(!this.sortByOrder){
            order = '; Order : Asc'
        }
        if (this.sortByOrder == 2) {
                order = '; Order : Desc';
        }
        if (this.plantId) {
            this.filterBy = `${this.filterBy}; Plant Id : ${this.plants.filter(x => x.id == this.plantId)[0].value}`;
        }
        if (this.creationFromDate && !this.creationToDate) {
            this.filterBy = `${this.filterBy}; Creation From Date : ${moment(this.creationFromDate).format('DD/MM/YYYY')}`;
        }
        if (this.creationToDate && !this.creationFromDate) {
            this.filterBy = `${this.filterBy}; Creation To Date : ${moment(this.creationToDate).format('DD/MM/YYYY')}`;
        }
        if (this.creationFromDate && this.creationToDate) {
            this.filterBy = `${this.filterBy}; Creation Date Range : ${moment(this.creationFromDate).format('DD/MM/YYYY')}-${moment(this.creationToDate).format('DD/MM/YYYY')}`;
        }
        if (this.mode && this.mode != -1) {
            this.filterBy = `${this.filterBy}; Mode : ${this.userModes.filter(x => x.id == this.mode)[0].value}`;
        }
        if (this.approvalStatusId && this.approvalStatusId != -1) {
            this.filterBy = `${this.filterBy}; Approval Status : ${this.approvalStatuses.filter(x => x.id == this.approvalStatusId)[0].value}`;

        }
        if (this.activeStatus && this.activeStatus != -1) {
            this.filterBy = `${this.filterBy}; Status : ${this.activeStatuses.filter(x => x.id == this.activeStatus)[0].value}`;

        }
        if (this.designation && this.designation != -1) {
            this.filterBy = `${this.filterBy}; Designation : ${this.userDesignations.filter(x => x.id == this.designation)[0].value}`;
        }
        if (this.sortBy && this.sortBy != -1) {
            this.filterBy = `${this.filterBy}; Sort By : ${this.userSortBy.filter(x => x.id == this.sortBy)[0].value} ${order}`;
        }

        if (this.filterBy && this.filterBy.length > 0) {

            this.filterBy = this.filterBy.replace(';', '');
            
        }

    }
    ApplySearch() {
        this.users = [];
        this.pageNumber = 1;
        this.isFilterOpen = !this.isFilterOpen;
        this.CreateFilterString();
        this.refresh();

    }
    ClearSearch() {
        this.users = [];
        this.pageNumber = 1;
        this.creationFromDate = null;
        this.creationToDate = null;
        this.mode = null;
        this.approvalStatusId = null;
        this.plantId = null;
        this.designation = null;
        this.sortBy = null;
        this.filterBy = null;
        this.activeStatus = null;
        this.sortByOrder = null;
        this.refresh();
        this.isFilterOpen = !this.isFilterOpen;

    }
    GetPlants() {
        this._selectListService.getPlantsOnUser().subscribe((plantSelectList: SelectListDto[]) => {
            this.plants = plantSelectList;
        });
    }
    GetActiveInactiveStatus() {
        this._selectListService.getStatus().subscribe((activeSelectList: SelectListDto[]) => {
            this.activeStatuses = activeSelectList;
        });
    }

    GetModes() {
        this._selectListService.getModes().subscribe((modeSelectList: SelectListDto[]) => {
            this.userModes = modeSelectList;
        });
    }

    GetUserDesignations() {
        this._selectListService.getDesignation().subscribe((designationSelectList: SelectListDto[]) => {
            this.userDesignations = designationSelectList;
        });
    }

    GetApprovalStatuses() {
        this._selectListService.getApprovalStatus().subscribe((approvalStatusSelectList: SelectListDto[]) => {
            this.approvalStatuses = approvalStatusSelectList;
        });
    }

    GetUserSortBy() {
        this._selectListService.getSortByUser().subscribe((sortBySelectList: SelectListDto[]) => {
            this.userSortBy = sortBySelectList;
        });
    }

    GoToViewUser(user: any) {
        this._changePwdService.encryptPassword(user.id).subscribe(
            data => {

                this._router.navigate(['../user', 'view', data], { relativeTo: this._route });

            }
        );

        // this._router.navigate(['../user', 'view', user.id], { relativeTo: this._route });
    }
   
    addUser() {
        debugger;
        this._router.navigate(['../add-user'], { relativeTo: this._route });
    }

    protected list(
        request: PagedUsersRequestDto,
        pageNumber: number,
        finishedCallback: Function
    ): void {
        abp.ui.setBusy();
        request.keyword = this.keyword;
        request.isActive = this.isActive;
        let userSortBy = this.GetSortBy(this.sortBy, this.sortByOrder);
        let userActiveInactive = null;
        if (this.activeStatus && this.activeStatus != -1) {
            userActiveInactive = this.activeStatus
        }

        this._userService
            .getAll(this.keyword,this.plantId, this.mode, this.designation, this.approvalStatusId, userActiveInactive, this.creationFromDate,this.creationToDate,userSortBy, request.skipCount, request.maxResultCount)
            .pipe(
                finalize(() => {
                    finishedCallback();
                    abp.ui.clearBusy();
                })
            )
            .subscribe((result: UsersListDtoPagedResultDto) => {
                if(result.items.length >0)
                {
                    this.users = this.users.concat(result.items);
                    this.showPaging(result, pageNumber);
                }
                
            });

            
    }
    
    public onScroll() {  
        this.pageNumber = this.pageNumber + 1;
        this.getDataPage(this.pageNumber);
      } 
      onSelect($event,user: UserDto): void
      {
        debugger;
         if($event.target.classList.contains('deleteUser'))
          {
              this.delete(user);
          }
          else
          {
              this.GoToViewUser(user);
          }
      }
    protected delete(user: UserDto): void {
        abp.message.confirm(
            this.l('Are you sure you want to delete this user?', user.userName),
            (result: boolean) => {
                if (result) {
                    abp.ui.setBusy();
                    this._userService.delete(user.id).pipe(
                finalize(() => {
                    
                    abp.ui.clearBusy();
                })
            ).subscribe(() => {
                        abp.notify.success(this.l('User deleted successfully.'));
                        this.users = [];
                        this.pageNumber=1;
                        this.refresh();
                    });
                }
            }
        );
    }

    
}