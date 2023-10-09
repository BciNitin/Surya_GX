import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { bininput } from '@app/masters/bin/bin.component';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
import { Observable } from 'rxjs';

@Injectable({
 providedIn: 'root'
})

export class ApiServiceService {
   BasUrl = 'http://localhost:21021/api/services/app/';
   apiUrlGetMaterialMaster = 'ElogSuryaApiService/GetMaterialMaster';
 
    //content_ = JSON.stringify(body);

    options_ : any = {
   //body: this.content_,
   observe: "response",
   responseType: "blob",
   headers: new HttpHeaders({
   "Content-Type": "application/json-patch+json",
   })
   };

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

   getBinMaster(): Observable<any[]> {
    return this.http.get<any[]>(this.BasUrl+'/ElogSuryaApiService/GetBinMaster');
   }

   getPlantCode(): Observable<SelectListDto[]> {
    return this.http.get<any[]>(this.BasUrl+'selectList/GetPlantCode');
   }

   getBinCode(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl+'ElogSuryaApiService/GetBinCode');
     }

   SaveBinMaster(input:bininput) {
     const content_ = JSON.stringify(input);
      return this.http.post<any[]>(this.BasUrl+'ElogSuryaApiService/CreateBinMaster', content_,this.options_);
   }
   getBinById(Id:Int32Array): Observable<any[]> {
      debugger;
      console.log("Id",Id);
      return this.http.get<any[]>(this.BasUrl+'ElogSuryaApiService/GetBinCGetBinById?id='+Id);
     }

     getLineWorkCenterNo(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl+'/SelectList/GetLineWorkNo');
     }

     getPackingOrderNo(plancode:string): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl+'SelectList/GetPackingOrderNo?plantCode='+plancode);
     }

     GenerateSerialNumber(plancode:string): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl+'SelectList/GetPackingOrderNo?plantCode='+plancode);
     }
     GetSerialNumberDetails(packingOrderNo:string): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl+'ElogSuryaApiService/GetSerialNumberDetails?packingOrder='+packingOrderNo);
     }
}