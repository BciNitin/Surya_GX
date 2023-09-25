using Abp.Authorization.Users;

using ELog.Core.Authorization.Users;
using ELog.Core.MultiTenancy;

namespace ELog.Core
{
    public interface ILdapExternalAuthenticationSource : IExternalAuthenticationSource<Tenant, User>
    {
    }
}
