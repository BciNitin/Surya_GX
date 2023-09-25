import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import {  finalize } from 'rxjs/operators';
import * as _ from 'lodash';
import { AppComponentBase, MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ModuleServiceProxy, SubModuleDto, SelectListDto, SelectListServiceProxy, ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';
import { FormGroup, Validators, FormBuilder, FormArray } from '@angular/forms';
import { RouterExtService } from './RouterExtService';

@Component({
    templateUrl: './add-edit-subModule.component.html',
    animations: [appModuleAnimation()],
    styleUrls: ['./add-edit-subModule.component.less'],
})

export class AddEditSubModuleComponent extends AppComponentBase
    implements OnInit {
    isOnlyView: boolean = false;
    isFromModule: boolean = false;

    isFirst: boolean = true;
    subModuleId: number;
    subModule: SubModuleDto = new SubModuleDto();
    subModules: SubModuleDto[];
    subModuleTypes: SelectListDto[] | null;

    constructor(
        injector: Injector,
        private _changePwdService: ChangePswdServiceProxy,private _modulesService: ModuleServiceProxy,
        private _routerExtService: RouterExtService,
        private _selectListService: SelectListServiceProxy,
        private _router: Router,
        private _route: ActivatedRoute,
        private formBuilder: FormBuilder
    ) {
        super(injector);
    }

    addEditFormGroup: FormGroup = this.formBuilder.group({
        nameFormControl: ['', [Validators.required, Validators.maxLength(32), NoWhitespaceValidator]],
        displayNameFormControl: ['', [Validators.required, Validators.maxLength(64), NoWhitespaceValidator]],
        descriptionFormControl: ['', [Validators.required, Validators.maxLength(5000), NoWhitespaceValidator]],
        isActiveFormControl: [''],
        subModuleTypeIdFormControl: ['', Validators.required],
        isApprovalRequiredFormControl:['']
    });

    matcher = new MyErrorStateMatcher();

    ngOnInit(): void {
        this.setTitle('Add Sub Module');
        this.subModule = new SubModuleDto();
        this.subModule.isActive = true;
        this.GetSubModuleType();
        this._route.params.subscribe((routeData: Params) => {
            if (routeData['subModuleId']) {
                let paramId = this._changePwdService.decryptPassword(routeData['subModuleId']);
                paramId.subscribe(id => {
                    this.subModuleId = parseInt(id);
                    this.GetModule(this.subModuleId);
                    this.setTitle('Edit Sub-Module');
                                });

             }

            if (routeData['action'] == 'view') {
                this.isOnlyView = true;
                this.markGroupDisabled(this.addEditFormGroup)
                this.setTitle('Sub Module Details');
            }
            if (routeData['action'] == 'edit') {
                this.addEditFormGroup.controls['nameFormControl'].disable();
                this.addEditFormGroup.controls['subModuleTypeIdFormControl'].disable();
            }

        })
    }

    GetSubModuleType() {
        this._selectListService.getSubModuleType().subscribe((modeSelectList: SelectListDto[]) => {
            this.subModuleTypes = modeSelectList;
        });
    }
    mapItems(items) {
        let selectedItems = items.filter((item) => item.checkbox).map((item) => item.id);
        return selectedItems.length ? selectedItems : null;
    }
   

    get subModulesArray() {
        return this.addEditFormGroup.get('subModuleCheckboxGroup').get("subModulesArray") as FormArray;
    }

    markDirty() {
        this.markGroupDirty(this.addEditFormGroup);
        return true;
    }

    someSelected(event, subModule) {
        let module = this.subModules.find(module => module.id == subModule.id);
        if (event.checked) {

            if (module != null) {
                module.isSelected = true;
            }
        } else { module.isSelected = false; }

    }

    UpdateModule(): void {
        abp.ui.setBusy();
        this._modulesService
            .updateSubModule(this.subModule)
            .pipe(
                finalize(() => {
                    abp.ui.clearBusy();
                })
            )
            .subscribe(() => {
                abp.notify.success('Sub-module updated successfully.');
                this.GoToModuleListFromEdit();
            });
    }

    GoToModuleListFromView() {
        this._router.navigate(['../../../subModules'], { relativeTo: this._route });
    }

    GoToModuleListFromEdit() {

        let previous = this._routerExtService.getPreviousUrl();

        if (previous) {
            this._router.navigateByUrl(previous);
        }
    }
    GoToModuleListFromAdd() {
        this._router.navigate(['../subModules'], { relativeTo: this._route });
    }
    GoToViewModule(subModuleId: number) {

        this._router.navigate(['../subModule', 'view', subModuleId], { relativeTo: this._route });
    }
    GoToAddEditModule(subModuleId: number) {
        this._router.navigate(['../../../edit-subModule', 'edit', subModuleId], { relativeTo: this._route });
    }

    GetModule(moduleId: number) {
        abp.ui.setBusy();
        this._modulesService.getSubModule(moduleId).pipe(
            finalize(() => {
                abp.ui.clearBusy();
            })
        ).subscribe((successData: SubModuleDto) => {
            this.subModule = successData;
        });
    }
}