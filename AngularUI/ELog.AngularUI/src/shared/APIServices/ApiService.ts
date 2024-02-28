import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Inject, Injectable, InjectionToken, OnInit } from '@angular/core';
import { bininput } from '@app/masters/bin/bin.component';
import { linework } from '@app/PlantOperation/line-work-center/line-work-center.component';
import { GenerateSerialNumber } from '@app/PlantOperation/serialbarcodegeneration/serialbarcodegeneration.component';

import { SelectListDto } from '@shared/service-proxies/service-proxies';
import { from, Observable } from 'rxjs';
import { AppConfigService } from './AppConfigService ';
import { switchMap } from 'rxjs/operators';

@Injectable({

  providedIn: 'root'
})

export class ApiServiceService {
  //baseUrl = 'http://180.151.246.51:8089/api/services/app/';
  //baseUrl = 'http://localhost:21021/api/services/app/';
  uri: any;
  apiUrlGetMaterialMaster = 'ElogSuryaApiService/GetMaterialMaster';


  constructor(private http: HttpClient, private appConfig: AppConfigService) {


  }

  async getUrl(url): Promise<string> {
    const data = await this.loadConfig().toPromise();
    return this.setBaseUrl(data.remoteServiceBaseUrl, url);
  }

  setBaseUrl(baseUrl: string, Url: string): any {
    return baseUrl + '/api/services/app/' + Url;
  }

