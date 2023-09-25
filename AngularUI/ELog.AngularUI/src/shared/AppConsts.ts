export class AppConsts {

    static remoteServiceBaseUrl: string;
    static azureMapsSubscriptionKey: string;    
    static appBaseUrl: string;
    static appBaseHref: string; // returns angular's base-href parameter value if used during the publish
    static externalAuthProvider: any;
    static buildVersion: string;
    
    static localeMappings: any = [];

    static readonly userManagement = {
        defaultAdminUserName: 'admin'
    };

    static readonly localization = {
        defaultLocalizationSourceName: 'ELog'
    };

    static readonly authorization = {
        encryptedAuthTokenName: 'enc_auth_token'
    };
}
