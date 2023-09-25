using Abp;
using Abp.Dependency;
using Abp.UI;

using System;

namespace ELog.Core.Authorization
{
    public class PMMSLoginResultTypeHelper : AbpServiceBase, ITransientDependency
    {
        public PMMSLoginResultTypeHelper()
        {
            LocalizationSourceName = PMMSConsts.LocalizationSourceName;
        }

        public Exception CreateExceptionForFailedLoginAttempt(int? accessAttemptCount, int? resetPasswordDaysLeft)
        {
            if (accessAttemptCount == 0)
            {
                return new UserFriendlyException(L("LoginFailed"), "Invalid Username or Password." +
                    "Your account has been locked out. Please contact administrator to reset password. ");
            }
            else if (accessAttemptCount > 0)
            {
                return new UserFriendlyException(L("LoginFailed"), "Invalid Username or Password. " +
                    "You have " + accessAttemptCount + " more attempt(s) left.");
            }
            else if (resetPasswordDaysLeft <= 0)
            {
                return new UserFriendlyException(L("LoginFailed"), "Your passowrd has been expired. Please contact administrator to reset password. ");
            }
            else
            {
                return new UserFriendlyException(L("LoginFailed"), "Invalid Username or Password");
            }
        }
    }
}