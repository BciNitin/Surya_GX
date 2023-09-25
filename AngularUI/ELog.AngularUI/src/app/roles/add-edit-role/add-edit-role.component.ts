import { Component, Injector, OnInit, Injectable } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { ErrorStateMatcher } from '@angular/material';
import { finalize, retry, debounce } from 'rxjs/operators';
import * as _ from 'lodash';
import { AppComponentBase, MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
    RoleServiceProxy,
    RoleDto,
    CreateRoleDto,
    RolePermissionsDto,
    ActionDto,
    SelectListDto,
    SelectListServiceProxy,
    ApprovalStatusDto,
    ChangePswdServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { FormGroupDirective, NgForm, FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import * as moment from 'moment';


@Component({
    templateUrl: './add-edit-role.component.html',
    animations: [appModuleAnimation()],
    styleUrls: ['./add-edit-role.component.less'],
})

export class AddEditRoleComponent extends AppComponentBase
    implements OnInit {
    saving = false;
    approvalStatusId: number;
    isOnlyView: boolean = false;
    isModuleSubModuleFlag: boolean = false;
    routeEncrypt: any;

    roleId: number;
    role: RoleDto = new RoleDto();
    RolePermissions: RolePermissionsDto[] = [];
    Permissions: ActionDto[] = [];
    allSubModuleSelected: boolean;
    allPermissionSelected: boolean;
    subModulesCheckedList: any;
    permissionsCheckedList: any;
    roleStatuses: SelectListDto[] | null;
    isdescriptionValid:boolean=true;
    isSuperAdminRole=false;
    constructor(
        injector: Injector,
        private _changePwdService: ChangePswdServiceProxy,private _roleService: RoleServiceProxy,
        private _selectListService: SelectListServiceProxy,
        private _router: Router,
        private _route: ActivatedRoute,
        private formBuilder: FormBuilder,
    ) {
        super(injector);
        this.allSubModuleSelected = false;
    }

    addEditFormGroup: FormGroup = this.formBuilder.group({
        nameFormControl: ['', [Validators.required,Validators.maxLength(32),NoWhitespaceValidator]],
        displayNameFormControl: ['', [Validators.required,Validators.maxLength(64),NoWhitespaceValidator]],
        descriptionFormControl: ['',[Validators.maxLength(5000)]],
        isActiveFormControl: [''],
        approvalStatusFormControl   : [''],
    });

    matcher = new MyErrorStateMatcher();

    ngOnInit(): void {
        this.setTitle('Add Role');
        this.role = new RoleDto();
        this.role.isActive = true;
        this._roleService.getAllSubModulesWithPermissions().subscribe((data) => {
            if (!this.isOnlyView) {
                this.RolePermissions = data;
                this.isModuleSubModuleFlag = true;
            }
        });

        this.GetRoleStatus();
        this._route.params.subscribe((routeData: Params) => {
            if (routeData['roleId']) {
                this.routeEncrypt = routeData['roleId'];
                let paramId = this._changePwdService.decryptPassword(routeData['roleId']);
                paramId.subscribe(id => {

                    this.roleId = parseInt(id);
                    this.GetRole(this.roleId);
                    this.setTitle('Edit Role');
                    this.addEditFormGroup.controls['approvalStatusFormControl'].disable();  
               });

            }
           
            if (routeData['action'] == 'view') {
                this.isOnlyView = true;
                this.markGroupDisabled(this.addEditFormGroup)
                this.setTitle('Role Details');
            }
            
            
        })
    }

   
    GetRoleStatus() {
        this._selectListService.getApprovalStatus().subscribe((modeSelectList: SelectListDto[]) => {
            this.roleStatuses = modeSelectList;
        });
    }

    isModuleSubModule(subModule: string, permission: string):boolean {
        if ((subModule == 'Module' || subModule == 'SubModule') && (permission == 'Add' || permission == 'Delete')) {
            return true;
        }
       
    }
    isSuperAdminPermission(subModule: string,superAdminPermission:boolean,permission: string):boolean{               
            if (subModule!='Password')
            {
                if(permission == 'Add' || permission == 'Edit' || permission == 'Delete' || permission == 'Print' || permission == 'Approver' || permission==null){
                return  superAdminPermission;
            }
        }
           
            
                 
    }
    IsPasswordSubModule(subModule: string,superAdminPermission:boolean,permission: string)
    {
        if (subModule=='Password')
        {  
            if(permission == 'Add'  || permission == 'Delete' || permission == 'Print' || permission == 'Approver' || permission==null)
                return superAdminPermission  ;
        }
    }

    isPermissionPresent(moduleSubModuleId: number, PermissionId: number): boolean {
        let isPresent;
        let module = this.RolePermissions.find(module => module.moduleSubModuleId == moduleSubModuleId);

        module.grantedPermissions.forEach(modulePermission => {
            if (modulePermission.id == PermissionId) {
                isPresent = true;
            } else { isPresent = false; }
        });

        return isPresent;
    }

    checkUncheckAllSubModules() {

        this.RolePermissions.filter(x=>!x.isSuperAdminPermission).forEach(a => {
            a.isGranted = this.allSubModuleSelected;
            a.grantedPermissions.forEach(p => {
                if ((a.subModuleName == 'Module' || a.subModuleName == 'SubModule') && (p.action == 'Add' || p.action == 'Delete')) {
                    p.isGranted = false;
                } else {
                    p.isGranted = this.allSubModuleSelected;
                }
            })
        })
        this.getCheckedSubModuleList();     
    }

    checkUncheckSubModulePermissions(moduleSubModuleId) {
       
        this.allSubModuleSelected = this.RolePermissions.filter(a => a.isGranted == false).length > 0 ? false : true;
        let module = this.RolePermissions.find(module => module.moduleSubModuleId == moduleSubModuleId);
        if (module != null) {
            for (var j = 0; j < module.grantedPermissions.length; j++) {

                module.grantedPermissions[j].isGranted = module.isGranted;
            }
        }
        this.getCheckedPermissionList();
    }

    selectModule() {
        this.allSubModuleSelected=this.RolePermissions.filter(a => a.isGranted == false).length > 0?false:true
    };

    selectSubModule(moduleSubModuleId) {
        let module = this.RolePermissions.find(module => module.moduleSubModuleId == moduleSubModuleId);
        if (module != null) {
            for (var j = 0; j < module.grantedPermissions.length; j++) {
                if (module.grantedPermissions[j].isGranted == false) {
                    module.isGranted = false;
                    return;
                }
            }
            module.isGranted = true;
        }
    };

    getCheckedSubModuleList() {
        this.subModulesCheckedList = [];
        for (var i = 0; i < this.RolePermissions.length; i++) {
            if (this.RolePermissions[i].isGranted)
                this.subModulesCheckedList.push(this.RolePermissions[i]);
        }
        this.subModulesCheckedList = JSON.stringify(this.subModulesCheckedList);
    }
    getCheckedPermissionList() {
        this.permissionsCheckedList = [];
        this.RolePermissions.filter(x=>!x.isSuperAdminPermission).forEach(rolePermission => {
            rolePermission.grantedPermissions.forEach(permission => {
                this.permissionsCheckedList.push(permission);
            })
        })
        this.permissionsCheckedList = JSON.stringify(this.permissionsCheckedList);
    }
    onPermissionChange(event, moduleSubModuleId, permissionId) {
        if (event.checked) {
            //update permissions on edit
            let rolePermissionDto: RolePermissionsDto = new RolePermissionsDto();
            let actionDto: ActionDto = new ActionDto();

            let module = this.RolePermissions.find(module => module.moduleSubModuleId == moduleSubModuleId);
            if (module != null) {
                let action = module.grantedPermissions.find((permission => permission.id == permissionId));
                if (action != null) {
                    action.isGranted = true;
                }
            }
            else {
                //add permissions on create
                actionDto.id = permissionId;
                rolePermissionDto.moduleSubModuleId = moduleSubModuleId;
                rolePermissionDto.grantedPermissions.push(actionDto);
                module.grantedPermissions.push(actionDto);
                this.RolePermissions.push(rolePermissionDto);
            }
        }
        else {
            //remove permission on unchecked and remove module if no permissions granted
            let module = this.RolePermissions.find(module => module.moduleSubModuleId == moduleSubModuleId);
            this.Permissions = module.grantedPermissions;
            let permission = module.grantedPermissions.find(action => action.id == permissionId);
            if (permission != null) {
                permission.isGranted = false;
                module.grantedPermissions = this.Permissions;
            }
        }
        this.role.modulePermissions = this.RolePermissions;
        this.selectSubModule(moduleSubModuleId);
        this.selectModule();
    }

    MarkViewAlsoCheckedIfAny(){
        this.RolePermissions.forEach(module=>{
            if(module){
                let grantedActions=module.grantedPermissions.filter(x=>x.action.toLowerCase()!="view" && x.isGranted);
                if(grantedActions && grantedActions.length>0){
                    let viewGrantAction=module.grantedPermissions.filter(x=>x.action.toLowerCase()=="view");
                    if(viewGrantAction && viewGrantAction.length>0){
                        viewGrantAction[0].isGranted=true;
                    }
                }
            }
        });
    }

    markDirty() {
        this.markGroupDirty(this.addEditFormGroup);
        return true;
    }

    GetCreateRoleDtoFromRoleDto(role: RoleDto) {
        let createRoleDto: CreateRoleDto = new CreateRoleDto();
        createRoleDto.name = role.name;
        createRoleDto.displayName = role.displayName;
        createRoleDto.description = role.description;
        createRoleDto.isDeleted = role.isDeleted;
        createRoleDto.isActive = role.isActive;
        createRoleDto.createdOn = moment(new Date());
        createRoleDto.modulePermissions = this.RolePermissions;
        return createRoleDto;
    }
    

    AddRole() {
        this.MarkViewAlsoCheckedIfAny();
        let rolToAdd = this.role;
        let createRoleDto: CreateRoleDto = this.GetCreateRoleDtoFromRoleDto(rolToAdd);

        const role_ = new CreateRoleDto();
        role_.init(createRoleDto);
        abp.ui.setBusy();
        this._roleService
            .create(role_)
            .pipe(
                finalize(() => {
                    abp.ui.clearBusy();
                })
            )
            .subscribe(() => {
                abp.notify.success('Role added successfully.');
                this.GoToRoleListFromAdd();
            });
    }

    UpdateRole(): void {
        abp.ui.setBusy();
        this.MarkViewAlsoCheckedIfAny();
        this._roleService
            .update(this.role)
            .pipe(
                finalize(() => {
                    abp.ui.clearBusy();
                })
            )
            .subscribe(() => {
                abp.notify.success('Role updated successfully.');
                this.GoToRoleListFromEdit();
            });
    }

    GoToRoleListFromView() {
        this._router.navigate(['../../../roles'], { relativeTo: this._route });
    }
    GoToRoleListFromEdit() {
        this._router.navigate(['../../roles'], { relativeTo: this._route });
    }
    GoToRoleListFromAdd() {
        this._router.navigate(['../roles'], { relativeTo: this._route });
    }
    GoToViewRole(roleId: number) {
        this._router.navigate(['../role', 'view', this.routeEncrypt], { relativeTo: this._route });
    }
    GoToAddEditRole(roleId: number) {
        this._router.navigate(['../../../edit-role', this.routeEncrypt], { relativeTo: this._route });
    }

    GetRole(roleId: number) {
        abp.ui.setBusy();
        this._roleService.get(roleId).pipe(
            finalize(() => {
                abp.ui.clearBusy();
            })
        ).subscribe((successData: RoleDto) => {
            this.role = successData;
            this.isSuperAdminRole=this.role.isSuperAdminRole;
            if (this.role.modulePermissions.length == 0) {
                this.role.modulePermissions = this.RolePermissions;
            } else {
                this.RolePermissions = this.role.modulePermissions;
            }
            this.selectModule();
        });
    }

    
    ApproveRole(){
        this.isdescriptionValid=true;
        let approvalStatusEntity=new ApprovalStatusDto();
        approvalStatusEntity.id=this.roleId;
        approvalStatusEntity.approvalStatusId=2;
        approvalStatusEntity.description=this.role.approvalStatusDescription;
        this._roleService.approveOrRejectRole(approvalStatusEntity).pipe(
            finalize(() => {
                abp.ui.clearBusy();
            })
        ).subscribe(result=>{

            abp.notify.success('Role approved successfully.');
            this.GoToRoleListFromView();
        })
    }

    RejectRole(){
        if(!this.role.approvalStatusDescription){
            this.isdescriptionValid=false;
            return true;
        }else if(this.role.approvalStatusDescription){
            if(this.role.approvalStatusDescription.trim().length==0){
                this.isdescriptionValid=false;
                return true;
            }
        }
        this.isdescriptionValid=true;
        let approvalStatusEntity=new ApprovalStatusDto();
        approvalStatusEntity.id=this.roleId;
        approvalStatusEntity.approvalStatusId=3;
        approvalStatusEntity.description=this.role.approvalStatusDescription;
        this._roleService.approveOrRejectRole(approvalStatusEntity).pipe(
            finalize(() => {
                abp.ui.clearBusy();
            })
        ).subscribe(result=>{

            abp.notify.success('Role rejected successfully.');
            this.GoToRoleListFromView();
        })
    }
}