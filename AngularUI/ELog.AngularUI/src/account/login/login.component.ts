import { Component, Injector } from '@angular/core';
import { AbpSessionService } from '@abp/session/abp-session.service';
import { AppComponentBase } from '@shared/app-component-base';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { LoginService } from './login.service';
import {
  AccountServiceProxy, ExternalLoginProviderInfoModel, IsTenantAvailableInput, IsTenantAvailableOutput, TokenAuthServiceProxy,
  AuthenticateModel, AuthenticateResultModel, ExternalAuthenticateModel, ExternalAuthenticateResultModel, SettingServiceProxy, SettingDto, ForgotPasswordOutput, ForgotPasswordDto
,ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { AppTenantAvailabilityState } from '@shared/AppEnums';
import { MatDialog } from '@angular/material';
import { PlantChangeDialogComponent } from './plant-change-dialog.component';
import { TokenService } from '@abp/auth/token.service';
import { LogService } from '@abp/log/log.service';
import { UtilsService } from '@abp/utils/utils.service';
import { UrlHelper } from '@shared/helpers/UrlHelper';
@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less'],
  animations: [accountModuleAnimation()]
})
export class LoginComponent extends AppComponentBase {
  submitting = false;
  externalLoginProviders: ExternalLoginProviderInfoModel[];
  versionText: string;
  currentYear: number;
  hidePassword: boolean = true;
  isReadOnly: boolean = true;
  isAdLogin: boolean = false;
  logoPath: string;
  authenticateModel: AuthenticateModel;
  authenticateResult: AuthenticateResultModel;
  externalAuthenticateModel: ExternalAuthenticateModel = new ExternalAuthenticateModel();
  externalAuthenticateResult: ExternalAuthenticateResultModel;
  rememberMe: boolean;
  buildVersion: string;
  model: ForgotPasswordDto = new ForgotPasswordDto();
  constructor(
    injector: Injector,
    public loginService: LoginService,
    private _sessionService: AbpSessionService,
    // private oauthService: OAuthService,
    private _tokenAuthService: TokenAuthServiceProxy,
    private _router: Router,
    private _accountService: AccountServiceProxy,
    private _dialog: MatDialog,
    private _tokenService: TokenService,
    private _logService: LogService,
    private _utilsService: UtilsService,
    private _settingService: SettingServiceProxy,
  ) {

    super(injector);
    this.clear();
    this.currentYear = new Date().getFullYear();
    this.versionText = this.appSession.application.version + ' [' + this.appSession.application.releaseDate.format('YYYYDDMM') + ']';
    this.checkIsLoginWithAdSetting();
    this.getLogo();
    this.buildVersion = AppConsts.buildVersion;
  }

  ngOnInit(): void {
    this.setTitle('Login');
    this._tokenAuthService
      .getExternalAuthenticationProviders()
      .subscribe(externalLoginProviders => {
        this.externalLoginProviders = externalLoginProviders;
      });
    this.loadDefaultTenant();
    
  }
  checkIsLoginWithAdSetting() {
    this._settingService.getAll().subscribe((result: SettingDto[]) => {
      let settingValue = result.filter(x => x.name == 'IsLoginWithAd')[0];
      if (settingValue) {
        this.isAdLogin = settingValue.value == 'true' ? true : false;
      }
    });
  }

  public loginAd() {
    // this.oauthService.initLoginFlow();
  }

  get multiTenancySideIsTeanant(): boolean {
    return this._sessionService.tenantId > 0;
  }

  get LoginWithAzureAd(): boolean {
    if (AppConsts.externalAuthProvider.LoginWithAzureAD) {
      return true;
    }
    return false;
  }

  get isSelfRegistrationAllowed(): boolean {
    // if (!this._sessionService.tenantId) {
    //   return false;
    // }

    return false;
  }