  loadConfig(): Observable<any> {
    return this.http.get('/assets/appconfig.json'); 
  }

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*'
    })
  };


  getMaterialMaster(): Observable<any[]> {
    return from(this.getUrl('ElogSuryaApiService/GetMaterialMaster')).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  getPlantMaster(): Observable<any[]> {
    return from(this.getUrl('ElogSuryaApiService/GetPlantMaster')).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }
  getCustomerMaster(): Observable<any[]> {
    return from(this.getUrl('ElogSuryaApiService/GetCustomerMaster')).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }
  getPackingMasters(): Observable<any[]> {
    return from(this.getUrl('ElogSuryaApiService/GetPackingMaster')).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  getLineMaster(): Observable<any[]> {
    return from(this.getUrl('ElogSuryaApiService/GetLineMaster')).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  getBinMaster(): Observable<any[]> {
    return from(this.getUrl('ElogSuryaApiService/GetBinMaster')).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  getPlantCode(): Observable<SelectListDto[]> {
    return from(this.getUrl('selectList/GetPlantCode')).pipe(
      switchMap(sturi => this.http.get<SelectListDto[]>(sturi))
    );
  }

  getBinCode(): Observable<any[]> {
    return from(this.getUrl('ElogSuryaApiService/GetBinCode')).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  SaveBinMaster(input: bininput): Observable<any[]> {
    return from(this.getUrl('ElogSuryaApiService/CreateBinMaster')).pipe(
      switchMap(sturi => this.http.post<any[]>(sturi, JSON.stringify(input), this.httpOptions))
    );
  }

  getBinById(Id: Int32Array): Observable<any[]> {
    return from(this.getUrl('ElogSuryaApiService/GetBinCGetBinById')).pipe(
      switchMap(sturi => this.http.get<any[]>(`${sturi}?id=${Id}`))
    );
  }

  getLineWorkCenterNo(): Observable<any[]> {
    return from(this.getUrl('SelectList/GetLineWorkNo')).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  getQyalityCheckingLineWorkCenterNo(PlantCode): Observable<any[]> {
    return from(this.getUrl(`QualitySampling/GetLineWorkNo?PlantCode=${PlantCode}`)).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  getqualitySamplingLineWorkCenterNo(PlantCode): Observable<any[]> {
    return from(this.getUrl(`QualityChecking/GetLineWorkNo?PlantCode=${PlantCode}`)).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  getPackingOrderNo(plancode: string): Observable<any[]> {
    return from(this.getUrl(`SelectList/GetPackingOrderNo?plantCode=${plancode}`)).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  GenerateSerialNumber(plancode: string): Observable<any[]> {
    return from(this.getUrl(`SelectList/GetPackingOrderNo?plantCode=${plancode}`)).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  GetSerialNumberDetails(packingOrderNo: string): Observable<any[]> {
    return from(this.getUrl(`SuryaGenerateSerialNo/GetSerialNumberDetails?packingOrder=${packingOrderNo}`)).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  downloadSerialNumberCSV(fileName: string): Observable<HttpResponse<Blob>> {
    return from(this.getUrl(`SuryaGenerateSerialNo/GetDownloadFile?FileName=${fileName}`)).pipe(
      switchMap(sturi => {
        const url = `${sturi}`;
        const headers = new HttpHeaders({
          'Content-Type': 'application/json',
          // Add any additional headers if needed
        });
        return this.http.get(url, { headers: headers, observe: 'response', responseType: 'blob' });
      })
    );
  }

  getPackingOrderNoForSerialNumber(plancode: string, linecode: string): Observable<any[]> {
    return from(this.getUrl(`SuryaGenerateSerialNo/GetPackingOrderNo?plantCode=${plancode}&linecode=${linecode}`)).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  getLineCodeasPerPlant(plancode: string): Observable<any[]> {
    return from(this.getUrl(`SuryaGenerateSerialNo/GetLineAsPerPlant?plantCode=${plancode}`)).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  SaveSerialBarcodeGen(input: GenerateSerialNumber): Observable<any[]> {
    return from(this.getUrl('SuryaGenerateSerialNo/GenerateSerialNo')).pipe(
      switchMap(sturi => this.http.post<any[]>(sturi, JSON.stringify(input), this.httpOptions))
    );
  }

  GetConfirming_PO_No_(planCode: string): Observable<any[]> {
    return from(this.getUrl(`PackingOrderConfirmation/GetConfirmationPackingOrderNo?PlantCode=${planCode}`)).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  GetPackingOrderConfirmingDetails(packingOrderNo: string, planCode: string): Observable<any[]> {
    return from(this.getUrl(`PackingOrderConfirmation/GetPackingOrderDetails?packingOrder=${packingOrderNo}&PlantCode=${planCode}`)).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  // Continue refactoring other methods in a similar manner

  PackingOrderConfirmation(data: any): Observable<any> {
    return from(this.getUrl('PackingOrderConfirmation/ConfirmPackingOrder')).pipe(
      switchMap(sturi => this.http.post(sturi, data[0], this.httpOptions))
    );
  }

  GetConfirmationPackingOrderNo(planCode): Observable<any[]> {
    return from(this.getUrl(`SuryaQualityConfirmation/GetPackingOrderNo?plantcode=${planCode}`)).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi, this.httpOptions))
    );
  }

  getStorageMaster(): Observable<any[]> {
    return from(this.getUrl('ElogSuryaApiService/GetStorageLocationMaster')).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  getShiftMaster(): Observable<any[]> {
    return from(this.getUrl('ElogSuryaApiService/GetSiftMaster')).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }


  // More methods refactored in the same pattern

  DeleteSiftMasterbyid(ShiftCode: string) {
    return from(this.getUrl(`ElogSuryaApiService/DeleteSift?id=${ShiftCode}`)).pipe(
      switchMap(sturi => this.http.delete(sturi))
    );
  }

  CreateSiftMaster(ShiftCode, ShiftDescription, sShiftStartTime, sShiftEndTime) {
    return from(this.getUrl(`ElogSuryaApiService/CreateSiftMaster?ShiftCode=${ShiftCode}&ShiftDescription=${ShiftDescription}&sShiftStartTime=${sShiftStartTime}&sShiftEndTime=${sShiftEndTime}`)).pipe(
      switchMap(sturi => this.http.post<any[]>(sturi, this.httpOptions))
    );
  }

  QualitySaplingPackingOrderNo(planCode, lineCode) {
    return from(this.getUrl(`QualitySampling/GetPackingOrderByPlantAndLine?PlantCode=${planCode}&LineNo=${lineCode}`)).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi, this.httpOptions))
    );
  }

  // More methods refactored in the same pattern

  ScanCartonBarCode(planCode, lineCode, cartonBarCode, packingOrderNo) {
    const url = `QualitySampling/ScanCartonBarCode?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&CartonBarCode=${cartonBarCode}&LineCode=${lineCode}`;
    return from(this.getUrl(url)).pipe(
      switchMap(sturi => this.http.post<any[]>(sturi, this.httpOptions))
    );
  }

  ScanItemBarCode(planCode, lineCode, cartonBarCode, childBarCode, packingOrderNo) {
    const url = `QualitySampling/ScanItemBarCode?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&CartonBarCode=${cartonBarCode}&ItemBarCode=${childBarCode}&LineCode=${lineCode}`;
    return from(this.getUrl(url)).pipe(
      switchMap(sturi => this.http.post<any[]>(sturi, this.httpOptions))
    );
  }

  GetQualitySamplingQuantity(planCode, lineCode, packingOrderNo) {
    const url = `QualitySampling/GetQualitySamplingQty?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&LineCode=${lineCode}`;
    return from(this.getUrl(url)).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi, this.httpOptions))
    );
  }

  SaveQualitySampling(planCode, lineCode, packingOrderNo, CartonBarCode) {
    const url = `QualitySampling/QualitySamplingSave?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&CartonBarCode=${CartonBarCode}&LineCode=${lineCode}`;
    return from(this.getUrl(url)).pipe(
      switchMap(sturi => this.http.post<any[]>(sturi, this.httpOptions))
    );
  }

  GetQualityChecking(planCode, lineCode, packingOrderNo) {
    const url = `QualityChecking/GetQualityCheckingDetails?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&LineCode=${lineCode}`;
    return from(this.getUrl(url)).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi, this.httpOptions))
    );
  }

  saveQualityChecking(data: any[]): Observable<any> {
    return from(this.getUrl('QualityChecking/SaveQualityChecking')).pipe(
      switchMap(sturi => this.http.post(sturi, data))
    );
  }

  GetQualityConfirmationDetails(platcode, packingorderno): Observable<any> {
    const url = `SuryaQualityConfirmation/GetQCCheckingDetails?plantcode=${platcode}&packingorderno=${packingorderno}`;
    return from(this.getUrl(url)).pipe(
      switchMap(sturi => this.http.get(sturi, this.httpOptions))
    );
  }

  GetQualityConfirmationPONo(platcode): Observable<any> {
    const url = `SuryaQualityConfirmation/GetPackingOrderNo?plantcode=${platcode}`;
    return from(this.getUrl(url)).pipe(
      switchMap(sturi => this.http.post(sturi, this.httpOptions))
    );
  }

  saveQualityConfirmation(data: any[]): Observable<any> {
    return from(this.getUrl('SuryaQualityConfirmation/QCConfirm')).pipe(
      switchMap(sturi => this.http.post(sturi, data,this.httpOptions))
    );
  }

  // More methods refactored in the same pattern

  getManualPackingPackingOrderNo(plancode, lineCode): Observable<any> {
    const url = `ManualPackingApi/GetPackingOrderNo?plantcode=${plancode}&lineCode=${lineCode}`;
    return from(this.getUrl(url)).pipe(
      switchMap(sturi => this.http.get(sturi, this.httpOptions))
    );
  }

  GetManualPackingDetails(packingOrderNo, plantCode, ScanItem) {
    const url = `ElogSuryaApiService/GetManualPackingDetails?packingOrderNo=${packingOrderNo}&plantCode=${plantCode}&ScanItem=${ScanItem}`;
    return from(this.getUrl(url)).pipe(
      switchMap(sturi => this.http.get(sturi, this.httpOptions))
    );
  }

  GetStorageLocation(plantcode:any): Observable<any> {
    return from(this.getUrl(`StorageLocationApi/GetStorageLocation?plantcode=${plantcode}`)).pipe(
      switchMap(sturi => this.http.get(sturi, this.httpOptions))
    );
  }

  GetStrLocationDtls(plancode, LocationID,MaterialCode) {
    return from(this.getUrl(`StorageLocationApi/GetStorageLocationDetails?plancode=${plancode}&LocationID=${LocationID}&MaterialCode=${MaterialCode}`)).pipe(
      switchMap(sturi => this.http.get(sturi, this.httpOptions))
    );
  }

  GetBarcodeScannedDetails(barcode, plantcode,LocationID,MaterialCode) {
    return from(this.getUrl(`StorageLocationApi/GetBarcodeScannedDetails?barcode=${barcode}&plantcode=${plantcode}&StrLocation=${LocationID}&MaterialCode=${MaterialCode}`)).pipe(
      switchMap(sturi => this.http.get(sturi, this.httpOptions))
    );
  }
  GetSTRMaterialCode(plantcode) {
    return from(this.getUrl(`StorageLocationApi/GetMaterialCode?plantcode=${plantcode}`)).pipe(
      switchMap(sturi => this.http.get(sturi, this.httpOptions))
    );
  }

  StorageLocationConfirmation(barcode, LocationID,MaterialCode) {
    const url = `StorageLocationApi/StorageLocationConfirmation?barcode=${barcode}&LocationID=${LocationID}&MaterialCode=${MaterialCode}`;
    return from(this.getUrl(url)).pipe(
      switchMap(sturi => this.http.post<any[]>(sturi, this.httpOptions))
    );
  }

  GetManualPackingDtls(plantcode, packingorder, linecode) {
    const url = `ManualPackingApi/GetManualPackingDetails?plantcode=${plantcode}&packingorder=${packingorder}&linecode=${linecode}`;
    return from(this.getUrl(url)).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  ValidateBarcode(BinBarCode, macAddresses, plantcode, packingorder, linecode, materialCode) {
    const url = `ManualPackingApi/ValidateBarcode?BinBarCode=${BinBarCode}&macAddresses=${macAddresses}&plantcode=${plantcode}&packingorder=${packingorder}&linecode=${linecode}&MaterialCode=${materialCode}`;
    return from(this.getUrl(url)).pipe(
      switchMap(sturi => this.http.post<any[]>(sturi, this.httpOptions))
    );
  }


  getIPAddress(): Observable<any[]> {
    return from(this.getUrl('ManualPackingApi/GetMacAddress')).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  GetQualityItemTestedDtls(itemBarcode, plantCode) {
    return from(this.getUrl(`QualityTested_ItemPlacementApi/GetQualityItemTestedDtls?itemBarcode=${itemBarcode}&plantCode=${plantCode}`)).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  ValidateShiperBarcode(itemBarcode, plantCode, ShiperBarcode) {
     return from(this.getUrl(`QualityTested_ItemPlacementApi/GetValidateShiperBarcode?itemBarcode=${itemBarcode}&plantCode=${plantCode}&ShiperBarcode=${ShiperBarcode}`)).pipe(
      switchMap(sturi => this.http.get<any[]>(sturi))
    );
  }

  QualityCheckingPackingOrderNo(planCode, lineCode): Observable<any[]> {
    return from(this.getUrl(`QualityChecking/GetPackingOrderByPlantAndLine?PlantCode=${planCode}&LineNo=${lineCode}`)).pipe(
      switchMap(url => this.http.get<any[]>(url))
    );
  }

  GetchallanNo(): Observable<any[]> {
    return from(this.getUrl('TransferToBranchFromPlantApi/GetchallanNo')).pipe(
      switchMap(url => this.http.get<any[]>(url))
    );
  }

  GetValidateScanCartonBarcode(DeliveryChallanNo, CartonBarcode, MaterialCode, FromPlantCode, ToPlantCode): Observable<any[]> {
    return from(this.getUrl(`TransferToBranchFromPlantApi/GetValidateScanCartonBarcode?DeliveryChallanNo=${DeliveryChallanNo}&CartonBarcode=${CartonBarcode}&MaterialCode=${MaterialCode}&FromPlantCode=${FromPlantCode}&ToPlantCode=${ToPlantCode}`)).pipe(
      switchMap(url => this.http.get<any[]>(url))
    );
  }

  GetSOchallanNo(): Observable<any[]> {
    return from(this.getUrl('TransferToDealerCustFromBranchLocApi/GetSOchallanNo')).pipe(
      switchMap(url => this.http.get<any[]>(url))
    );
  }

  GetSOMaterialCode(DeliveryChallanNo): Observable<any[]> {
    return from(this.getUrl(`TransferToDealerCustFromBranchLocApi/GetMaterialCode?DeliveryChallanNo=${DeliveryChallanNo}`)).pipe(
      switchMap(url => this.http.get<any[]>(url))
    );
  }

  GetSTOMaterialCode(DeliveryChallanNo): Observable<any[]> {
    return from(this.getUrl(`TransferToBranchFromPlantApi/GetMaterialCode?DeliveryChallanNo=${DeliveryChallanNo}`)).pipe(
      switchMap(url => this.http.get<any[]>(url))
    );
  }
  
// Remaining refactored methods
GetSOChallanDetails(DeliveryChallanNo): Observable<any[]> {
  return from(this.getUrl(`GRN_ConfirmationApi/GetChallanNoDetails?DeliveryChallanNo=${DeliveryChallanNo}`)).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetDealerTransferSOChallanDetails(DeliveryChallanNo, MaterialCode): Observable<any[]> {
  return from(this.getUrl(`TransferToDealerCustFromBranchLocApi/GetSOChallanDetails?DeliveryChallanNo=${DeliveryChallanNo}&MaterialCode=${MaterialCode}`)).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetValidateSOScanCartonBarcode(DeliveryChallanNo, CartonBarcode, MaterialCode, SONo, PlantCode, CustomerCode): Observable<any[]> {
  return from(this.getUrl(`TransferToDealerCustFromBranchLocApi/GetValidateSOScanCartonBarcode?DeliveryChallanNo=${DeliveryChallanNo}&CartonBarcode=${CartonBarcode}&MaterialCode=${MaterialCode}&SONo=${SONo}&PlantCode=${PlantCode}&CustomerCode=${CustomerCode}`)).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetItemCodes(): Observable<any[]> {
  return from(this.getUrl('RevalidationProcessBranchPlantApi/GetExpiredItemCode')).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetPlantToBranchPlantCode(): Observable<any[]> {
  return from(this.getUrl('RevalidationProcessBranchPlantApi/GetPlantCode')).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetExpiredItemCodeDetails(MaterialCode, PlantCode): Observable<any[]> {
  return from(this.getUrl(`RevalidationProcessBranchPlantApi/GetExpiredItemCodeDetails?MaterialCode=${MaterialCode}&PlantCode=${PlantCode}`)).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

ValidateItem(data: any[]): Observable<any> {
  return from(this.getUrl('RevalidationProcessBranchPlantApi/ValidateItem')).pipe(
    switchMap(sturi => this.http.post<any[]>(sturi, data, this.httpOptions))
  );
}

ValidateItemByBarCode(Barcode): Observable<any[]> {
  return from(this.getUrl(`RevalidationProcessBranchPlantApi/ValidateItemByBarCode?Barcode=${Barcode}`)).pipe(
    switchMap(url => this.http.get<any[]>(url, this.httpOptions))
  );
}

GetValidateGRNConfirmation(DeliveryChallanNo, CartonBarcode, CheckNGOK, toPlantCode, fromPlantCode): Observable<any[]> {
  return from(this.getUrl(`GRN_ConfirmationApi/GetValidateGRNConfirmation?DeliveryChallanNo=${DeliveryChallanNo}&CartonBarcode=${CartonBarcode}&Status=${CheckNGOK}&ToPlantCode=${toPlantCode}&FromPlantCode=${fromPlantCode}`)).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

ConfirmGRN(DeliveryChallanNo, CartonBarcode, CheckNGOK, toPlantCode, fromPlantCode): Observable<any[]> {
  return from(this.getUrl(`GRN_ConfirmationApi/GetGRNConfirmation?DeliveryChallanNo=${DeliveryChallanNo}&CartonBarcode=${CartonBarcode}&Status=${CheckNGOK}&ToPlantCode=${toPlantCode}&FromPlantCode=${fromPlantCode}`)).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetSTOChallanDetails(DeliveryChallanNo, MaterialCode): Observable<any[]> {
  return from(this.getUrl(`TransferToBranchFromPlantApi/GetChallanDetails?DeliveryChallanNo=${DeliveryChallanNo}&MaterialCode=${MaterialCode}`)).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetCustomerCode(): Observable<any[]> {
  return from(this.getUrl('BarcodedWarrantyClaimApi/GetCustomerCode')).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetChallanNoForGRNConfirm(): Observable<any[]> {
  return from(this.getUrl('GRN_ConfirmationApi/GetDeliveryChallanNumber')).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetWarrantyDetails(Barcode, CustomerCode): Observable<any[]> {
  return from(this.getUrl(`BarcodedWarrantyClaimApi/GetWarrantyDetails?Barcode=${Barcode}&CustomerCode=${CustomerCode}`)).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetValidateWarrranty(Barcode, CustomerCode, BarCodeApprovedQty): Observable<any[]> {
  return from(this.getUrl(`BarcodedWarrantyClaimApi/GetValidateWarrranty?Barcode=${Barcode}&CustomerCode=${CustomerCode}&BarCodeApprovedQty=${BarCodeApprovedQty}`)).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetNonBarcodedWarrantyDetails(Qty, CustomerCode, ApprovedQty, MaterialCode): Observable<any[]> {
  return from(this.getUrl(`NonBarcodedWarrantyClaimApi/GetNonBarcodedWarrantyDetails?Qty=${Qty}&CustomerCode=${CustomerCode}&ApprovedQty=${ApprovedQty}&MaterialCode=${MaterialCode}`)).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetDealerCode(): Observable<any> {
  return from(this.getUrl('SuryaRevalidationDealerLocation/GetDealerCode')).pipe(
    switchMap(url => this.http.get(url, this.httpOptions))
  );
}

GetValidateNonBarcodedWarrranty(MaterialCode, CustomerCode, Qty, ApprovedQty): Observable<any[]> {
  return from(this.getUrl(`NonBarcodedWarrantyClaimApi/GetValidateNonBarcodedWarrranty?MaterialCode=${MaterialCode}&CustomerCode=${CustomerCode}&Qty=${Qty}&ApprovedQty=${ApprovedQty}`)).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetWarrantyTrackingDtls(QrCode): Observable<any[]> {
  return from(this.getUrl(`WarrantyTrackingApi/GetWarrantyTrackingDtls?QrCode=${QrCode}`)).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetDelarLocationApproveDetails(): Observable<any[]> {
  return from(this.getUrl('SuryaRevalidationDealerLocation/GetDelarLocationApproveDetails')).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetApprovalDtlsById(dealerCode, ItemBarCode): Observable<any[]> {
  return from(this.getUrl(`SuryaRevalidationDealerLocation/GetDelarLocationApproveDetailsById?dealercode=${dealerCode}&itemcode=${ItemBarCode}`)).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetRevalidationOnDealerCarton(dealerCode, barCode): Observable<any> {
  return from(this.getUrl(`SuryaRevalidationDealerLocation/GetRevalidationOnCarton?DealerCode=${dealerCode}&BarCode=${barCode}`)).pipe(
    switchMap(url => this.http.get(url, this.httpOptions))
  );
}

GetRevalidationDealerOnItem(dealerCode, ItemBarCode): Observable<any> {
  return from(this.getUrl(`SuryaRevalidationDealerLocation/GetRevalidationOnItem?DealerCode=${dealerCode}&ItemBarCode=${ItemBarCode}`)).pipe(
    switchMap(url => this.http.get(url, this.httpOptions))
  );
}

ConfirmRevalidation(itemBarcode): Observable<any> {
  return from(this.getUrl(`SuryaRevalidationDealerLocation/ApproveRevalidation`)).pipe(
    switchMap(url => this.http.post(url, itemBarcode))
  );
}

EncryptPassword(input): Observable<any> {
  return from(this.getUrl(`ChangePswd/EncryptPassword?input=${input}`)).pipe(
    switchMap(url => this.http.post(url, this.httpOptions))
  );
}

GetPackingReportOrderNo(PlantCode: string): Observable<any[]> {
  return from(this.getUrl(`QualityReportsApi/GetPackingReportOrderNo?PlantCode=${PlantCode}`)).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetPackingReport(data: any): Observable<any> {
  return from(this.getUrl(`PackingReportsApi/GetPackingReport`)).pipe(
    switchMap(url => this.http.post(url,data, this.httpOptions))
    );
}

GetPackingOrderbarCodeDtlsReport(data: any): Observable<any> {
  return from(this.getUrl(`PackingOrderBarcodeDtlsReportsApi/GetPackingOrderbarCodeDtlsReport`)).pipe(
    switchMap(url => this.http.post<any>(url, data,this.httpOptions))
  );
}

GetQualityReport(data: any): Observable<any> {
  return from(this.getUrl(`QualityReportsApi/GetQualityReport`)).pipe(
    switchMap(url => this.http.post<any>(url, data,this.httpOptions))
  );
}

GetTransferOrderNo(): Observable<any[]> {
  return from(this.getUrl('TransferPlantToWarehouseApi/GetTransferOrderNo')).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetTranferPlantToWarehouseReport(data: any): Observable<any> {
  return from(this.getUrl(`TransferPlantToWarehouseApi/GetTranferPlantToWarehouseReport`)).pipe(
    switchMap(url => this.http.post<any>(url, data,this.httpOptions))
  );
}

GetAsOnDateInventoryReport(data: any): Observable<any> {
  return from(this.getUrl(`AsOnDateInventoryReportsApi/GetAsOnDateInventoryReport`)).pipe(
    switchMap(url => this.http.post<any>(url, data,this.httpOptions))
  );
}

GetDelieveryNo(): Observable<any[]> {
  return from(this.getUrl('DispatchFromWarehouseReportsApi/GetDelieveryNo')).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetDispatchFromWarehouseReport(data: any): Observable<any> {
  return from(this.getUrl(`DispatchFromWarehouseReportsApi/GetDispatchFromWarehouseReport`)).pipe(
    switchMap(url => this.http.post<any>(url, data,this.httpOptions))
  );
}

SaveLineWork(barcode, lineBarCode) {
  return from(this.getUrl(`ElogSuryaApiService/LineBinMapping_Mapping?barcode=${barcode}&lineBarCode=${lineBarCode}`)).pipe(
    switchMap(url => this.http.post<any[]>(url, this.httpOptions))
  );
}

GetGRN_AtBranchReport(data: any): Observable<any> {
  return from(this.getUrl(`GRN_AtBranchReportsApi/GetGRN_AtBranchReport`)).pipe(
    switchMap(url => this.http.post<any>(url, data,this.httpOptions))
  );
}

GetDispatchFromBranchReport(data: any): Observable<any> {
  return from(this.getUrl(`DispatchFromBranchReportsApi/GetDispatchFromBranchReport`)).pipe(
    switchMap(url => this.http.post<any>(url, data,this.httpOptions))
  );
}

GetReturnToBranchLocationFromDealer(data: any): Observable<any> {
  return from(this.getUrl(`ReturnToBranchLocationFromDealerReportsApi/GetReturnToBranchLocationFromDealer`)).pipe(
    switchMap(url => this.http.post<any>(url, data,this.httpOptions))
  );
}

GetRevalidationReport(data: any): Observable<any> {
  return from(this.getUrl(`RevalidationReportsApi/GetRevalidationReport`)).pipe(
    switchMap(url => this.http.post<any>(url, data,this.httpOptions))
  );
}

DeleteShift(id) {
  return from(this.getUrl(`ElogSuryaApiService/DeleteSift?id=${id}`)).pipe(
    switchMap(url => this.http.post<any[]>(url, this.httpOptions))
  );
}

DeleteBin(id) {
  return from(this.getUrl(`ElogSuryaApiService/DeleteBin?id=${id}`)).pipe(
    switchMap(url => this.http.post<any[]>(url, this.httpOptions))
  );
}

GetMonthlyInspectionReportForDealer(): Observable<any[]> {
  return from(this.getUrl('MonthlyInspectionReportForDealerApi/GetMonthlyInspectionReportForDealer')).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetDealerWiseFailureDetails(): Observable<any[]> {
  return from(this.getUrl('DealerWiseFailureDetailsApi/GetDealerWiseFailureDetails')).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetNonBarcodedProductDetails(): Observable<any[]> {
  return from(this.getUrl('NonBarcodedProductsReportsApi/GetNonBarcodedProductDetails')).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetLifeCycleReport(): Observable<any[]> {
  return from(this.getUrl('LifeCycleReportApi/GetLifeCycleReport')).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetConsNonBarcodedProductDetails(): Observable<any[]> {
  return from(this.getUrl('Consoli_NonBarcodedProductsReportsApi/GetConsNonBarcodedProductDetails')).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetManufacturingMonthWiseDefective(): Observable<any[]> {
  return from(this.getUrl('ManufacturingMonthWiseDefectiveApi/GetManufacturingMonthWiseDefective')).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetManufacturingTimeWiseDefective(): Observable<any[]> {
  return from(this.getUrl('ManufacturingTimeWiseDefectiveApi/GetManufacturingTimeWiseDefective')).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

GetMaterialCode(): Observable<SelectListDto[]> {
  return from(this.getUrl('MaterialToleranceApi/GetMaterialCode')).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
}

 GetItemDesc(plancode: string): Observable<any[]> {
  return from(this.getUrl(`MaterialToleranceApi/GetMaterailDescription?ItemCode=${plancode}`)).pipe(
    switchMap(url => this.http.get<any[]>(url))
  );
 }

 ApproveOnDealerLocation(model: any, type: boolean): Observable<any> {
  if (!type) {
    return from(this.getUrl(`SuryaRevalidationDealerLocation/ApproveRevalidationLocationByCarton`)).pipe(
      switchMap(url => this.http.post(url, model))
    );
  } else {
    return from(this.getUrl(`SuryaRevalidationDealerLocation/ApproveRevalidationLocationByItem`)).pipe(
      switchMap(url => this.http.post(url, model))
    );
  }
}

MaterialToleranceSave(materialCode, minWeight, maxWeight) {
  return from(this.getUrl(`MaterialToleranceApi/MaterialTolerance?materialCode=${materialCode}&minWeight=${minWeight}&maxWeight=${maxWeight}`)).pipe(
    switchMap(url => this.http.post<any[]>(url, this.httpOptions)),
  );
}
}