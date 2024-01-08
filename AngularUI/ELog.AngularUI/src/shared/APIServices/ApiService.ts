import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
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
   BasUrl = 'http://180.151.246.51:8089/api/services/app/';
  // BasUrl = 'http://localhost:21021/api/services/app/';
   apiUrlGetMaterialMaster ='ElogSuryaApiService/GetMaterialMaster';

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

   downloadSerialNumberCSV(fileName: string): Observable<HttpResponse<Blob>> {
      // const url = `${this.BasUrl}SuryaGenerateSerialNo/DownloadFile?FileName=`+fileName;
      const url = `${this.BasUrl}SuryaGenerateSerialNo/GetDownloadFile?FileName=`+fileName
      const headers = new HttpHeaders({
        'Content-Type': 'application/json',
        // Add any additional headers if needed
      });
    debugger
      return this.http.get(url, { // <-- Corrected syntax
        headers: headers,
        observe: 'response',
        responseType: 'blob',
      });
    }
    
   getPackingOrderNoForSerialNumber(plancode: string,linecode:string): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + `SuryaGenerateSerialNo/GetPackingOrderNo?plantCode=${plancode}&linecode=${linecode}`);
   }
   getLineCodeasPerPlant(plancode: string): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + `SuryaGenerateSerialNo/GetLineAsPerPlant?plantCode=${plancode}`);
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

    getManualPackingPackingOrderNo(plancode,lineCode): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + `ManualPackingApi/GetPackingOrderNo?plantcode=${plancode}&lineCode=${lineCode}`);
   }

   GetManualPackingDetails(packingOrderNo,plantCode,ScanItem) {
     
      return this.http.get<any[]>(this.BasUrl + `ElogSuryaApiService/GetManualPackingDetails?packingOrderNo=${packingOrderNo}&plantCode=${plantCode}&ScanItem=${ScanItem}`);
   }
   GetStorageLocation(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'StorageLocationApi/GetStorageLocation');
   }
   
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
      return this.http.get<any[]>(this.BasUrl + 'BarcodedWarrantyClaimApi/GetCustomerCode');
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
      return this.http.get<any[]>(this.BasUrl + `BarcodedWarrantyClaimApi/GetWarrantyDetails?Barcode=${Barcode}&CustomerCode=${CustomerCode}`);

   }
   GetValidateWarrranty(Barcode,CustomerCode,BarCodeApprovedQty) {
      
       //const content_ = JSON.stringify(input);
       const options_: any = {
          //body: this.content_,
          observe: "response",
          responseType: "blob",
          headers: new HttpHeaders({
             "Content-Type": "application/json-patch+json",
          }),
       };
       return this.http.get<any[]>(this.BasUrl + `BarcodedWarrantyClaimApi/GetValidateWarrranty?Barcode=${Barcode}&CustomerCode=${CustomerCode}&BarCodeApprovedQty=${BarCodeApprovedQty}`);

    }

    GetNonBarcodedWarrantyDetails(Qty,CustomerCode,ApprovedQty,MaterialCode) {
      
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.get<any[]>(this.BasUrl + `NonBarcodedWarrantyClaimApi/GetNonBarcodedWarrantyDetails?Qty=${Qty}&CustomerCode=${CustomerCode}&ApprovedQty=${ApprovedQty}&MaterialCode=${MaterialCode}`);

    }

    GetDealerCode(): Observable<any> {
      return this.http.get(this.BasUrl + `SuryaRevalidationDealerLocation/GetDealerCode`,this.httpOptions);
    }
    GetValidateNonBarcodedWarrranty(MaterialCode,CustomerCode,Qty,ApprovedQty) {
      
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.get<any[]>(this.BasUrl + `NonBarcodedWarrantyClaimApi/GetValidateNonBarcodedWarrranty?MaterialCode=${MaterialCode}&CustomerCode=${CustomerCode}&Qty=${Qty}&ApprovedQty=${ApprovedQty}`);

   }
   GetWarrantyTrackingDtls(QrCode) {
      
      //const content_ = JSON.stringify(input);
      const options_: any = {
         //body: this.content_,
         observe: "response",
         responseType: "blob",
         headers: new HttpHeaders({
            "Content-Type": "application/json-patch+json",
         }),
      };
      return this.http.get<any[]>(this.BasUrl + `WarrantyTrackingApi/GetWarrantyTrackingDtls?QrCode=${QrCode}`);

    }

    GetDelarLocationApproveDetails() {
      return this.http.get<any[]>(this.BasUrl + `SuryaRevalidationDealerLocation/GetDelarLocationApproveDetails`);
    }
    
    GetApprovalDtlsById(dealerCode,ItemBarCode) {
      return this.http.get<any[]>(this.BasUrl + `SuryaRevalidationDealerLocation/GetDelarLocationApproveDetailsById?dealercode=${dealerCode}&itemcode=${ItemBarCode}`);
    }

    GetRevalidationOnDealerCarton(dealerCode,barCode): Observable<any> {
      return this.http.get(this.BasUrl + `SuryaRevalidationDealerLocation/GetRevalidationOnCarton?DealerCode=${dealerCode}&BarCode=${barCode}`,this.httpOptions);
    }

    GetRevalidationDealerOnItem(dealerCode,ItemBarCode): Observable<any> {
      return this.http.get(this.BasUrl + `SuryaRevalidationDealerLocation/GetRevalidationOnItem?DealerCode=${dealerCode}&ItemBarCode=${ItemBarCode}`,this.httpOptions);
    }

    ApproveOnDealerLocation(model: any,type:boolean): Observable<any> {
      if(!type)
      {
       return this.http.post(this.BasUrl + `SuryaRevalidationDealerLocation/ApproveRevalidationLocationByCarton`,model);
      }
      else{
      return this.http.post(this.BasUrl + `SuryaRevalidationDealerLocation/ApproveRevalidationLocationByItem`,model);
    }
   }
   ConfirmRevalidation(itemBarcode): Observable<any> {
      return this.http.post(this.BasUrl + `SuryaRevalidationDealerLocation/ApproveRevalidation`,itemBarcode);
    }

   EncryptPassword(input): Observable<any> {
      return this.http.post(this.BasUrl + `ChangePswd/EncryptPassword?input=${input}`,this.httpOptions);
    }
    GetPackingReportOrderNo(): Observable<any[]> {
      return this.http.get<any[]>(this.BasUrl + 'PackingReportsApi/GetPackingReportOrderNo');
   }

   GetPackingReport(data:any): Observable<any> {
      
      return this.http.post<any>(this.BasUrl + `PackingReportsApi/GetPackingReport`, data);
  }

  GetPackingOrderbarCodeDtlsReport(data:any): Observable<any> {
      
   return this.http.post<any>(this.BasUrl + `PackingOrderBarcodeDtlsReportsApi/GetPackingOrderbarCodeDtlsReport`, data);
}
GetQualityReport(data:any): Observable<any> {
      
   return this.http.post<any>(this.BasUrl + `QualityReportsApi/GetQualityReport`, data);
}
GetTransferOrderNo(): Observable<any[]> {
   return this.http.get<any[]>(this.BasUrl + 'TransferPlantToWarehouseApi/GetTransferOrderNo');
}
GetTranferPlantToWarehouseReport(data:any): Observable<any> {
      
   return this.http.post<any>(this.BasUrl + `TransferPlantToWarehouseApi/GetTranferPlantToWarehouseReport`, data);
}
GetAsOnDateInventoryReport(data:any): Observable<any> {
      
   return this.http.post<any>(this.BasUrl + `AsOnDateInventoryReportsApi/GetAsOnDateInventoryReport`, data);
}
GetDelieveryNo(): Observable<any[]> {
   return this.http.get<any[]>(this.BasUrl + 'DispatchFromWarehouseReportsApi/GetDelieveryNo');
}
GetDispatchFromWarehouseReport(data:any): Observable<any> {
      
   return this.http.post<any>(this.BasUrl + `DispatchFromWarehouseReportsApi/GetDispatchFromWarehouseReport`, data);
}
SaveLineWork(barcode,lineBarCode) {
     
   //const content_ = JSON.stringify(input);
   const options_: any = {
      //body: this.content_,
      observe: "response",
      responseType: "blob",
      headers: new HttpHeaders({
         "Content-Type": "application/json-patch+json",
      }),
   };
   return this.http.post<any[]>(this.BasUrl + `ElogSuryaApiService/LineBinMapping_Mapping?barcode=${barcode}&lineBarCode=${lineBarCode}`, { responseType: 'text', options_ });

}
GetGRN_AtBranchReport(data:any): Observable<any> {
      
   return this.http.post<any>(this.BasUrl + `GRN_AtBranchReportsApi/GetGRN_AtBranchReport`, data);
}
GetDispatchFromBranchReport(data:any): Observable<any> {
      
   return this.http.post<any>(this.BasUrl + `DispatchFromBranchReportsApi/GetDispatchFromBranchReport`, data);
}

