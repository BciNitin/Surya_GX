import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Inject, Injectable, InjectionToken, OnInit } from '@angular/core';
import { bininput } from '@app/masters/bin/bin.component';
import { linework } from '@app/PlantOperation/line-work-center/line-work-center.component';
import { GenerateSerialNumber } from '@app/PlantOperation/serialbarcodegeneration/serialbarcodegeneration.component';

import { SelectListDto } from '@shared/service-proxies/service-proxies';
import { Observable } from 'rxjs';
import { AppConfigService } from './AppConfigService ';

@Injectable({

   providedIn: 'root'
})

export class ApiServiceService {
   //baseUrl = 'http://180.151.246.51:8089/api/services/app/';
   baseUrl = 'http://localhost:21021/api/services/app/';
   apiUrlGetMaterialMaster = 'ElogSuryaApiService/GetMaterialMaster';

   httpOptions = {
      headers: new HttpHeaders({
         'Content-Type': 'application/json',
         'Access-Control-Allow-Origin': '*'
      })
   };

   // constructor(private http: HttpClient, private appConfigService: AppConfigService) {
   //    // this.appConfigService.loadConfig().subscribe((config) => {
   //    //    //this.appConfigService.config = config;
   //    //    this.baseUrl = config.remoteServiceBaseUrl+'/';
   //    //  });

   constructor(private http: HttpClient, private appConfigService: AppConfigService) {
   }

