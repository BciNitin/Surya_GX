using ELog.Application.Configuration.Dto;

using System.Threading.Tasks;


namespace ELog.Application.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
