import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientJsonpModule } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';

import { ModalModule } from 'ngx-bootstrap/modal';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgxPrintModule } from 'ngx-print';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { AbpModule } from '@abp/abp.module';

import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';

import { HomeComponent } from '@app/home/home.component';
import { TopBarComponent } from '@app/layout/topbar.component';
import { SideBarUserAreaComponent } from '@app/layout/sidebar-user-area.component';
import { SideBarNavComponent } from '@app/layout/sidebar-nav.component';
import { SideBarFooterComponent } from '@app/layout/sidebar-footer.component';
import { RightSideBarComponent } from '@app/layout/right-sidebar.component';
// tenants
import { TenantsComponent } from '@app/tenants/tenants.component';
import { CreateTenantDialogComponent } from './tenants/create-tenant/create-tenant-dialog.component';
import { EditTenantDialogComponent } from './tenants/edit-tenant/edit-tenant-dialog.component';
// roles
import { RolesComponent } from '@app/roles/roles.component';
// users
import { StandardweightduedatedilogComponent } from './home/standardweightduedatedilog/standardweightduedatedilog.component';

import { UsersComponent } from '@app/users/users.component';
import { ModulesComponent } from '@app/modules/modules.component';
import { UsersFilterDialog } from './users/usersfilter-dialog';
import { RolesFilterDialog } from './roles/rolesfilter-dialog';
import { AddEditUserComponent } from './users/add-edit-user/add-edit-user.component';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material';
import { AddEditRoleComponent } from './roles/add-edit-role/add-edit-role.component';
import { RxReactiveFormsModule } from '@rxweb/reactive-form-validators';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import * as _moment from 'moment';
import { adapterFactory } from 'angular-calendar/date-adapters/moment';
import * as moment from 'moment';
import { CalendarModule } from 'angular-calendar';
import { FlatpickrModule } from 'angularx-flatpickr';
import { AddEditModuleComponent } from './modules/add-edit-module/add-edit-module.component';
import { AddEditSubModuleComponent } from './subModules/add-edit-subModule/add-edit-subModule.component';
import { SubModulesComponent } from './subModules/subModules.component';
import { RouterExtService } from './subModules/add-edit-subModule/RouterExtService';
import { IConfig, NgxMaskModule } from 'ngx-mask';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { MomentUtcDateAdapter } from './adapter/MomentUtcDateAdapter';
import { DataTablesModule } from 'angular-datatables';
import { MatSelectFilterModule } from 'mat-select-filter';
import { PasswordComponent } from './password/password.component';
import { ResetPasswordComponent } from './password/reset-password/reset-password.component';
import { WeighingMachineStampingDueOnListComponent } from './home/weighing-machine-stamping-due-on-list.component';
import { DigitsAfterDecimalPipe } from '@shared/pipes/digits-after-decimal.pipe';
import { ZeroDecimalDirective } from '@shared/directives/zero-decimal.directive';
import { LogFormsListComponent } from './log-forms-list/log-forms-list.component';
import { AddEditFormsComponent } from './log-forms-list/add-edit-forms/add-edit-forms.component';
import { CreateformsComponent } from './log-forms-list/createforms/createforms.component';
import { ApproveformsComponent } from './log-forms-list/approveforms/approveforms.component';
import { ElogPanelComponent } from './elog-panel/elog-panel.component';
import { NewPanelComponent } from './elog-panel/new-panel/new-panel.component';
import { ApproveFormlistComponent } from './approve-forms-list/approve-forms-list.component';
import { LogdataapprovalComponent } from './log-forms-list/log-data-approval/log-data-approval.component';

import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';

import { NotificationsCenterComponent } from './notifications-center/notifications-center.component';
import { PlantComponent } from './masters/plant/plant.component';


import { LineWorkCenterComponent } from './PlantOperation/line-work-center/line-work-center.component';

import { CustomerComponent } from './masters/customer/customer.component';
import { ManualPackingComponent } from './PlantOperation/manual-packing/manual-packing.component';


import { SearchFilterPipe } from '@shared/SearchFilter/search-filter.pipe';



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
import { RevalidationDealerLocationComponent } from './PlantOperation/revalidation-dealer-location/revalidation-dealer-location.component';
import { GrnConfirmationComponent } from './PlantOperation/grn-confirmation/grn-confirmation.component';
import { ApprovalForZonalManagerComponent } from './PlantOperation/approval-for-zonal-manager/approval-for-zonal-manager.component';
import { WarrantyClaimComponent } from './PlantOperation/warranty-claim/warranty-claim.component';
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

