using Abp.Application.Services;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.UI;
using ELog.Application.Settings.Dto;
using ELog.Core;
using ELog.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ELog.Application.Settings
{
    //[PMMSAuthorize]
    public class SettingAppService : ApplicationService, ISettingAppService
    {
        private readonly IRepository<Setting, long> _settingRepository;
        private readonly IRepository<LogoMaster> _logoRepository;
        public SettingAppService(IRepository<Setting, long> settingRepository, IRepository<LogoMaster> logoRepository)
        {
            _settingRepository = settingRepository;
            _logoRepository = logoRepository;
        }

        public async Task<List<SettingDto>> GetAll()
        {
            return await _settingRepository.GetAll()
                .Select(setting => ObjectMapper.Map<SettingDto>(setting))
                .ToListAsync();
        }

        public async Task<SettingDto> Get(long Id)
        {
            return await _settingRepository.GetAll()
                .Select(setting => ObjectMapper.Map<SettingDto>(setting))
                .FirstOrDefaultAsync(x => x.Id == Id);
        }
        /// <summary>
        /// To upload image for current tenant.
        /// </summary>
        /// <param name="file">The input.</param>
        public async Task<string> UploadLogoAsync(IFormFile file)
        {
            var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
            if (fileExt.ToLower() != "png")
            {
                throw new UserFriendlyException(PMMSValidationConst.LogoFileExtensionValidationMsg);
            }
            var existingLogo = await _logoRepository.FirstOrDefaultAsync(x => x.TenantId == AbpSession.TenantId);
            MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);
            if (existingLogo != null)
            {
                existingLogo.ImageTitle = file.FileName;
                existingLogo.ImageData = ms.ToArray();
                await _logoRepository.UpdateAsync(existingLogo);
            }
            else
            {
                var image = new LogoMaster
                {
                    ImageTitle = file.FileName,
                    ImageData = ms.ToArray(),
                    TenantId = AbpSession.TenantId
                };
                await _logoRepository.InsertAsync(image);
            }
            CurrentUnitOfWork.SaveChanges();
            return PMMSValidationConst.LogoSuccessMsg;
        }
        /// <summary>
        /// To get uploaded image details for current tenant.
        /// </summary>
        public async Task<SettingDto> GetLogoAsync()
        {
            var settingDto = new SettingDto();
            var img = await _logoRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).FirstOrDefaultAsync();
            if (img != null)
            {
                string imageBase64Data =
                    Convert.ToBase64String(img.ImageData);
                string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                settingDto.Name = img.ImageTitle;
                settingDto.Value = imageDataURL;
            }
            return settingDto;
        }
    }
}