import { Component, Injector, OnInit } from '@angular/core';
import { SettingServiceProxy, SettingDto, TokenAuthServiceProxy, AuthenticateModel, AuthenticateResultModel, ForgotPasswordDto, AccountServiceProxy, UserServiceProxy, ForgotPasswordOutput ,ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { finalize } from 'rxjs/operators';
import { TokenService } from 'abp-ng2-module/dist/src/auth/token.service';
import { UtilsService } from 'abp-ng2-module/dist/src/utils/utils.service';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/app-component-base';
import { Router } from '@angular/router';
@Component({
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.less'],
  animations: [accountModuleAnimation()]
})
export class ForgotPasswordComponent extends AppComponentBase {
  submitting = false;
  logoPath: string;
  model: ForgotPasswordDto = new ForgotPasswordDto();
  buildVersion: string;
  isReadOnly: boolean = true;
  isSubmit: boolean = false;

  constructor(private _settingService: SettingServiceProxy, private _accountService: AccountServiceProxy,
    private _tokenService: TokenService, private _utilsService: UtilsService, private _router: Router,
    injector: Injector,) {
    super(injector);
    this.getLogo();
    this.buildVersion = AppConsts.buildVersion;
  }

  ngOnInit() {
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

  forgotPassword(): void {
    this.submitting = true;
    this.authenticate(() => (this.submitting = false));

  }

  authenticate(finallyCallback?: () => void): void {
    finallyCallback = finallyCallback || (() => { });
    this.model.passwordStatus = 1;
    this._accountService.

      forgotPassword(false,this.model)
      .pipe(
        finalize(() => {
          this.submitting = false;
        })
      )
      .subscribe((result: ForgotPasswordOutput) => {
        if (result.result) {
        if(result.userRole=="SuperAdmin")
        {
          this.notify.info(this.l('Super admin can reset password using swagger url'));
        }
        else if(result.userRole=="Admin")
        {
        this.isSubmit = true;
        this.notify.success(this.l('Notified to superAdmin '));
        }
        else if(result.userRole=="user")
        {
          this.isSubmit = true;
          this.notify.success(this.l('Notified to admin'));
        }
          return;
        }
        else {
          this.isSubmit = false;
          this.notify.error(this.l('User Name is not found'));
        }


      });
  }

  Close() {
    this._router.navigate(['account/login']);
  }
}



