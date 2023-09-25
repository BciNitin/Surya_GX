import { TokenService } from '@abp/auth/token.service';
import { LogService } from '@abp/log/log.service';
import { MessageService } from '@abp/message/message.service';
import { UtilsService } from '@abp/utils/utils.service';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { finalize } from 'rxjs/operators';
import { AuthenticateModel, AuthenticateResultModel, TokenAuthServiceProxy, ExternalAuthenticateModel, ExternalAuthenticateResultModel ,ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';
import { truncate } from 'fs';

@Injectable()
export class LoginService {
    static readonly twoFactorRememberClientTokenName = 'TwoFactorRememberClientToken';
    authenticateModel: AuthenticateModel;
    authenticateResult: AuthenticateResultModel;
    externalAuthenticateModel: ExternalAuthenticateModel = new ExternalAuthenticateModel();
    externalAuthenticateResult: ExternalAuthenticateResultModel;
    rememberMe: boolean;

    constructor(
        private _tokenAuthService: TokenAuthServiceProxy,
        private _router: Router,
        private _utilsService: UtilsService,
        private _messageService: MessageService,
        private _tokenService: TokenService,
        private _logService: LogService
    ) {
        this.clear();
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

    authenticateWithActiveDirectory(finallyCallback?: () => void): void {
        finallyCallback = finallyCallback || (() => { });

        this._tokenAuthService
            .authenticateUsingActiveDirectory(this.authenticateModel)
            .pipe(finalize(() => { finallyCallback(); }))
            .subscribe((result: AuthenticateResultModel) => {
                this.processAuthenticateResult(result);
            });
    }

    private processAuthenticateResult(authenticateResult: AuthenticateResultModel) {
        this.authenticateResult = authenticateResult;

        if (authenticateResult.accessToken) {
            // Successfully logged in
            this.login(
                authenticateResult.accessToken,
                authenticateResult.encryptedAccessToken,
                authenticateResult.expireInSeconds);
        } else {
            // Unexpected result!

            this._logService.warn('Unexpected authenticateResult!');
            this._router.navigate(['/']);
        }
    }

    authenticateWithExternalProvider(finallyCallback?: () => void): void {
        finallyCallback = finallyCallback || (() => { });
        this._tokenAuthService
            .externalAuthenticate(this.externalAuthenticateModel)
            .pipe(
                finalize(() => {
                    finallyCallback();
                }),
            )
            .subscribe(
                (result: ExternalAuthenticateResultModel) => {
                    this.externalAuthenticateResult = result;
                    this.processAuthenticateResultForExternalProvider(
                        result.accessToken,
                        result.encryptedAccessToken,
                        result.expireInSeconds
                    );
                },
                error => {
                    console.log('External Authentication Failed');
                    // Navigate to login page again if error occurs
                    this._router.navigate(['/']);
                },
            );
    }

    private processAuthenticateResultForExternalProvider(
        accessToken: string,
        encryptedAccessToken: string,
        expireInSeconds: number
    ) {
        if (accessToken) {
            // Successfully logged in
            this.login(
                accessToken,
                encryptedAccessToken,
                expireInSeconds
            );
        } else {
            // Unexpected result!
            this._logService.warn('Unexpected authenticateResult!');
            this._router.navigate(['/']);
        }
    }

    private login(accessToken: string, encryptedAccessToken: string, expireInSeconds: number): void {
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

        location.href = initialUrl;
    }

    private clear(): void {
        this.authenticateModel = new AuthenticateModel();
        this.authenticateResult = null;
        this.rememberMe = false;
    }
}