  login(): void {
    this.submitting = true;
    this.authenticate(() => (this.submitting = false));

  }
  openPlantDialogComponent(initialUrl: string): void {
    const dialogRef = this._dialog.open(PlantChangeDialogComponent, {
      width: '500px',
      height: '240px',
      data: { userId: this.authenticateResult.userId }
    });

    dialogRef.afterClosed().subscribe((plantid) => {
      this.setPlantId(plantid, initialUrl);
    });
  }
  loginWithActiveDirectory(): void {
    this.submitting = true;
    this.authenticateWithActiveDirectory(() => (this.submitting = false));
  }
  authenticateWithActiveDirectory(finallyCallback?: () => void): void {
    finallyCallback = finallyCallback || (() => { });

    this._tokenAuthService
      .authenticateUsingActiveDirectory(this.authenticateModel)
      .pipe(finalize(() => { finallyCallback(); }))
      .subscribe((result: AuthenticateResultModel) => {
        this.processAuthenticateResult(result);
      });
  }
  loadDefaultTenant() {
    const input = new IsTenantAvailableInput();
    input.tenancyName = "Default";


    this._accountService
      .isTenantAvailable(input)
      .pipe(
        finalize(() => {

        })
      )
      .subscribe((result: IsTenantAvailableOutput) => {
        switch (result.state) {
          case AppTenantAvailabilityState.Available:
            abp.multiTenancy.setTenantIdCookie(result.tenantId);

            //location.reload();
            return;
          case AppTenantAvailabilityState.InActive:
            this.message.warn(this.l('TenantIsNotActive', 'Default'));
            break;
          case AppTenantAvailabilityState.NotFound:
            this.message.warn(
              this.l('ThereIsNoTenantDefinedWithName{0}', 'Default')
            );
            break;
        }
      });
  }
    authenticate(finallyCallback?: () => void): void {
      
    finallyCallback = finallyCallback || (() => { });

    this._tokenAuthService
      .authenticate(this.authenticateModel)
      .pipe(finalize(() => { finallyCallback(); }))
      .subscribe((result: AuthenticateResultModel) => {
        this.processAuthenticateResult(result);
      });
  }
  private processAuthenticateResult(authenticateResult: AuthenticateResultModel) {
    this.authenticateResult = authenticateResult;
      
    if (authenticateResult.accessToken) {
      // Successfully logged in
      this.loginWithPlant(
        authenticateResult.accessToken,
        authenticateResult.encryptedAccessToken,
        authenticateResult.expireInSeconds,
        authenticateResult.plantId,
        authenticateResult.isMultiplePlantExists);
        localStorage.setItem('PasswordStatus', JSON.stringify(this.authenticateResult.passwordStatus));
        this.setRefreshToken(this.authenticateResult.refreshToken);


    } else {
      // Unexpected result!

        this._logService.warn('Unexpected authenticateResult!');
        this.notify.error(this.l('Invalid Username or Password'));
      this._router.navigate(['account/login']);
    }
  }
  private loginWithPlant(accessToken: string, encryptedAccessToken: string, expireInSeconds: number, plantId: number, isMultiplePlantExists: boolean): void {
    const tokenExpireDate = new Date(new Date().getTime() + 1000 * expireInSeconds);

    this._tokenService.setToken(
      accessToken,
      tokenExpireDate
    );

    this._utilsService.setCookieValue(
      AppConsts.authorization.encryptedAuthTokenName,
      encryptedAccessToken,
      tokenExpireDate,
      abp.appPath
    );

    // let initialUrl = UrlHelper.initialUrl;
    // if (initialUrl.indexOf('/login') > 0) {
    //     initialUrl = AppConsts.appBaseUrl;
    // } 
      let initialUrl = UrlHelper.initialUrl;
    if (
      initialUrl.indexOf('/login') > 0
    ) {
      initialUrl = AppConsts.appBaseUrl;
    }
    if (isMultiplePlantExists) {
      this.openPlantDialogComponent(initialUrl);
    } else {
      this.setPlantId(plantId, initialUrl);
       location.href = initialUrl ;
       // location.href = initialUrl
    }
  }
  private setPlantId(plantId: number, initialUrl: string) {
    if (plantId != null) {
      localStorage.setItem('plantId', JSON.stringify(plantId));
      location.href = initialUrl;
    }
    else {
      localStorage.setItem('plantId', '');
    }
    }
    private setRefreshToken(refreshToken: string) {
        if (refreshToken != null) {
            localStorage.setItem('refreshToken', refreshToken);     
        }
        else {
            localStorage.setItem('refreshToken', '');
        }
    }
  private clear(): void {
    this.authenticateModel = new AuthenticateModel();
    this.authenticateResult = null;
    this.rememberMe = false;
  }
  getLogo() {
    this._settingService.getLogo().subscribe((result: SettingDto) => {
      if (result && result.value != null) {
        this.logoPath = result.value;
      } else {
        this.logoPath = "./assets/images/logo.png";
      }
      localStorage.setItem('logo', this.logoPath);
    });
  }
  ForgotPassword() {
    if (this.authenticateModel.userNameOrEmailAddress == ''||this.authenticateModel.userNameOrEmailAddress == undefined) {
      this.notify.error(this.l('User Name is required'));
      
    }
    else {
      this.model.employeeCode = this.authenticateModel.userNameOrEmailAddress
      this._accountService.

        forgotPassword(true, this.model)
        .pipe(
          finalize(() => {
            this.submitting = false;
          })
        ).subscribe((result: ForgotPasswordOutput) => {
          if (result.result) {
            if (result.userRole == "SuperAdmin") {
              this.notify.info(this.l('Super admin can reset password using swagger url'));
            }
            else {
              this._router.navigate(['account/forgotpassword']);
            }
          }
          else {
              this.notify.error(this.l('User name not found or is Inactive , Contact to Admin'));
          }
        });
    }
  }
  
  
}

