import { Component, ElementRef, Injector, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
    PagedListingComponentBase,
    PagedRequestDto
} from '@shared/paged-listing-component-base';
import {
    SelectListServiceProxy,
    SelectListDto, ModuleServiceProxy, ModuleDto, ModuleListDto, ModuleListDtoPagedResultDto, ChangePswdServiceProxy
} from '@shared/service-proxies/service-proxies';
import { ActivatedRoute, Router } from '@angular/router';
import { debounceTime, distinctUntilChanged, finalize, map } from 'rxjs/operators';
import { fromEvent } from 'rxjs';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

class PagedModuleRequestDto extends PagedRequestDto {
    keyword: string;
    isActive: boolean | null;
}

@Component({
    templateUrl: './modules.component.html',
    animations: [appModuleAnimation()],
    styleUrls: ['./modules.component.less']
})

export class ModulesComponent extends PagedListingComponentBase<ModuleDto> {

    modules: ModuleListDto[] = [];
    keyword = '';
    animal: string;
    status: number | null;
    approvalStatus: number | null;
    name: number | null;
    displayName: string | null;
    description: string | null;
    sortBy: number | null;

    roleNames: SelectListDto[] | null;
    moduleSortBy: SelectListDto[] | null;
    moduleStatuses: SelectListDto[] | null;
    filterBy: string;
    sortByOrder: number | null;
    statuses: SelectListDto[] | null;
    @ViewChild('searchTextBox', { static: true }) searchTextBox: ElementRef;

    constructor(
        injector: Injector,
        private _changePwdService: ChangePswdServiceProxy, private _modulesService: ModuleServiceProxy,
        private _selectListService: SelectListServiceProxy,
        private _dialog: MatDialog,
        private _router: Router,
        private _route: ActivatedRoute
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.setTitle('Module');
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
            this.modules = [];
            this.refresh();
        });

        this.GetModuleSortBy();
        this.GetStatus();
        super.ngOnInit();
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

    RefreshListBySearch(event) {
        if (this.keyword.length > 2 && this.keyword.trim().length > 2) {
            this.refresh();
        }
        else if (this.keyword.trim().length == 0) {
            this.refresh();
        }
    }
    protected delete(entity: ModuleDto): void {
        throw new Error("Delete not implemented.");
    }
    ClearSearch() {
        this.modules = [];
        this.pageNumber = 1;
        this.name = null;
        this.status = null;
        this.approvalStatus = null;
        this.sortBy = null;
        this.filterBy = null;
        this.sortByOrder = null;
        this.refresh();
        this.isFilterOpen = !this.isFilterOpen;
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
        if (this.status && this.status != -1) {
            this.filterBy = `${this.filterBy}; Status : ${this.statuses.filter(x => x.id == this.status)[0].value}`;

        }

        if (this.sortBy && this.sortBy != -1) {
            this.filterBy = `${this.filterBy}; Sort By : ${this.moduleSortBy.filter(x => x.id == this.sortBy)[0].value} ${order}`;
        }

        if (this.filterBy && this.filterBy.length > 0) {
            this.filterBy = this.filterBy.replace(';', '');

        }
    }

    ApplySearch() {
        this.modules = [];
        this.pageNumber = 1;
        this.isFilterOpen = !this.isFilterOpen;
        this.refresh();
        this.CreateFilterString();
    }

    GetModuleSortBy() {
        this._selectListService.getSortByModule().subscribe((moduleSelectList: SelectListDto[]) => {
            this.moduleSortBy = moduleSelectList;
        });
    }

    GetStatus() {
        this._selectListService.getStatus().subscribe((statusSelectList: SelectListDto[]) => {
            this.statuses = statusSelectList;
        });
    }

    list(
        request: PagedModuleRequestDto,
        pageNumber: number,
        finishedCallback: Function
    ): void {
        request.keyword = this.keyword;
        let moduleSortBy = this.GetSortBy(this.sortBy, this.sortByOrder);

        this._modulesService
            .getAllModule(this.keyword, this.status, moduleSortBy, request.skipCount, request.maxResultCount)
            .pipe(
                finalize(() => {
                    finishedCallback();
                })
            )
            .subscribe((result: ModuleListDtoPagedResultDto) => {
                if (result.items.length > 0) {
                    this.modules = this.modules.concat(result.items);
                    this.showPaging(result, pageNumber);
                }
            });
    }


    onSelect($event, moduleDto: ModuleDto): void {
        this.GoToViewModule(moduleDto);
    }
    public onScroll() {
        console.log('scrolled...')
        this.pageNumber = this.pageNumber + 1;
        this.getDataPage(this.pageNumber);
    }
    GoToAddEditModule(roleId: number) {
        this._router.navigate(['../../../edit-module', roleId], { relativeTo: this._route });
    }
    GoToViewModule(role: any) {
        this._changePwdService.encryptPassword(role.id).subscribe(
            data => {

                this._router.navigate(['../module', 'view', data], { relativeTo: this._route });

            }
        );

        // this._router.navigate(['../module', 'view', role.id], { relativeTo: this._route });
    }
    addModule() {
        this._router.navigate(['../add-module'], { relativeTo: this._route });
    }
    GoToModulesList() {
        this._router.navigate(['../../../modules'], { relativeTo: this._route });
    }

    showCreateOrEditmoduleDialog(id?: number): void {
    }
}