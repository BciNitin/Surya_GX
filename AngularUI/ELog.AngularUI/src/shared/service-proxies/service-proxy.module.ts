import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AbpHttpInterceptor } from '@abp/abpHttpInterceptor';

import * as ApiServiceProxies from './service-proxies';
import { PlantIdInterceptor } from '@shared/Interceptor/plant-id-interceptor';

@NgModule({
    providers: [
        ApiServiceProxies.RoleServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.TenantServiceProxy,
        ApiServiceProxies.UserServiceProxy,
        ApiServiceProxies.SelectListServiceProxy,
        ApiServiceProxies.TokenAuthServiceProxy,
        ApiServiceProxies.AccountServiceProxy,
        ApiServiceProxies.ConfigurationServiceProxy,
        ApiServiceProxies.SelectListServiceProxy,
        // ApiServiceProxies.PlantServiceProxy,
        // ApiServiceProxies.GateServiceProxy,
        // ApiServiceProxies.LocationServiceProxy,
        // ApiServiceProxies.CubicleServiceProxy,
        // ApiServiceProxies.EquipmentServiceProxy,
        // ApiServiceProxies.HandlingUnitServiceProxy,
        ApiServiceProxies.ModuleServiceProxy,
        // ApiServiceProxies.StandardWeightBoxServiceProxy,
        // ApiServiceProxies.StandardWeightServiceProxy,
        // ApiServiceProxies.WeighingMachineServiceProxy,
        // ApiServiceProxies.DeviceServiceProxy,
        ApiServiceProxies.ClientFormsServiceServiceProxy,
        // ApiServiceProxies.PurchaseOrderServiceProxy,
        // ApiServiceProxies.VehicleInspectionServiceProxy,
        // ApiServiceProxies.MaterialInspectionServiceProxy,
        // ApiServiceProxies.InwardServiceProxy,
        // ApiServiceProxies.WeightCaptureServiceProxy,
        // ApiServiceProxies.PalletizationServiceProxy,
        // ApiServiceProxies.CalenderServiceProxy,
        // ApiServiceProxies.PutAwaysServiceProxy,
        // ApiServiceProxies.BinToBinTransferServiceProxy,
        // ApiServiceProxies.DispensingServiceProxy,
        // ApiServiceProxies.CubicleCleaningServiceProxy,
        // ApiServiceProxies.ProcessOrderServiceProxy,
        // ApiServiceProxies.CubicleAssignmentServiceProxy,
        // ApiServiceProxies.WeighingCalibrationServiceProxy,
        // ApiServiceProxies.EquipmentCleaningServiceProxy,
        ApiServiceProxies.SettingServiceProxy,
        // ApiServiceProxies.EquipmentAssignmentServiceProxy,
        // ApiServiceProxies.LineClearanceServiceProxy,
        // ApiServiceProxies.PickingServiceProxy,
        // ApiServiceProxies.PreStageServiceProxy,
        // ApiServiceProxies.StageServiceProxy,
        // ApiServiceProxies.MaterialDispensingServiceProxy,
        // ApiServiceProxies.SampleDestructionServiceProxy,
        // ApiServiceProxies.StageOutServiceProxy,
        // ApiServiceProxies.ReturnToVendorServiceProxy,
        // ApiServiceProxies.DestructionServiceProxy,
        // ApiServiceProxies.ReportServiceProxy,
        // ApiServiceProxies.ReportFiltersServiceProxy,
        // ApiServiceProxies.ReportFiltersServiceProxy,
        ApiServiceProxies.ChangePswdServiceProxy,
        // ApiServiceProxies.IssueToProductionServiceProxy,
        // ApiServiceProxies.MaterialIStatusServiceProxy,
        // ApiServiceProxies.MaterialStatusLabelServiceProxy,
        { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: PlantIdInterceptor, multi: true },
        ApiServiceProxies.ApprovalLevelServiceProxy,
        ApiServiceProxies.ApprovalUserModuleMappingServiceServiceProxy,
        // ApiServiceProxies.ActivityMasterServiceServiceProxy,
        // ApiServiceProxies.RecipeMasterServiceServiceProxy,
        // ApiServiceProxies.AreaUsageLogServiceServiceProxy,
        // ApiServiceProxies.EquipmentUsageLogServiceServiceProxy,
        // ApiServiceProxies.CubicleAssignmentsServiceProxy,
        // ApiServiceProxies.WeightVerificationServiceServiceProxy,
        // ApiServiceProxies.ConsumptionServiceProxy,
        // ApiServiceProxies.PutawayServiceServiceProxy,
        // ApiServiceProxies.WipPickingServiceServiceProxy,
        // ApiServiceProxies.PackingServiceServiceProxy,
        // ApiServiceProxies.RecipePOMappingServiceServiceProxy,
        // ApiServiceProxies.PalletMasterServiceServiceProxy,
        // ApiServiceProxies.PostToSAPServiceProxy,
        // ApiServiceProxies.MaterialReturnSapServiceServiceProxy,
        // ApiServiceProxies.FgPutAwayServiceServiceProxy,
        // ApiServiceProxies.WIPLineClearanceServiceProxy,
        // ApiServiceProxies.ProcessOrderAfterReleasServiceProxy,
        // ApiServiceProxies.FgPickingServiceServiceProxy,
        // ApiServiceProxies.LoadingServiceServiceProxy,
        // ApiServiceProxies.MaterialVerificationServiceServiceProxy,
        ApiServiceProxies.ElogApiServiceServiceProxy,
        // ApiServiceProxies.CSVBulkUploadServiceServiceProxy,
       
        { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true }
    ]
})
export class ServiceProxyModule { }