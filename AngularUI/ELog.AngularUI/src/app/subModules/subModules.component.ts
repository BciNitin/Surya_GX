import { Component, ElementRef, Injector, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { SelectListServiceProxy, SelectListDto, ModuleServiceProxy, SubModuleDto, SubModuleListDtoPagedResultDto, SubModuleListDto, ModuleDto, ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute, Router } from '@angular/router';
import { debounceTime, distinctUntilChanged, finalize, map } from 'rxjs/operators';
import { fromEvent } from 'rxjs';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

class PagedSubModuleRequestDto extends PagedRequestDto {
    keyword: string;
    isActive: boolean | null;
    moduleId: number | null;
}

@Component({
    templateUrl: './subModules.component.html',
    animations: [appModuleAnimation()],
    styleUrls: ['./subModules.component.less']
})

export class SubModulesComponent extends PagedListingComponentBase<SubModuleDto> {
    protected delete(entity: SubModuleDto): void {
        throw new Error("Method not implemented.");
    }

    subModules: SubModuleListDto[] = [];
    keyword = '';
    animal: string;
    status: number | null;
    name: number | null;
    displayName: string | null;
    description: string | null;
    sortBy: number | null;
    moduleId: number | null;
    approvalRequired: number | null;
    subModuleSortBy: SelectListDto[] | null;
    filterBy: string;
    sortByOrder: number | null;
    statuses: SelectListDto[] | null;
    modules: SelectListDto[] | null;
    @ViewChild('searchTextBox', { static: true }) searchTextBox: ElementRef;

    constructor(
        injector: Injector,
        private _changePwdService: ChangePswdServiceProxy,private _modulesService: ModuleServiceProxy,
        private _selectListService: SelectListServiceProxy,
        private _dialog: MatDialog,
        private _router: Router,
        private _route: ActivatedRoute
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.setTitle('Sub-Module');
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
            this.subModules = [];
            this.refresh();
        });

        this.GetModuleSortBy();
        this.GetStatus();
        this.GetModules();
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
        }
        else if (sortBy == 3) {
            return `isActive ${order}`
        } else if (sortBy == 4) {
            return `SubModuleType ${order}`
        } else if (sortBy == 5) {
            return `ModuleName ${order}`
        } else if (sortBy == 6) {
            return `UserEnteredApprovalRequired ${order}`
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
        this.subModules = [];
        this.pageNumber = 1;
        this.name = null;
        this.status = null;
        this.moduleId = null;
        this.sortBy = null;
        this.filterBy = null;
        this.sortByOrder = null;
        this.approvalRequired = null;
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
        if (this.moduleId && this.moduleId != -1) {
            this.filterBy = `${this.filterBy}; Module : ${this.modules.filter(x => x.id == this.moduleId)[0].value} ${order}`;
        }
        if (this.approvalRequired) {
            this.filterBy = `${this.filterBy}; Is Approval Required : ${this.approvalRequired == 1 ? "Required" : "Not Required"}`;
        }
        if (this.status && this.status != -1) {
            this.filterBy = `${this.filterBy}; Status : ${this.statuses.filter(x => x.id == this.status)[0].value}`;
        }

        if (this.sortBy && this.sortBy != -1) {
            this.filterBy = `${this.filterBy}; Sort By : ${this.subModuleSortBy.filter(x => x.id == this.sortBy)[0].value} ${order}`;
        }

        if (this.filterBy && this.filterBy.length > 0) {
            this.filterBy = this.filterBy.replace(';', '');
        }
    }

    ApplySearch() {
        this.subModules = [];
        this.pageNumber = 1;
        this.isFilterOpen = !this.isFilterOpen;
        this.refresh();
        this.CreateFilterString();
    }

    GetModuleSortBy() {
        this._selectListService.getSortBySubModule().subscribe((subModuleSelectList: SelectListDto[]) => {
            this.subModuleSortBy = subModuleSelectList;
        });
    }

    GetStatus() {
        this._selectListService.getStatus().subscribe((statusSelectList: SelectListDto[]) => {
            this.statuses = statusSelectList;
        });
    }

    GetModules() {
        this._selectListService.getModules().subscribe((moduleSelectList: SelectListDto[]) => {
            this.modules = moduleSelectList;
        });
    }

    list(
        request: PagedSubModuleRequestDto,
        pageNumber: number,
        finishedCallback: Function
    ): void {
        request.keyword = this.keyword;
        let moduleSortBy = this.GetSortBy(this.sortBy, this.sortByOrder);
        if (moduleSortBy == "") {
            moduleSortBy = "SubModuleType ASC";
        }
        this._modulesService
            .getAllSubModule(this.keyword, this.status, this.moduleId, this.approvalRequired, moduleSortBy, request.skipCount, request.maxResultCount)
            .pipe(
                finalize(() => {
                    finishedCallback();
                })
            )
            .subscribe((result: SubModuleListDtoPagedResultDto) => {
                if (result.items.length > 0) {
                    this.subModules = this.subModules.concat(result.items);
                    this.showPaging(result, pageNumber);
                }
            });
    }
    onSelect($event, subModule: SubModuleListDto): void {
        this.GoToViewModule(subModule);
    }
    public onScroll() {
        console.log('scrolled...')
        this.pageNumber = this.pageNumber + 1;
        this.getDataPage(this.pageNumber);
    }
    GoToAddEditModule(subModuleId: number) {
        this._router.navigate(['../../../edit-subModule', subModuleId], { relativeTo: this._route });
    }
    GoToViewModule(subModule: any) {
        this._changePwdService.encryptPassword(subModule.id).subscribe(
            data => {

                this._router.navigate(['../subModule', 'view', data], { relativeTo: this._route });

            }
        );

        // this._router.navigate(['../subModule', 'view', subModule.id], { relativeTo: this._route });
    }
    addModule() {
        this._router.navigate(['../add-subModule'], { relativeTo: this._route });
    }
    GoToModulesList() {
        this._router.navigate(['../../../subModules'], { relativeTo: this._route });
    }
}