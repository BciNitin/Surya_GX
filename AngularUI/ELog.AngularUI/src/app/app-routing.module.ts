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
                   
                ]
            }
        ])

    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }