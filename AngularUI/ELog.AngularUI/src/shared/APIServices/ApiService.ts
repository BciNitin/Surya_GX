import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { bininput } from '@app/masters/bin/bin.component';
import { linework } from '@app/PlantOperation/line-work-center/line-work-center.component';
import { GenerateSerialNumber } from '@app/PlantOperation/serialbarcodegeneration/serialbarcodegeneration.component';

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
     ;
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
      return this.http.get<any[]>(this.BasUrl + 'SuryaGenerateSerialNo/GetSerialNumberDetails?packingOrder=' + packingOrderNo);
   }

   getPackingOrderNoForSerialNumber(plancode: string,linecode:string): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + `SuryaGenerateSerialNo/GetPackingOrderNo?plantCode=${plancode}&linecode=${linecode}`);
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
      return this.http.post<any[]>(this.BasUrl + 'SuryaGenerateSerialNo/GenerateSerialNo', content_, httpOptions);
   }

   GetConfirming_PO_No_(planCode:string): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + `PackingOrderConfirmation/GetConfirmationPackingOrderNo?PlantCode=${planCode}`);
   }

   GetPackingOrderConfirmingDetails(packingOrderNo: string,planCode:string): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + `PackingOrderConfirmation/GetPackingOrderDetails?packingOrder=${packingOrderNo}&PlantCode=${planCode}`);
   }

   PackingOrderConfirmation(data: any): Observable<any> {
      return this.http.post(this.BasUrl + 'PackingOrderConfirmation/ConfirmPackingOrder', data[0], this.httpOptions);
    }
    
    GetConfirmationPackingOrderNo(planCode): Observable<any> {
      return this.http.get<any[]>(this.BasUrl + `SuryaQualityConfirmation/GetPackingOrderNo?plantcode=${planCode}`,this.httpOptions);
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
   
   GetQualitySamplingQuantity(planCode,lineCode,packingOrderNo) {
      return this.http.get<any[]>(this.BasUrl + `QualitySampling/GetQualitySamplingQty?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&LineCode=${lineCode}`,this.httpOptions);
   }

   SaveQualitySampling(planCode,lineCode,packingOrderNo,CartonBarCode) {
      return this.http.post<any[]>(this.BasUrl + `QualitySampling/QualitySamplingSave?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&CartonBarCode=${CartonBarCode}&LineCode=${lineCode}`,this.httpOptions);
   }


   GetQualityChecking(planCode,lineCode,packingOrderNo) {
      return this.http.get<any[]>(this.BasUrl + `QualityChecking/GetQualityCheckingDetails?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&LineCode=${lineCode}`,this.httpOptions);
   }

   saveQualityChecking(data: any[]): Observable<any> {
      return this.http.post(this.BasUrl + 'QualityChecking/SaveQualityChecking', data);
    }

    GetQualityConfirmationDetails(platcode,packingorderno): Observable<any> {
      return this.http.get(this.BasUrl + `SuryaQualityConfirmation/GetQCCheckingDetails?plantcode=${platcode}&packingorderno=${packingorderno}`,this.httpOptions);
    }

    GetQualityConfirmationPONo(platcode): Observable<any> {
      return this.http.post(this.BasUrl + `SuryaQualityConfirmation/GetPackingOrderNo?plantcode=${platcode}`,this.httpOptions);
    }

    saveQualityConfirmation(data: any[]): Observable<any> {
      return this.http.post(this.BasUrl + 'SuryaQualityConfirmation/QCConfirm', data);
    }

   GetManualPackingDetails(packingOrderNo,plantCode,ScanItem) {
     
      return this.http.get<any[]>(this.BasUrl + `ElogSuryaApiService/GetManualPackingDetails?packingOrderNo=${packingOrderNo}&plantCode=${plantCode}&ScanItem=${ScanItem}`);
   }
   GetStorageLocation(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'StorageLocationApi/GetStorageLocation');
   }
   // GetStrLocationDtls(plancode: string): Observable<any[]> {
   //    return this.http.get<any[]>(this.BasUrl + 'StorageLocationApi/GetStorageLocationDetails?plancode=' + plancode);
   // }
   GetStrLocationDtls(plancode,LocationID) {
     
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.get<any[]>(this.BasUrl + `StorageLocationApi/GetStorageLocationDetails?plancode=${plancode}&LocationID=${LocationID}`);
   }
   GetBarcodeScannedDetails(barcode,plantcode) {
     
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.get<any[]>(this.BasUrl + `StorageLocationApi/GetBarcodeScannedDetails?barcode=${barcode}&plantcode=${plantcode}`);
   }

   StorageLocationConfirmation(barcode, LocationID) {
     
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.post<any[]>(this.BasUrl + `StorageLocationApi/StorageLocationConfirmation?barcode=${barcode}&LocationID=${LocationID}`, { responseType: 'text', options_ });

   }

   GetManualPackingDtls(plantcode,packingorder,linecode) {
     
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.get<any[]>(this.BasUrl + `ManualPackingApi/GetManualPackingDetails?plantcode=${plantcode}&packingorder=${packingorder}&linecode=${linecode}`);
   }

   ValidateBarcode(BinBarCode,macAddresses,plantcode,packingorder,linecode) {
     
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.post<any[]>(this.BasUrl + `ManualPackingApi/ValidateBarcode?BinBarCode=${BinBarCode}&macAddresses=${macAddresses}&plantcode=${plantcode}&packingorder=${packingorder}&linecode=${linecode}`, { responseType: 'text', options_ });

   }

   
   getIPAddress(): Observable<any[]> {
     
      return this.http.get<any[]>(this.BasUrl + 'ManualPackingApi/GetMacAddress');
   }

   GetQualityItemTestedDtls(itemBarcode,plantCode) {
     
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.get<any[]>(this.BasUrl + `QualityTested_ItemPlacementApi/GetQualityItemTestedDtls?itemBarcode=${itemBarcode}&plantCode=${plantCode}`);

   }
   ValidateShiperBarcode(itemBarcode,plantCode,ShiperBarcode) {
      //const content_ = JSON.stringify(input);
      return this.http.get<any[]>(this.BasUrl + `QualityTested_ItemPlacementApi/GetValidateShiperBarcode?itemBarcode=${itemBarcode}&plantCode=${plantCode}&ShiperBarcode=${ShiperBarcode}`);
   }

    QualityCheckingPackingOrderNo(planCode,lineCode) {
      return this.http.get<any[]>(this.BasUrl + `QualityChecking/GetPackingOrderByPlantAndLine?PlantCode=${planCode}&LineNo=${lineCode}`,this.httpOptions);
   }
   GetchallanNo(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'TransferToBranchFromPlantApi/GetchallanNo');
   }
   // GetChallanDetails(DeliveryChallanNo: string): Observable<any[]> {
   //    return this.http.get<any[]>(this.BasUrl + 'TransferToBranchFromPlantApi/GetChallanDetails?DeliveryChallanNo=' + DeliveryChallanNo);
   // }

   GetValidateScanCartonBarcode(DeliveryChallanNo,CartonBarcode) {
      
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.get<any[]>(this.BasUrl + `TransferToBranchFromPlantApi/GetValidateScanCartonBarcode?DeliveryChallanNo=${DeliveryChallanNo}&CartonBarcode=${CartonBarcode}`);

   }
   GetSOchallanNo(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'TransferToDealerCustFromBranchLocApi/GetSOchallanNo');
   }
   
