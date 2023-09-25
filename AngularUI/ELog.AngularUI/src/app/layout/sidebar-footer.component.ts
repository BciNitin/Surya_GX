import { Component, Injector, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { AppConsts } from '@shared/AppConsts';
import { AppAuthService } from '@shared/auth/app-auth.service';

@Component({
    templateUrl: './sidebar-footer.component.html',
    selector: 'sidebar-footer',
    encapsulation: ViewEncapsulation.None
})
export class SideBarFooterComponent extends AppComponentBase  {

    versionText: string;
    currentYear: number;
    buildVersion: string;

    constructor(
        injector: Injector,private _authService: AppAuthService
    ) {
        super(injector);

        this.currentYear = new Date().getFullYear();
        this.versionText = this.appSession.application.version + ' [' + this.appSession.application.releaseDate.format('YYYYDDMM') + ']';
        this.buildVersion = AppConsts.buildVersion;
    }

    ngOnInit():void {
       
       /* window.onunload = () => {
            console.log("Logging out automatically");
            this.logout();

        }*/ 
    }

    logout(): void {
        abp.utils.deleteCookie("All"); 
        localStorage.setItem('plantId','');       
            this._authService.logout();
    }
}
