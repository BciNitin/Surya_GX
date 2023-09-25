using Abp.Zero.Ldap.Configuration;

namespace ELog.Core.Interfaces
{
    public interface IPMMSLdapSettings : ILdapSettings
    {
        string GetCommonFetchUrl();
        string GetAdminFetchUrl();
    }
}
