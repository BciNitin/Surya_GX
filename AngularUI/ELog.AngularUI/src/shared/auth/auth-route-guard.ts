import { Injectable } from '@angular/core';
import { PermissionCheckerService } from '@abp/auth/permission-checker.service';
import { AppSessionService } from '../session/app-session.service';

import {
    CanActivate, Router,
    ActivatedRouteSnapshot,
    RouterStateSnapshot,
    CanActivateChild
} from '@angular/router';
import { ModeMaster } from '@shared/PmmsEnums';

@Injectable()
export class AppRouteGuard implements CanActivate, CanActivateChild {

    constructor(
        private _router: Router,
        private _sessionService: AppSessionService,
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {

        let passwordStatus = localStorage.getItem("PasswordStatus");
        if (!this._sessionService.user) {
            this._router.navigate(['/account/login']);
            return false;
        }
        if( this._sessionService.user) {
            let remaininngDaysForResetPassword = this._sessionService.getShownPasswordResetDaysLeft();
            //console.log(this._sessionService.user);
            
            if(remaininngDaysForResetPassword <= 0) {

                console.log("session expired");
                this._router.navigate([this.selectBestRoute()]);
                 return false;

            }
           // this._router.navigate(['/app/reset-password', this._sessionService.user.id]);
          
            
        }

        if(passwordStatus === '2' ) {

             console.log("User Needs to reset his password");
             this._router.navigate([this.selectBestRoute('2')]);
             return false;

        }


        if (!route.data || !route.data['permission']) {
            return true;
        }

       
        if (this._sessionService.isPermissionGranted(route.data['permission'])) {
            let reportsPermission: string[] = ['VehicleInspectionReport.View', 'MaterialInspectionReport.View', 'CubicleAssignmentReport.View','WeighingCalibrationReport.View',
                'LineClearanceReport.View', 'AllocationReport.View', 'PickingReport.View', 'CubicleCleaningReport.View', 'EquipmentCleaningReport.View', 'DispensingReport.View', 'DispensingTrackingReport.View','DispatchReport.View'];
            if (reportsPermission.indexOf(route.data['permission']) > -1) {
                let roles: Number[] = [ModeMaster.Store, ModeMaster.Quality];
                if (!(roles.indexOf(this._sessionService.getUserAssingedMode()) > -1)) {
                    abp.notify.error("only store and quality user can access report");
                    return false;
                }
                return true;             
            }
            return true;
        }
        this._router.navigate([this.selectBestRoute()]);
        return false;
    }

    canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        return this.canActivate(route, state);
    }

    selectBestRoute(passwordStatus?:any): string {
        let remaininngDaysForResetPassword = this._sessionService.getShownPasswordResetDaysLeft();
        
        if (!this._sessionService.user) {
            return '/account/login';
        }

        if (this._sessionService.isPermissionGranted('Users.View')) {
            return '/app/admin/users';
        }

        if(remaininngDaysForResetPassword <=0 || (passwordStatus && passwordStatus === '2')) {
            return '/app/reset-password/' +this._sessionService.user.id ;
        }

        

        return '/app/home';
    }
}
