import { Component, Injector, OnInit, Injectable } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { filter, finalize} from 'rxjs/operators';
import * as _ from 'lodash';
import { AppComponentBase, MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
    SelectListDto,
    SelectListServiceProxy,
    ModuleServiceProxy,
    ModuleDto,
    SubModuleDto,
    ChangePswdServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { FormGroup, Validators, FormBuilder, FormArray, FormControl, ValidatorFn } from '@angular/forms';
import * as moment from 'moment';

@Component({
    templateUrl: './add-edit-module.component.html',
    animations: [appModuleAnimation()],
    styleUrls: ['./add-edit-module.component.less'],
})

export class AddEditModuleComponent extends AppComponentBase
    implements OnInit {
    isOnlyView: boolean = false;
    isFirst: boolean = true;
    moduleId: number;
    module: ModuleDto = new ModuleDto();
    subModules: SubModuleDto[];
    constructor(
        injector: Injector,
        private _changePwdService: ChangePswdServiceProxy,private _modulesService: ModuleServiceProxy,
        private _router: Router,
        private _route: ActivatedRoute,
        private formBuilder: FormBuilder,
    ) {
        super(injector);
    }
    
    addEditFormGroup: FormGroup = this.formBuilder.group({
        nameFormControl: ['', [Validators.required, Validators.maxLength(32), NoWhitespaceValidator]],
        displayNameFormControl: ['', [Validators.required, Validators.maxLength(64), NoWhitespaceValidator]],
        descriptionFormControl: ['', [Validators.required, Validators.maxLength(5000), NoWhitespaceValidator]],
        isActiveFormControl: [''],
    });

    matcher = new MyErrorStateMatcher();

    ngOnInit(): void {
        this.setTitle('Add Module');
        this.module = new ModuleDto();
        this.module.isActive = true;   
        
        this._route.params.subscribe((routeData: Params) => {
            if (routeData['moduleId']) {
                let paramId = this._changePwdService.decryptPassword(routeData['moduleId']);
                paramId.subscribe(id => {

                    this.moduleId = parseInt(id);
                    this.GetModule(this.moduleId);
                    this.setTitle('Edit Module');
                    });
            }

            if (routeData['action'] == 'view') {
                this.isOnlyView = true;
                this.markGroupDisabled(this.addEditFormGroup)
                this.setTitle('Module Details');
            }
            if (routeData['action'] == 'edit') {
                this.addEditFormGroup.controls['nameFormControl'].disable();
            }

        })
    }
   

    markDirty() {
 
        this.markGroupDirty(this.addEditFormGroup);
        return true;
    }
    
    isSubModuleAssigned(): boolean {
        if (this.subModules == null) {
            return false;
        }
        return this.subModules.filter(t => t.isSelected).length > 0;
    }

    someSelected(event,subModule) {
        let module = this.subModules.find(module => module.id == subModule.id);
        if (event.checked) {
          
            if (module != null) {
                module.isSelected = true;
            }
        } else { module.isSelected = false; }
        
    }
    

    UpdateModule(): void {
        abp.ui.setBusy();

        this.module.subModules = this.subModules;
        this._modulesService
            .update(this.module)
            .pipe(
                finalize(() => {
                    abp.ui.clearBusy();
                })
            )
            .subscribe(() => {
                abp.notify.success('Module updated successfully.');
                this.GoToModuleListFromEdit();
            });
    }

    GoToModuleListFromView() {
        this._router.navigate(['../../../modules'], { relativeTo: this._route });
    }
    GoToModuleListFromEdit() {
        this._router.navigate(['../../../modules'], { relativeTo: this._route });
    }
    GoToModuleListFromAdd() {
        this._router.navigate(['../modules'], { relativeTo: this._route });
    }
    GoToViewModule(moduleId: number) {
        this._router.navigate(['../module', 'view', moduleId], { relativeTo: this._route });
    }
    GoToAddEditModule(moduleId: number) {
        this._router.navigate(['../../../edit-module', 'edit',moduleId], { relativeTo: this._route });
    }

    GetModule(moduleId: number) {
        abp.ui.setBusy();
        this._modulesService.get(moduleId).pipe(
            finalize(() => {
                abp.ui.clearBusy();
            })
        ).subscribe((successData: ModuleDto) => {
            this.module = successData;
            this.subModules = this.module.subModules;
        });
    }
}