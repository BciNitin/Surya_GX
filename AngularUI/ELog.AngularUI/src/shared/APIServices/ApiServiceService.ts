import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
 providedIn: 'root'
})

export class ApiServiceService {
   BasUrl = 'http://localhost:21021/api/services/app/';
 apiUrlGetMaterialMaster = 'ElogSuryaApiService/GetMaterialMaster';

  // options = new RequestOptions({
  // headers: this.getAuthorizedHeaders(),
  // responseType: ResponseContentType.Json,
  // params: params,
  // withCredentials: false
  // });

   constructor(private http: HttpClient ) {

     }



 getMaterialMaster(): Observable<any[]> {
  return this.http.get<any[]>(this.BasUrl+'ElogSuryaApiService/GetMaterialMaster');
 }

 getPlantMaster(): Observable<any[]> {
    return this.http.get<any[]>(this.BasUrl+'/ElogSuryaApiService/GetPlantMaster');
   }
   getCustomerMaster(): Observable<any[]> {
    return this.http.get<any[]>(this.BasUrl+'/ElogSuryaApiService/GetCustomerMaster');
   }

   getLineMaster(): Observable<any[]> {
    return this.http.get<any[]>(this.BasUrl+'/ElogSuryaApiService/GetLineMaster');
   }
   getStorageMaster(): Observable<any[]> {
    return this.http.get<any[]>(this.BasUrl+'/ElogSuryaApiService/GetStorageLocationMaster');
   }
   getShiftMaster(): Observable<any[]> {
    return this.http.get<any[]>(this.BasUrl+'/ElogSuryaApiService/GetSiftMaster');
   }
   DeleteSiftMasterbyid(ShiftCode:string){
    return this.http.delete(this.BasUrl+'ElogSuryaApiService/DeleteSiftMaster?ShiftCode='+ShiftCode);
   }
}