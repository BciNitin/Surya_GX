using Abp.Application.Services.Dto;

namespace ELog.Application.Users.Dto
{
    public class UserConfigurationDto : EntityDto<long>
    {
        public int UserCreationMaxValue { get; set; }
        public bool IsUserCreationAllowed { get; set; }
    }
}