   getMaterialMaster(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'ElogSuryaApiService/GetMaterialMaster');
   }

   getPlantMaster(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'ElogSuryaApiService/GetPlantMaster');
   }
   getCustomerMaster(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'ElogSuryaApiService/GetCustomerMaster');
   }
   getPackingMasters(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'ElogSuryaApiService/GetPackingMaster');

   }
   getLineMaster(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'ElogSuryaApiService/GetLineMaster');
   }

   getBinMaster(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'ElogSuryaApiService/GetBinMaster');
   }

   getPlantCode(): Observable<SelectListDto[]> {
      const httpOptions = {
         headers: new HttpHeaders({
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
         })
      };
      return this.http.get<any[]>(this.baseUrl + 'selectList/GetPlantCode', httpOptions);
   }

   getBinCode(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'ElogSuryaApiService/GetBinCode');
   }


   SaveBinMaster(input: bininput) {
      const content_ = JSON.stringify(input);
      return this.http.post<any[]>(this.baseUrl + 'ElogSuryaApiService/CreateBinMaster', content_, this.httpOptions);
   }

   getBinById(Id: Int32Array): Observable<any[]> {
      ;
      console.log("Id", Id);
      return this.http.get<any[]>(this.baseUrl + 'ElogSuryaApiService/GetBinCGetBinById?id=' + Id);
   }

   getLineWorkCenterNo(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'SelectList/GetLineWorkNo');
   }

   getQyalityCheckingLineWorkCenterNo(PlantCode): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + `QualitySampling/GetLineWorkNo?PlantCode=${PlantCode}`);
   }

   getqualitySamplingLineWorkCenterNo(PlantCode): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + `QualityChecking/GetLineWorkNo?PlantCode=${PlantCode}`);
   }

   getPackingOrderNo(plancode: string): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'SelectList/GetPackingOrderNo?plantCode=' + plancode);
   }

   GenerateSerialNumber(plancode: string): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'SelectList/GetPackingOrderNo?plantCode=' + plancode);
   }

   GetSerialNumberDetails(packingOrderNo: string): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'SuryaGenerateSerialNo/GetSerialNumberDetails?packingOrder=' + packingOrderNo);
   }

   downloadSerialNumberCSV(fileName: string): Observable<HttpResponse<Blob>> {
      // const url = `${this.baseUrl}SuryaGenerateSerialNo/DownloadFile?FileName=`+fileName;
      const url = `${this.baseUrl}SuryaGenerateSerialNo/GetDownloadFile?FileName=` + fileName
      const headers = new HttpHeaders({
         'Content-Type': 'application/json',
         // Add any additional headers if needed
      });
      return this.http.get(url, { // <-- Corrected syntax
         headers: headers,
         observe: 'response',
         responseType: 'blob',
      });
   }

   getPackingOrderNoForSerialNumber(plancode: string, linecode: string): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + `SuryaGenerateSerialNo/GetPackingOrderNo?plantCode=${plancode}&linecode=${linecode}`);
   }
   getLineCodeasPerPlant(plancode: string): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + `SuryaGenerateSerialNo/GetLineAsPerPlant?plantCode=${plancode}`);
   }


   SaveSerialBarcodeGen(input: GenerateSerialNumber) {
      const httpOptions = {
         headers: new HttpHeaders({
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
         })
      };
      const content_ = JSON.stringify(input);
      return this.http.post<any[]>(this.baseUrl + 'SuryaGenerateSerialNo/GenerateSerialNo', content_, httpOptions);
   }

   GetConfirming_PO_No_(planCode: string): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + `PackingOrderConfirmation/GetConfirmationPackingOrderNo?PlantCode=${planCode}`);
   }

   GetPackingOrderConfirmingDetails(packingOrderNo: string, planCode: string): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + `PackingOrderConfirmation/GetPackingOrderDetails?packingOrder=${packingOrderNo}&PlantCode=${planCode}`);
   }

   PackingOrderConfirmation(data: any): Observable<any> {
      return this.http.post(this.baseUrl + 'PackingOrderConfirmation/ConfirmPackingOrder', data[0], this.httpOptions);
   }

   GetConfirmationPackingOrderNo(planCode): Observable<any> {
      return this.http.get<any[]>(this.baseUrl + `SuryaQualityConfirmation/GetPackingOrderNo?plantcode=${planCode}`, this.httpOptions);
   }

   getStorageMaster(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'ElogSuryaApiService/GetStorageLocationMaster');
   }
   getShiftMaster(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'ElogSuryaApiService/GetSiftMaster');
   }
   DeleteSiftMasterbyid(ShiftCode: string) {
      return this.http.delete(this.baseUrl + 'ElogSuryaApiService/DeleteSift?id=' + ShiftCode);
   }

   CreateSiftMaster(ShiftCode, ShiftDescription, sShiftStartTime, sShiftEndTime) {
      return this.http.post<any[]>(this.baseUrl + `ElogSuryaApiService/CreateSiftMaster?ShiftCode=${ShiftCode}&ShiftDescription=${ShiftDescription}&sShiftStartTime=${sShiftStartTime}&sShiftEndTime=${sShiftEndTime}`, this.httpOptions);
   }
   QualitySaplingPackingOrderNo(planCode, lineCode) {
      return this.http.get<any[]>(this.baseUrl + `QualitySampling/GetPackingOrderByPlantAndLine?PlantCode=${planCode}&LineNo=${lineCode}`, this.httpOptions);
   }
   ScanCartonBarCode(planCode, lineCode, cartonBarCode, packingOrderNo) {
      return this.http.post<any[]>(this.baseUrl + `QualitySampling/ScanCartonBarCode?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&CartonBarCode=${cartonBarCode}&LineCode=${lineCode}`, this.httpOptions);
   }

   ScanItemBarCode(planCode, lineCode, cartonBarCode, childBarCode, packingOrderNo) {
      return this.http.post<any[]>(this.baseUrl + `QualitySampling/ScanItemBarCode?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&CartonBarCode=${cartonBarCode}&ItemBarCode=${childBarCode}&LineCode=${lineCode}`, this.httpOptions);
   }

   GetQualitySamplingQuantity(planCode, lineCode, packingOrderNo) {
      return this.http.get<any[]>(this.baseUrl + `QualitySampling/GetQualitySamplingQty?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&LineCode=${lineCode}`, this.httpOptions);
   }

   SaveQualitySampling(planCode, lineCode, packingOrderNo, CartonBarCode) {
      return this.http.post<any[]>(this.baseUrl + `QualitySampling/QualitySamplingSave?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&CartonBarCode=${CartonBarCode}&LineCode=${lineCode}`, this.httpOptions);
   }


   GetQualityChecking(planCode, lineCode, packingOrderNo) {
      return this.http.get<any[]>(this.baseUrl + `QualityChecking/GetQualityCheckingDetails?PackingOrderNo=${packingOrderNo}&PlantCode=${planCode}&LineCode=${lineCode}`, this.httpOptions);
   }

   saveQualityChecking(data: any[]): Observable<any> {
      return this.http.post(this.baseUrl + 'QualityChecking/SaveQualityChecking', data);
   }

   GetQualityConfirmationDetails(platcode, packingorderno): Observable<any> {
      return this.http.get(this.baseUrl + `SuryaQualityConfirmation/GetQCCheckingDetails?plantcode=${platcode}&packingorderno=${packingorderno}`, this.httpOptions);
   }

   GetQualityConfirmationPONo(platcode): Observable<any> {
      return this.http.post(this.baseUrl + `SuryaQualityConfirmation/GetPackingOrderNo?plantcode=${platcode}`, this.httpOptions);
   }

   saveQualityConfirmation(data: any[]): Observable<any> {
      return this.http.post(this.baseUrl + 'SuryaQualityConfirmation/QCConfirm', data);
   }

   getManualPackingPackingOrderNo(plancode, lineCode): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + `ManualPackingApi/GetPackingOrderNo?plantcode=${plancode}&lineCode=${lineCode}`);
   }

   GetManualPackingDetails(packingOrderNo, plantCode, ScanItem) {

      return this.http.get<any[]>(this.baseUrl + `ElogSuryaApiService/GetManualPackingDetails?packingOrderNo=${packingOrderNo}&plantCode=${plantCode}&ScanItem=${ScanItem}`);
   }
   GetStorageLocation(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'StorageLocationApi/GetStorageLocation');
   }

   GetStrLocationDtls(plancode, LocationID) {
      return this.http.get<any[]>(this.baseUrl + `StorageLocationApi/GetStorageLocationDetails?plancode=${plancode}&LocationID=${LocationID}`);
   }
   GetBarcodeScannedDetails(barcode, plantcode) {
      return this.http.get<any[]>(this.baseUrl + `StorageLocationApi/GetBarcodeScannedDetails?barcode=${barcode}&plantcode=${plantcode}`);
   }

   StorageLocationConfirmation(barcode, LocationID) {
      return this.http.post<any[]>(this.baseUrl + `StorageLocationApi/StorageLocationConfirmation?barcode=${barcode}&LocationID=${LocationID}`, this.httpOptions);
   }

   GetManualPackingDtls(plantcode, packingorder, linecode) {
      return this.http.get<any[]>(this.baseUrl + `ManualPackingApi/GetManualPackingDetails?plantcode=${plantcode}&packingorder=${packingorder}&linecode=${linecode}`);
   }

   ValidateBarcode(BinBarCode, macAddresses, plantcode, packingorder, linecode,materialCode) {
      return this.http.post<any[]>(this.baseUrl + `ManualPackingApi/ValidateBarcode?BinBarCode=${BinBarCode}&macAddresses=${macAddresses}&plantcode=${plantcode}&packingorder=${packingorder}&linecode=${linecode}&MaterialCode=${materialCode}`, this.httpOptions);
   }


   getIPAddress(): Observable<any[]> {

      return this.http.get<any[]>(this.baseUrl + 'ManualPackingApi/GetMacAddress');
   }

   GetQualityItemTestedDtls(itemBarcode, plantCode) {
      return this.http.get<any[]>(this.baseUrl + `QualityTested_ItemPlacementApi/GetQualityItemTestedDtls?itemBarcode=${itemBarcode}&plantCode=${plantCode}`);
   }
   ValidateShiperBarcode(itemBarcode, plantCode, ShiperBarcode) {
      //const content_ = JSON.stringify(input);
      return this.http.get<any[]>(this.baseUrl + `QualityTested_ItemPlacementApi/GetValidateShiperBarcode?itemBarcode=${itemBarcode}&plantCode=${plantCode}&ShiperBarcode=${ShiperBarcode}`);
   }

   QualityCheckingPackingOrderNo(planCode, lineCode) {
      return this.http.get<any[]>(this.baseUrl + `QualityChecking/GetPackingOrderByPlantAndLine?PlantCode=${planCode}&LineNo=${lineCode}`, this.httpOptions);
   }
   GetchallanNo(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'TransferToBranchFromPlantApi/GetchallanNo');
   }
   // GetChallanDetails(DeliveryChallanNo: string): Observable<any[]> {
   //    return this.http.get<any[]>(this.baseUrl + 'TransferToBranchFromPlantApi/GetChallanDetails?DeliveryChallanNo=' + DeliveryChallanNo);
   // }

   GetValidateScanCartonBarcode(DeliveryChallanNo,CartonBarcode,MaterialCode,FromPlantCode,ToPlantCode) {
      return this.http.get<any[]>(this.baseUrl + `TransferToBranchFromPlantApi/GetValidateScanCartonBarcode?DeliveryChallanNo=${DeliveryChallanNo}&CartonBarcode=${CartonBarcode}&MaterialCode=${MaterialCode}&FromPlantCode=${FromPlantCode}&ToPlantCode=${ToPlantCode}`);
   }

   GetSOchallanNo(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'TransferToDealerCustFromBranchLocApi/GetSOchallanNo');
   }

   GetSOMaterialCode(DeliveryChallanNo): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + `TransferToDealerCustFromBranchLocApi/GetMaterialCode?DeliveryChallanNo=${DeliveryChallanNo}`);
   }

   GetSTOMaterialCode(DeliveryChallanNo): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + `TransferToBranchFromPlantApi/GetMaterialCode?DeliveryChallanNo=${DeliveryChallanNo}`);
   }
   
   GetSOChallanDetails(DeliveryChallanNo) {
      return this.http.get<any[]>(this.baseUrl + `GRN_ConfirmationApi/GetChallanNoDetails?DeliveryChallanNo=${DeliveryChallanNo}`);
   }
   GetDealerTransferSOChallanDetails(DeliveryChallanNo,MaterialCode) {
      return this.http.get<any[]>(this.baseUrl + `TransferToDealerCustFromBranchLocApi/GetSOChallanDetails?DeliveryChallanNo=${DeliveryChallanNo}&MaterialCode=${MaterialCode}`);
   }
   GetValidateSOScanCartonBarcode(DeliveryChallanNo,CartonBarcode,MaterialCode,SONo,PlantCode,CustomerCode) {
      return this.http.get<any[]>(this.baseUrl + `TransferToDealerCustFromBranchLocApi/GetValidateSOScanCartonBarcode?DeliveryChallanNo=${DeliveryChallanNo}&CartonBarcode=${CartonBarcode}&MaterialCode=${MaterialCode}&SONo=${SONo}&PlantCode=${PlantCode}&CustomerCode=${CustomerCode}`)
   }
   GetItemCodes(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'RevalidationProcessBranchPlantApi/GetExpiredItemCode');
   }

   GetPlantToBranchPlantCode(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'RevalidationProcessBranchPlantApi/GetPlantCode');
   }

   GetExpiredItemCodeDetails(MaterialCode, PlantCode) {
      return this.http.get<any[]>(this.baseUrl + `RevalidationProcessBranchPlantApi/GetExpiredItemCodeDetails?MaterialCode=${MaterialCode}&PlantCode=${PlantCode}`);
   }

   ValidateItem(data: any[]): Observable<any> {
      return this.http.post(this.baseUrl + 'RevalidationProcessBranchPlantApi/ValidateItem', data);
   }

   ValidateItemByBarCode(Barcode) {
      return this.http.post<any[]>(this.baseUrl + `RevalidationProcessBranchPlantApi/ValidateItemByBarCode?Barcode=`+Barcode,this.httpOptions);
   }

   GetValidateGRNConfirmation(DeliveryChallanNo, CartonBarcode, CheckNGOK, toPlantCode, fromPlantCode) {
      return this.http.get<any[]>(this.baseUrl + `GRN_ConfirmationApi/GetValidateGRNConfirmation?DeliveryChallanNo=${DeliveryChallanNo}&CartonBarcode=${CartonBarcode}&Status=${CheckNGOK}&ToPlantCode=${toPlantCode}&FromPlantCode=${fromPlantCode}`);
   }

   ConfirmGRN(DeliveryChallanNo, CartonBarcode, CheckNGOK, toPlantCode, fromPlantCode) {
      return this.http.get<any[]>(this.baseUrl + `GRN_ConfirmationApi/GetGRNConfirmation?DeliveryChallanNo=${DeliveryChallanNo}&CartonBarcode=${CartonBarcode}&Status=${CheckNGOK}&ToPlantCode=${toPlantCode}&FromPlantCode=${fromPlantCode}`);
   }

   GetSTOChallanDetails(DeliveryChallanNo,MaterialCode) {
      return this.http.get<any[]>(this.baseUrl + `TransferToBranchFromPlantApi/GetChallanDetails?DeliveryChallanNo=${DeliveryChallanNo}&MaterialCode=${MaterialCode}`);
   }
   
   GetCustomerCode(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'BarcodedWarrantyClaimApi/GetCustomerCode');
   }
   GetChallanNoForGRNConfirm(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'GRN_ConfirmationApi/GetDeliveryChallanNumber');
   }

   GetWarrantyDetails(Barcode, CustomerCode) {
      return this.http.get<any[]>(this.baseUrl + `BarcodedWarrantyClaimApi/GetWarrantyDetails?Barcode=${Barcode}&CustomerCode=${CustomerCode}`);
   }
   GetValidateWarrranty(Barcode, CustomerCode, BarCodeApprovedQty) {
      return this.http.get<any[]>(this.baseUrl + `BarcodedWarrantyClaimApi/GetValidateWarrranty?Barcode=${Barcode}&CustomerCode=${CustomerCode}&BarCodeApprovedQty=${BarCodeApprovedQty}`);
   }

   GetNonBarcodedWarrantyDetails(Qty, CustomerCode, ApprovedQty, MaterialCode) {
      return this.http.get<any[]>(this.baseUrl + `NonBarcodedWarrantyClaimApi/GetNonBarcodedWarrantyDetails?Qty=${Qty}&CustomerCode=${CustomerCode}&ApprovedQty=${ApprovedQty}&MaterialCode=${MaterialCode}`);
   }

   GetDealerCode(): Observable<any> {
      return this.http.get(this.baseUrl + `SuryaRevalidationDealerLocation/GetDealerCode`, this.httpOptions);
   }
   GetValidateNonBarcodedWarrranty(MaterialCode, CustomerCode, Qty, ApprovedQty) {
      return this.http.get<any[]>(this.baseUrl + `NonBarcodedWarrantyClaimApi/GetValidateNonBarcodedWarrranty?MaterialCode=${MaterialCode}&CustomerCode=${CustomerCode}&Qty=${Qty}&ApprovedQty=${ApprovedQty}`);
   }
   GetWarrantyTrackingDtls(QrCode) {
      return this.http.get<any[]>(this.baseUrl + `WarrantyTrackingApi/GetWarrantyTrackingDtls?QrCode=${QrCode}`);
   }

   GetDelarLocationApproveDetails() {
      return this.http.get<any[]>(this.baseUrl + `SuryaRevalidationDealerLocation/GetDelarLocationApproveDetails`);
   }

   GetApprovalDtlsById(dealerCode, ItemBarCode) {
      return this.http.get<any[]>(this.baseUrl + `SuryaRevalidationDealerLocation/GetDelarLocationApproveDetailsById?dealercode=${dealerCode}&itemcode=${ItemBarCode}`);
   }

   GetRevalidationOnDealerCarton(dealerCode, barCode): Observable<any> {
      return this.http.get(this.baseUrl + `SuryaRevalidationDealerLocation/GetRevalidationOnCarton?DealerCode=${dealerCode}&BarCode=${barCode}`, this.httpOptions);
   }

   GetRevalidationDealerOnItem(dealerCode, ItemBarCode): Observable<any> {
      return this.http.get(this.baseUrl + `SuryaRevalidationDealerLocation/GetRevalidationOnItem?DealerCode=${dealerCode}&ItemBarCode=${ItemBarCode}`, this.httpOptions);
   }

   ApproveOnDealerLocation(model: any, type: boolean): Observable<any> {
      if (!type) {
         return this.http.post(this.baseUrl + `SuryaRevalidationDealerLocation/ApproveRevalidationLocationByCarton`, model);
      }
      else {
         return this.http.post(this.baseUrl + `SuryaRevalidationDealerLocation/ApproveRevalidationLocationByItem`, model);
      }
   }
   ConfirmRevalidation(itemBarcode): Observable<any> {
      return this.http.post(this.baseUrl + `SuryaRevalidationDealerLocation/ApproveRevalidation`, itemBarcode);
   }

   EncryptPassword(input): Observable<any> {
      return this.http.post(this.baseUrl + `ChangePswd/EncryptPassword?input=${input}`, this.httpOptions);
   }
   GetPackingReportOrderNo(PlantCode: string): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'QualityReportsApi/GetPackingReportOrderNo?PlantCode=' + PlantCode);
   }

   GetPackingReport(data: any): Observable<any> {

      return this.http.post<any>(this.baseUrl + `PackingReportsApi/GetPackingReport`, data);
   }

   GetPackingOrderbarCodeDtlsReport(data: any): Observable<any> {

      return this.http.post<any>(this.baseUrl + `PackingOrderBarcodeDtlsReportsApi/GetPackingOrderbarCodeDtlsReport`, data);
   }
   GetQualityReport(data: any): Observable<any> {

      return this.http.post<any>(this.baseUrl + `QualityReportsApi/GetQualityReport`, data);
   }
   GetTransferOrderNo(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'TransferPlantToWarehouseApi/GetTransferOrderNo');
   }
   GetTranferPlantToWarehouseReport(data: any): Observable<any> {

      return this.http.post<any>(this.baseUrl + `TransferPlantToWarehouseApi/GetTranferPlantToWarehouseReport`, data);
   }
   GetAsOnDateInventoryReport(data: any): Observable<any> {

      return this.http.post<any>(this.baseUrl + `AsOnDateInventoryReportsApi/GetAsOnDateInventoryReport`, data);
   }
   GetDelieveryNo(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'DispatchFromWarehouseReportsApi/GetDelieveryNo');
   }
   GetDispatchFromWarehouseReport(data: any): Observable<any> {

      return this.http.post<any>(this.baseUrl + `DispatchFromWarehouseReportsApi/GetDispatchFromWarehouseReport`, data);
   }
   SaveLineWork(barcode, lineBarCode) {
      return this.http.post<any[]>(this.baseUrl + `ElogSuryaApiService/LineBinMapping_Mapping?barcode=${barcode}&lineBarCode=${lineBarCode}`, this.httpOptions);
   }
   GetGRN_AtBranchReport(data: any): Observable<any> {

      return this.http.post<any>(this.baseUrl + `GRN_AtBranchReportsApi/GetGRN_AtBranchReport`, data);
   }
   GetDispatchFromBranchReport(data: any): Observable<any> {

      return this.http.post<any>(this.baseUrl + `DispatchFromBranchReportsApi/GetDispatchFromBranchReport`, data);
   }

   GetReturnToBranchLocationFromDealer(data: any): Observable<any> {

      return this.http.post<any>(this.baseUrl + `ReturnToBranchLocationFromDealerReportsApi/GetReturnToBranchLocationFromDealer`, data);
   }
   GetRevalidationReport(data: any): Observable<any> {

      return this.http.post<any>(this.baseUrl + `RevalidationReportsApi/GetRevalidationReport`, data);
   }
   DeleteShift(id) {
      return this.http.post<any[]>(this.baseUrl + `ElogSuryaApiService/DeleteSift?id=${id}`, this.httpOptions);
   }

   DeleteBin(id) {
      return this.http.post<any[]>(this.baseUrl + `ElogSuryaApiService/DeleteBin?id=${id}`, this.httpOptions);
   }

   GetMonthlyInspectionReportForDealer(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'MonthlyInspectionReportForDealerApi/GetMonthlyInspectionReportForDealer');
   }

   GetDealerWiseFailureDetails(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'DealerWiseFailureDetailsApi/GetDealerWiseFailureDetails');
   }

   GetNonBarcodedProductDetails(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'NonBarcodedProductsReportsApi/GetNonBarcodedProductDetails');
   }

   GetLifeCycleReport(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'LifeCycleReportApi/GetLifeCycleReport');
   }
   GetConsNonBarcodedProductDetails(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'Consoli_NonBarcodedProductsReportsApi/GetConsNonBarcodedProductDetails');
   }
   GetManufacturingMonthWiseDefective(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'ManufacturingMonthWiseDefectiveApi/GetManufacturingMonthWiseDefective');
   }
   GetManufacturingTimeWiseDefective(): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + 'ManufacturingTimeWiseDefectiveApi/GetManufacturingTimeWiseDefective');
   }
   GetMaterialCode(): Observable<SelectListDto[]> {
      return this.http.get<any[]>(this.baseUrl + 'MaterialToleranceApi/GetMaterialCode');
   }
   GetItemDesc(plancode: string): Observable<any[]> {
      return this.http.get<any[]>(this.baseUrl + `MaterialToleranceApi/GetMaterailDescription?ItemCode=${plancode}`);
   }

   MaterialToleranceSave(materialCode, minWeight, maxWeight) {
      return this.http.post<any[]>(this.baseUrl + `MaterialToleranceApi/MaterialTolerance?materialCode=${materialCode}&minWeight=${minWeight}&maxWeight=${maxWeight}`, this.httpOptions);
   }
}