export function momentAdapterFactory() {
    return adapterFactory(moment);
};

export function maskConfigFunction() {
    return {
        validation: false,
        dropSpecialCharacters: false
        
    };
};

export const PMMS_FORMATS = {
    parse: {
        dateInput: 'DD/MM/YYYY',
    },
    display: {
        dateInput: 'DD/MM/YYYY',
        monthYearLabel: 'MMM YYYY',
        dateA11yLabel: 'LL',
        monthYearA11yLabel: 'MMMM YYYY',
    },
};
@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        TopBarComponent,
        SideBarUserAreaComponent,
        SideBarNavComponent,
        SideBarFooterComponent,
        RightSideBarComponent,
        ElogPanelComponent,
        NewPanelComponent,
        StandardweightduedatedilogComponent,
        // tenants
        TenantsComponent,
        CreateTenantDialogComponent,
        EditTenantDialogComponent,
        // roles
        RolesComponent,
        // users
        UsersComponent,
        UsersFilterDialog,
        RolesFilterDialog,
        AddEditUserComponent,
        AddEditRoleComponent,
        AddEditModuleComponent,
        ModulesComponent,
        AddEditSubModuleComponent,
        SubModulesComponent,
        PasswordComponent,
        ResetPasswordComponent,
        WeighingMachineStampingDueOnListComponent,
        
        DigitsAfterDecimalPipe,
        ZeroDecimalDirective,
       
        LogFormsListComponent,
        AddEditFormsComponent,
        CreateformsComponent,
        ApproveformsComponent,
        ApproveFormlistComponent,
        LogdataapprovalComponent,
        NotificationsCenterComponent,
        PlantComponent,
        MaterialComponent,
        LineWorkCenterComponent,
        CustomerComponent,
        ManualPackingComponent,
        LineMasterComponent,
        SearchFilterPipe,
        AddEditCustomerComponent,
        StorageLocationComponent,
        ShiftMasterComponent,
        QualitySamplingComponent,
        SerialbarcodegenerationComponent,
        PackingOrderConfirmationComponent,
        PackingOrderComponent,
        BinComponent,
        AddeditbinComponent,
        //EditbinComponent,
        QualityCheckingComponent,
        QualityConfirmationComponent,
        StorageLocationTransferComponent,
        AddEditShiftComponent,
        QualityTestedItemplacementComponent,
        TransferToBranchFromPlantComponent,
        TransferDealerCustomerfromBranchComponent,
        RevalidationProcessBranchComponent,
        RevalidationDealerLocationComponent,
        GrnConfirmationComponent,
        ApprovalForZonalManagerComponent,
        WarrantyClaimComponent,
        AddEditZonalComponent,
        WarrantyTrackingComponent,
        PackingReportsComponent,
        PackingOrderBarcodeDtlsComponent,
        QualityReportComponent,
        PlantToWarehouseComponent,
        AsOnDateInventoryComponent,
        DispatchFromWarehouseComponent,
        GrnAtBranchComponent,
        DispatchFromBranchComponent,
        BranchLocationFromDealerComponent,
        RevalidationReportComponent
    ],
    imports: [
        HttpClientModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        HttpClientJsonpModule,
        ModalModule.forRoot(),
        AbpModule,
        AppRoutingModule,
        ServiceProxyModule,
        SharedModule,
        NgxPaginationModule,
        NgxPrintModule,
        RxReactiveFormsModule,
        FlatpickrModule.forRoot(),
        CalendarModule.forRoot({ provide: DateAdapter, useFactory: momentAdapterFactory }),
        NgxMaskModule.forRoot(maskConfigFunction),
        InfiniteScrollModule,
        TabsModule.forRoot(),
        DataTablesModule, MatSelectFilterModule,  MatFormFieldModule,
        MatSelectModule,
        MatButtonModule,
        
    ],
    providers: [DatePipe,
        { provide: MAT_DATE_LOCALE, useValue: 'en-IN' },
        { provide: DateAdapter, useClass: MomentUtcDateAdapter },
       // { provide: LocationStrategy,useClass:HashLocationStrategy},
        { provide: MAT_DATE_FORMATS, useValue: PMMS_FORMATS },
        RouterExtService

    ],
    entryComponents: [
        // tenants
        CreateTenantDialogComponent,
        EditTenantDialogComponent,
        UsersFilterDialog,
        RolesFilterDialog,
        StandardweightduedatedilogComponent,
        WeighingMachineStampingDueOnListComponent,
        
    ]
})
export class AppModule { }