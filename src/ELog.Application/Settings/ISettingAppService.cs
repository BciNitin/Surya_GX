using Abp.Application.Services;
using ELog.Application.Settings.Dto;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.Settings
{
    public interface ISettingAppService : IApplicationService
    {
        Task<List<SettingDto>> GetAll();

        Task<SettingDto> Get(long Id);
        Task<string> UploadLogoAsync(IFormFile file);
        Task<SettingDto> GetLogoAsync();
    }
}