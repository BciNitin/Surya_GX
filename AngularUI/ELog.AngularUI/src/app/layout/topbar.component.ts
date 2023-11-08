import { Component, Injector, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { } from '@abp/utils/utils.service'
import { ActivatedRoute, Router } from '@angular/router';
import { ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';
@Component({
    templateUrl: './topbar.component.html',
    selector: 'top-bar',
    encapsulation: ViewEncapsulation.None
})
export class TopBarComponent extends AppComponentBase {
    shownLoginName = '';
    showCurrentTime: Date = null;
    plantCode: string = '';
    roleName: string = '';
    remaininngDaysForResetPassword: number ;
    logo: string;
    constructor(
        injector: Injector,
        private _changePwdService: ChangePswdServiceProxy,
        private _router: Router,
        private _route: ActivatedRoute,
    ) {
        super(injector);
    }

    ngOnInit() {
        this.shownLoginName = this.appSession.getShownLoginName();
        this.showCurrentTime = abp.timing.localClockProvider.now();
        this.plantCode = this.appSession.getPlantName();
        this.remaininngDaysForResetPassword =this.appSession.getShownPasswordResetDaysLeft();
       //this.remaininngDaysForResetPassword = 0;
       
        if (this.appSession.getRoles() && this.appSession.getRoles().length > 0) {
            this.roleName = this.appSession.getRoles()[0]
        }
        this.logo = this.logo = localStorage.getItem('logo');
        if(this.remaininngDaysForResetPassword <= 5 && this.remaininngDaysForResetPassword > 0) {
            this.message.info((this.l('Password will expire in '+this.remaininngDaysForResetPassword+' days ,Please reset your password.')));
            console.log("reset password")

        }

       else if(this.remaininngDaysForResetPassword <= 0) {
        this.message.info((this.l('Your Password has been expired , Please reset your password to proceed.')));
            this._router.navigate(['../app/reset-password', this.appSession.user.id], { relativeTo: this._route });
        }
    }
    GoToViewUser() {
        var sId = this.appSession.user.id.toString();
        this._changePwdService.encryptPassword(sId).subscribe(
            data => {

                this._router.navigate(['profile', 'view', data], { relativeTo: this._route });

            }
        );
      //  this._router.navigate(['profile', 'view', this.appSession.user.id], { relativeTo: this._route });
    }

    GoToResetPasswordScreen() {
        this._router.navigate(['../app/reset-password', this.appSession.user.id], { relativeTo: this._route });
    }

}
