using Abp.Configuration;
using ELog.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.Host
{
    [ExcludeFromCodeCoverage]
    public class DefaultSettingsForTenantsCreator
    {
        private readonly PMMSDbContext _context;
        private readonly int _tenantId;

        public DefaultSettingsForTenantsCreator(PMMSDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            // User Creation Limit configuration
            AddSettingIfNotExists(PMMSConsts.UserCreationLimitSetting, PMMSConsts.UserCreationLimitSettingValue, _tenantId);

            // IsCommonCreator for Inspection Checklist Master
            AddSettingIfNotExists(PMMSConsts.IsCommonCreatorSetting, PMMSConsts.IsCommonCreatorSettingValue, _tenantId);

            //IsLogin WithAd Setting 
            AddSettingIfNotExists(PMMSConsts.IsLoginWithAd, PMMSConsts.IsLoginWithAdValue, _tenantId);

            // User Creation Limit configuration
            AddSettingIfNotExists(PMMSConsts.UserLockoutEnabledByDefault, PMMSConsts.UserLockoutEnabledByDefaultValue, _tenantId);
            // User Creation Limit configuration
            AddSettingIfNotExists(PMMSConsts.DefaultAccountLockoutTimeSpan, PMMSConsts.DefaultAccountLockoutTimeSpanValue, _tenantId);
            // User Creation Limit configuration
            AddSettingIfNotExists(PMMSConsts.MaxFailedAccessAttemptsBeforeLockout, PMMSConsts.MaxFailedAccessAttemptsBeforeLockoutValue, _tenantId);

        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.IgnoreQueryFilters().Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }
            if (name == PMMSConsts.UserCreationLimitSetting)
            {
                var userCreationLimitSettingValue = EncryptDecryptHelper.Encrypt(Convert.ToString(value));
                _context.Settings.Add(new Setting(tenantId, null, name, userCreationLimitSettingValue));
            }
            else
            {
                _context.Settings.Add(new Setting(tenantId, null, name, value));
            }
            _context.SaveChanges();
        }
    }
}
