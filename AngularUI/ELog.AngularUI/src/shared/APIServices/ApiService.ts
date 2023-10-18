import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { bininput } from '@app/masters/bin/bin.component';
import { linework } from '@app/PlantOperation/line-work-center/line-work-center.component';
import { GenerateSerialNumber } from '@app/PlantOperation/serialbarcodegeneration/serialbarcodegeneration.component';
import { PackingOrder } from '@app/PlantOperation/packing-order-confirmation/packing-order-confirmation.component';

import { SelectListDto } from '@shared/service-proxies/service-proxies';
import { Observable } from 'rxjs';

@Injectable({
   providedIn: 'root'
})

export class ApiServiceService {
   //BasUrl = 'http://180.151.246.51:8089/api/services/app/';
   BasUrl = 'http://localhost:21021/api/services/app/';
   apiUrlGetMaterialMaster = 'ElogSuryaApiService/GetMaterialMaster';

   //content_ = JSON.stringify(body);

   // options_: any = {
   //    //body: this.content_,
   //    observe: "response",
   //    responseType: "blob",
   //    headers: new HttpHeaders({
   //       "Content-Type": "application/json-patch+json",
   //    }),
   // };

   //  httpOptions = {
   //    headers: new HttpHeaders({
   //      'Content-Type':  'application/json',
   //      'Access-Control-Allow-Origin':'*'
   //    })
   //  };

   httpOptions = {
      headers: new HttpHeaders({
     'Content-Type':  'application/json',
     'Access-Control-Allow-Origin':'*'
   })
 };

   constructor(private http: HttpClient) {

   }

   getMaterialMaster(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'ElogSuryaApiService/GetMaterialMaster');
   }

   getPlantMaster(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'ElogSuryaApiService/GetPlantMaster');
   }
   getCustomerMaster(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'ElogSuryaApiService/GetCustomerMaster');
   }
   getPackingMasters(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'ElogSuryaApiService/GetPackingMaster');

   }
   getLineMaster(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'ElogSuryaApiService/GetLineMaster');
   }

   getBinMaster(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'ElogSuryaApiService/GetBinMaster');
   }

   getPlantCode(): Observable<SelectListDto[]> {
      const httpOptions = {
         headers: new HttpHeaders({
           'Content-Type':  'application/json',
           'Access-Control-Allow-Origin':'*'
         })
       };
      return this.http.get<any[]>(this.BasUrl + 'selectList/GetPlantCode', httpOptions);
   }

   getBinCode(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'ElogSuryaApiService/GetBinCode');
   }

   SaveBinMaster(input: bininput) {
      const content_ = JSON.stringify(input);
      return this.http.post<any[]>(this.BasUrl + 'ElogSuryaApiService/CreateBinMaster', content_, this.httpOptions);
   }
   
   getBinById(Id: Int32Array): Observable<any[]> {
      debugger;
      console.log("Id", Id);
      return this.http.get<any[]>(this.BasUrl + 'ElogSuryaApiService/GetBinCGetBinById?id=' + Id);
   }

   getLineWorkCenterNo(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'SelectList/GetLineWorkNo');
   }

   getPackingOrderNo(plancode: string): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'SelectList/GetPackingOrderNo?plantCode=' + plancode);
   }

   GenerateSerialNumber(plancode: string): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'SelectList/GetPackingOrderNo?plantCode=' + plancode);
   }
   GetSerialNumberDetails(packingOrderNo: string): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'ElogSuryaApiService/GetSerialNumberDetails?packingOrder=' + packingOrderNo);
   }
   SaveLineWork(input: linework) {
      const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.post<any>(this.BasUrl + 'ElogSuryaApiService/LineBinMapping_Mapping', { content_, responseType: 'text', options_ });
   }
   SaveSerialBarcodeGen(input: GenerateSerialNumber) {
      const httpOptions = {
         headers: new HttpHeaders({
           'Content-Type':  'application/json',
           'Access-Control-Allow-Origin':'*'
         })
       };
      const content_ = JSON.stringify(input);
      return this.http.post<any[]>(this.BasUrl + 'ElogSuryaApiService/GenerateSerialNo', content_, httpOptions);
   }

   GetPackingOrderConfirmation(packingOrderNo: string): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'ElogSuryaApiService/GetPackingOrderConfirmation?packingOrder=' + packingOrderNo);
   }

   PackingOrderConfirmation(plantcode, packingorderNo) {
      debugger;
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.post<any[]>(this.BasUrl + `ElogSuryaApiService/PackingOrderConfirmation?packingOrder=${plantcode}&PlantCode=${packingorderNo}`, { responseType: 'text', options_ });
   }
   getStorageMaster(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'ElogSuryaApiService/GetStorageLocationMaster');
   }
   getShiftMaster(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'ElogSuryaApiService/GetSiftMaster');
   }
   DeleteSiftMasterbyid(ShiftCode: string) {
      return this.http.delete(this.BasUrl + 'ElogSuryaApiService/DeleteSiftMasterById?id=' + ShiftCode);
   }

   CreateSiftMaster(ShiftCode,ShiftDescription,sShiftStartTime,sShiftEndTime) {
      debugger;
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.post<any[]>(this.BasUrl + `ElogSuryaApiService/CreateSiftMaster?ShiftCode=${ShiftCode}&ShiftDescription=${ShiftDescription}&sShiftStartTime=${sShiftStartTime}&sShiftEndTime=${sShiftEndTime}`, { responseType: 'text', options_ });
   }
   

   QualitySaplingPackingOrderNo(planCode,lineCode) {
      return this.http.get<any[]>(this.BasUrl + `QualitySampling/GetPackingOrderByPlantAndLine?PlantCode=${planCode}&LineNo=${lineCode}`,this.httpOptions);
   }
   ScanCartonBarCode(planCode,lineCode,cartonBarCode,packingOrderNo) {
      return this.http.post<any[]>(this.BasUrl + `QualitySampling/ScanCartonBarCode?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&CartonBarCode=${cartonBarCode}&LineCode=${lineCode}`,this.httpOptions);
   }

   ScanItemBarCode(planCode,lineCode,cartonBarCode,childBarCode,packingOrderNo) {
      return this.http.post<any[]>(this.BasUrl + `QualitySampling/ScanItemBarCode?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&CartonBarCode=${cartonBarCode}&ItemBarCode=${childBarCode}&LineCode=${lineCode}`,this.httpOptions);
   }
   
   GetQuantity(planCode,lineCode,packingOrderNo) {
      return this.http.get<any[]>(this.BasUrl + `QualitySampling/GetQualityCheckingQty?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&LineCode=${lineCode}`,this.httpOptions);
   }

   SaveQualitySampling(planCode,lineCode,packingOrderNo,CartonBarCode) {
      return this.http.post<any[]>(this.BasUrl + `QualitySampling/QualityCheckingSave?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&CartonBarCode=${CartonBarCode}&LineCode=${lineCode}`,this.httpOptions);
   }

   GetQualityChecking(planCode,lineCode,packingOrderNo) {
      return this.http.get<any[]>(this.BasUrl + `QualityChecking/GetQualityCheckingDetails?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&LineCode=${lineCode}`,this.httpOptions);
   }

   saveQualityChecking(data: any[]): Observable<any> {
      return this.http.post(this.BasUrl + 'QualityChecking/SaveQualityChecking', data);
    }
}