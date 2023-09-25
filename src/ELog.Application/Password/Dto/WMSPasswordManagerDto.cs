using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.Password.Dto
{
    [AutoMapFrom(typeof(WMSPasswordManager))]
    public class WMSPasswordManagerDto : EntityDto<int>

    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }



    }
}

