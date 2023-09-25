using Abp.Application.Services.Dto;
using System;

namespace ELog.Application.Password.Dto
{

    public class WMSPasswordManagerDetailsDto : EntityDto<int>

    {

        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }
}

