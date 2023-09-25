using Abp.Configuration;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Net.Mail;
using ELog.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.Host
{
    [ExcludeFromCodeCoverage]
    public class DefaultSettingsCreator
    {
        private readonly PMMSDbContext _context;

        public DefaultSettingsCreator(PMMSDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            int? tenantId = null;

            if (!PMMSConsts.MultiTenancyEnabled)
            {
#pragma warning disable CS0162 // Unreachable code detected
                tenantId = MultiTenancyConsts.DefaultTenantId;
#pragma warning restore CS0162 // Unreachable code detected
            }

            // Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, PMMSConsts.DefaultFromAddressValue, tenantId);
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, PMMSConsts.DefaultFromDisplayNameValue, tenantId);

            // Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "en", tenantId);

            // IsCommonCreator for Inspection Checklist Master
            AddSettingIfNotExists(PMMSConsts.IsCommonCreatorSetting, PMMSConsts.IsCommonCreatorSettingValue, tenantId);
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.IgnoreQueryFilters().Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}
