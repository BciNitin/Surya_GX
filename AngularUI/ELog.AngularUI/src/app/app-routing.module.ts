import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { HomeComponent } from './home/home.component';
import { UsersComponent } from './users/users.component';
import { TenantsComponent } from './tenants/tenants.component';
import { RolesComponent } from 'app/roles/roles.component';
import { AddEditUserComponent } from './users/add-edit-user/add-edit-user.component';
import { AddEditRoleComponent } from './roles/add-edit-role/add-edit-role.component';
import { ModulesComponent } from './modules/modules.component';
import { AddEditModuleComponent } from './modules/add-edit-module/add-edit-module.component';
import { SubModulesComponent } from './subModules/subModules.component';
import { AddEditSubModuleComponent } from './subModules/add-edit-subModule/add-edit-subModule.component';
import { ResetPasswordComponent } from './password/reset-password/reset-password.component';

import { CreateformsComponent } from './log-forms-list/createforms/createforms.component';
import { ApproveformsComponent } from './log-forms-list/approveforms/approveforms.component';
import { LogFormsListComponent } from './log-forms-list/log-forms-list.component';
import { ElogPanelComponent } from './elog-panel/elog-panel.component';
import { NewPanelComponent } from './elog-panel/new-panel/new-panel.component';
import { ApproveFormlistComponent } from './approve-forms-list/approve-forms-list.component'
import { LogdataapprovalComponent } from './log-forms-list/log-data-approval/log-data-approval.component';
import { NotificationsCenterComponent } from './notifications-center/notifications-center.component';
import { PasswordComponent } from './password/password.component';
import { LineWorkCenterComponent } from './PlantOperation/line-work-center/line-work-center.component';
import { PlantComponent } from './masters/plant/plant.component';
import { CustomerComponent } from './masters/customer/customer.component';
import { ManualPackingComponent } from './PlantOperation/manual-packing/manual-packing.component';
import { QualitySamplingComponent } from './PlantOperation/quality-sampling/quality-sampling.component';
import { SerialbarcodegenerationComponent } from './PlantOperation/serialbarcodegeneration/serialbarcodegeneration.component';
import { PackingOrderConfirmationComponent } from './PlantOperation/packing-order-confirmation/packing-order-confirmation.component';
import { PackingOrderComponent } from './masters/packing-order/packing-order.component';
import { MaterialComponent } from './masters/material/material.component';
import { LineMasterComponent } from './masters/line-master/line-master.component';
import { AddEditCustomerComponent } from './masters/customer/add-edit-customer/add-edit-customer.component';
import { StorageLocationComponent } from './masters/storage-location/storage-location.component';
import { ShiftMasterComponent } from './masters/shift-master/shift-master.component';
import { BinComponent } from './masters/bin/bin.component';
import { AddeditbinComponent } from './masters/bin/addeditbin/addeditbin.component';
//import { EditbinComponent } from './masters/bin/editbin/editbin/editbin.component';
import { QualityCheckingComponent } from './PlantOperation/quality-checking/quality-checking.component';
import { QualityConfirmationComponent } from './PlantOperation/quality-confirmation/quality-confirmation.component';
import { StorageLocationTransferComponent } from './PlantOperation/storage-location-transfer/storage-location-transfer.component';
import { AddEditShiftComponent } from './masters/shift-master/add-edit-shift/add-edit-shift.component';
import { QualityTestedItemplacementComponent } from './PlantOperation/quality-tested-itemplacement/quality-tested-itemplacement.component';
import { TransferToBranchFromPlantComponent } from './PlantOperation/transfer-to-branch-from-plant/transfer-to-branch-from-plant.component';
import { TransferDealerCustomerfromBranchComponent } from './PlantOperation/transfer-dealer-customerfrom-branch/transfer-dealer-customerfrom-branch.component';
import { RevalidationProcessBranchComponent } from './PlantOperation/revalidation-process-branch/revalidation-process-branch.component';
import { GrnConfirmationComponent } from './PlantOperation/grn-confirmation/grn-confirmation.component';
import { ApprovalForZonalManagerComponent } from './PlantOperation/approval-for-zonal-manager/approval-for-zonal-manager.component';
import { WarrantyClaimComponent } from './PlantOperation/warranty-claim/warranty-claim.component';
import { RevalidationDealerLocationComponent } from './PlantOperation/revalidation-dealer-location/revalidation-dealer-location.component';
import { WarrantyTrackingComponent } from './PlantOperation/warranty-tracking/warranty-tracking.component';
import { AddEditZonalComponent } from './PlantOperation/approval-for-zonal-manager/add-edit-zonal/add-edit-zonal.component';
import { PackingReportsComponent } from './Reports/packing-reports/packing-reports.component';
import { PackingOrderBarcodeDtlsComponent } from './Reports/packing-order-barcode-dtls/packing-order-barcode-dtls.component';
import { QualityReportComponent } from './Reports/quality-report/quality-report.component';
import { PlantToWarehouseComponent } from './Reports/plant-to-warehouse/plant-to-warehouse.component';
import { AsOnDateInventoryComponent } from './Reports/as-on-date-inventory/as-on-date-inventory.component';
import { DispatchFromWarehouseComponent } from './Reports/dispatch-from-warehouse/dispatch-from-warehouse.component';
import { GrnAtBranchComponent } from './Reports/grn-at-branch/grn-at-branch.component';
import { DispatchFromBranchComponent } from './Reports/dispatch-from-branch/dispatch-from-branch.component';
import { BranchLocationFromDealerComponent } from './Reports/branch-location-from-dealer/branch-location-from-dealer.component';
import { RevalidationReportComponent } from './Reports/revalidation-report/revalidation-report.component';
import { MonthlyInspecForDealerComponent } from './Reports/monthly-inspec-for-dealer/monthly-inspec-for-dealer.component';
import { DealerWiseFailureDetailsComponent } from './Reports/dealer-wise-failure-details/dealer-wise-failure-details.component';
import { NonBarcodedProductsComponent } from './Reports/non-barcoded-products/non-barcoded-products.component';
import { ConsolidatedNonBarcodedProductsComponent } from './Reports/consolidated-non-barcoded-products/consolidated-non-barcoded-products.component';
import { LifeCycleReportComponent } from './Reports/life-cycle-report/life-cycle-report.component';
import { ManufacturingMonthWiseDefectiveComponent } from './Reports/manufacturing-month-wise-defective/manufacturing-month-wise-defective.component';
import { ManufacturingTimeWiseDefectiveComponent } from './Reports/manufacturing-time-wise-defective/manufacturing-time-wise-defective.component';
import { MaterialToleranceComponent } from './masters/material-tolerance/material-tolerance.component';



