using Abp.Authorization;

using ELog.Core.Authorization.Roles;
using ELog.Core.Authorization.Users;

namespace ELog.Core.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
