import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent,HttpHandler,HttpRequest } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { SessionServiceProxy ,ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';
import { AbpSessionService } from 'abp-ng2-module/dist/src/session/abp-session.service';
import { AppSessionService } from '@shared/session/app-session.service';
import { catchError } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class PlantIdInterceptor implements HttpInterceptor
{
 
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    
    var plantHeaderItem= localStorage.getItem('plantId');
    const PlantId = plantHeaderItem!=null?plantHeaderItem:'';

    var appLevelId= localStorage.getItem('approvalLevelId');
      const UserApprovalLevelId = appLevelId != null ? appLevelId : '';
      var refreshTokenData = localStorage.getItem('refreshToken');
      var refreshToken = refreshTokenData != null ? refreshTokenData:'';
    let headers = request.headers
            .set('PlantId', PlantId)
        .set('UserApprovalLevelId', UserApprovalLevelId)
        .set('RefreshToken', refreshToken);

        const cloneReq = request.clone({ headers });

        return next.handle(cloneReq).pipe(
          catchError((error) => {
           if(error.status==300){
            abp.message.info('Another session is active for user.Please login again to proceed.', 'You are not authenticated!');
           }
            return throwError(error.message);
          }));
    
    //return next.handle(request.clone({ setHeaders: { 'PlantId':PlantId } }));
  }
}
