import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientJsonpModule } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { ModalModule } from 'ngx-bootstrap/modal';
import { AbpModule } from '@abp/abp.module';
import { AccountRoutingModule } from './account-routing.module';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';
import { AccountComponent } from './account.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AccountLanguagesComponent } from './layout/account-languages.component';
import { LoginService } from './login/login.service';
// tenants
import { TenantChangeComponent } from './tenant/tenant-change.component';
import { TenantChangeDialogComponent } from './tenant/tenant-change-dialog.component';
import { PlantChangeDialogComponent } from './login/plant-change-dialog.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        HttpClientJsonpModule,
        AbpModule,
        SharedModule,
        ServiceProxyModule,
        AccountRoutingModule,
        ModalModule.forRoot(),
    ],
    declarations: [
        AccountComponent,
        LoginComponent,
        RegisterComponent,
        AccountLanguagesComponent,
        // tenant
        TenantChangeComponent,
        TenantChangeDialogComponent,
        PlantChangeDialogComponent,
        ForgotPasswordComponent,
    ],
    providers: [
        LoginService,
        // AdalService
        // ,OAuthService
    ],
    entryComponents: [
        // tenant
        TenantChangeDialogComponent,
        //plant
        PlantChangeDialogComponent
    ]
})
export class AccountModule {
}