@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                children: [
                    { path: 'home', component: HomeComponent, canActivate: [AppRouteGuard] },

                    { path: 'Creator', component: NewPanelComponent, canActivate: [AppRouteGuard] }, 
                    { path: 'Creator/:id', component: NewPanelComponent, canActivate: [AppRouteGuard] },
                    
                    { path: 'tenants', component: TenantsComponent, data: { permission: 'Pages.Tenants' }, canActivate: [AppRouteGuard] },

                    { path: 'users', component: UsersComponent, canActivate: [AppRouteGuard] },
                    { path: 'add-user', component: AddEditUserComponent,data: { permission: 'User.Add' }, canActivate: [AppRouteGuard] },
                    { path: 'user/:action/:userId', component: AddEditUserComponent, data: { permission: 'User.View' }, canActivate: [AppRouteGuard] },
                    { path: 'edit-user/:userId', component: AddEditUserComponent, data: { permission: 'User.Edit' }, canActivate: [AppRouteGuard] },
                    { path: 'profile/:action/:profileId', component: AddEditUserComponent, data: { permission: '' }, canActivate: [AppRouteGuard] },
                    { path: 'edit-profile/:profileId', component: AddEditUserComponent, data: { permission: '' }, canActivate: [AppRouteGuard] },

                    { path: 'log-forms-list', component: LogFormsListComponent, canActivate: [AppRouteGuard] },

                    // { path: 'add-edit-forms', component: AddEditFormsComponent, canActivate: [AppRouteGuard] },                   
                  
                    { path: 'createforms', component: CreateformsComponent, canActivate: [AppRouteGuard] },
                    { path: 'createforms/:formId', component: CreateformsComponent, canActivate: [AppRouteGuard] }, 
                    { path: 'createforms/edit/:formId/:dataId', component: CreateformsComponent, canActivate: [AppRouteGuard] }, 
                   
                    { path: 'notifications', component: NotificationsCenterComponent},
                    
                    { path: 'logDataList', component: LogdataapprovalComponent, canActivate: [AppRouteGuard] },
                    { path: 'logDataList/:formId', component: LogdataapprovalComponent, canActivate: [AppRouteGuard] }, 
                    

                    { path: 'approveforms', component: ApproveformsComponent, canActivate: [AppRouteGuard] },
                    { path: 'approveforms/:formId', component: ApproveformsComponent, canActivate: [AppRouteGuard] },
                   

                    { path: 'roles', component: RolesComponent, canActivate: [AppRouteGuard] },
                    { path: 'add-role', component: AddEditRoleComponent, data: { permission: 'Role.Add' }, canActivate: [AppRouteGuard] },
                    { path: 'role/:action/:roleId', component: AddEditRoleComponent, data: { permission: 'Role.View' }, canActivate: [AppRouteGuard] },
                    { path: 'edit-role/:roleId', component: AddEditRoleComponent, data: { permission: 'Role.Edit' }, canActivate: [AppRouteGuard] },


                    { path: 'modules', component: ModulesComponent, canActivate: [AppRouteGuard] },
                    { path: 'module/:action/:moduleId', component: AddEditModuleComponent, canActivate: [AppRouteGuard] },
                    { path: 'edit-module/:action/:moduleId', component: AddEditModuleComponent, canActivate: [AppRouteGuard] },

                    { path: 'subModules', component: SubModulesComponent, canActivate: [AppRouteGuard] },
                    { path: 'subModule/:action/:subModuleId', component: AddEditSubModuleComponent , canActivate: [AppRouteGuard]},
                    { path: 'edit-subModule/:action/:subModuleId', component: AddEditSubModuleComponent, canActivate: [AppRouteGuard] },

                    { path: 'password', component: PasswordComponent, canActivate: [AppRouteGuard] },
                    { path: 'reset-password/:userId', component: ResetPasswordComponent, data: { permission: 'Password.Edit' } },
                     { path: 'reset-password/:userId/:action', component: ResetPasswordComponent, data: { permission: 'Password.Edit' } },
                    { path: 'logData/createforms/edit/:formId/:dataId', component: CreateformsComponent, data: { permission: '' }, canActivate: [AppRouteGuard] },
                    { path: 'approve-forms-list', component: ApproveFormlistComponent, canActivate: [AppRouteGuard] },
                    { path: 'line-work-center', component: LineWorkCenterComponent, canActivate: [AppRouteGuard] },
                    { path: 'customer', component: CustomerComponent, canActivate: [AppRouteGuard] },
                    { path: 'manual-packing', component: ManualPackingComponent, canActivate: [AppRouteGuard] },

                    { path: 'plant', component: PlantComponent, canActivate: [AppRouteGuard],data: { permission: 'Plant.View' } },
                    { path: 'material', component: MaterialComponent, canActivate: [AppRouteGuard],data: { permission: 'Plant.View' } },
                    { path: 'line-master', component: LineMasterComponent, canActivate: [AppRouteGuard],data: { permission: 'Plant.View' } },
                    //{ path: 'add-customer', component: AddEditCustomerComponent,data: { permission: 'Customer.Add' }, canActivate: [AppRouteGuard] },
                    { path: 'customer/:action/:userId', component: AddEditCustomerComponent, canActivate: [AppRouteGuard] },
                    { path: 'add-customer', component: AddEditCustomerComponent, canActivate: [AppRouteGuard] },
                    { path: 'customer/:action/:transporterId', component: AddEditCustomerComponent, canActivate: [AppRouteGuard] },
                    { path: 'edit-customer/:transporterId', component: AddEditCustomerComponent, canActivate: [AppRouteGuard] },
                    { path: 'storage-location', component: StorageLocationComponent, canActivate: [AppRouteGuard] },
                    { path: 'shift-master', component: ShiftMasterComponent, canActivate: [AppRouteGuard] },
                    //{ path: 'add-edit-shift', component: AddEditShiftComponent, canActivate: [AppRouteGuard] },
                    // { path: 'add-shift', component: AddEditShiftComponent, canActivate: [AppRouteGuard] },
                    { path: 'quality-sampling', component: QualitySamplingComponent, canActivate: [AppRouteGuard] },
                    { path: 'serialbarcodegeneration', component: SerialbarcodegenerationComponent, canActivate: [AppRouteGuard] },
                    { path: 'packing-order-confirmation', component: PackingOrderConfirmationComponent, canActivate: [AppRouteGuard] },
                    { path: 'packing-order', component: PackingOrderComponent, canActivate: [AppRouteGuard] },
                    { path: 'bin', component: BinComponent, canActivate: [AppRouteGuard] },
                    { path: 'add-bin', component: AddeditbinComponent, canActivate: [AppRouteGuard] },
                    //{ path: 'edit-bin', component: EditbinComponent, canActivate: [AppRouteGuard] },
                    { path: 'quality-checking', component: QualityCheckingComponent, canActivate: [AppRouteGuard] },
                    { path: 'quality-confirmation', component: QualityConfirmationComponent, canActivate: [AppRouteGuard] },
                    { path: 'storage-location-transfer', component: StorageLocationTransferComponent, canActivate: [AppRouteGuard] },
                    { path: 'add-shift', component: AddEditShiftComponent, canActivate: [AppRouteGuard] },
                    { path: 'quality-tested-itemplacement', component: QualityTestedItemplacementComponent, canActivate: [AppRouteGuard] },
                    { path: 'transfer-to-branch-from-plant', component: TransferToBranchFromPlantComponent, canActivate: [AppRouteGuard] },
                    { path: 'transfer-dealer-customerfrom-branch', component: TransferDealerCustomerfromBranchComponent, canActivate: [AppRouteGuard] },
                    { path: 'revalidation-process-branch', component: RevalidationProcessBranchComponent, canActivate: [AppRouteGuard] },
                    { path: 'grn-confirmation', component: GrnConfirmationComponent, canActivate: [AppRouteGuard] },
                    { path: 'approval-for-zonal-manager', component: ApprovalForZonalManagerComponent, canActivate: [AppRouteGuard] },
                    { path: 'warranty-claim', component: WarrantyClaimComponent, canActivate: [AppRouteGuard] },
                    { path: 'revalidation-dealer-location', component: RevalidationDealerLocationComponent, canActivate: [AppRouteGuard] },
                    { path: 'warranty-tracking', component: WarrantyTrackingComponent, canActivate: [AppRouteGuard] },
                    { path: 'add-edit-zonal/:action/:approvalId', component: AddEditZonalComponent, canActivate: [AppRouteGuard] },
                    { path: 'packing-reports', component: PackingReportsComponent, canActivate: [AppRouteGuard] },
                    { path: 'packing-order-barcode-dtls', component: PackingOrderBarcodeDtlsComponent, canActivate: [AppRouteGuard] },
                    { path: 'quality-report', component: QualityReportComponent, canActivate: [AppRouteGuard] },
                    { path: 'plantToWarehouse', component: PlantToWarehouseComponent, canActivate: [AppRouteGuard] },
                    { path: 'as-on-date-inventory', component: AsOnDateInventoryComponent, canActivate: [AppRouteGuard] },
                    { path: 'dispatch-from-warehouse', component: DispatchFromWarehouseComponent, canActivate: [AppRouteGuard] },
                    { path: 'grn-at-branch', component: GrnAtBranchComponent, canActivate: [AppRouteGuard] },
                    { path: 'dispatch-from-branch', component: DispatchFromBranchComponent, canActivate: [AppRouteGuard] },
                    { path: 'branch-location-from-dealer', component: BranchLocationFromDealerComponent, canActivate: [AppRouteGuard] },
                    { path: 'Revalidation-report', component: RevalidationReportComponent, canActivate: [AppRouteGuard] },
                    { path: 'MonthlyInspectionForDealer', component: MonthlyInspecForDealerComponent, canActivate: [AppRouteGuard] },
                    { path: 'DealerWiseFailure', component: DealerWiseFailureDetailsComponent, canActivate: [AppRouteGuard] },
                    { path: 'NonBarcodedProducts', component: NonBarcodedProductsComponent, canActivate: [AppRouteGuard] },
                    { path: 'ConsolidatNonBarcodedProducts', component: ConsolidatedNonBarcodedProductsComponent, canActivate: [AppRouteGuard] },
                    { path: 'lifeCycleReport', component: LifeCycleReportComponent, canActivate: [AppRouteGuard] },
                    { path: 'MfgMonthByDefective', component: ManufacturingMonthWiseDefectiveComponent, canActivate: [AppRouteGuard] },
                    { path: 'MfgTimeWiseDefective', component: ManufacturingTimeWiseDefectiveComponent, canActivate: [AppRouteGuard] },
                    { path: 'MaterialTolerance', component: MaterialToleranceComponent, canActivate: [AppRouteGuard] },
                   
                ]
            }
        ])

    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }