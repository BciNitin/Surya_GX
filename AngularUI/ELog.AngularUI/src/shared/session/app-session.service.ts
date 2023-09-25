import { AbpMultiTenancyService } from '@abp/multi-tenancy/abp-multi-tenancy.service';
import { Injectable } from '@angular/core';
import {
    ApplicationInfoDto,
    GetCurrentLoginInformationsOutput,
    SessionServiceProxy,
    TenantLoginInfoDto,
    UserLoginInfoDto
,ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';
import { retry } from 'rxjs/operators';

@Injectable()
export class AppSessionService {

    private _user: UserLoginInfoDto;
    private _tenant: TenantLoginInfoDto;
    private _application: ApplicationInfoDto;
    private _permission: string[];
    private _isGateEntrySubModuleActive:boolean;
    constructor(
        private _sessionService: SessionServiceProxy,
        private _abpMultiTenancyService: AbpMultiTenancyService) {
    }

    get application(): ApplicationInfoDto {
        return this._application;
    }

    get user(): UserLoginInfoDto {
        return this._user;
    }

    get userId(): number {
        return this.user ? this.user.id : null;
    }

    get tenant(): TenantLoginInfoDto {
        return this._tenant;
    }

    get tenantId(): number {
        return this.tenant ? this.tenant.id : null;
    }

    getShownLoginName(): string {
        const userName = this._user.name + ' ' + this._user.surname;

        return userName;

    }
    getShownPasswordResetDaysLeft(): number {
        return this.user.resetPasswordDaysLeft;
    }
    getRoles(): string[] {
        return this._user.roleNames;
    }
    getPermissions(): string[] {
        return this._user.permissions;
    }
    getPlantName(): string {
        return this._user.plantCode;
    }
    getUserAssingedMode(): number {
        return this._user.modeId;
    }
    getIsGateEntrySubModuleActive():boolean{
        return this._user.isGateEntrySubModuleActive;
    }
    getIsControllerMode():boolean{
        return this._user.isControllerMode;
    }
    getActiveSubmodules(): string[] {
        return this._user.roleNames;
    }
    getIsMaterialInspectionModuleSelected():boolean{
        return this._user.isMaterialInspectionModuleSelected;
    }
    isPermissionGranted(permissionName?: string) {
        if (permissionName != null && this.user.permissions != null) {
            this._permission = permissionName.split(',');
            for (var i = 0; i < this._permission.length; i++) {
                for (var j = 0; j < this.user.permissions.length; j++) {
                    if (this.user.permissions[j].toLowerCase() == this._permission[i].toLowerCase()) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    isSubmoduleActive(permissionName?: string) {
        if (permissionName != null && this.user.transactionActiveSubModules != null) {
            var submoduleNames = permissionName.split(',');
            for (var i = 0; i < submoduleNames.length; i++) {
                for (var j = 0; j < this.user.transactionActiveSubModules.length; j++) {
                    var submoduleName = submoduleNames[i].split('.', 1)[0];
                    if (this.user.transactionActiveSubModules[j].toLowerCase() == submoduleName.toLowerCase()) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    init(): Promise<boolean> {
        return new Promise<boolean>((resolve, reject) => {
            this._sessionService.getCurrentLoginInformations().toPromise().then((result: GetCurrentLoginInformationsOutput) => {
                this._application = result.application;
                this._user = result.user;
                this._tenant = result.tenant;
                if(result.user=== undefined)
                {
                
                }
                else
                {
                    localStorage.setItem('approvalLevelId',result.user.approvalLevelId.toString());
                }
                resolve(true);
            }, (err) => {
                reject(err);
            });
        });
    }

    changeTenantIfNeeded(tenantId?: number): boolean {
        if (this.isCurrentTenant(tenantId)) {
            return false;
        }

        abp.multiTenancy.setTenantIdCookie(tenantId);
        location.reload();
        return true;
    }

    private isCurrentTenant(tenantId?: number) {
        if (!tenantId && this.tenant) {
            return false;
        } else if (tenantId && (!this.tenant || this.tenant.id !== tenantId)) {
            return false;
        }

        return true;
    }

}
