using Abp.Authorization;
using Abp.Runtime.Session;

using ELog.Application.Configuration.Dto;
using ELog.Core.Configuration;

using System.Threading.Tasks;

namespace ELog.Application.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : PMMSAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}