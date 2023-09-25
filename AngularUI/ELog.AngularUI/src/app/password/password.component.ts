import { Component, ElementRef, Injector, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { ChangePasswordDto, RequestedUsersListDto, RequestedUsersListDtoPagedResultDto, SelectListDto, SelectListServiceProxy, UserDto ,ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';

import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, finalize, map } from 'rxjs/operators';


class PagedPasswordRequestDto extends PagedRequestDto {
    keyword: string;
    isActive: boolean | null;
}
@Component({
    selector: 'app-password',
    templateUrl: './password.component.html',
    styleUrls: ['./password.component.css']
})
export class PasswordComponent extends PagedListingComponentBase<RequestedUsersListDto> {

    requestedUsers: RequestedUsersListDto[] = [];
    usersInfo: RequestedUsersListDto[] = [];
    status: number | null;
    keyword = '';
    sortBy: number | null;
    sortByOrder: number | null;
    userNameList: SelectListDto[] | null;
    firstNameList: string[] | null;
    lastNameList: string[] | null;
    userName: string | null;
    firstName: string | null;
    lastName: string | null;
    userSortBy: SelectListDto[] | null;
    filterBy: string;
    activeStatuses: SelectListDto[];
    activeStatus: number | null;
    isActive: boolean | null;
    approvalStatusId: number | null;
    approvalStatuses: SelectListDto[];
    @ViewChild('searchTextBox', { static: true }) searchTextBox: ElementRef;

    constructor(injector: Injector,
        private _changePasswordService: ChangePswdServiceProxy,
        private _selectListService: SelectListServiceProxy,
        private _dialog: MatDialog,
        private _route: ActivatedRoute,
        private _router: Router,) {
        super(injector);
    }

    ngOnInit(): void {
        this.setTitle('Password Management');

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
            this.requestedUsers = [];
            this.refresh();
        });

        this.GetUserInfoforFilter();
        this.filterBy = null;
        super.ngOnInit();
    }

    GetUserInfoforFilter() {
        this._changePasswordService
            .getAll(this.appSession.user.id, this.keyword, this.userName, this.firstName, this.lastName, '', 0, 100)
            .pipe(
                finalize(() => {
                    abp.ui.clearBusy();
                })
            )
            .subscribe((result: RequestedUsersListDtoPagedResultDto) => {
                if (result.items.length > 0) {
                    this.usersInfo = this.usersInfo.concat(result.items);
                    this.firstNameList = this.usersInfo
                        .map(item => item.firstName)
                        .filter((value, index, self) => self.indexOf(value) === index)
                    this.lastNameList = this.usersInfo
                        .map(item => item.lastName)
                        .filter((value, index, self) => self.indexOf(value) === index)
                }

            });
    }

    protected list(
        request: PagedPasswordRequestDto,
        pageNumber: number,
        finishedCallback: Function
    ): void {
        abp.ui.setBusy();
        request.keyword = this.keyword;
        request.isActive = this.isActive;
        let userSortBy = this.GetRequestedPswdSortBy(this.sortBy, this.sortByOrder);
        let userActiveInactive = null;
        if (this.activeStatus && this.activeStatus != -1) {
            userActiveInactive = this.activeStatus
        }

        this._changePasswordService
            .getAll(this.appSession.user.id, this.keyword, this.userName, this.firstName, this.lastName, userSortBy, request.skipCount, request.maxResultCount)
            .pipe(
                finalize(() => {
                    finishedCallback();
                    abp.ui.clearBusy();
                })
            )
            .subscribe((result: RequestedUsersListDtoPagedResultDto) => {
                if (result.items.length > 0) {
                    this.requestedUsers = this.requestedUsers.concat(result.items);
                    this.showPaging(result, pageNumber);
                }
            });
 }

    GetRequestedPswdSortBy(sortBy: number, orderBy: number) {
        let order = '';
        if (orderBy && orderBy != -1) {
            if (orderBy == 1) {
                order = 'asc'
            } else if (orderBy == 2) {
                order = 'desc';
            }
        }
        this._selectListService.getSortByPasswordRequestedUsers().subscribe((sortBySelectList: SelectListDto[]) => {
            this.userSortBy = sortBySelectList;
        });
        if (sortBy == 1) {
            return `UserName ${order}`;
        } else if (sortBy == 2) {
            return `FirstName ${order}`;
        } else if (sortBy == 3) {
            return `LastName ${order}`
        }
        else {
            return '';
        }
    }

    public onScroll() {
        this.pageNumber = this.pageNumber + 1;
        this.getDataPage(this.pageNumber);
    }


    ApplySearch() {
        this.requestedUsers = [];
        this.pageNumber = 1;
        this.isFilterOpen = !this.isFilterOpen;
        this.refresh();

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
        this.requestedUsers = [];
        this.pageNumber = 1;
        this.userName = null;
        this.firstName = null;
        this.lastName = null;
        this.approvalStatusId = null;
        this.sortBy = null;
        this.filterBy = null;
        this.sortByOrder = null;
        this.refresh();
        this.isFilterOpen = !this.isFilterOpen;

    }

    GoToViewUser(user: RequestedUsersListDto) {
        this._router.navigate(['../reset-password', user.userId,'view'], { relativeTo: this._route });
    }

    onSelect($event, user: RequestedUsersListDto): void {
        this.GoToViewUser(user);
    }

    protected delete(user: RequestedUsersListDto): void {

    }

}