GetSOChallanDetails(DeliveryChallanNo) {
      
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.get<any[]>(this.BasUrl + `TransferToDealerCustFromBranchLocApi/GetSOChallanDetails?DeliveryChallanNo=${DeliveryChallanNo}`);

   }
   GetValidateSOScanCartonBarcode(DeliveryChallanNo,CartonBarcode) {
      
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.get<any[]>(this.BasUrl + `TransferToDealerCustFromBranchLocApi/GetValidateSOScanCartonBarcode?DeliveryChallanNo=${DeliveryChallanNo}&CartonBarcode=${CartonBarcode}`);

   }
   GetItemCodes(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'RevalidationProcessBranchPlantApi/GetExpiredItemCode');
   }
   GetExpiredItemCodeDetails(MaterialCode) {
     
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.get<any[]>(this.BasUrl + `RevalidationProcessBranchPlantApi/GetExpiredItemCodeDetails?MaterialCode=${MaterialCode}`);

   }
   GetValidateItem(barcode,MaterialCode) {
      
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.get<any[]>(this.BasUrl + `RevalidationProcessBranchPlantApi/GetValidateItem?barcode=${barcode}&MaterialCode=${MaterialCode}`);

   }

   GetValidateGRNConfirmation(DeliveryChallanNo,CartonBarcode) {
      
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.get<any[]>(this.BasUrl + `GRN_ConfirmationApi/GetValidateGRNConfirmation?DeliveryChallanNo=${DeliveryChallanNo}&CartonBarcode=${CartonBarcode}`);

   }

   GetChallanDetails(DeliveryChallanNo) {
      
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.get<any[]>(this.BasUrl + `TransferToBranchFromPlantApi/GetChallanDetails?DeliveryChallanNo=${DeliveryChallanNo}`);

   }
   GetCustomerCode(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'WarrantyClaimApi/GetCustomerCode');
   }
   GetWarrantyDetails(Barcode,CustomerCode) {
      
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.get<any[]>(this.BasUrl + `WarrantyClaimApi/GetWarrantyDetails?Barcode=${Barcode}&CustomerCode=${CustomerCode}`);

   }
   GetValidateWarrranty(Barcode,CustomerCode) {
      
       //const content_ = JSON.stringify(input);
       const options_: any = {
          //body: this.content_,
          observe: "response",
          responseType: "blob",
          headers: new HttpHeaders({
             "Content-Type": "application/json-patch+json",
          }),
       };
       return this.http.get<any[]>(this.BasUrl + `WarrantyClaimApi/GetValidateWarrranty?Barcode=${Barcode}&CustomerCode=${CustomerCode}`);

    }
    GetDealerCode(): Observable<any> {
      return this.http.get(this.BasUrl + `SuryaRevalidationDealerLocation/GetDealerCode`,this.httpOptions);
    }
}