GetReturnToBranchLocationFromDealer(data:any): Observable<any> {
      
   return this.http.post<any>(this.BasUrl + `ReturnToBranchLocationFromDealerReportsApi/GetReturnToBranchLocationFromDealer`, data);
}
GetRevalidationReport(data:any): Observable<any> {
      
   return this.http.post<any>(this.BasUrl + `RevalidationReportsApi/GetRevalidationReport`, data);
}
DeleteShift(id) {
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
   return this.http.post<any[]>(this.BasUrl + `ElogSuryaApiService/DeleteSift?id=${id}`, { responseType: 'text', options_ });
}

DeleteBin(id) {
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
 return this.http.post<any[]>(this.BasUrl + `ElogSuryaApiService/DeleteBin?id=${id}`, { responseType: 'text', options_ });
}

GetMonthlyInspectionReportForDealer(): Observable<any[]> {
   return this.http.get<any[]>(this.BasUrl + 'MonthlyInspectionReportForDealerApi/GetMonthlyInspectionReportForDealer');
}

GetDealerWiseFailureDetails(): Observable<any[]> {
   return this.http.get<any[]>(this.BasUrl + 'DealerWiseFailureDetailsApi/GetDealerWiseFailureDetails');
}

GetNonBarcodedProductDetails(): Observable<any[]> {
   return this.http.get<any[]>(this.BasUrl + 'NonBarcodedProductsReportsApi/GetNonBarcodedProductDetails');
}

GetLifeCycleReport(): Observable<any[]> {
   return this.http.get<any[]>(this.BasUrl + 'LifeCycleReportApi/GetLifeCycleReport');
}
GetConsNonBarcodedProductDetails(): Observable<any[]> {
   return this.http.get<any[]>(this.BasUrl + 'Consoli_NonBarcodedProductsReportsApi/GetConsNonBarcodedProductDetails');
}
GetManufacturingMonthWiseDefective(): Observable<any[]> {
   return this.http.get<any[]>(this.BasUrl + 'ManufacturingMonthWiseDefectiveApi/GetManufacturingMonthWiseDefective');
}
GetManufacturingTimeWiseDefective(): Observable<any[]> {
   return this.http.get<any[]>(this.BasUrl + 'ManufacturingTimeWiseDefectiveApi/GetManufacturingTimeWiseDefective');
}
}