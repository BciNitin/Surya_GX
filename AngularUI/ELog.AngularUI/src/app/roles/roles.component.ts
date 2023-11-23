import { Component, ElementRef, Injector, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
    PagedListingComponentBase,
    PagedRequestDto
} from '@shared/paged-listing-component-base';
import {
    RoleServiceProxy, RoleDto, RoleListDtoPagedResultDto, UsersListDto, SelectListServiceProxy,
    SelectListDto, RoleListDto, ChangePswdServiceProxy
} from '@shared/service-proxies/service-proxies';
import { RolesFilterDialog } from './rolesfilter-dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, finalize, map } from 'rxjs/operators';
import { fromEvent } from 'rxjs';
import * as moment from 'moment';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

class PagedRolesRequestDto extends PagedRequestDto {
    keyword: string;
    isDeleted: boolean | null;
}

@Component({
    templateUrl: './roles.component.html',
    animations: [appModuleAnimation()],
    styleUrls: ['./roles.component.less']
})

export class RolesComponent extends PagedListingComponentBase<RoleDto> {
    roles: RoleListDto[] = [];
    keyword = '';
    animal: string;
    status: number | null;
    name: number | null;
    displayName: string | null;
    description: string | null;
    sortBy: number | null;

    roleNames: SelectListDto[] | null;
    roleSortBy: SelectListDto[] | null;
    filterBy: string;
    sortByOrder: number | null;
    statuses: SelectListDto[] | null;
    approvalStatusId:number|null;
    approvalStatuses: SelectListDto[];
    @ViewChild('searchTextBox', { static: true }) searchTextBox: ElementRef;

    constructor(
        injector: Injector,
        private _changePwdService: ChangePswdServiceProxy,private _rolesService: RoleServiceProxy,
        private _selectListService: SelectListServiceProxy,
        private _dialog: MatDialog,
        private _router: Router,
        private _route: ActivatedRoute
    ) {
        super(injector);
    }
    ngOnInit(): void {
        debugger;
        this.setTitle('Role Management');
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
            this.roles = [];
            this.refresh();
        });
        this.GetRoles();
        this.GetRoleSortBy();
        this.GetApprovalStatuses();
        this.GetStatus();
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
            return `Name ${order}`;
        } else if (sortBy == 2) {
            return `DisplayName ${order}`;
        } else if (sortBy == 3) {
            return `Description ${order}`
        }
        else if (sortBy == 4) {
            return `ApprovalStatusId ${order}`
        }
        else if (sortBy == 5) {
            return `isActive ${order}`
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
    ClearSearch() {
        this.roles = [];
        this.pageNumber = 1;
        this.name = null;
        this.status = null;
        this.approvalStatusId = null;
        this.sortBy = null;
        this.filterBy = null;
        this.sortByOrder = null;
        this.refresh();
        this.isFilterOpen = !this.isFilterOpen;
        

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
        if (this.status && this.status != -1) {
            this.filterBy = `${this.filterBy}; Status : ${this.statuses.filter(x => x.id == this.status)[0].value}`;

        }
        if (this.approvalStatusId && this.approvalStatusId != -1) {
            this.filterBy = `${this.filterBy}; Approval Status : ${this.approvalStatuses.filter(x => x.id == this.approvalStatusId)[0].value}`;
        }

        if (this.sortBy && this.sortBy != -1) {
            this.filterBy = `${this.filterBy}; Sort By : ${this.roleSortBy.filter(x => x.id == this.sortBy)[0].value} ${order}`;
        }


        if (this.filterBy && this.filterBy.length > 0) {
            this.filterBy = this.filterBy.replace(';', '');
            
        }
    }
    ApplySearch() {
        this.roles = [];
        this.pageNumber = 1;
        this.isFilterOpen = !this.isFilterOpen;
        this.refresh();
        this.CreateFilterString();

    }
    GetRoles() {
        debugger;
        this._selectListService.getRoles().subscribe((modeSelectList: SelectListDto[]) => {
            this.roleNames = modeSelectList;
        });
    }

    GetRoleSortBy() {
        this._selectListService.getSortByRole().subscribe((roleSelectList: SelectListDto[]) => {
            this.roleSortBy = roleSelectList;
        });
    }
    GetApprovalStatuses() {
        this._selectListService.getApprovalStatus().subscribe((approvalStatusSelectList: SelectListDto[]) => {
            this.approvalStatuses = approvalStatusSelectList;
        });
    }
    GetStatus() {
        this._selectListService.getStatus().subscribe((statusSelectList: SelectListDto[]) => {
            this.statuses = statusSelectList;
        });
    }
    list(
        request: PagedRolesRequestDto,
        pageNumber: number,
        finishedCallback: Function
    ): void {
        debugger;
        request.keyword = this.keyword;
        let roleSortBy = this.GetSortBy(this.sortBy, this.sortByOrder);
        
        this._rolesService
            .getAll(this.keyword, this.approvalStatusId, this.status, roleSortBy, request.skipCount, request.maxResultCount)
            .pipe(
                finalize(() => {
                    finishedCallback();
                    
                })
            )
            .subscribe((result: RoleListDtoPagedResultDto) => {
                
                if(result.items.length >0)
                {
                    this.roles = this.roles.concat(result.items);
                    this.showPaging(result, pageNumber);
                }
            });
    }
    public onScroll() {  
        console.log('scrolled...')
        this.pageNumber = this.pageNumber + 1;
        this.getDataPage(this.pageNumber);
      } 
    delete(role: RoleDto): void {
        abp.message.confirm(
            this.l('Are you sure you want to delete this role?', role.displayName),
            (result: boolean) => {
                if (result) {
                    this._rolesService
                        .delete(role.id)
                        .pipe(
                            finalize(() => {
                               
                            })
                        )
                        .subscribe(() => {
                            abp.notify.success(this.l('Role deleted successfully.'));
                            this.roles = [];
                            this.pageNumber=1;
                            this.refresh();
                          
                         });
                }
            }
        );
    }

    GoToAddEditRole(roleId: number) {
        this._router.navigate(['../../../edit-role', roleId], { relativeTo: this._route });
    }
    GoToViewRole(role: any) {
        this._changePwdService.encryptPassword(role.id).subscribe(
            data => {

                this._router.navigate(['../role', 'view', data], { relativeTo: this._route });

            }
        );

        // this._router.navigate(['../role', 'view', role.id], { relativeTo: this._route });
    }
    addRole() {
        this._router.navigate(['../add-role'], { relativeTo: this._route });
    }
    GoToRolesList() {
        this._router.navigate(['../../../roles'], { relativeTo: this._route });
    }
    editRole(role: RoleDto): void {
        this.showCreateOrEditRoleDialog(role.id);
    }

    showCreateOrEditRoleDialog(id?: number): void {
    }

    openDialog(): void {
        let dialogRef = this._dialog.open(RolesFilterDialog, {
            width: '250px',
            backdropClass: 'no-backdrop',
            panelClass: 'container-position',
            data: { name: this.name, animal: this.animal }
        });

        dialogRef.afterClosed().subscribe(result => {
            this.animal = result;
        });
    }
    onSelect($event,role: any): void
    {
       if($event.target.classList.contains('deleteRole'))
        {
            this.delete(role);
        }
        else
        {
            this.GoToViewRole(role);
        }
    }
}